using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class Editor_canvas_main : MonoBehaviour
{
    public GameObject _camera;
    public GameObject[] music;

    public GameObject LoaderMode;
    public GameObject EditMode;

    public Slider Slider;

    private readonly System.Random rnd = new System.Random();
    void Start()
    {
        SetProgress(0);
        LoaderMode.SetActive(true);
        EditMode.SetActive(false);
        int rando = rnd.Next(0, 3);
        music[rando].SetActive(true);

    }

    public void SetProgress(float progress)
    {
        Slider.value = progress;

    }
    public void EnableEditMode()
    {
        DisableMusic();
        LoaderMode.SetActive(false);
        EditMode.SetActive(true);
    }

    // Powrót do głównego menu
    public void Back_to_main_menu()
    {

        DisableMusic();
        SceneManager.LoadScene("Main_menu");

    }

    private void DisableMusic()
    {
        for (int i = 0; i < music.Length; i++)
        {
            music[i].SetActive(false);
        }
    }
}
