using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]

public class generator_siatki : MonoBehaviour
{
    public int szerokość = 200;  //szerokość siatki (oś X)
    public int długość = 200;  //długość siatki (oś Z)
    public string przesada;
    public string input_heightmap;
    public string output_SC;
    public int mnożnik;
    public Text Ostrzeżenia;
    public InputField multipler;
    public InputField heightmap_path;
    public InputField ScFile_path;
    public InputField Xsiatki;
    public InputField Zsiatki;
    int X = 0;
    int Z = 0;
    Mesh mesh;
    Vector3[] vertices;
    Vector3[] points;
    int[] triangles;
    bool genSCN = false;
    bool genSC = false;
    bool genMesh = false;

    public string output;
    public string stack;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        output = logString;
        stack = stackTrace;
    }

    IEnumerator Start()
    {
        przesada = "Brak Ostrzeżeń";
        begin:
        Ostrzeżenia.text = przesada;
        if (null != output)
        {
            przesada = output;
            Ostrzeżenia.text = przesada;
            yield return new WaitForSeconds(3);
            output = null;
            przesada = "Brak Ostrzeżeń";
            Ostrzeżenia.text = przesada;

        }
        if (szerokość * długość > 65536)
        {
            przesada = "Łooo panie, za duże. We no zmniejsz, bo TD2 zdechnie";
            Ostrzeżenia.text = przesada;
            yield return new WaitForSeconds(3);
        }
        if (genSC == true)
        {
            genSC = false;
            przesada = "Wygenerowano plikSC";
            Ostrzeżenia.text = przesada;
            yield return new WaitForSeconds(3);
            przesada = "Brak Ostrzeżeń";
            Ostrzeżenia.text = przesada;
        }
        if (genSCN == true)
        {
            genSCN = false;
            przesada = "Wygenerowano plikSCN";
            Ostrzeżenia.text = przesada;
            yield return new WaitForSeconds(3);
            przesada = "Brak Ostrzeżeń";
            Ostrzeżenia.text = przesada;
        }
        if (genMesh == true)
        {
            genMesh = false;
            przesada = "Wygenerowano siatkę podglądu";
            Ostrzeżenia.text = przesada;
            yield return new WaitForSeconds(3);
            przesada = "Brak Ostrzeżeń";
            Ostrzeżenia.text = przesada;
        }



        yield return new WaitForSeconds(0.1f);
        goto begin;
    }





    // Update is called once per frame
    void Update()
    {


    }



   public void GenerateScFile() //Tworzy plik .sc dla TD2
    {
        szerokość = int.Parse(Xsiatki.text);
        długość = int.Parse(Zsiatki.text);
        mnożnik = int.Parse(multipler.text);
        input_heightmap = heightmap_path.text;
        output_SC = ScFile_path.text;
        genSC = true;
        byte[] imagedata = File.ReadAllBytes(input_heightmap);
        Texture2D tex = new Texture2D(1, 1);
        tex.LoadImage(imagedata);
        float Y = tex.GetPixel(Z, X).grayscale;
        using (StreamWriter plik = new StreamWriter(output_SC))  //tworzy i otwiera nowy plik sc
        {
            plik.WriteLine("scv023;test;5;;https://td2.info.pl/scenerie/;");
            plik.WriteLine("shv001");
            plik.WriteLine("MainCamera;0;0;-4.508744;-17.88779;491.7646;50.00821;65.3754;267.0764");
            plik.WriteLine("CameraHome;0;0;170;470;0;67.00001;270;-1.311041E-05");  //wpisanie do pliku domyślnych linijek dla edytora TD2

            for (int i = 0; i < długość; i++)
            {
                X = 0;

                for (int i2 = 0; i2 < szerokość; i2++)
                {
                    Y = tex.GetPixel(Z, X).grayscale;
                    plik.WriteLine("TerrainPoint;;#TerrainPoint;" + X + ";" + Y * mnożnik + ";" + Z + ";0;0;0;0;");  //Algorytm zajmujący się generowaniem terrainpointów do pliku SC
                    X++;

                }

                Z++;

            }
            Z = 0;
            plik.Close();
        }


    }
    public void GenerateScnFile() //Tworzy plik .sc dla TD2
    {
        szerokość = int.Parse(Xsiatki.text);
        długość = int.Parse(Zsiatki.text);
        mnożnik = int.Parse(multipler.text);
        input_heightmap = heightmap_path.text;
        output_SC = ScFile_path.text;
        genSCN = true;
        byte[] imagedata = File.ReadAllBytes(input_heightmap);
        Texture2D tex = new Texture2D(1, 1);
        tex.LoadImage(imagedata);
        float Y = tex.GetPixel(Z, X).grayscale;
        using (StreamWriter plik = new StreamWriter(output_SC))  //tworzy i otwiera nowy plik SCN
        {
            plik.WriteLine("sky skydome_clouds.t3d endsky");
            plik.WriteLine("atmo 0 0 0 1000 2000 0 0 00 endatmo");
            plik.WriteLine("time 10:30 06:30 19:00 endtime");
            plik.WriteLine("config movelight 0 endconfig");   //wpisanie do pliku domyślnych linijek dla pliku SCN


            plik.WriteLine("node -1 0 start track normal 100.0 1.435 0.15 25.0 20 0 flat vis ");
            plik.WriteLine("rail_screw_used1 6 1435mm/tpbps-new2 0.2 0.5 1.1");
            plik.WriteLine("0 0.2 0  0  //point 1");
            plik.WriteLine("0.0 0.0 0.0  //control vector 1");
            plik.WriteLine("0 0 0  //control vector 2");
            plik.WriteLine("0 0.2 236.111  0.0  //point 2");
            plik.WriteLine("0");
            plik.WriteLine("event2 stacja1_a_sem_info");
            plik.WriteLine("endtrack");
            plik.WriteLine("trainset none start 20 0");
            plik.WriteLine(@"node 0 0 WMB10-6457 dynamic PKP\WMB10_V1 WMB10-6457 WMB10  0 headdriver -119 0 enddynamic");
            plik.WriteLine("endtrainset");                                               //wpisanie do pliku linijek umieszczających na scenerii pojazd i kawałek toru


            points = new Vector3[długość * szerokość];
            for (int i1 = 0, i = 0; i1 < długość; i1++)  //pętle ładujące do pamięci dane o wysokości danych punktów terenu
            {
                X = 0;
                for (int i2 = 0; i2 < szerokość; i2++)
                {
                    Y = mnożnik*tex.GetPixel(Z, X).grayscale;
                    points[i].x = X;
                    points[i].y = Y;
                    points[i].z = Z;
                    X++;
                    i++;
                }                                       
                Z++;
            }
            Z = 0;



            int point_number = 0;
            for (int i = 0; i < długość; i++)  //Algorytm generujący trójkąty terenu i wpisujący je do pliku SCN
            {
                for (int i2 = 0; i2 < szerokość; i2++)
                {
                    if (point_number >= długość*szerokość-szerokość*2)
                    {
                        goto interrupt;
                    }


                    plik.WriteLine("node -1 0 none triangles material ambient: 255 255 255 diffuse: 255 255 255 specular: 20 20 20 endmaterial grass"); 
                           //domyślne linijki dla trójkątów terenu


                    X = i2 + 0;
                    Y = (int)points[point_number + 0].y;
                    Z = i + 0;
                    plik.WriteLine(X + " " + Y + " " + Z + " " + "0.0 1.0 0.0  550.887 1130.69 end");



                    X = i2 + 0; 
                    Y = (int)points[point_number + szerokość].y;
                    Z = i + 1;
                    plik.WriteLine(X+" "+Y+" "+Z+" "+ "0.0 2.0 0.0  335.016 1116.46 end");


                    X = i2 + 1;
                    Y = (int)points[point_number + 1].y;
                    Z = i + 0;
                    plik.WriteLine(X + " " + Y + " " + Z + " " + "0.0 1.0 0.0  335.016 841.902");  //właściwe punkty definiujące trójkąt terenu

                    plik.WriteLine("endtri"); //znak oznaczający koniec danych danego trójkąta 


                    plik.WriteLine("node -1 0 none triangles material ambient: 255 255 255 diffuse: 255 255 255 specular: 20 20 20 endmaterial grass");
                    //domyślne linijki dla trójkątów terenu


                    X = i2 + 0;
                    Y = (int)points[point_number + szerokość].y;
                    Z = i + 1;
                    plik.WriteLine(X + " " + Y + " " + Z + " " + "0.0 1.0 0.0  550.887 1130.69 end");



                    X = i2 + 1;
                    Y = (int)points[point_number + szerokość+ 1].y;
                    Z = i + 1;
                    plik.WriteLine(X + " " + Y + " " + Z + " " + "0.0 2.0 0.0  335.016 1116.46 end");


                    X = i2 + 1;
                    Y = (int)points[point_number + 1].y;
                    Z = i + 0;
                    plik.WriteLine(X + " " + Y + " " + Z + " " + "0.0 1.0 0.0  335.016 841.902");  //właściwe punkty definiujące trójkąt terenu

                    plik.WriteLine("endtri"); //znak oznaczający koniec danych danego trójkąta

                    point_number++;
                    X = 0;
                    Y = 0;
                    Z = 0;
                    
                }

            }

            interrupt:


            plik.Close();
        }
    }


   public void GenerateMesh() //Generuje siatkę dla podglądu
    {
        szerokość = int.Parse(Xsiatki.text);
        długość = int.Parse(Zsiatki.text);
        mnożnik = int.Parse(multipler.text);
        input_heightmap = heightmap_path.text;
        genMesh = true;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }


    void CreateShape() //algorytm do generowania siatki
    {
        byte[] imagedata = File.ReadAllBytes(input_heightmap);
        Texture2D tex = new Texture2D(1, 1);
        tex.LoadImage(imagedata);
        float Y = tex.GetPixel(Z, X).grayscale;
        vertices = new Vector3[(szerokość + 1) * (długość + 1)];
        X = 0;
        for (int i = 0, z = 0; z <= długość; z++)
        {
            for (int x = 0; x <= szerokość; x++)
            {
                Y = tex.GetPixel(X, Z).grayscale;
                vertices[i] = new Vector3(X+20, Y * mnożnik, Z+20);
                i++;
                X++;


            }
            Z++;
            X = 0;
        }
        Z = 0;
        triangles = new int[długość * szerokość * 6];
        int vert = 0;
        int tris = 0;
        for (int i = 0; i < długość; i++)
        {
            for (int i2 = 0; i2 < szerokość; i2++)
            {

                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + szerokość + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + szerokość + 1;
                triangles[tris + 5] = vert + szerokość + 2;
                tris += 6;
                vert++;

            }
            vert++;
        }




    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }



}
