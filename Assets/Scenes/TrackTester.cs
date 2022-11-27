using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TrackTester : MonoBehaviour
{

    public GameObject TrackNormal;
    public List<string> buffer = new List<string>();

    public string serialized;
    void Start()
    {
        Instantiate(TrackNormal, new Vector3(0, 0, 0),Quaternion.identity);
        Parser parser = gameObject.AddComponent(typeof(Parser)) as Parser;
        Normal normal = TrackNormal.GetComponent(typeof(Normal)) as Normal;

        FileStream file = new FileStream("C:\\Users\\Komp\\Documents\\test.scn", FileMode.Open, FileAccess.Read);


        string token;
        while (true)
        {
            token = parser.GetToken(file);
            if (token == null)
            {
                break;
            }
            if (token == "node")
            {
                token = parser.GetToken(file);
            }
            if (token == "track")
            {
                token = parser.GetToken(file);
                token = parser.GetToken(file);
            }
            buffer.Add(token);

            if (token == "endtrack")
            {
                break;
            }
        }


        

        normal.Deserialize(buffer);

        normal.UpdateTrack();

        serialized = normal.Serialize();

    }

}
