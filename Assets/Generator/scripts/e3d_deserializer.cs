using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class e3d_deserializer : MonoBehaviour
{
    byte[] e3dBytes;
    string e3dContent;
    public GameObject deserialize_E3D(string path)
    {
        Debug.Log("Deserializing e3d model");
        // Tworzymy obiekt modelu
        GameObject e3dModel = new GameObject("e3dBinaryModel"); 
        e3dModel.AddComponent<MeshRenderer>();
        e3dModel.AddComponent<MeshFilter>();

        // Odczytujemy bajty z pliku binarnego
        e3dBytes = File.ReadAllBytes(path);
        using (StreamReader sr = new StreamReader(path))
        {
            e3dContent = sr.ReadToEnd();
        }
        // Zapiszmy se podstawowe informacje do zmiennych
        byte sliceVersion = e3dBytes[3]; // wersja kromki E3D
        int sliceLength = e3dBytes[4]; // dlugosc kromki
        // E3D0xxxxSUB0 jest juz przeanalizowane. Teraz analizujemy kromke SUB0

        // Numer submodelu następnego, -1 gdy brak
        int nextSubmodelNumber = intSubmodelProperities(e3dBytes, 0, 4);

        // Numer pierwszego submodelu potomnego, -1 gdy brak.
        int firstChildSubmodel = intSubmodelProperities(e3dBytes, 4, 4);

        // Typ submodelu. Możliwe jest użycie wartości takich jak GL_TRIANGLES.
        // Wartości powyżej 256 oznaczają typy specjalne.
        int submodelType = intSubmodelProperities(e3dBytes, 8, 4);

        // 	Numer nazwy. Nazwy są numerowane od zera. 
        // Wartość -1 oznacza brak nazwy (nie jest potrzebna, jeśli submodel nie jest animowany zdarzeniami).
        int submodelName = intSubmodelProperities(e3dBytes, 12, 4);

        // Rodzaj animacji.
        int animationType = intSubmodelProperities(e3dBytes, 16, 4);

        // Flagi submodelu. Bity 0..15 dotyczą danego submodelu, 
        // bity 16..31 są skumulowanymi bitami 0..7 obiektów następnych oraz potomnych
        // i umożliwiają pomijanie całych gałęzi podczas wyświetlania.
        int submodelFlag = intSubmodelProperities(e3dBytes, 20, 4);

        // Numer macierzy przekształcenia widoku. 
        // Macierz jest potrzebna do ustalenia osi układu współrzędnych dla animacji. 
        // Jeśli submodel nie będzie animowany, można jego wierzchołki przeliczyć tak, 
        // aby uzyskać macierz jednostkową. W takim przypadku nie jest ona zapisywana do pliku, 
        // a w tym miejscu należy podać wartość -1.
        int vievTransformMatrixNumber = intSubmodelProperities(e3dBytes, 24, 4);

        // Ilość wierzchołków. Dla typu GL_TRIANGLES musi być podzielna przez 3, dla GL_LINES parzysta.
        int numberOfVertices = intSubmodelProperities(e3dBytes, 28, 4);

        // Numer pierwszego wierzchołka w VNT0 albo pierwszego indeksu wierzchołka w IDX1, IDX2 albo IDX4.
        int firstVertex = intSubmodelProperities(e3dBytes, 32, 4);

        // 	Numer materiału. Materiały są numerowane od 1. Wartość 0 oznacza materiał "colored". 
        // Wartości ujemne to numer materiału wymiennej, pierwszy materiał wymienny ma numer -1.
        int materialNumber = intSubmodelProperities(e3dBytes, 36, 4);

        // Próg jasności załączania submodelu. Dla wartości 0.0÷1.0 model będzie wyświetlany, 
        // jeśli będzie ciemniej niż podana wartość. Dla wartości -1.0÷0.0 - gdy będzie jaśniej.
        int brightnessToggleThreshold = intSubmodelProperities(e3dBytes, 40, 4);

        // Próg zapalenia światła. Świecenie automatyczne zostanie włączone, jeśli będzie ciemniej niż podana wartość 0.0÷1.0.
        int brightnessThreshold = intSubmodelProperities(e3dBytes, 44, 4);

        // Kolor RGBA ambient. Nie używany.
        // Mimo wszystko zmienna bedzie
        float[] ambientRGBA = { floatSubmodelProperities(e3dBytes, 48, 4),
        floatSubmodelProperities(e3dBytes, 52, 4),
        floatSubmodelProperities(e3dBytes, 56, 4),
        floatSubmodelProperities(e3dBytes, 60, 4)};

        // Kolor RGBA diffuse
        float[] diffuseRGBA = { floatSubmodelProperities(e3dBytes, 64, 4),
        floatSubmodelProperities(e3dBytes, 68, 4),
        floatSubmodelProperities(e3dBytes, 72, 4),
        floatSubmodelProperities(e3dBytes, 76, 4)};

        // Kolor RGBA specular. Nie używany.
        // Mimo wszystko zmienna bedzie a co
        float[] spectularRGBA = { floatSubmodelProperities(e3dBytes, 80, 4),
        floatSubmodelProperities(e3dBytes, 84, 4),
        floatSubmodelProperities(e3dBytes, 88, 4),
        floatSubmodelProperities(e3dBytes, 92, 4)};

        // Kolor RGBA świecenia. Używany po załączeniu świecenia (selfillum).
        float[] selfIllumRGBA = { floatSubmodelProperities(e3dBytes, 96, 4),
        floatSubmodelProperities(e3dBytes, 100, 4),
        floatSubmodelProperities(e3dBytes, 104, 4),
        floatSubmodelProperities(e3dBytes, 108, 4)};

        // Rozmiar linii dla GL_LINES.
        float lineSize = floatSubmodelProperities(e3dBytes, 112, 4);

        // Kwadrat maksymalnej odległości widoczności. W większej odległości submodel i jego potomne 
        // nie będą widoczne (np. można je uznać za pomijalnie małe albo są zastąpione fazą LoD).
        float maxViewDistance = floatSubmodelProperities(e3dBytes, 116, 4);

        // Kwadrat minimalnej odległości widoczności. W mniejszej odległości submodel i jego potomne
        // nie będą widoczne(np.faza LoD dla oddalenia).
        float minViewDistance = floatSubmodelProperities(e3dBytes, 120, 4);

        // Parametry światła dla punktów świecących:
        // fNearAttenStart, fNearAttenEnd, bUseNearAtten (bool), iFarAttenDecay (int),
        // fFarDecayRadius, fCosFalloffAngle, fCosHotspotAngle, fCosViewAngle.
        float[] lightParameters =
        {
            floatSubmodelProperities(e3dBytes, 124, 4),
            floatSubmodelProperities(e3dBytes, 128, 4),
            floatSubmodelProperities(e3dBytes, 132, 4),
            floatSubmodelProperities(e3dBytes, 136, 4),
            floatSubmodelProperities(e3dBytes, 140, 4),
            floatSubmodelProperities(e3dBytes, 144, 4),
            floatSubmodelProperities(e3dBytes, 148, 4),
            floatSubmodelProperities(e3dBytes, 152, 4)
        }


        return e3dModel; // Zwracamy gotowca
    }

    int intSubmodelProperities(byte[] bytes, int Byte, int size)
    {
        // od 12 zaczyna sie sub0
        int i = 12 + Byte;
        int toReturn = 0;
        while (i < (i + size))
        {
            toReturn += bytes[12 + i];
            i++;
        }
        return toReturn;
    }
    float floatSubmodelProperities(byte[] bytes, int Byte, int size)
    {
        // od 12 zaczyna sie sub0
        int i = 12 + Byte;
        float toReturn = 0f;
        while (i < (i + size))
        {
            toReturn += bytes[12 + i];
            i++;
        }
        return toReturn;
    }
}
