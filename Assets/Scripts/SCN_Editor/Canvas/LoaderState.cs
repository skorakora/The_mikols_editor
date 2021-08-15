using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderState : MonoBehaviour
{
    public GameObject canvas;
    Editor_canvas_main editor_Canvas_Main;
    void Start()
    {
        editor_Canvas_Main = canvas.GetComponent<Editor_canvas_main>();
    }
    void Update()
    {
        if (!Globals.IsSCNLoaderActive())
        {
            editor_Canvas_Main.EnableEditMode();
        }
        
    }
}
