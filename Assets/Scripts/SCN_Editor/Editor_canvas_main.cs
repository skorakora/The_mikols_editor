﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Editor_canvas_main : MonoBehaviour
{

    void Start()
    {

    }
    // Powrót do głównego menu
    public void Back_to_main_menu()
    {
        SceneManager.LoadScene("Main_menu");
    }
}
