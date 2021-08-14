using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace Resources
{
    public class E3D_model
    {
        string ObjectName;
        BinaryParser parser = new BinaryParser();
        List<SUB> submodel_data = new List<SUB>();
        List<TEX> TEX0 = new List<TEX>();
        List<NAM> NAM0 = new List<NAM>();
        List<TRA> TRA0 = new List<TRA>();
        List<GameObject> submodelList = new List<GameObject>();
        VNT VNT0 = new VNT();
        Math math = new Math();

        //------------------------------------------methods---------------------------------------------------------

        public E3D_model(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError("ERROR: E3D file not found!");
                return;
            }

            FileStream file = File.OpenRead(path);
            ObjectName = Path.GetFileNameWithoutExtension(path);
            if (!(parser.GetString(file, 4) == "E3D0"))
            {
                Debug.LogError("ERROR: Unknown main chunk");
                return;
            }
            int MainChunkSize = parser.GetInt(file);
            while (file.Position < MainChunkSize - 8)
            {
                string chunk_name = parser.GetString(file, 4);
                if (chunk_name == "SUB0")//loading submodel data
                {
                    int size = parser.GetInt(file);
                    size = (size - 8) / 256;
                    for (int i = 0; i < size; i++)
                    {
                        SUB SUB0 = new SUB
                        {
                            nextSubmodel = parser.GetInt(file),
                            firstSubmodel = parser.GetInt(file),
                            submodelType = parser.GetInt(file),
                            nameNumber = parser.GetInt(file),
                            animationType = parser.GetInt(file),
                            submodelFlags = parser.GetInt(file),
                            viewMatrixNumber = parser.GetInt(file),
                            vertexSize = parser.GetInt(file),
                            firstVertexPosition = parser.GetInt(file),
                            materialNumber = parser.GetInt(file)
                        };
                        parser.Skip(file, 4);//skip - submodelLigntOnBrightnessThreshold
                        parser.Skip(file, 4);//skip - submodelLightOnThreshold
                        parser.Skip(file, 16);//skip - RGBAcolorAmbient
                        parser.Skip(file, 16);//skip - RGBAdiffuseColor
                        parser.Skip(file, 16);//skip - RGBAspecularColor
                        parser.Skip(file, 16);//skip - RGBAselfillumColor
                        SUB0.lineSize = parser.GetFloat(file);
                        SUB0.maxDistance = parser.GetFloat(file);
                        SUB0.minDistance = parser.GetFloat(file);
                        parser.Skip(file, 32);//skip - lightParameters
                        parser.Skip(file, 100);


                        submodel_data.Add(SUB0);
                    }
                }
                if (chunk_name == "SUB1") //currently unused
                {
                    int size = parser.GetInt(file);
                    for (int i = 0; i < size - 8; i++)
                    {
                        file.ReadByte();
                    }
                }
                if (chunk_name == "VNT0") //loading mesh data
                {
                    int size = parser.GetInt(file);
                    size = (size - 8) / 32;
                    for (int i = 0; i < size; i++)
                    {
                        
                        VNT0.position.Add(new Vector3(parser.GetFloat(file), parser.GetFloat(file), parser.GetFloat(file)));
                        VNT0.normal.Add(new Vector3(parser.GetFloat(file), parser.GetFloat(file), parser.GetFloat(file)));
                        float U = parser.GetFloat(file);
                        float V = parser.GetFloat(file);
                        VNT0.uv.Add(new Vector2(U,-V));
                    }
                }
                if (chunk_name == "TEX0") //loading material names
                {
                    int size = parser.GetInt(file) - 8;
                    long counterStart = file.Position; //początek czytanego fragmentu pliku
                    long counterEnd = counterStart + size; //koniec czytanego fragmentu pliku
                    while (file.Position < counterEnd)
                    {
                        TEX _TEX0 = new TEX
                        {
                            materialName = parser.GetName(file)
                        };
                        TEX0.Add(_TEX0);
                    }

                }
                if (chunk_name == "NAM0") //loading submodel names
                {
                    int size = parser.GetInt(file) - 8;
                    long counterStart = file.Position; //początek czytanego fragmentu pliku
                    long counterEnd = counterStart + size; //koniec czytanego fragmentu pliku
                    while (file.Position < counterEnd)
                    {
                        NAM _NAM0 = new NAM
                        {
                            submodelName = parser.GetName(file)
                        };
                        NAM0.Add(_NAM0);
                    }
                }
                if (chunk_name == "TRA0") //loading transform matrix
                {
                    int size = parser.GetInt(file);
                    size = (size - 8) / 64;
                    for (int i = 0; i < size; i++)
                    {
                        TRA _TRA0 = new TRA
                        {
                            transformMatrix = parser.GetTransformMatrix(file)
                        };
                        TRA0.Add(_TRA0);
                    }
                }
                if (chunk_name == "TRA1")//unused
                {
                    Debug.LogWarning("Unsuported chunk detected! TRA1 is currently not supported");
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

            Math math = new Math();
            GameObject root = new GameObject();
            root.name = ObjectName;
            submodelList = GetSubmodelList();

            int submodelSize = submodel_data.ToArray().Length;
            int currentObj = 0;
            while (currentObj != -1)
            {
                if (submodel_data[currentObj].firstSubmodel != -1)
                {
                    SetChierarchy(currentObj);
                }
                submodelList[currentObj].transform.parent = root.transform;
                currentObj = submodel_data[currentObj].nextSubmodel;
            }

            for (int i = 0; i < submodelSize; i++)
            {
                if (i >= TRA0.ToArray().Length)
                {
                    break;
                }
                GameObject obj = submodelList[i];
                MeshFilter meshfilter = obj.GetComponent(typeof(MeshFilter)) as MeshFilter;
                meshfilter.mesh =  CreateMesh(GetTransformMatrix(i), i);
            }


            for (int i = 0; i < submodelSize; i++)
            {
                //submodelList[i].name = NAM0[i].submodelName;
            }

            return root;
        }


        //----------------------------------------------------------------------------------------------------------

        //-----------private methods-----------------

        private void SetChierarchy(int rootNumber)
        {
            int currentObj = submodel_data[rootNumber].firstSubmodel;
            while (currentObj != -1)
            {
                if (submodel_data[currentObj].firstSubmodel != -1)
                {
                    SetChierarchy(currentObj);
                }
                submodelList[currentObj].transform.parent = submodelList[rootNumber].transform;
                currentObj = submodel_data[currentObj].nextSubmodel;
            }

        }

        private float[,] GetTransformMatrix(int objNumber)
        {
            float[,] matrix = TRA0[objNumber].transformMatrix;
            List<float[,]> transformMatrixList = new List<float[,]>();
            GameObject pointer = submodelList[objNumber];
            pointer = pointer.transform.parent.gameObject;
            while (pointer.name != ObjectName)
            {
                transformMatrixList.Add(TRA0[int.Parse(pointer.name)].transformMatrix);
                pointer = pointer.transform.parent.gameObject;
            }
            for (int i = transformMatrixList.ToArray().Length; i > 1; i--)
            {
                matrix = math.MultiplyTransformMatrix(matrix, transformMatrixList[i-2]);
            }
            return matrix;
        }
        private Mesh CreateMesh(float[,] transformMatrix, int objNumber)
        {
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uv = new List<Vector2>();
            List<int> triangles = new List<int>();
            int position = submodel_data[objNumber].firstVertexPosition;
            for (int i = 0; i < submodel_data[objNumber].vertexSize; i++)
            {
                
                vertices.Add(math.MultiplyTransformMatrixByVector(transformMatrix, VNT0.position[position + i]));
                normals.Add(math.MultiplyTransformMatrixByVector(transformMatrix, VNT0.normal[position + i]));
                uv.Add(VNT0.uv[position + i]);
                triangles.Add(i);
            }

            mesh.vertices = vertices.ToArray();
            mesh.normals = normals.ToArray();
            mesh.uv = uv.ToArray();
            mesh.triangles = triangles.ToArray();

            return mesh;
        }

        List<GameObject> GetSubmodelList()
        {
            List<GameObject> _objList = new List<GameObject>();
            for (int i = 0; i < submodel_data.ToArray().Length; i++)
            {
                GameObject obj = new GameObject();
                obj.name = submodel_data[i].nameNumber.ToString();
                obj.AddComponent(typeof(MeshRenderer));
                obj.AddComponent(typeof(MeshFilter));
                _objList.Add(obj);
            }
            return _objList;
        }


        //--------------Data blocks-----------------

        private class VNT
        {
            public List<Vector3> position = new List<Vector3>();
            public List<Vector3> normal = new List<Vector3>();
            public List<Vector2> uv = new List<Vector2>();
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
            //public float submodelLigntOnBrightnessThreshold; - unused 4 bytes
            //public float submodelLightOnThreshold; - unused 4 bytes
            //public float[] RGBAcolorAmbient = new float[4]; - unused 16 bytes
            //public float[] RGBAdiffuseColor = new float[4]; - unused 16 bytes
            //public float[] RGBAspecularColor = new float[4]; - unused 16 bytes
            //public float[] RGBAselfillumColor = new float[4]; - unused 16 bytes
            public float lineSize;
            public float maxDistance;
            public float minDistance;
            //public float[] lightParameters new float[8]; - unused 32 bytes
        }

        private class TEX
        {
            public string materialName;
        }

        private class NAM
        {
            public string submodelName;
        }

        private class TRA
        {
            public float[,] transformMatrix = new float[4, 4];
        }

        private class TRA_1 //unused
        {
            public double[,] transformMatrix = new double[4, 4];
        }


    }
    public class ResourceLoader
    {
        public Texture2D LoadTexture(string tex_path)
        {
            Globals.Simulator_root = "C:\\Program Files (x86)\\Maszyna";


            Debug.Log("Loading texture: " + Globals.Simulator_root + "\\textures\\" + tex_path+".dds");


            if (!File.Exists(Globals.Simulator_root + "\\textures\\" + tex_path + ".dds"))
            {
                Debug.LogError("ERROR: File not found");
                return GetPinkTex();
            }

            byte[] ddsData = File.ReadAllBytes(Globals.Simulator_root + "\\textures\\" + tex_path + ".dds");
                if (ddsData[4] != 124)
            {
                Debug.LogError("ERROR: Wrong DDS header, unable to load");
                return GetPinkTex();
            }

            int height = ddsData[13] * 256 + ddsData[12];
            int width = ddsData[17] * 256 + ddsData[16];

            int DDS_HEADER_SIZE = 128;
            byte[] dxtBytes = new byte[ddsData.Length - DDS_HEADER_SIZE];
            Buffer.BlockCopy(ddsData, DDS_HEADER_SIZE, dxtBytes, 0, ddsData.Length - DDS_HEADER_SIZE);


            Texture2D tex = new Texture2D(width, height,TextureFormat.DXT1,false);
            tex.LoadRawTextureData(dxtBytes);
            tex.Apply();

            Debug.Log("Texture load ok");

            return tex;
        }

        //------------------------------------------PRIVATE-----------------------------------------------------
        private Texture2D GetPinkTex()
        {
            Texture2D tex = new Texture2D(512, 512);
            Color fillColor = new Color(255, 192, 203);
            Color[] fillColorArray = tex.GetPixels();

            for (int i = 0; i < fillColorArray.Length; i++)
            {
                fillColorArray[i] = fillColor;
            }

            tex.SetPixels(fillColorArray);
            tex.Apply();

            return tex;
        }
    }
}