using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

public class Main: MonoBehaviour
{
    Parser parser = new Parser();

    void Start()
    {
        Deserialize(Globals.SCN_path);
    }

    public void Deserialize(string path)
    {
        StreamReader file = new StreamReader(path);

    }

    public void Serialize(string path)
    {

    }

}