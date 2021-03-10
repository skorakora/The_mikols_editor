using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class E3D_loader : MonoBehaviour
{
    Sn_utils sn_utils = new Sn_utils();
    
    public string path = "C:\\Program Files (x86)\\Maszyna\\dynamic\\pkp\\en57_v1\\en57-840s.e3d";
    // Start is called before the first frame update
    void Start()
    {
        Load(path);

    }

    public void Load(string _path)
    {
        FileStream file = new FileStream(_path, FileMode.Open, FileAccess.Read);
        Debug.Log(sn_utils.ld_uint16(file));
    }

}
