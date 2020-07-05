using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{

    public GameObject cursor;
    public GameObject trawnik1;
    public GameObject trawnik2;
    public GameObject trawnik3;
    public GameObject trawnik4;
    Vector3 cursor_position;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            cursor_position = hit.point;
        }

        cursor.transform.position = cursor_position;

        if (Input.GetKey (KeyCode.Alpha1))
        {
            if (Input.GetMouseButtonUp(0))
            {
                Instantiate(trawnik1, cursor_position, Quaternion.identity);
            }
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            if (Input.GetMouseButtonUp(0))
            {
                Instantiate(trawnik2, cursor_position, Quaternion.identity);
            }
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            if (Input.GetMouseButtonUp(0))
            {
                Instantiate(trawnik3, cursor_position, Quaternion.identity);
            }
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            if (Input.GetMouseButtonUp(0))
            {
                Instantiate(trawnik1, cursor_position, Quaternion.identity);
            }
        }
    }
}
