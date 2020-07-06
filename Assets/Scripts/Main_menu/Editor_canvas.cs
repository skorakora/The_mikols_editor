using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Editor_canvas : MonoBehaviour
{
    public string path;
    public GameObject scenery_button;
    public Transform Scroll_content;
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
