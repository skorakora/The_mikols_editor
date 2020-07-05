using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;



[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Triangulator))]

public class Terrainpoint : MonoBehaviour
{
    Mesh mesh;
    public GameObject trawnik;
    public GameObject drzewo;
    public GameObject terrainpoint;
    string scdata;
    string[] data;
    float X;
    float Y;
    float Z;
    int[] triangles;
    List<Vector3> Vertices = new List<Vector3>();
    List<Vector2> Terrainpoint_data = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        StreamReader plik = new StreamReader(@"C:\Users\skors\Downloads\Parzeczewo.sc");
        while (plik.EndOfStream == false)
        {
            scdata = plik.ReadLine();
            data = scdata.Split(';');
            if (data[0] == "TerrainPoint" )
            {
                Vertices.Add(new Vector3(float.Parse(data[3]), float.Parse(data[4]), float.Parse(data[5])));
                Terrainpoint_data.Add(new Vector2(float.Parse(data[3]), float.Parse(data[5])));
                Instantiate(terrainpoint, new Vector3(float.Parse(data[3]), float.Parse(data[4]), float.Parse(data[5])), Quaternion.identity);
                terrainpoint.name = "Terrainpoint" + data[3] + ";" + data[4] + ";" + data[5];
            }
            if (data[0] == "Misc")
            {
                if (data[2].StartsWith("grass"))
                {
                    Instantiate(trawnik, new Vector3(float.Parse(data[3]), float.Parse(data[4]), float.Parse(data[5])), Quaternion.identity);
                }
                if (data[2].StartsWith("tree"))
                {
                    Instantiate(drzewo, new Vector3(float.Parse(data[3]), float.Parse(data[4]), float.Parse(data[5])), Quaternion.identity);
                }
            }
        }
        plik.Close();
        print("pomyślnie załadowano teren do pamięci");
        Triangulator triangulator = new Triangulator();
        Mesh mesh = new Mesh();
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.Clear();
        mesh.vertices = Vertices.ToArray();
        mesh.triangles = triangulator.TriangulatePolygon(Terrainpoint_data.ToArray());
        Vector3[] normals = mesh.normals;
        for (int i = 0; i < normals.Length; i++)
            normals[i] = Vector3.right;
        
        mesh.normals = normals;
        mesh.RecalculateNormals();
    }
}



