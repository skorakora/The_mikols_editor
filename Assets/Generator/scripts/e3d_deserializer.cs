using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class e3d_deserializer : MonoBehaviour
{
    byte[] e3dBytes;
    string e3dContent;
    public GameObject deserialize_E3D(string path)
    {
        // Odczytywanie pliku i tworzenie nowego GameObjectu
        e3dBytes = File.ReadAllBytes(path);
        using (StreamReader sr = new StreamReader(path))
        {
            e3dContent = sr.ReadToEnd();
        }
        string gameObjectName = generateName(16);
        GameObject model = new GameObject(gameObjectName);
        model.AddComponent<MeshFilter>();
        model.AddComponent<MeshRenderer>();
        Debug.Log("Created model! Name: " + gameObjectName);
        Debug.Log("Loading binary format 3d model data from \"" + path + "\"...");

        Int32 type  ;
        Int32 size;

        Debug.Log("Finished loading 3d model data from \"" + path + "\"");

        return model;
    }
    // deserialize little endian uint32
    public static uint ld_uint32(byte[] s)
    {
        byte[] buf = new byte[4];
        //s.Read(buf, 4);
        
        uint v = (uint)((buf[3] << 24) | (buf[2] << 16) | (buf[1] << 8) | buf[0]);
        //C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
        return (v);
    }

    string generateName(int length)
    {
        // creating a StringBuilder object()
        StringBuilder str_build = new StringBuilder();
        System.Random random = new System.Random();

        char letter;

        for (int i = 0; i < length; i++)
        {
            double flt = random.NextDouble();
            int shift = Convert.ToInt32(Math.Floor(25 * flt));
            letter = Convert.ToChar(shift + 65);
            str_build.Append(letter);
        }
        return str_build.ToString();
    }
}
