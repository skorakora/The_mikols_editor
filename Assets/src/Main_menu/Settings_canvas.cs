using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEditor;
using System;

public class Settings_canvas : MonoBehaviour
{
    public InputField scenery_dir;
    public Text scenery_dir_text;
    public GameObject canvas;


    void Start()
    {
        scenery_dir_text.text = "Domyślnie scenerii szukam w:"+ System.IO.Directory.GetCurrentDirectory()+@"\scenery";
    }

    public void Select_scn_path()
    {
        scenery_dir.text = EditorUtility.OpenFolderPanel("Wybierz folder symulatora", "", "") + @"\scenery";

    }

    public void Save_settings()
    {
        StreamWriter plik = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\The_mikols\settings.xml");
        plik.WriteLine("[Settings_save_file]");
        plik.WriteLine("SCN_Path;" + scenery_dir.text);
        plik.Close();
        print("SCN_Path;" + scenery_dir.text);
        Main_menu m_menu = canvas.GetComponent<Main_menu>();
        m_menu.Mainmenu();

    }



}
