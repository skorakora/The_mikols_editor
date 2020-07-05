using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Main_menu : MonoBehaviour
{
    public GameObject Main_buttons;
    // Start is called before the first frame update

    void Start()
    {
        Main_buttons.SetActive(true);
    }
    public void Aplicationquit()
    {
        Application.Quit();
    }

    public void Editor()
    {
        Main_buttons.SetActive(false);
    }
}
