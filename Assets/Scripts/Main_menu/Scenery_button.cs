using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;

public class Scenery_button : MonoBehaviour
{
    public string scenery_name;
    public Text scn_text;
    private Image img;
    public string file_path;
    public Texture tex;
    public bool select;
    string image_name = "";
    public GameObject description_stripe;
    string Settings_save_path;
    // Start is called before the first frame update
    void Start()
    {
        Settings_save_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\The_mikols\settings.xml";
        StreamReader plik = new StreamReader(Settings_save_path);
        plik.ReadLine();
        string[] data = plik.ReadLine().Split(';');
        file_path = data[1];
        plik.Close();

        img = GetComponent<Image>();
        img.enabled = false;
        scn_text.text = scenery_name;
    }


    public void Select()
    {

        img = GetComponent<Image>();
        img.enabled = true;
        scn_text.color = Color.gray;
        string data;
        string[] subdata;
        bool controll = false;
        StreamReader plik = new StreamReader(file_path + @"\" + scenery_name + ".scn");
        GameObject description_parent = GameObject.Find("description_list"); //znajduje i usuwa wszystkie paski opisu przed wstawieniem nowych
        var children = new List<GameObject>();
        foreach (Transform child in description_parent.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
        controll = false;
        while (plik.EndOfStream == false)
        {
            data = plik.ReadLine();
            if (data.StartsWith("//$i"))
            {
                GameObject gm = GameObject.Find("scn_image");
                RawImage scn_img = gm.GetComponent(typeof(RawImage))as RawImage;
                subdata = data.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                image_name = subdata[1];
                print(file_path + @"\images\" + subdata[1]);
                Texture2D tex = null;
                byte[] fileData;
                if (File.Exists(file_path + @"\images\" + subdata[1]))
                {
                    fileData = File.ReadAllBytes(file_path + @"\images\" + subdata[1]);
                    tex = new Texture2D(2, 2);
                    tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
                }

                scn_img.texture = tex;
            }
            if (data.StartsWith("//$d"))
            {
                subdata = new string[1];
                subdata[0] = data.Replace("//$d", "");
                print(subdata[0]);
                GameObject description = Instantiate(description_stripe, new Vector3(0, 0, 0), Quaternion.identity);
                description.transform.SetParent(description_parent.transform,false);
                description.name = subdata[0];
                Scenery_description Jackie_lynn = description.GetComponent<Scenery_description>();
                Jackie_lynn.scn_description = subdata[0];
                controll = true;
            }


        }
        if (controll == false)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject description = Instantiate(description_stripe, new Vector3(0, 0, 0), Quaternion.identity);
                description.transform.SetParent(description_parent.transform, false);
                description.name = "Tu powinien być opis";
                Scenery_description Jackie_lynn = description.GetComponent<Scenery_description>();
                Jackie_lynn.scn_description = "Tu powinien być opis";
            }

        }
        controll = false;
        if (String.IsNullOrEmpty(image_name))
        {
            GameObject gm = GameObject.Find("scn_image");
            RawImage scn_img = gm.GetComponent(typeof(RawImage)) as RawImage;
            scn_img.texture = tex;
        }
        image_name = "";
        plik.Close();
        GameObject gm1 = GameObject.Find("Editor");
        Editor_canvas editor_canvas = gm1.GetComponent<Editor_canvas>();
        editor_canvas.scn_name = scenery_name;

        
        
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            select = false;
        }
        if (select == true)
        {
            img.enabled = true;
            scn_text.color = Color.gray;
        }
        else
        {
            img.enabled = false;
            scn_text.color = Color.white;
        }
    }

}
