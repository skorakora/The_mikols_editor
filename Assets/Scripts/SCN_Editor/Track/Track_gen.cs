using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class Track_gen : MonoBehaviour
{

    public Transform pk1;
    public Transform pk2;
    public Transform pk3;
    public Transform pk4;

    float spacing = 15;
    float tiling = 0.3f;



    void Start()
    {


    }

    void Update()
    {
        gen_track(pk1.position, pk2.position, pk3.position, pk4.position);
    }



    // p1 p4 punkty początku i końca toru   p2 p3 punkty kontrolne krzywej beziera.
    public void gen_track(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4) //to jest główna funkcja.
    {
        Vector3[] points = CalculateEvenlySpacedPoints(p1, p2, p3, p4);
        GetComponent<MeshFilter>().mesh = CreateTrackMesh(points);

        int textureRepeat = Mathf.RoundToInt(tiling * points.Length * spacing * .05f);
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
            verts[vertIndex] = points[i] + left * 1 * .5f;
            verts[vertIndex + 1] = points[i] - left * 1 * .5f;

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




    public Vector3 EvaluateQuadratic(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 p0 = Vector3.Lerp(a, b, t);
        Vector3 p1 = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(p0, p1, t);
    }

    public Vector3 EvaluateCubic(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 p0 = EvaluateQuadratic(a, b, c, t);
        Vector3 p1 = EvaluateQuadratic(b, c, d, t);
        return Vector3.Lerp(p0, p1, t);
    }






    public Vector3[] CalculateEvenlySpacedPoints(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
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
