using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;


[RequireComponent(typeof(MeshFilter))]

public class Terrain_gen : MonoBehaviour
{
    public string przesada;
    public Text Ostrzeżenia;
    public string output;
    public string stack;
    public InputField heightmap_path;
    public InputField nazwa_pliku;
    public GameObject segment;
    string line;
    int Xpos = 0;
    int Zpos = 0;
    bool gen_scenery = false;
    // Start is called before the first frame update
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
        if (gen_scenery == true)
        {
            gen_scenery = false;
            przesada = "Wygenerowano teren";
            Ostrzeżenia.text = przesada;
            yield return new WaitForSeconds(3);
            przesada = "Brak Ostrzeżeń";
            Ostrzeżenia.text = przesada;
        }
        yield return new WaitForSeconds(0.1f);
        goto begin;
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        output = logString;
        stack = stackTrace;
    }

    // Update is called once per frame
    public void GenScenery()
    {
        System.IO.StreamReader plik = new System.IO.StreamReader(heightmap_path.text+nazwa_pliku.text);
        line = plik.ReadLine();
        if (line == "[terrain_data]")
        {
            begin:
            line = plik.ReadLine();
            if (line == "[end_terrain_data]")
            {
               
                goto stop_read_terrain_data;

            }
            string[] data = line.Split(',');
            Xpos = int.Parse(data[0]);
            Zpos = int.Parse(data[1]);
            Segment_gen generator_siatki = segment.GetComponent<Segment_gen>();
            generator_siatki.szerokość = 200;
            generator_siatki.długość = 200;
            generator_siatki.mnożnik = 30;
            generator_siatki.input_heightmap = heightmap_path.text + @"Terrain_data\" + Xpos + ";" + Zpos + ".png";
            generator_siatki.GenerateMesh();
            Instantiate(segment, new Vector3(Xpos * 200, 0, Zpos * 200), Quaternion.identity);
            goto begin;





        }
        
        stop_read_terrain_data:
        plik.Close();
        gen_scenery = true;
    }



}
