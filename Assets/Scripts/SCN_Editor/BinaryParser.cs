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


}
