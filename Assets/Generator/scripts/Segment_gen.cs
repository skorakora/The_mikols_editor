using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]


public class Segment_gen : MonoBehaviour
{
    public int szerokość = 200;  //szerokość siatki (oś X)
    public int długość = 200;  //długość siatki (oś Z)
    public string przesada;
    public string input_heightmap;
    public string output_SC;
    public int mnożnik;
    public Text Ostrzeżenia;
    int X = 0;
    int Z = 0;
    Mesh mesh;
    Vector3[] vertices;
    Vector3[] points;
    Vector3[] normals;
    int[] triangles;

    public string output;
    public string stack;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {


    }
    public void GenerateMesh() //Generuje siatkę dla podglądu
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;

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
        normals = new Vector3[(szerokość + 1) * (długość + 1)];
        X = 0;
        for (int i = 0, z = 0; z <= długość; z++)
        {
            for (int x = 0; x <= szerokość; x++)
            {
                Y = tex.GetPixel(X, Z).grayscale;
                vertices[i] = new Vector3(X + 20, Y * mnożnik, Z + 20);
                normals[i] = Vector3.up;
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
        mesh.normals = normals;
    }

}

