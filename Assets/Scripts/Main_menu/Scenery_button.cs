using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scenery_button : MonoBehaviour
{
    public string scenery_name;
    public Text scn_text;
    // Start is called before the first frame update
    void Start()
    {
        scn_text.text = scenery_name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
