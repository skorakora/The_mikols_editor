using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]



public class E3d_Testy : MonoBehaviour
{
    List<int> tri = new List<int>();
    List<Vector3> vert = new List<Vector3>();
    byte[] x = new byte[4];
    byte[] y = new byte[4];
    byte[] z = new byte[4];
    int tri_counter = 0;

    public string f_path = "C:\\Program Files (x86)\\Maszyna\\models\\sochaczew\\magazyny.e3d";
    // Start is called before the first frame update
    void Start()
    {


        GetComponent<MeshFilter>().mesh = load_e3d(f_path);
    }

    Mesh load_e3d(string path)
    {
        Mesh siatka = new Mesh();
        int d = 8;
        byte[] data = File.ReadAllBytes(path);
        for (int i = 0; i < data.Length - 8; i++)
        {
            char[] block_name = new char[4];
            for (int i2 = 0; i2 < 4; i2++)
            {
                block_name[i2] = Convert.ToChar(data[d]);
                d++;
                i++;
            }
            Debug.Log(new string(block_name));
            byte[] block_size = new byte[4];
            for (int i2 = 0; i2 < 4; i2++)
            {
                block_size[i2] = data[d];
                d++;
                i++;
            }
            Debug.Log("block size: " + BitConverter.ToInt32(block_size, 0));
            if (new string(block_name) == "VNT0")
            {
                Debug.Log("tu so wierzchołki milordzie:" + BitConverter.ToInt32(block_size, 0));
                for (int i2 = 0; i2 < (BitConverter.ToInt32(block_size, 0) - 8) / 32; i2++)
                {
                    for (int i3 = 0; i3 < 4; i3++)
                    {
                        x[i3] = data[d];
                        d++;
                        i++;
                    }
                    for (int i3 = 0; i3 < 4; i3++)
                    {
                        y[i3] = data[d];
                        d++;
                        i++;
                    }
                    for (int i3 = 0; i3 < 4; i3++)
                    {
                        z[i3] = data[d];
                        d++;
                        i++;
                    }
                    vert.Add(new Vector3(BitConverter.ToSingle(x, 0), BitConverter.ToSingle(y, 0), BitConverter.ToSingle(z, 0)));
                    tri.Add(tri_counter);
                    Debug.Log("załadowano wierzchołek: " + tri_counter);
                    tri_counter++;
                    d = d + 20;
                    i = i + 20;
                }
                continue;
            }
            
            for (int i2 = 0; i2 < BitConverter.ToInt32(block_size, 0) - 8; i2++)
            {
                d++;
                i++;
            }
            Debug.Log("block complete");
        }
        Debug.Log("file complete");

        siatka.vertices = vert.ToArray();
        siatka.triangles = tri.ToArray();
        return siatka;
    }

}