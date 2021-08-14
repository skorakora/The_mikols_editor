using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using System;
using System.Globalization;

public class Scenery : MonoBehaviour
{
    Dictionary<string, GameObject> Categories = new Dictionary<string, GameObject>();//List of categories on scenery (objects avaible to spawn)
    Dictionary<string, Texture2D> TextureBank = new Dictionary<string, Texture2D>();


    Parser parser = new Parser();
    Resources.ResourceLoader resourceLoader = new Resources.ResourceLoader();


    void Start()//Scenery initialization from Globals
    {
        
        Categories.Add("terrain", AddCategory("terrain"));
        string path = Globals.SCN_path;
        StartCoroutine(Deserialize(path));
        Debug.Log("finished loading scenery!");
    }

    GameObject AddCategory(string name)//porządkuje obiekty w drzewku by nie było syfu
    {
        GameObject obj = new GameObject(name);
        obj.transform.parent = this.transform;
        return obj;
    }

    //-----------------------------------------------------METHODS---------------------------------------------------------

    public void AddMesh(Mesh mesh, float range_max, float range_min, string name, Texture2D texture)
    {
        GameObject ROOT = Categories["terrain"];
        GameObject obj = new GameObject(name);
        obj.transform.parent = ROOT.transform;
        MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
        meshFilter.mesh = mesh;
        meshRenderer.material.mainTexture = texture;

    }

