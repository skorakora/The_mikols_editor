using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Normal : MonoBehaviour
{
    //------------------ELEMENTY TORU W EDYTORZE------------------
    public GameObject ArrowModel;
    public Material ArrowGreen;
    public Material ArrowRed;

    private GameObject P1_Track;
    private GameObject P2_Track; 
    //------------------WŁAŚCIWOŚCI TORU------------------
    public float range_max; 
    public float range_min;
    public string track_name;
    public float length;
    public float trackWidth;
    public float friction;
    public float soundDistance;
    public string qualityFlag;
    public string damageFlag;
    public string environment;
    public string visible;

    public string material1;
    public string texLength;
    public string material2;
    public string texHeight;
    public string texWidth;
    public string texSlope;

    public Vector3 P1 = new Vector3();
    public float roll1;
    public Vector3 CV1 = new Vector3();
    public Vector3 CV2 = new Vector3();
    public Vector3 P2 = new Vector3();
    public float roll2;
    public string radius;
    public List<string> optionalparameter = new List<string>();

    public Texture2D tex;

    float spacing = 5; 
    float tiling = 0.2f;


    public  Normal()
    {
        //Creates default track

        //TODO: Nie wiem jak deafultowy tor powinien wyglądać
    }

    public void Deserialize(List<string> buffer)//DONE
    {
        MeshFilter mf = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        MeshRenderer mr = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;

        range_max = float.Parse(buffer[0]);
        range_min = float.Parse(buffer[1]);
        track_name = buffer[2];
        length = float.Parse(buffer[3]);
        trackWidth = float.Parse(buffer[4]);
        friction = float.Parse(buffer[5]);
        soundDistance = float.Parse(buffer[6]);
        qualityFlag = buffer[7];
        damageFlag = buffer[8];
        environment = buffer[9];
        visible = buffer[10];

        material1 = buffer[11];
        texLength = buffer[12];
        material2 = buffer[13];
        texHeight = buffer[14];
        texWidth = buffer[15];
        texSlope = buffer[16];

        P1.x = float.Parse(buffer[17]);
        P1.y = float.Parse(buffer[18]);
        P1.z = float.Parse(buffer[19]);

        roll1 = float.Parse(buffer[20]);

        CV1.x = float.Parse(buffer[21]);
        CV1.y = float.Parse(buffer[22]);
        CV1.z = float.Parse(buffer[23]);

        CV2.x = float.Parse(buffer[24]);
        CV2.y = float.Parse(buffer[25]);
        CV2.z = float.Parse(buffer[26]);

        P2.x = float.Parse(buffer[27]);
        P2.y = float.Parse(buffer[28]);
        P2.z = float.Parse(buffer[29]);

        roll2 = float.Parse(buffer[30]);
        radius = buffer[31];

        for (int i = 0; i < (buffer.Count()-32)/2; i++) //to moze byc zbagowane na razie nie chce mi sie tego poprawiac
        {
            optionalparameter.Add(buffer[32+i]+" "+buffer[33+i]);
        }





}//DONE

    public string Serialize()//DONE
    {
        string trackBuffer = "";
        List<string> trackParameterBuffer = new List<string>();
        trackParameterBuffer.Add("node");
        trackParameterBuffer.Add(range_max.ToString());
        trackParameterBuffer.Add(range_min.ToString());
        trackParameterBuffer.Add(track_name.ToString());
        trackParameterBuffer.Add("track");
        trackParameterBuffer.Add("normal");
        trackParameterBuffer.Add(length.ToString());
        trackParameterBuffer.Add(trackWidth.ToString());
        trackParameterBuffer.Add(friction.ToString());
        trackParameterBuffer.Add(soundDistance.ToString());
        trackParameterBuffer.Add(qualityFlag.ToString());
        trackParameterBuffer.Add(damageFlag.ToString());
        trackParameterBuffer.Add(environment.ToString());
        trackParameterBuffer.Add(visible.ToString());
        trackParameterBuffer.Add(material1.ToString());
        trackParameterBuffer.Add(texLength.ToString());
        trackParameterBuffer.Add(material2.ToString());
        trackParameterBuffer.Add(texHeight.ToString());
        trackParameterBuffer.Add(texWidth.ToString());
        trackParameterBuffer.Add(texSlope.ToString());

        trackParameterBuffer.Add(P1.x.ToString());
        trackParameterBuffer.Add(P1.y.ToString());
        trackParameterBuffer.Add(P1.z.ToString());

        trackParameterBuffer.Add(roll1.ToString());

        trackParameterBuffer.Add(CV1.x.ToString());
        trackParameterBuffer.Add(CV1.y.ToString());
        trackParameterBuffer.Add(CV1.z.ToString());

        trackParameterBuffer.Add(CV2.x.ToString());
        trackParameterBuffer.Add(CV2.y.ToString());
        trackParameterBuffer.Add(CV2.z.ToString());

        trackParameterBuffer.Add(P2.x.ToString());
        trackParameterBuffer.Add(P2.y.ToString());
        trackParameterBuffer.Add(P2.z.ToString());

        trackParameterBuffer.Add(radius.ToString());

        for (int i = 0; i < optionalparameter.Count(); i++)
        {
            trackParameterBuffer.Add(optionalparameter[i].ToString());
        }

        trackParameterBuffer.Add("endtrack");


        for (int i = 0; i < trackParameterBuffer.Count(); i++)
        {
            trackBuffer = trackBuffer + " " + trackParameterBuffer[i];
        }


        return trackBuffer;
    }//DONE

    public void UpdateTrack()
    {

    }


    public void EditorUpdate()
    {

    }


    private Mesh GetMesh()
    {
        MeshFilter meshfilter = gameObject.GetComponent(typeof(MeshFilter)) as MeshFilter;
        MeshRenderer meshrenderer = gameObject.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<int> triangles = new List<int>();

       




    }




    private GameObject FindClosestTrack(Vector3 point)
    {

        Collider[] hitCollider = Physics.OverlapSphere(point, 0.5f);


        GameObject foundTrack = GameObject.Find("");



        return foundTrack;
    }


}
