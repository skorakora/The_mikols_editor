using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class BinaryParser
{
    public string GetString(FileStream file, int offset) //czyta string z binarki
    {
        byte[] buf = new byte[offset];
        for (int i = 0; i < offset; i++)
        {
            buf[i] = Convert.ToByte(file.ReadByte());
        }
        return System.Text.Encoding.UTF8.GetString(buf); ;
    }

    public string GetName(FileStream file) //to samo co get string, tylko że czyta do bajtu "0", używane tylko przy chunkach "TEX" i "NAM"
    {
        List<byte> buf = new List<byte>();
        while (true)
        {
            int data = file.ReadByte();
            if (data == -1)
            {
                Debug.LogError("E3D: reached end of file, while reading name. Is zero byte missing ?");
                break;
            }
            if (data == 0)
            {
                break;
            }
            buf.Add(Convert.ToByte(data));
        }

        return System.Text.Encoding.UTF8.GetString(buf.ToArray());
    }

    public float GetFloat(FileStream file) //czyta float z pliku binarnego
    {
        byte[] buf = new byte[4];
        for (int i = 0; i < 4; i++)
        {
            buf[i] = Convert.ToByte(file.ReadByte());
        }
        return System.BitConverter.ToSingle(buf, 0);
    }

    public int GetInt(FileStream file) // czyta Int z pliku binarnego
    {
        byte[] buf = new byte[4];
        for (int i = 0; i < 4; i++)
        {
            buf[i] = Convert.ToByte(file.ReadByte());
        }
        return BitConverter.ToInt32(buf, 0);
    }
    public UInt32 GetUint32(FileStream file) // czyta uint32 z pliku binarnego
    {
        byte[] buf = new byte[4];
        for (int i = 0; i < 4; i++)
        {
            buf[i] = Convert.ToByte(file.ReadByte());
        }
        return System.BitConverter.ToUInt32(buf, 0);
    }

    public float[,] GetTransformMatrix(FileStream file)
    {
        float[,] TransformMatrix = new float[4, 4];
        for (int i = 0; i < 4; i++)
        {
            for (int i2 = 0; i2 < 4; i2++)
            {
                TransformMatrix[i2, i] = GetFloat(file);
            }
        }
        return TransformMatrix;
    }
    public void Skip(FileStream file,int offset)
    {
        for (int i = 0; i < offset; i++)
        {
            file.ReadByte();
        }
    }


}
