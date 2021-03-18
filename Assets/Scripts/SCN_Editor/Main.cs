using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

public class Main: MonoBehaviour
{
    Parser parser = new Parser();


    public void Deserialize(string path) //scn deserializer Created by skorakora (Daniel Skorski)
    {
        StreamReader file = new StreamReader(path);
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == "atmo")
            {
                Deserialize_Atmo(file);
            }
            else if (token == "camera")
            {
                Deserialize_Camera(file);
            }
            else if (token == "config")
            {
                Deserialize_Config(file);
            }
            else if (token == "description")
            {
                Deserialize_Description(file);
            }
            else if (token == "event")
            {
                Deserialize_Event(file);
            }
            else if (token == "FirstInit")
            {
                Deserialize_FirstInit(file);
            }
            else if (token == "include")
            {
                Deserialize_Include(file);
            }
            else if (token == "light")
            {
                Deserialize_Light(file);
            }
            else if (token == "lua")
            {
                Deserialize_Lua(file);
            }
            else if (token == "node")
            {
                Deserialize_Node(file);
            }
            else if (token == "origin")
            {
                Deserialize_Origin(file);
            }
            else if (token == "rotate")
            {
                Deserialize_Rotate(file);
            }
            else if (token == "sky")
            {
                Deserialize_Sky(file);
            }
            else if (token == "test")
            {
                Deserialize_Test(file);
            }
            else if (token == "time")
            {
                Deserialize_Time(file);
            }
            else if (token == "trainset")
            {
                Deserialize_Trainset(file);
            }
            else if (token == null)
            {
                break;
            }
            else
            {
                Debug.Log("ERROR: unknown function");
            }
        }

        file.Close();
        Debug.Log("finished loading scenery!");

    }

    public void Serialize(string path)
    {

    }

    //----------------------------------------------deserializer - dyrektywy------------------------------------------------

    private void Deserialize_Atmo(StreamReader file)
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            //tutaj funkcje odpowiedzialne za atmo
            else if (token == "endatmo")
            {
                return;
            }
        }
    }

    private void Deserialize_Camera(StreamReader file)
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            //tutaj funkcje odpowiedzialne za kamerę
            else if (token == "endcamera")
            {
                return;
            }
        }
    }

    private void Deserialize_Config(StreamReader file)
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            //tutaj funkcje odpowiedzialne za config
            else if (token == "endconfig")
            {
                return;
            }
        }
    }

    private void Deserialize_Description(StreamReader file)
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            //tutaj funkcje odpowiedzialne za opis scenerii
            else if (token == "enddescription")
            {
                return;
            }
        }
    }

    private void Deserialize_Event(StreamReader file)
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            //tutaj funkcje odpowiedzialne za eventy
            else if (token == "endevent")
            {
                return;
            }
        }
    }

    private void Deserialize_FirstInit(StreamReader file)
    {
        Debug.Log("First init");
    }

    private void Deserialize_Include(StreamReader file)
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            //tutaj funkcje odpowiedzialne za include
            else if (token == "end")
            {
                return;
            }
        }
    }

    private void Deserialize_Light(StreamReader file)//unused
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            else if (token == "endlight")
            {
                return;
            }
        }
    }

    private void Deserialize_Lua(StreamReader file)
    {
        parser.GetToken(file);//pobiera ścieżkę dostępu pliku lua
    }

    private void Deserialize_Node(StreamReader file)
    {
        // to co tygryski lubią najbardziej - funcja node. WIP
        parser.GetToken(file);//range_max
        parser.GetToken(file);//Range_min

    }

    private void Deserialize_Origin(StreamReader file)
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            //tutaj funkcje odpowiedzialne za origin
            else if (token == "endorigin")
            {
                return;
            }
        }
    }

    private void Deserialize_Rotate(StreamReader file)
    {
        parser.GetToken(file);
    }

    private void Deserialize_Sky(StreamReader file)
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            //tutaj funkcje odpowiedzialne za niebo
            else if (token == "endsky")
            {
                return;
            }
        }
    }

    private void Deserialize_Test(StreamReader file)
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            // unused
            else if (token == "endtest")
            {
                return;
            }
        }
    }

    private void Deserialize_Time(StreamReader file)
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            //tutaj funkcje odpowiedzialne za czas
            else if (token == "endtime")
            {
                return;
            }
        }
    }

    private void Deserialize_Trainset(StreamReader file)
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            //tutaj funkcje odpowiedzialne za składy
            else if (token == "endtrainset")
            {
                return;
            }
        }
    }

    //--------------------------------------------------deserializer - podfunkcje dyrektywy node----------------------------------------

    private void Deserialize_Dynamic(StreamReader file,float range_max,float range_min)
    {
        
    }

    private void Deserialize_EventLauncher(StreamReader file, float range_max, float range_min)
    {

    }

    private void Deserialize_Isolated(StreamReader file, float range_max, float range_min)
    {

    }

    private void Deserialize_Lines(StreamReader file, float range_max, float range_min)
    {

    }

    private void Deserialize_Line_strip(StreamReader file, float range_max, float range_min)
    {

    }

    private void Deserialize_Line_loop(StreamReader file, float range_max, float range_min)
    { 

    }

    private void Deserialize_Memcell(StreamReader file, float range_max, float range_min)
    {

    }

    private void Deserialize_Model(StreamReader file, float range_max, float range_min)
    {

    }

    private void Deserialize_Sound(StreamReader file, float range_max, float range_min)
    {

    }

    private void Deserialize_Track(StreamReader file, float range_max, float range_min)
    {

    }

    private void Deserialize_Traction(StreamReader file, float range_max, float range_min)
    {

    }

    private void Deserialize_TractionPowerSource(StreamReader file, float range_max, float range_min)
    {

    }

    private void Deserialize_Triangles(StreamReader file, float range_max, float range_min)
    {

    }

    private void Deserialize_Triangle_strip(StreamReader file, float range_max, float range_min)
    {

    }

    private void Deserialize_triangle_fan(StreamReader file, float range_max, float range_min)
    {

    }




}