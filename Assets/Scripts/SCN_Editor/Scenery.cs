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
    long LoaderPosition = 0;
    long SceneryFileSize = 0;

    public GameObject canvas;
    public const string str = "objects";
    public GameObject Track;

    Dictionary<string, GameObject> Categories = new Dictionary<string, GameObject>();//List of categories on scenery (objects avaible to spawn)


    Parser parser = new Parser();
    void Start()//Scenery initialization from Globals
    {
        //InitializeScenery(); Initialization of scenery, before deserialization starts.
        Categories.Add("terrain", AddCategory("terrain"));//add to InitializeScenery()
        Categories.Add("track", AddCategory("track"));//add to InitializeScenery()

        if (Globals.CreateNewScenery == false)
        {
            string path = Globals.Simulator_root + @"\scenery\" + Globals.Scenery_name;

            Debug.Log("Starting The Mikols - Maszyna scenery deserializer. Please wait...");
            Globals.SCNLoaderInstanceCounter = 0;
            StartCoroutine(Deserialize(path));
            return;
        }

        GenerateDefaultScenery();

    }

    GameObject AddCategory(string name)//porządkuje obiekty w drzewku by nie było syfu
    {
        GameObject obj = new GameObject(name);
        obj.transform.parent = this.transform;
        return obj;
    }

    private void GenerateDefaultScenery()// do poprawki
    {
        Mesh mesh = new Mesh();

        List<Vector3> vrt = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> UV = new List<Vector2>();

        List<int> triangles = new List<int>();
        vrt.Add(new Vector3(-200, 0, 200));
        UV.Add(new Vector2(-200f, -200f));

        vrt.Add(new Vector3(-200, 0, -200));
        UV.Add(new Vector2(200, -200f));

        vrt.Add(new Vector3(200, 0, 200));
        UV.Add(new Vector2(-200f, 200));

        vrt.Add(new Vector3(200, 0, -200));
        UV.Add(new Vector2(200, 200));

        normals.Add(Vector3.up);
        normals.Add(Vector3.up);
        normals.Add(Vector3.up);
        normals.Add(Vector3.up);

        triangles.Add(0);
        triangles.Add(3);
        triangles.Add(1);

        triangles.Add(0);
        triangles.Add(2);
        triangles.Add(3);

        mesh.vertices = vrt.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = UV.ToArray();

        AddMesh(mesh, 1, 0, "Default Terrain", Globals.GetTexture("grassdarkgreen4"));
        //AddTrack(pk1,pk2,pk3,pk4,default,HelloWorld);
        //PlaceTrain(HelloWorld,EN57-001RA,EN57-001RS,EN57-001RB)

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

    public void AddTrack(
        float range_max,
        float range_min,
        string name,
        string track_type,
        float length,
        float widith,
        float friction,
        float stukot,
        float jakosc,
        float uszkodzenia,
        string srodowisko,
        string widocznosc,
        string tekstura1,
        string powtarzanie_tekstury,
        string tekstura2,
        float wysokosc_podsypki,
        float szerokosc_podsypki,
        float szerokosc_pochylenia,
        Vector3 Point1,
        float przechylka1,
        Vector3 ControlVector1,
        Vector3 ControlVector2,
        Vector3 Point2,
        float przechylka2,
        float promien
        )
    {
        GameObject ROOT = Categories["track"];
        GameObject obj = Instantiate(Track, new Vector3(0,0,0), Quaternion.identity);
        obj.transform.parent = ROOT.transform;
        Track_gen track = obj.GetComponent<Track_gen>();
        track.range_max = range_max;
        track.range_min = range_min;
        track.name = name;
        track.length = length;
        track.widith = widith;
        track.friction = friction;
        track.stukot = stukot;
        track.jakosc = jakosc;
        track.uszkodzenia = uszkodzenia;
        track.srodowisko = srodowisko;
        track.widocznosc = widocznosc;
        track.tekstura1 = tekstura1;
        track.powtarzanie_tekstury = powtarzanie_tekstury;
        track.tekstura2 = tekstura2;
        track.wysokosc_podsypki = wysokosc_podsypki;
        track.szerokosc_podsypki = szerokosc_podsypki;
        track.szerokosc_pochylenia = szerokosc_pochylenia;
        track.Point1 = Point1;
        track.przechylka1 = przechylka1;
        track.ControlVector1 = ControlVector1;
        track.ControlVector2 = ControlVector2;
        track.Point2 = Point2;
        track.przechylka2 = przechylka2;
        track.promien = promien;
        track.gen_track();//generuje ustawiony tor

    }

    public IEnumerator Deserialize(string path) //scn deserializer Created by skorakora (Daniel Skorski)
    {
        Editor_canvas_main editor_Canvas_Main = canvas.GetComponent<Editor_canvas_main>();
        long LocalLoaderPosition = 0;
        int counter = 0;
        FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
        Globals.SCNLoaderInstanceCounter++;
        SceneryFileSize = file.Length + SceneryFileSize;
        string token;
        while (true)
        {
            counter++;
            if (counter == 100)
            {
                float progress;
                long filePositionChange;

                counter = 0;
                filePositionChange = file.Position - LocalLoaderPosition;
                LoaderPosition = filePositionChange + LoaderPosition;
                LocalLoaderPosition = filePositionChange + LocalLoaderPosition;
                progress = Convert.ToSingle((Convert.ToSingle(LoaderPosition) / Convert.ToSingle(SceneryFileSize)) * 100);
                editor_Canvas_Main.SetProgress(progress / 100);
                Debug.Log(progress + "%");
                yield return null;
            }
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
                Debug.Log("ERROR: unknown function: " + token);
            }
        }

        file.Close();
        Globals.SCNLoaderInstanceCounter--;




    }

    public IEnumerable Serialize(string path)
    {
        FileStream file = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
        for (int i = 0; i < Categories.Count; i++)
        {
            if (Categories.ElementAt(i).Key == "terrain")
            {
                GameObject ROOT = Categories["terrain"];
                foreach (Transform child in ROOT.transform)
                {

                }
            }
            yield return null;
        }

    }

    //-----------------------------------------------------PRIVATE METHODS-------------------------------------------------

    //-------------------------------------------------//-Serializer--//----------------------------------------------------

    private void SerializeTrack(GameObject obj, FileStream file)
    {
        Track_gen Track = obj.GetComponent<Track_gen>();

    }


    // -------------------------------------------------//-Deserializer-//----------------------------------------------------
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
                StartCoroutine(Deserialize(Globals.Simulator_root+@"\scenery" + "/" + token));
            }
            else if (token.Contains(".ctr"))
            {
                StartCoroutine(Deserialize(Globals.Simulator_root + @"\scenery" + "/" + token));
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
            Deserialize_Track(file, range_max, range_min, name);
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

    private void Deserialize_Track(FileStream file, float range_max, float range_min,string name)
    {
        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                return;
            }
            string track_type = token;
            if (token == "normal")
            {
                float length = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                float widith = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                float friction = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                float stukot = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                float jakosc = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                float uszkodzenia = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                string srodowisko = parser.GetToken(file);
                string widocznosc = parser.GetToken(file);
                string tekstura1 = parser.GetToken(file);
                string powtarzanie_tekstury = parser.GetToken(file);
                string tekstura2 = parser.GetToken(file);
                float wysokosc_podsypki = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                float szerokosc_podsypki = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                float szerokosc_pochylenia = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                Vector3 Point1 = new Vector3();
                Point1.x = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                Point1.y = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                Point1.z = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                float przechylka1 = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                Vector3 ControlVector1 = new Vector3();
                ControlVector1.x = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                ControlVector1.y = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                ControlVector1.z = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                Vector3 ControlVector2 = new Vector3();
                ControlVector2.x = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                ControlVector2.y = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                ControlVector2.z = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                Vector3 Point2 = new Vector3();
                Point2.x = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                Point2.y = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                Point2.z = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                float przechylka2 = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);
                float promien = float.Parse(parser.GetToken(file), CultureInfo.InvariantCulture);

                AddTrack(range_max, range_min, name, track_type, length, widith, friction, stukot, jakosc, uszkodzenia, srodowisko, widocznosc, tekstura1, powtarzanie_tekstury, tekstura2, wysokosc_podsypki, szerokosc_podsypki, szerokosc_pochylenia, Point1, przechylka1, ControlVector1, ControlVector2, Point2, przechylka2, promien);

            }


            if (token == "endtrack")
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

            texture = Globals.GetTexture(textureName);

            token = Deserialize_Vertex(file, terrain, token);

            if (token == "endtri")
            {
                texture = Globals.GetTexture(textureName);
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

    //--------------------------------------------------Pozostałe narzędzia----------------------------------------


}


