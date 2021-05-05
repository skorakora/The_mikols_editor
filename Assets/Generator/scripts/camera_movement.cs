using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class camera_movement : MonoBehaviour
{
    // Co każdy frame
    private float vel;
    void Update()
    {
        
        if (Input.GetKey("w"))
        {
            vel = (float) GameObject.Find("VelSlider").GetComponent<Slider>().value; //Pobieramy wartosc slidera po nazwie 
            transform.Translate(0, 0, 1*vel); //mnożymy przez wartość slidera (min. 0.01 max 2)
            //krótka adnotacja: pobranie wartości tylko wtedy gdy klikniemy guzik wpłynie na optymalizację "bez niczego"
        }
        if (Input.GetKey("s"))
        {
            vel = (float)GameObject.Find("VelSlider").GetComponent<Slider>().value;
            transform.Translate(0, 0, -1*vel);
        }
        if (Input.GetKey("a"))
        {
            vel = (float)GameObject.Find("VelSlider").GetComponent<Slider>().value;
            transform.Translate(-1*vel, 0, 0);
        }
        if (Input.GetKey("d"))
        {
            vel = (float)GameObject.Find("VelSlider").GetComponent<Slider>().value;
            transform.Translate(1*vel, 0, 0);
        }
        if (Input.GetKey("q"))
        {
            vel = (float)GameObject.Find("VelSlider").GetComponent<Slider>().value;
            transform.Translate(0, -1*vel, 0);
        }
        if (Input.GetKey("e"))
        {
            vel = (float)GameObject.Find("VelSlider").GetComponent<Slider>().value;
            transform.Translate(0, 1*vel, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            vel = (float)GameObject.Find("VelSlider").GetComponent<Slider>().value;
            transform.Rotate (0, -1*vel, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            vel = (float)GameObject.Find("VelSlider").GetComponent<Slider>().value;
            transform.Rotate (0, 1*vel, 0);
        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

    }
}
