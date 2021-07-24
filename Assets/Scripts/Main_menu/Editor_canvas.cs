using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class Editor_canvas : MonoBehaviour
{
    public string path;
    public GameObject scenery_button;
    public Transform Scroll_content;
    public string scn_name;
    string Settings_save_path;
    public GameObject scn_list;
    public Text scenery_path_text;

    // Start is called before the first frame update
    void Start()
    {
        var children = new List<GameObject>();
        foreach (Transform child in scn_list.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        Settings_save_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\The_mikols\settings.xml";
        StreamReader plik = new StreamReader(Settings_save_path);
        plik.ReadLine();
        string[] data = plik.ReadLine().Split(';');
        path = data[1];
        Globals.SCN_folder_path = data[1];
        scenery_path_text.text = "Obecnie szukam scenerii w:" + data[1];
        plik.Close();

        DirectoryInfo dir = new DirectoryInfo(path);
        if (dir.Exists)
        {
            foreach (var file in dir.GetFiles())
            {
                if (file.Name.Contains(".scn") && !file.Name.StartsWith("$"))
                {
                    string[] scnname;
                    scnname = file.Name.Split('.');
                    GameObject scenery_button_instatiate = Instantiate(scenery_button, new Vector3(0, 0, 0), Quaternion.identity);
                    scenery_button_instatiate.name = scnname[0];
                    scenery_button_instatiate.transform.SetParent(Scroll_content, false);
                    scenery_button_instatiate.GetComponent<Scenery_button>().scenery_name = scnname[0];

                }
            }
        }
        else
        {
            print("ACHTUNG ACHTUNG ERROR.EXE, Nie znaleziono folderu");
        }
        print(path);
    }

    void Update()
    {
        if (String.IsNullOrEmpty(scn_name))
        {
            return;
        }
        GameObject gm = GameObject.Find(scn_name);
        Scenery_button scnb = gm.GetComponent<Scenery_button>();
        scnb.select = true;

    }

    public void Start_editor()
    {
        SceneManager.LoadScene("Editor");
    }

}
