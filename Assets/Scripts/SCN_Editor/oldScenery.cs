//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class oldScenery : MonoBehaviour
//{
//    //--------------------------------------------------CONSTRUCTOR--------------------------------------------------------
    

//    public oldScenery()
//    {
//        GameObject SCN_root = GameObject.Find("Scenery");
//        //elementy scenerii
//        GameObject obj = new GameObject();
//        Map.Add("terrain", AddCategory("terrain", SCN_root));
//    }

//    GameObject AddCategory(string name, GameObject Root)//porządkuje obiekty w drzewku by nie było syfu
//    {
//        GameObject obj = new GameObject(name);
//        obj.transform.parent = Root.transform;
//        return obj;
//    }


//    //-----------------------------------------------------METHODS---------------------------------------------------------

//    public void AddMesh(Mesh mesh, float range_max, float range_min, string name, Texture2D texture)
//    {
//        GameObject ROOT = Map["terrain"];
//        GameObject obj = new GameObject(name);
//        obj.transform.parent = ROOT.transform;
//        MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
//        MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
//        meshFilter.mesh = mesh;
//        meshRenderer.material.mainTexture = texture;

//    }
//}

