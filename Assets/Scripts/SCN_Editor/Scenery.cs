using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenery : MonoBehaviour
{
    Dictionary<string, GameObject> Categories = new Dictionary<string, GameObject>();//List of categories on scenery (objects avaible to spawn)
    void Start()//Scenery initialization from Globals
    {
        Parser parser = new Parser();
        Categories.Add("terrain", AddCategory("terrain"));
    }

    GameObject AddCategory(string name)//porządkuje obiekty w drzewku by nie było syfu
    {
        GameObject obj = new GameObject(name);
        obj.transform.parent = this.transform;
        return obj;
    }

}
