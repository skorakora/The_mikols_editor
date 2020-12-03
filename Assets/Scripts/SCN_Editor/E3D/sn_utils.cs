using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Sn_utils
{
    // deserialize little endian uint16
    public UInt16 ld_uint16 (FileStream s)
    {
        byte[] buf = new byte[2];
        s.Read(buf,0,2);
        return BitConverter.ToUInt16(buf, 0);
    }

    // deserialize little endian uint32
    public UInt32 ld_uint32(FileStream s)
    {
        byte[] buf = new byte[4];
        s.Read(buf, 0, 4);
        return BitConverter.ToUInt32(buf, 0);
    }

    // deserialize little endian int32
    public Int32 ld_int32(FileStream s)
    {
        byte[] buf = new byte[4];
        s.Read(buf, 0, 4);
        return BitConverter.ToInt32(buf, 0);
    }

    // deserialize little endian float32
    public float ld_float32(FileStream s)
    {
        byte[] buf = new byte[4];
        s.Read(buf, 0, 4);
        return BitConverter.ToSingle(buf, 0);
    }

    // deserialize little endian float64
    public double ld_float64(FileStream s)
    {
        byte[] buf = new byte[8];
        s.Read(buf, 0, 8);
        return BitConverter.ToDouble(buf, 0);
    }


}
 
