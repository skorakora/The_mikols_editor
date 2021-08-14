using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    string terrain_name;
    List<Vector3> vertices = new List<Vector3>();
    List<Vector3> normals = new List<Vector3>();
    List<Vector2> UV = new List<Vector2>();
    List<int> triangles = new List<int>();
    int triangle_counter = 0;
    Texture2D texture;

    public Terrain(string _name)
    {
        terrain_name = _name;
        triangle_counter = 0;
    }

    public void AddVertex(Vector3 vertex,Vector3 normal,Vector2 _UV)
    {
        vertices.Add(vertex);
        normals.Add(normal);
        UV.Add(_UV);
        triangles.Add(triangle_counter);
        triangle_counter++;
    }

    public Mesh GetMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = UV.ToArray();
        mesh.triangles = triangles.ToArray();
        return mesh;

    }

    public string GetName()
    {
        return terrain_name;
    }


}
