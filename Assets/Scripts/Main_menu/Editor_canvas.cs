using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Editor_canvas : MonoBehaviour
{
    public string path;
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
                    print("sceneria:  "+scnname[0]);
                }
            }
        }
        else
        {
            print("ACHTUNG ACHTUNG ERROR.EXE, Nie znaleziono folderu");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
