using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class Model_loader : MonoBehaviour
{
    float[,] macierz_transformacji;
    GameObject Parent;
    string Parent_name;
    string data;
    string[] subdata;
    float VertX;
    float VertY;
    float VertZ;
    float U;
    float V;
    float X;
    float Y;
    float Z;
    int found = 0;
    int NumVerts;
    Mesh mesh;
    Vector2[] uv;
    Vector3[] vertices;
    Vector3[] points;
    Vector3[] normals;
    int[] triangles;
    GameObject objToSpawn;
    StreamReader plik;
    void Start()
    {
        macierz_transformacji = new float[4, 4];
        StreamReader plik = new StreamReader(@"C:\ProgramData\OnRailTeam\TrainDriver2\Custom\110ab.t3d");
        while (plik.EndOfStream == false)
        {
            data = plik.ReadLine();
            if (data.StartsWith("Parent:"))
            {
                found = data.IndexOf(": ");
                Parent_name = data.Substring(found + 2);
                Parent = GameObject.Find(Parent_name);

            }
            if (data.StartsWith("Name:")) //wykrywa słowo kluczowe w pliku "name" i ładuje jego wartość do zmiennej
            {
                found = data.IndexOf(": ");
                objToSpawn = new GameObject(data.Substring(found + 2));  //spawnuje nowy obiekt o nazwie zdefinowanej za name
                objToSpawn.AddComponent<MeshFilter>();
                objToSpawn.AddComponent<MeshRenderer>();
                objToSpawn.transform.parent = Parent.transform;
            }
            if (data.StartsWith("Transform:"))  //ładuje dane do macierzy transformacji
            {
                for (int i1 = 0; i1 < 4; i1++)
                {
                    int i = 0;
                    data = plik.ReadLine();
                    subdata = data.Split(' ');
                    while (string.IsNullOrEmpty(subdata[i])) //do poprawy - na razie nie wiem jak pomijać te yebane spacje
                    {
                        i++;
                    }
                    macierz_transformacji[0, i1] = float.Parse(subdata[i]);
                    macierz_transformacji[1, i1] = float.Parse(subdata[i + 1]);
                    macierz_transformacji[2, i1] = float.Parse(subdata[i + 2]);
                    macierz_transformacji[3, i1] = float.Parse(subdata[i + 3]);
                }
            }
            if (data.StartsWith("NumVerts:"))  //ładuje siatkę modelu
            {
                mesh = new Mesh();
                objToSpawn.GetComponent<MeshFilter>().mesh = mesh; 
                found = data.IndexOf(": ");
                NumVerts = int.Parse(data.Substring(found + 2));
                vertices = new Vector3[NumVerts];
                normals = new Vector3[NumVerts];
                triangles = new int[NumVerts];
                uv = new Vector2[NumVerts];
                int i = 0;
                for (int i1 = 0; i1 < NumVerts / 3; i1++)
                {


                    data = plik.ReadLine();
                    while (string.IsNullOrEmpty(data))
                    {
                        data = plik.ReadLine();
                    }
                    for (int i2 = 0; i2 < 3; i2++)
                    {
                        data = plik.ReadLine();
                        subdata = data.Split(null);
                        VertX = float.Parse(subdata[0]);
                        VertY = float.Parse(subdata[1]);
                        VertZ = float.Parse(subdata[2]);
                        int leng = subdata.Length;
                        X = (VertX * macierz_transformacji[0, 0]) + (VertY * macierz_transformacji[0, 1]) + (VertZ * macierz_transformacji[0, 2]) + (1 * macierz_transformacji[0, 3]);
                        Y = (VertX * macierz_transformacji[1, 0]) + (VertY * macierz_transformacji[1, 1]) + (VertZ * macierz_transformacji[1, 2]) + (1 * macierz_transformacji[1, 3]);
                        Z = (VertX * macierz_transformacji[2, 0]) + (VertY * macierz_transformacji[2, 1]) + (VertZ * macierz_transformacji[2, 2]) + (1 * macierz_transformacji[2, 3]);
                        vertices[i] = new Vector3(-X,Z,Y);
                        V = float.Parse(subdata[leng - 2]);
                        U = float.Parse(subdata[leng - 3]);
                        normals[i] = Vector3.right;
                        triangles[i] = i;
                        uv[i] = new Vector2(U, V);
                        i++;
                    }

                }

                UpdateMesh();
            }
        }
        plik.Close();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;
    }

}
