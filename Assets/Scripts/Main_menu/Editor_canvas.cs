using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;

public class Editor_canvas : MonoBehaviour
{
    public string path;
    public GameObject scenery_button;
    public Transform Scroll_content;
    public string scn_name;


    // Start is called before the first frame update
    void Start()
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        if (dir.Exists)
        {
            foreach (var file in dir.GetFiles())
            {
                if (file.Name.Contains(".scn"))
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

}
