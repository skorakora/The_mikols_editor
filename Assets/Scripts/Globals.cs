using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals   //----------------------------------Global Variables--------------------------------------------------
{
    private static string simulator_root,scn_path,scn_folder_path;
    public static string Simulator_root
    {
        get
        {
            return simulator_root;
        }
        set
        {
            simulator_root = value;
        }
    }
    public static string SCN_path //do wywalenia - patrz github
    {
        get
        {
            return scn_path;
        }
        set
        {
            scn_path = value;
        }
    }
    public static string SCN_folder_path //do wywalenia - patrz github
    {
        get
        {
            return scn_folder_path;
        }
        set
        {
            scn_folder_path = value;
        }
    }

    public static int SCNLoaderInstanceCounter;//licznik aktywnych instancji loadera scenerii
    public static bool IsSCNLoaderActive()
    {
        if (SCNLoaderInstanceCounter == 0)
        {
            return false;
        }
        return true;
    }

}
