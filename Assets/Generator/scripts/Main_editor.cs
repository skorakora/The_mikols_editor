using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class Main_editor : MonoBehaviour
{
    public string scn_path;
    string scn_data;
    string scn_folder_path;
    string[] scn_subdata;
    List<Vector3> Vertices = new List<Vector3>();
    List<int> Triangles = new List<int>();
    int triangle_counter = 0;
    Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        scn_path = Static_data_class.SCN_path;
        scn_folder_path = Static_data_class.SCN_folder_path;
        StreamReader scenery_file = new StreamReader(scn_path);      
        while (scenery_file.EndOfStream == false)
        {
            scn_data = scenery_file.ReadLine();
            if(scn_data.StartsWith("include"))
            {
                scn_subdata = scn_data.Split(new char[] { '\n', '\r', '\t', ' '}, StringSplitOptions.RemoveEmptyEntries);
                if (scn_subdata[1].Contains("teren"))
                {
                    StreamReader include_file = new StreamReader(scn_folder_path + @"\" + scn_subdata[1]); //to docelowo zostanie usunięte, jak uda się ustandaryzować metodę zapisu
                    while (include_file.EndOfStream == false)
                    {
                        scn_data = include_file.ReadLine();
                        if (scn_data.StartsWith("node"))
                        {
                            scn_subdata = scn_data.Split(new char[] { '\n', '\r', '\t', ' ', ',', ';', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                            if (scn_subdata[4] == "triangles")
                            {
                                scn_data = include_file.ReadLine();
                                while (false == scn_data.Contains("endtri"))
                                {
                                    scn_subdata = scn_data.Split(new char[] { '\n', '\r', '\t', ' ', ',', ';', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                                    Vertices.Add(new Vector3(float.Parse(scn_subdata[0]), float.Parse(scn_subdata[1]), float.Parse(scn_subdata[2])));
                                    Triangles.Add(triangle_counter);
                                    scn_data = include_file.ReadLine();
                                    triangle_counter++;
                                }

                            }


                        }
                    }
                    include_file.Close();
                }


            }
            if (scn_data.StartsWith("node"))
            {
                scn_subdata = scn_data.Split(new char[] { '\n', '\r', '\t', ' ', ',', ';', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                if (scn_subdata[4] == "triangles")
                {
                    scn_data = scenery_file.ReadLine();
                    while (false == scn_data.Contains("endtri"))
                    {
                        scn_subdata = scn_data.Split(new char[] { '\n', '\r', '\t', ' ', ',', ';', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                        Vertices.Add(new Vector3(float.Parse(scn_subdata[0]), float.Parse(scn_subdata[1]), float.Parse(scn_subdata[2])));
                        Triangles.Add(triangle_counter);
                        scn_data = scenery_file.ReadLine();
                        triangle_counter++;
                    }

                }


            }

        }
        scenery_file.Close();
        Mesh mesh = new Mesh();
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.Clear();
        mesh.vertices = Vertices.ToArray();
        mesh.triangles = Triangles.ToArray();
        Vector3[] normals = mesh.normals;
        for (int i = 0; i < normals.Length; i++)
            normals[i] = Vector3.right;

        mesh.normals = normals;
        mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
