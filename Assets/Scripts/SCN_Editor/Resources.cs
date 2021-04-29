using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Resources
{
    public class E3D_model
    {
        BinaryParser parser = new BinaryParser();
        List<SUB> submodel_data = new List<SUB>();
        List<TEX> TEX0 = new List<TEX>();
        List<NAM> NAM0 = new List<NAM>();
        List<TRA> TRA0 = new List<TRA>();
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
                if (chunk_name == "SUB0")//loading submodel data
                {
                    int size = parser.GetInt(file);
                    size = (size - 8) / 256;
                    for (int i = 0; i < size; i++)
                    {
                        SUB SUB0 = new SUB();
                        SUB0.nextSubmodel = parser.GetInt(file);
                        SUB0.firstSubmodel = parser.GetInt(file);
                        SUB0.submodelType = parser.GetInt(file);
                        SUB0.nameNumber = parser.GetInt(file);
                        SUB0.animationType = parser.GetInt(file);
                        SUB0.submodelFlags = parser.GetInt(file);
                        SUB0.viewMatrixNumber = parser.GetInt(file);
                        SUB0.vertexSize = parser.GetInt(file);
                        SUB0.firstVertexPosition = parser.GetInt(file);
                        SUB0.materialNumber = parser.GetInt(file);
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
                        VNT0.uv.Add(new Vector2(parser.GetFloat(file), parser.GetFloat(file)));
                    }
                }
                if (chunk_name == "TEX0") //loading material names
                {
                    int size = parser.GetInt(file) - 8;
                    long counterStart = file.Position; //początek czytanego fragmentu pliku
                    long counterEnd = counterStart + size; //koniec czytanego fragmentu pliku
                    while (file.Position < counterEnd)
                    {
                        TEX _TEX0 = new TEX();
                        _TEX0.materialName = parser.GetName(file);
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
                        NAM _NAM0 = new NAM();
                        _NAM0.submodelName = parser.GetName(file);
                        NAM0.Add(_NAM0);
                    }
                }
                if (chunk_name == "TRA0") //loading transform matrix
                {
                    int size = parser.GetInt(file);
                    size = (size - 8) / 64;
                    for (int i = 0; i < size; i++)
                    {
                        TRA _TRA0 = new TRA();
                        _TRA0.transformMatrix = parser.GetTransformMatrix(file);
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
            //TODO: convert to gameobject.
            return null;
        }


        //----------------------------------------------------------------------------------------------------------



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

}