﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
        int sliceLength = e3dBytes[4]; // długość kromki
        // E3D0xxxxSUB0 jest juz przeanalizowane. Teraz analizujemy kromke SUB0

        #region SUBx parameter reader [DISABLED]
        /*
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
        }; */
        #endregion

        // Tu stworzymy liste submodeli o nazwie SUB
        int SUBxes = Regex.Matches(e3dContent, "SUB[0-9]", RegexOptions.IgnoreCase).Count;
        int i = 0;
        List<SUBx> SUB = new List<SUBx>(); // Komórki submodeli
        while (i < SUBxes)
        {
            SUB.Add(submodelProperities(i - 1, e3dBytes));
            i++;
        }

        // Kromki submodeli (SUB) mamy juz za sobą
        // Teraz czas na odczyt geometrii (VNT0)
        // Odczytay to do listy VNT


        // Tu trzeba bedzie odczytac wartosci VNT do tablicy trojkatow e3dTriangle -- Dla pruka

        List<e3dTriangle> e3dTriangles = new List<e3dTriangle>();

        #region Ogarnianie kromek VNT
        // XYZIJKUV i to sie powtarza i to definiuje ci jeden punkt w swiecie 3D
        // Gdy powyzsza konfiguracja powtorzy sie 3 razy to dodajesz do listy e3dTriangles
        // pobrane wartosci
        List<Vector3> position3D = new List<Vector3>();
        List<Vector2> positionUV = new List<Vector2>();
        i = 0;
        int przesuniecie = 0; // wez sobie to wylicz tyle ci zostawiam
        int triangles = przesuniecie / 8;
        while(i < triangles)
        {
            float[] xyz = { e3dBytes[0 + przesuniecie],
                e3dBytes[1 + przesuniecie], 
                e3dBytes[2 + przesuniecie] };
            position3D.Append<Vector3>(new Vector3(xyz[0], xyz[1], xyz[2]));
            positionUV.Add(new Vector2(e3dBytes[7 + przesuniecie],
                e3dBytes[8 + przesuniecie]));
            przesuniecie = przesuniecie + 8;
            i++;
        }
        // Teraz stworzymy trojkąty

        int trianglesCount = position3D.Count / 3; // Liczba trojkątów
        int currentTriangle = 0;
        List<e3dTriangle> trojkaty = new List<e3dTriangle>();
        while(currentTriangle < trianglesCount)
        {
            // To-do przesuniecia tablicy
            trojkaty.Add(new e3dTriangle(position3D[currentTriangle + 0], position3D[currentTriangle + 1],
                position3D[currentTriangle + 2], //
                positionUV[currentTriangle + 0], positionUV[currentTriangle + 1], positionUV[currentTriangle + 2]));
        }
        List<Mesh> submodelMeshes = new List<Mesh>();
        i = 0;

        // Tu trza ogarnac metode
        #endregion
        // lista trojkaty zawiera wszystkie triangles z danego VNTx
        return e3dModel; // Zwracamy gotowca
    }

    // To odczytuje wartość submodelu w binarce
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

    // To stworzy element SUBx
    // submodel (int SUBx)
    // zawartość bajtów (e3dbytes)
    SUBx submodelProperities(int SUBx, byte[] e3dBytes)
    {
        if (SUBx > 0)
        {
            SUBx = SUBx * 320 + (64 * SUBx - 1);
        }
        else if (SUBx == 0)
        {
            SUBx = SUBx * 256;
        }
        SUBx subx = new SUBx();
        #region SUBx parameter reader
        // Numer submodelu następnego, -1 gdy brak
        int nextSubmodelNumber = intSubmodelProperities(e3dBytes, SUBx + 0, 4);

        // Numer pierwszego submodelu potomnego, -1 gdy brak.
        int firstChildSubmodel = intSubmodelProperities(e3dBytes, SUBx + 4, 4);

        // Typ submodelu. Możliwe jest użycie wartości takich jak GL_TRIANGLES.
        // Wartości powyżej 256 oznaczają typy specjalne.
        int submodelType = intSubmodelProperities(e3dBytes, SUBx + 8, 4);

        // 	Numer nazwy. Nazwy są numerowane od zera. 
        // Wartość -1 oznacza brak nazwy (nie jest potrzebna, jeśli submodel nie jest animowany zdarzeniami).
        int submodelName = intSubmodelProperities(e3dBytes, SUBx + 12, 4);

        // Rodzaj animacji.
        int animationType = intSubmodelProperities(e3dBytes, SUBx + 16, 4);

        // Flagi submodelu. Bity 0..15 dotyczą danego submodelu, 
        // bity 16..31 są skumulowanymi bitami 0..7 obiektów następnych oraz potomnych
        // i umożliwiają pomijanie całych gałęzi podczas wyświetlania.
        int submodelFlag = intSubmodelProperities(e3dBytes, SUBx + 20, 4);

        // Numer macierzy przekształcenia widoku. 
        // Macierz jest potrzebna do ustalenia osi układu współrzędnych dla animacji. 
        // Jeśli submodel nie będzie animowany, można jego wierzchołki przeliczyć tak, 
        // aby uzyskać macierz jednostkową. W takim przypadku nie jest ona zapisywana do pliku, 
        // a w tym miejscu należy podać wartość -1.
        int vievTransformMatrixNumber = intSubmodelProperities(e3dBytes, SUBx + 24, 4);

        // Ilość wierzchołków. Dla typu GL_TRIANGLES musi być podzielna przez 3, dla GL_LINES parzysta.
        int numberOfVertices = intSubmodelProperities(e3dBytes, SUBx + 28, 4);

        // Numer pierwszego wierzchołka w VNT0 albo pierwszego indeksu wierzchołka w IDX1, IDX2 albo IDX4.
        int firstVertex = intSubmodelProperities(e3dBytes, SUBx + 32, 4);

        // 	Numer materiału. Materiały są numerowane od 1. Wartość 0 oznacza materiał "colored". 
        // Wartości ujemne to numer materiału wymiennej, pierwszy materiał wymienny ma numer -1.
        int materialNumber = intSubmodelProperities(e3dBytes, SUBx + 36, 4);

        // Próg jasności załączania submodelu. Dla wartości 0.0÷1.0 model będzie wyświetlany, 
        // jeśli będzie ciemniej niż podana wartość. Dla wartości -1.0÷0.0 - gdy będzie jaśniej.
        int brightnessToggleThreshold = intSubmodelProperities(e3dBytes, SUBx + 40, 4);

        // Próg zapalenia światła. Świecenie automatyczne zostanie włączone, jeśli będzie ciemniej niż podana wartość 0.0÷1.0.
        int brightnessThreshold = intSubmodelProperities(e3dBytes, SUBx + 44, 4);

        // Kolor RGBA ambient. Nie używany.
        // Mimo wszystko zmienna bedzie
        float[] ambientRGBA = { floatSubmodelProperities(e3dBytes, SUBx + 48, 4),
        floatSubmodelProperities(e3dBytes, SUBx + 52, 4),
        floatSubmodelProperities(e3dBytes, SUBx + 56, 4),
        floatSubmodelProperities(e3dBytes, SUBx + 60, 4)};

        // Kolor RGBA diffuse
        float[] diffuseRGBA = { floatSubmodelProperities(e3dBytes, SUBx + 64, 4),
        floatSubmodelProperities(e3dBytes, SUBx + 68, 4),
        floatSubmodelProperities(e3dBytes, SUBx + 72, 4),
        floatSubmodelProperities(e3dBytes, SUBx + 76, 4)};

        // Kolor RGBA specular. Nie używany.
        // Mimo wszystko zmienna bedzie a co
        float[] spectularRGBA = { floatSubmodelProperities(e3dBytes, SUBx + 80, 4),
        floatSubmodelProperities(e3dBytes, SUBx + 84, 4),
        floatSubmodelProperities(e3dBytes, SUBx + 88, 4),
        floatSubmodelProperities(e3dBytes, SUBx + 92, 4)};

        // Kolor RGBA świecenia. Używany po załączeniu świecenia (selfillum).
        float[] selfIllumRGBA = { floatSubmodelProperities(e3dBytes, SUBx + 96, 4),
        floatSubmodelProperities(e3dBytes, SUBx + 100, 4),
        floatSubmodelProperities(e3dBytes, SUBx + 104, 4),
        floatSubmodelProperities(e3dBytes, SUBx + 108, 4)};

        // Rozmiar linii dla GL_LINES.
        float lineSize = floatSubmodelProperities(e3dBytes, SUBx + 112, 4);

        // Kwadrat maksymalnej odległości widoczności. W większej odległości submodel i jego potomne 
        // nie będą widoczne (np. można je uznać za pomijalnie małe albo są zastąpione fazą LoD).
        float maxViewDistance = floatSubmodelProperities(e3dBytes, SUBx + 116, 4);

        // Kwadrat minimalnej odległości widoczności. W mniejszej odległości submodel i jego potomne
        // nie będą widoczne(np.faza LoD dla oddalenia).
        float minViewDistance = floatSubmodelProperities(e3dBytes, SUBx + 120, 4);

        // Parametry światła dla punktów świecących:
        // fNearAttenStart, fNearAttenEnd, bUseNearAtten (bool), iFarAttenDecay (int),
        // fFarDecayRadius, fCosFalloffAngle, fCosHotspotAngle, fCosViewAngle.
        float[] lightParameters =
        {
            floatSubmodelProperities(e3dBytes, SUBx + 124, 4),
            floatSubmodelProperities(e3dBytes, SUBx + 128, 4),
            floatSubmodelProperities(e3dBytes, SUBx + 132, 4),
            floatSubmodelProperities(e3dBytes, SUBx + 136, 4),
            floatSubmodelProperities(e3dBytes, SUBx + 140, 4),
            floatSubmodelProperities(e3dBytes, SUBx + 144, 4),
            floatSubmodelProperities(e3dBytes, SUBx + 148, 4),
            floatSubmodelProperities(e3dBytes, SUBx + 152, 4)
        };
        #endregion
        return subx;
    }
}
public class SUBx
{
    #region Zmienne klasy
    public int nextSubmodelNumber { get; set; }
    public int firstChildSubmodel { get; set; }
    public int submodelType { get; set; }
    public int submodelName { get; set; }
    public int animationType { get; set; }
    public int submodelFlag { get; set; }
    public int viewTransformMatrixNumber { get; set; }
    public int numberOfVertices { get; set; }
    public int firstVertex { get; set; }
    public int materialNumber { get; set; }
    public int brightnessToggleThreshold { get; set; }
    public int brightnessThreshold { get; set; }
    public float[] ambientRGBA { get; set; }
    public float[] diffuseRGBA { get; set; }
    public float[] spectularRGBA { get; set; }
    public float[] selfIllumRGBA { get; set; }
    public float lineSize { get; set; }
    public float maxViewDistance { get; set; }
    public float minViewDistance { get; set; }
    public float[] lightParameters { get; set; }
    #endregion
    #region Nie uzywane
    /*
    public SUBx(int nextSubmodelNumber, int firstChildSubmodel, int submodelType, int submodelName, int animationType, int viewTransformMatrixNumber, int numberOfVertices, int firstVertex, int materialNumber, int brightnessToggleThreshold, int brightnessTreshold)
    {

    }
    public SUBx()
    {
        // Tylko po to aby nie było zamieszania ze nie dzialają zmienne cholera xD // Psycha siada
    } 
    */
    #endregion
}
// Typ obiektu pod odczytywanie wierzchołków wraz z pozycjami mapy UV
public class e3dTriangle
{
    public Vector3[] vector = new Vector3[3];
    public Vector2[] UV = new Vector2[3];
    public e3dTriangle(float x1, float y1, float z1, float x2, float y2, float z2,
        float x3, float y3, float z3, float uvX_X, float uvX_Y, float uvY_X, float uvY_Y, 
        float uvZ_X, float uvZ_Y)
    {
        vector[0] = new Vector3(x1, y1, z1);
        vector[1] = new Vector3(x2, y2, z2);
        vector[2] = new Vector3(x3, y3, z3);

        Vector2 uvX = new Vector2(uvX_X, uvX_Y);
        Vector2 uvY = new Vector2(uvY_X, uvY_Y);
        Vector2 uvZ = new Vector2(uvZ_X, uvZ_Y);
        UV[0] = uvX;
        UV[1] = uvY;
        UV[2] = uvZ;
    }
    public e3dTriangle(Vector3 a, Vector3 b, Vector3 c, Vector2 aUV, Vector2 bUV, Vector2 cUV)
    {
        vector[0] = a;
        vector[1] = b;
        vector[2] = c;
        UV[0] = aUV;
        UV[1] = bUV;
        UV[2] = cUV;
    }

    public Vector3[] getVector()
    {
        return vector;
    }
    public Vector2[] getUV()
    {
        return UV;
    }
}
