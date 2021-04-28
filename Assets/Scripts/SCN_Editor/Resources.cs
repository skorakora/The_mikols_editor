using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Math;

namespace Resources
{
    public class E3D_model
    {
        BinaryParser parser = new BinaryParser();
        List<SUB> submodel_data = new List<SUB>();
        VNT VNT0 = new VNT();
        //------------------------------------------methods---------------------------------------------------------

        public E3D_model(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError("ERROR: E3D file not found!");
                return;
            }

            FileStream file = File.OpenRead(path);
            if (!(parser.GetString(file, 4) == "E3D0"))
            {
                Debug.LogError("ERROR: Unknown main chunk");
                return;
            }
            int MainChunkSize = parser.GetInt(file);
            while (file.Position < MainChunkSize - 8)
            {
                string chunk_name = parser.GetString(file, 4);
                if (chunk_name == "SUB0")//Submodel data
                {
                    int size = parser.GetInt(file);
                    size = (size - 8) / 256;
                    for (int i = 0; i < size - 8; i++)
                    {
                        SUB SUB0 = new SUB();
                        SUB0.nextSubmodel = parser.GetInt(file);
                        SUB0.firstSubmodel = parser.GetInt(file);
                        SUB0.submodelType = parser.GetInt(file);
                        SUB0.nameNumber = parser.GetInt(file);

                        submodel_data.Add(SUB0);
                    }
                }
                if (chunk_name == "SUB1")
                {
                    int size = parser.GetInt(file);
                    for (int i = 0; i < size - 8; i++)
                    {
                        file.ReadByte();
                    }
                }
                if (chunk_name == "VNT0")
                {
                    int size = parser.GetInt(file);
                    size = (size - 8) / 32;
                    for (int i = 0; i < size / 32; i++)
                    {
                        VNT0.position.Add(new Vector3(parser.GetFloat(file), parser.GetFloat(file), parser.GetFloat(file)));
                        VNT0.normal.Add(new Vector3(parser.GetFloat(file), parser.GetFloat(file), parser.GetFloat(file)));
                        VNT0.uv.Add(new Vector2(parser.GetFloat(file), parser.GetFloat(file)));
                        VNT0.triangles.Add(i);
                    }
                }
                if (chunk_name == "TEX0")
                {
                    int size = parser.GetInt(file);
                    for (int i = 0; i < size - 8; i++)
                    {
                        file.ReadByte();
                    }
                }
                if (chunk_name == "NAM0")
                {
                    int size = parser.GetInt(file);
                    for (int i = 0; i < size - 8; i++)
                    {
                        file.ReadByte();
                    }
                }
                if (chunk_name == "TRA0")
                {
                    int size = parser.GetInt(file);
                    for (int i = 0; i < size - 8; i++)
                    {
                        file.ReadByte();
                    }
                }
                if (chunk_name == "TRA1")
                {
                    int size = parser.GetInt(file);
                    for (int i = 0; i < size - 8; i++)
                    {
                        file.ReadByte();
                    }
                }

            }
        }
        public GameObject ToGameObject()
        {
            GameObject obj = new GameObject();
            MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();

            meshFilter.mesh = CreateMesh(VNT0);

            return obj;
        }


        //----------------------------------------------------------------------------------------------------------

        private class VNT
        {
            public List<Vector3> position = new List<Vector3>();
            public List<Vector3> normal = new List<Vector3>();
            public List<Vector2> uv = new List<Vector2>();
            public List<int> triangles = new List<int>();
            Matrix4x4 matrix = new Matrix4x4();

        }

        private class SUB
        {
            public int nextSubmodel;
            public int firstSubmodel;
            public int submodelType;
            public int nameNumber;
            public int animationType;
            public int submodelFlags;
            public int viewMatrixNumber;
            public int vertexSize;
            public int firstVertexPosition;
            public int materialNumber;
            public float submodelLigntOnThreshold;
            //todo: Fill this
        }

        private Mesh CreateMesh(VNT VNT0)
        {
            Mesh mesh = new Mesh();
            mesh.vertices = VNT0.position.ToArray();
            mesh.normals = VNT0.normal.ToArray();
            mesh.uv = VNT0.uv.ToArray();
            mesh.triangles = VNT0.triangles.ToArray();

            return mesh;
        }



    }

}