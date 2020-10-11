using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_movement : MonoBehaviour
{
    // Co każdy frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            transform.Translate(0, 0, 1);
        }
        if (Input.GetKey("s"))
        {
            transform.Translate(0, 0, -1);
        }
        if (Input.GetKey("a"))
        {
            transform.Translate(-1, 0, 0);
        }
        if (Input.GetKey("d"))
        {
            transform.Translate(1, 0, 0);
        }
        if (Input.GetKey("q"))
        {
            transform.Translate(0, -1, 0);
        }
        if (Input.GetKey("e"))
        {
            transform.Translate(0, 1, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate (0, -1, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate (0, 1, 0);
        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

    }
}