    IEnumerator Deserialize(string path) //scn deserializer Created by skorakora (Daniel Skorski)
    {

        Debug.Log("Starting The Mikols - Maszyna scenery deserializer. Please wait...");

        FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
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
                yield return null;
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
            else if (token.StartsWith("//"))
            {
                //todo - wklepać to do parsera
                int _char;
                while (true)
                {
                    _char = file.ReadByte();
                    if (_char == -1)
                    {
                        break;
                    }
                    if (_char == 13)
                    {
                        _char = file.ReadByte();
                        if (_char == 10)
                        {
                            break;
                        }
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("ERROR: unknown function");
            }
        }

        file.Close();
        yield return null;


    }

    //-----------------------------------------------------PRIVATE METHODS-------------------------------------------------
    private void Deserialize_Atmo(FileStream file)
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

    private void Deserialize_Camera(FileStream file)
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

    private void Deserialize_Config(FileStream file)
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

    private void Deserialize_Description(FileStream file)
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

    private void Deserialize_Event(FileStream file)
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

    private void Deserialize_FirstInit(FileStream file)
    {
        Debug.Log("First init");
    }

    private void Deserialize_Include(FileStream file)//todo inc.deserializer
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            else if (token.Contains(".inc"))
            {
                //todo inc.deserializer
            }
            else if (token.Contains(".scm"))
            {
                StartCoroutine(Deserialize(Globals.SCN_folder_path + "/" + token));
            }
            else if (token.Contains(".ctr"))
            {
                Deserialize(Globals.SCN_folder_path + "/" + token);
            }
            else if (token == "end")
            {
                return;
            }
        }
    }

    private void Deserialize_Light(FileStream file)//unused
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

    private void Deserialize_Lua(FileStream file)
    {
        parser.GetToken(file);//pobiera ścieżkę dostępu pliku lua
    } //unused in editor

    private void Deserialize_Node(FileStream file)
    {
        // to co tygryski lubią najbardziej - funcja node. WIP
        float range_max = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);//range_max
        float range_min = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);//Range_min
        string name = parser.GetToken(file);
        string token = parser.GetToken(file);
        if (token == null)
        {
            Debug.Log("ERROR: Empty node!");
            return;
        }
        else if (token == "dynamic")
        {
            Deserialize_Dynamic(file, range_max, range_min);
        }
        else if (token == "eventlauncher")
        {
            Deserialize_EventLauncher(file, range_max, range_min);
        }
        else if (token == "isolated")
        {
            Deserialize_Isolated(file, range_max, range_min);
        }
        else if (token == "lines")
        {
            Deserialize_Lines(file, range_max, range_min);
        }
        //brakuje tu "line_strim" i "line_loop", nie wiem jak to jest definiowane
        else if (token == "memcell")
        {
            Deserialize_Memcell(file, range_max, range_min);
        }
        else if (token == "model")
        {
            Deserialize_Model(file, range_max, range_min);
        }
        else if (token == "sound")
        {
            Deserialize_Sound(file, range_max, range_min);
        }
        else if (token == "track")
        {
            Deserialize_Track(file, range_max, range_min);
        }
        else if (token == "traction")
        {
            Deserialize_Traction(file, range_max, range_min);
        }
        else if (token == "tractionpowersource")
        {
            Deserialize_TractionPowerSource(file, range_max, range_min);
        }
        else if (token == "triangles")
        {
            Deserialize_Triangles(file, range_max, range_min, name);
        }
        else
        {
            Debug.Log("Error: Wrong node!");
        }
    }

    private void Deserialize_Origin(FileStream file)
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

    private void Deserialize_Rotate(FileStream file)
    {
        parser.GetToken(file);
    }

    private void Deserialize_Sky(FileStream file)
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

    private void Deserialize_Test(FileStream file)
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

    private void Deserialize_Time(FileStream file)
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

    private void Deserialize_Trainset(FileStream file)
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

    private void Deserialize_Dynamic(FileStream file, float range_max, float range_min)
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            //tutaj funkcje odpowiedzialne za obiekty dynamic
            else if (token == "enddynamic")
            {
                return;
            }
        }
    }

    private void Deserialize_EventLauncher(FileStream file, float range_max, float range_min)
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            //tutaj funkcje odpowiedzialne za event launcher
            else if (token == "end")
            {
                return;
            }
        }
    }

    private void Deserialize_Isolated(FileStream file, float range_max, float range_min)//unused
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            else if (token == "endisolated")
            {
                return;
            }
        }
    }

    private void Deserialize_Lines(FileStream file, float range_max, float range_min)
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            //tutaj funkcje odpowiedzialne za linie
            else if (token == "endline")
            {
                return;
            }
        }
    }

    private void Deserialize_Memcell(FileStream file, float range_max, float range_min)
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            //tutaj funkcje odpowiedzialne za memcell
            else if (token == "endmemcell")
            {
                return;
            }
        }
    }

    private void Deserialize_Model(FileStream file, float range_max, float range_min)
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            //tutaj funkcje odpowiedzialne za model
            else if (token == "endmodel")
            {
                return;
            }
        }
    }

    private void Deserialize_Sound(FileStream file, float range_max, float range_min)
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            //tutaj funkcje odpowiedzialne za obiekt sound
            else if (token == "endsound")
            {
                return;
            }
        }
    }

    private void Deserialize_Track(FileStream file, float range_max, float range_min)
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
            else if (token == "endtrack")
            {
                return;
            }
        }
    }

    private void Deserialize_Traction(FileStream file, float range_max, float range_min)
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            //tutaj funkcje odpowiedzialne za sieć trakcyjną
            else if (token == "endtraction")
            {
                return;
            }
        }
    }

    private void Deserialize_TractionPowerSource(FileStream file, float range_max, float range_min)
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            //tutaj funkcje odpowiedzialne za tractionpowersource
            else if (token == "end")
            {
                return;
            }
        }
    }

    private void Deserialize_Triangles(FileStream file, float range_max, float range_min, string name)
    {
        string token;
        string textureName;
        Texture2D texture;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                Debug.LogError("ERROR: can't load terrain!");
                return;
            }

            Terrain terrain = new Terrain(name);

            if (token == "material")
            {
                Deserialize_Material(file, terrain);
                token = parser.GetToken(file);
            }
            textureName = token;
            try
            {
                texture = TextureBank[textureName];
            }
            catch (KeyNotFoundException)
            {

                TextureBank.Add(textureName, resourceLoader.LoadTexture(textureName));
            }


            token = Deserialize_Vertex(file, terrain, token);

            if (token == "endtri")
            {
                texture = TextureBank[textureName];
                AddMesh(terrain.GetMesh(), range_max, range_min, terrain.GetName(), texture);
                return;
            }
        }
    }

    //--------------------------------------------------deserializer - podfunkcje dyrektywy triangles----------------------------------------


    private void Deserialize_Material(FileStream file, Terrain terrain)
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            //tutaj funkcje odpowiedzialne za odczyt materiału (na razie pusto)
            if (token == "endmaterial")
            {
                return;
            }
        }
    }
    private string Deserialize_Vertex(FileStream file, Terrain terrain, string token)
    {
        Vector3 vertex = new Vector3();
        Vector3 normal = new Vector3();
        Vector2 UV = new Vector2();

    start_vertex:

        vertex.x = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
        vertex.y = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
        vertex.z = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
        normal.x = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
        normal.y = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
        normal.z = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
        UV.x = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
        UV.y = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);

        terrain.AddVertex(vertex, normal, UV);

        token = parser.GetToken(file);

        if (token == null)
        {
            return null;
        }

        if (token == "end")
        {
            goto start_vertex;
        }

        if (token == "endtri")
        {
            return "endtri";
        }
        Debug.LogWarning("Bad Triangle at:" + terrain.name);
        return "endtri";
    }
}
