using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier_generator : MonoBehaviour
{
    public GameObject o1;
    public GameObject o2;
    public GameObject o3;
    public GameObject o4;
    public Transform punkt1;
    public Transform punkt2;
    public Transform punkt3;
    public Transform punkt4;
    public Transform punkt0;
    float pozycjax;
    float pozycjay;
    float pozycjaz;
    Vector3 pozycja;
    int Name = 0;
    GameObject point;

    // Start is called before the first frame update
    void Start()
    {
        for (float t = 0; t <= 1; t += 0.01f)
        {
            pozycjax = punkt1.position.x * Mathf.Pow(1 - t, 3) + 3 * punkt2.position.x * t * Mathf.Pow(1 - t, 2) + 3 * punkt3.position.x * t * t * (1 - t) + punkt4.position.x * t * t * t;
            pozycjay = punkt1.position.z * Mathf.Pow(1 - t, 3) + 3 * punkt2.position.z * t * Mathf.Pow(1 - t, 2) + 3 * punkt3.position.z * t * t * (1 - t) + punkt4.position.z * t * t * t;
            pozycja = new Vector3(pozycjax, 0, pozycjay);

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = pozycja;
            cube.name = Name.ToString();
            Name++;
        }
        Name = 0;
    }

    // Update is called once per frame
    void Update()
    {
        for (float t = 0; t <= 1; t += 0.01f)
        {
            pozycjax = punkt1.position.x * Mathf.Pow(1 - t, 3) + 3 * punkt2.position.x * t * Mathf.Pow(1 - t, 2) + 3 * punkt3.position.x * t * t * (1 - t) + punkt4.position.x * t * t * t;
            pozycjaz = punkt1.position.y * Mathf.Pow(1 - t, 3) + 3 * punkt2.position.y * t * Mathf.Pow(1 - t, 2) + 3 * punkt3.position.y * t * t * (1 - t) + punkt4.position.y * t * t * t;
            pozycjay = punkt1.position.z * Mathf.Pow(1 - t, 3) + 3 * punkt2.position.z * t * Mathf.Pow(1 - t, 2) + 3 * punkt3.position.z * t * t * (1 - t) + punkt4.position.z * t * t * t;
            pozycja = new Vector3(pozycjax, pozycjaz, pozycjay);

            point = GameObject.Find(Name.ToString());
            point.transform.position = pozycja;

            point.name = Name.ToString();

            Name++;
        }
        Name = 0;

        o2.transform.position = (punkt1.position + punkt3.position) * 0.5f;
        o3.transform.position = (punkt4.position - punkt0.position) + punkt4.position;


    }
}
