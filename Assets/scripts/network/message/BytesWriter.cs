using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BytesWriter
{
    public static void Write_int_to_bytes(byte[] src,int offset,int data)
    {
        byte[] intBuff = BitConverter.GetBytes(data);
        if (!BitConverter.IsLittleEndian)
        {
            Array.Reverse(intBuff);
        }
        Array.Copy(intBuff, 0, src, offset, intBuff.Length);
    }

    public static void Write_short_to_bytes(byte[] src, int offset,short data)
    {
        byte[] shortBuff = BitConverter.GetBytes(data);
        if (!BitConverter.IsLittleEndian)
        {
            Array.Reverse(shortBuff);
        }
        Array.Copy(shortBuff, 0, src, offset, shortBuff.Length);
    }

    public static void Write_Byte_to_bytes(byte[] src,int src_offset,byte[] des, int des_offset)
    {
        Array.Copy(src, src_offset, des, des_offset, des.Length- des_offset);
    }
}
