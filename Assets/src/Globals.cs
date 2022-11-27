using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals   //----------------------------------Global Variables--------------------------------------------------
{
    public static Dictionary<string, Texture2D> TextureBank = new Dictionary<string, Texture2D>();
    public static string Simulator_root; //Ta zmienna przechowuje ścieżkę dostępu do symulatora
    public static string Scenery_name;
    public static int SCNLoaderInstanceCounter;//licznik aktywnych instancji loadera scenerii
    public static bool IsSCNLoaderActive()
    {
        if (SCNLoaderInstanceCounter == 0)
        {
            return false;
        }
        return true;
    }

    public static bool CreateNewScenery = false;// ta flaga mówi edytorowi czy powinien wygenerować nową scenerię podczas startu


    //---------------------------------Globalne funkncje-------------------------

    public static Texture2D GetTexture(string name)
    {
        Texture2D texture;
        try
        {
            texture = TextureBank[name];
        }
        catch (KeyNotFoundException)
        {
            Debug.Log("Loading texture: " + Globals.Simulator_root + "\\textures\\" + name + ".dds");
            TextureBank.Add(name, Resources.ResourceLoader.LoadTexture(name));
        }
        texture = TextureBank[name];
        return texture;
    }
}
