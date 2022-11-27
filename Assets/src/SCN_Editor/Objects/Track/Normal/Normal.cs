using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Normal : MonoBehaviour
{
    //na razie public, w celach debugowania
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


     void Start()
    {
        // do notihng
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

        Vector3[] points = CalculateEvenlySpacedPoints(P1, P1 + CV1, P2 + CV2, P2);
        GetComponent<MeshFilter>().mesh = CreateTrackMesh(points);
        int textureRepeat = Mathf.RoundToInt(tiling * points.Length * spacing * .05f);

        GetComponent<MeshRenderer>().material.mainTexture = tex;
        GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(1, textureRepeat);


    }


    Mesh CreateTrackMesh(Vector3[] points) //tutaj siedzi model toru - do poprawienia on jest.
    {
        bool isClosed = false;
        Vector3[] verts = new Vector3[points.Length * 2];
        Vector2[] uvs = new Vector2[verts.Length];
        int numTris = 2 * (points.Length - 1) + ((isClosed) ? 2 : 0);
        int[] tris = new int[numTris * 3];
        int vertIndex = 0;
        int triIndex = 0;

        for (int i = 0; i < points.Length; i++)
        {
            Vector2 forward2d = Vector2.zero;
            Vector3 forward = new Vector3(forward2d.x, 0, forward2d.y);
            if (i < points.Length - 1)
            {
                forward += points[(i + 1) % points.Length] - points[i];
            }
            if (i > 0)
            {
                forward += points[i] - points[(i - 1 + points.Length) % points.Length];
            }

            forward.Normalize();
            Vector2 left2d = new Vector2(-forward.z, forward.x);
            Vector3 left = new Vector3(left2d.x, 0, left2d.y);
            verts[vertIndex] = points[i] + left * 1 * 2.5f;
            verts[vertIndex + 1] = points[i] - left * 1 * 2.5f;

            float completionPercent = i / (float)(points.Length - 1);
            float v = 1 - Mathf.Abs(2 * completionPercent - 1);
            uvs[vertIndex] = new Vector2(0, v);
            uvs[vertIndex + 1] = new Vector2(1, v);

            if (i < points.Length - 1 || isClosed)
            {
                tris[triIndex] = vertIndex;
                tris[triIndex + 1] = (vertIndex + 2) % verts.Length;
                tris[triIndex + 2] = vertIndex + 1;

                tris[triIndex + 3] = vertIndex + 1;
                tris[triIndex + 4] = (vertIndex + 2) % verts.Length;
                tris[triIndex + 5] = (vertIndex + 3) % verts.Length;
            }

            vertIndex += 2;
            triIndex += 6;
        }

        List<Vector3> norm = new List<Vector3>();
        for (int i = 0; i < verts.Length; i++)
        {
            norm.Add(Vector3.up);
        }
        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.normals = norm.ToArray();

        return mesh;
    }




    private Vector3 EvaluateQuadratic(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 p0 = Vector3.Lerp(a, b, t);
        Vector3 p1 = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(p0, p1, t);
    }

    private Vector3 EvaluateCubic(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 p0 = EvaluateQuadratic(a, b, c, t);
        Vector3 p1 = EvaluateQuadratic(b, c, d, t);
        return Vector3.Lerp(p0, p1, t);
    }






    private Vector3[] CalculateEvenlySpacedPoints(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {

        float spacing = 0.3f;
        float resolution = 1;
        List<Vector3> evenlySpacedPoints = new List<Vector3>();
        evenlySpacedPoints.Add(p1);
        Vector3 previousPoint = p1;
        float dstSinceLastEvenPoint = 0;


        Vector3[] p = new Vector3[4];
        p[0] = p1;
        p[1] = p2;
        p[2] = p3;
        p[3] = p4;


        float controlNetLength = Vector3.Distance(p[0], p[1]) + Vector3.Distance(p[1], p[2]) + Vector3.Distance(p[2], p[3]);
        float estimatedCurveLength = Vector3.Distance(p[0], p[3]) + controlNetLength / 2f;
        int divisions = Mathf.CeilToInt(estimatedCurveLength * resolution * 10);
        float t = 0;
        while (t <= 1)
        {
            t += 1f / divisions;
            Vector3 pointOnCurve = EvaluateCubic(p[0], p[1], p[2], p[3], t);
            dstSinceLastEvenPoint += Vector3.Distance(previousPoint, pointOnCurve);

            while (dstSinceLastEvenPoint >= spacing)
            {
                float overshootDst = dstSinceLastEvenPoint - spacing;
                Vector3 newEvenlySpacedPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized * overshootDst;
                evenlySpacedPoints.Add(newEvenlySpacedPoint);
                dstSinceLastEvenPoint = overshootDst;
                previousPoint = newEvenlySpacedPoint;
            }

            previousPoint = pointOnCurve;
        }

        return evenlySpacedPoints.ToArray();
    }

}
