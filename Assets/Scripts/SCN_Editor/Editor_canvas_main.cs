using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Editor_canvas_main : MonoBehaviour
{

    void Start()
    {
       Main main = new Main();
       main.Deserialize(Globals.SCN_path);
    }
    // Powrót do głównego menu
    public void Back_to_main_menu()
    {
        SceneManager.LoadScene("Main_menu");
    }
}
