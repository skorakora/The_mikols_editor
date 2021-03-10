using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals   //----------------------------------Global Variables--------------------------------------------------
{
    private static string scn_path,scn_folder_path;
    public static string SCN_path
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
    public static string SCN_folder_path
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


}
