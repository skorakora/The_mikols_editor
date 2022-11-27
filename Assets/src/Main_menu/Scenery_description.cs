using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scenery_description : MonoBehaviour
{
    public string scn_description;

    void Update()
    {
        Text description_text = GetComponent<Text>();
        description_text.text = scn_description;
    }
}
