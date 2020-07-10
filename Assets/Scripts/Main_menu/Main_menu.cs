using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class Main_menu : MonoBehaviour
{
    public GameObject Main_buttons;
    public GameObject Editor_menu;
    public GameObject Settings_menu;
    string Settings_save_path;
    // Start is called before the first frame update

    void Start()
    {
        Main_buttons.SetActive(true);
        string settings_save_dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\The_mikols";
        Settings_save_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\The_mikols\settings.xml";
        print("szukam folderu: " + settings_save_dir);
        if (Directory.Exists(settings_save_dir))
        {
            print("no i kurwa Pan Paweł - folder jest");
            if (File.Exists(Settings_save_path))
            {
                StreamReader plik = new StreamReader(Settings_save_path);
                string data;
                data = plik.ReadLine();
                if (data == "[Settings_save_file]")
                {
                    return;
                }
                else
                {
                    Create_settings_file();
                }
            }
            else
            {
                Create_settings_file();
            }
        }
        else
        {

            print("no kuurła, panie prezesie, nie znaleziono folderu/pliku");
            print("trza stworzyć ten yebany folder");
            Directory.CreateDirectory(settings_save_dir);
            print("stworzono folder");
            Create_settings_file();
        }

    }

    void Create_settings_file()
    {
        StreamWriter plik = new StreamWriter(Settings_save_path);
        plik.WriteLine("[Settings_save_file]");
        plik.WriteLine("SCN_Path;" + Application.dataPath + @"\scenery");
        plik.Close();
    }
    public void Aplicationquit()
    {
        Application.Quit();
    }
    public void Mainmenu()
    {
        Main_buttons.SetActive(true);
        Editor_menu.SetActive(false);
        Settings_menu.SetActive(false);

    }

    public void Editor()
    {
        Main_buttons.SetActive(false);
        Editor_menu.SetActive(true);
    }
    public void Settings()
    {
        Main_buttons.SetActive(false);
        Settings_menu.SetActive(true);
    }
    public void Save_settings(string SCN_looking_path)
    {
        File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\The_mikols\settings.xml");
        StreamWriter plik = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\The_mikols\settings.xml");
        plik.WriteLine("[Settings_save_file]");
        plik.WriteLine(SCN_looking_path);
        plik.Close();

    }
}
