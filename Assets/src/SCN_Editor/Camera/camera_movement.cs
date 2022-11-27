using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_movement : MonoBehaviour
{
    public GameObject cursor;
    public Camera MainCamera;

    GameObject trackpoint;
    bool EditTrack = false;

    void Start()
    {
        cursor = Instantiate(cursor, new Vector3(0, 0, 0), Quaternion.identity);  //spawns cursor on the map 
        
    }
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

        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast (ray,out hitInfo))
        {
            cursor.transform.position = hitInfo.point;
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            EditTrack = false;
            trackpoint.GetComponent<SphereCollider>().enabled = true;
        }
        if (EditTrack == true)
        {

        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray1 = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo1;
            if (Physics.Raycast(ray, out hitInfo1))
            {

            }
        }
    }


}
