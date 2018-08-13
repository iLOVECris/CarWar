using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using ProtoBuf;

public class EncodeCmd
{

    static byte[] Serialize(ProtoBuf.IExtensible data)
    {
        try
        {
            using (MemoryStream m = new MemoryStream())
            {
                byte[] buffer = null;
                Serializer.Serialize(m, data);
                m.Position = 0;
                int length = (int)m.Length;
                buffer = new byte[length];
                m.Read(buffer, 0, length);
                return buffer;
            }
        }
        catch (Exception ex)
        {
            Debug.Log("序列化失败: " + ex.ToString());
            return null;
        }
    }




    public static byte[] Encode_Protobuf(int stype,int ctype, ProtoBuf.IExtensible body)
    {
        int Header_Len = 8;
        byte[] byte_array = null;
        int body_len = 0;
        if (body!=null)
        {
            byte_array = Serialize(body);
            body_len = byte_array.Length;
        }
        byte[] Encode_Bytes = new byte[Header_Len + body_len];
        BytesWriter.Write_short_to_bytes(Encode_Bytes, 0, (short)stype);
        BytesWriter.Write_short_to_bytes(Encode_Bytes, 2, (short)ctype);
        if(body != null)
        {
            BytesWriter.Write_Byte_to_bytes(byte_array, 0, Encode_Bytes, 8);
        }    
        return Encode_Bytes;
    }
    public static byte[] Encode_Json(int stype,int ctype,string json_str)
    {
        int Header_Len = 8;
        byte[] byte_array = null;
        if (json_str != null)
        {
            byte_array = System.Text.Encoding.UTF8.GetBytes(json_str);
        }
        byte[] Encode_Bytes = new byte[Header_Len + byte_array.Length];
        BytesWriter.Write_short_to_bytes(Encode_Bytes, 0, (short)stype);
        BytesWriter.Write_short_to_bytes(Encode_Bytes, 2, (short)ctype);
        BytesWriter.Write_Byte_to_bytes(byte_array, 0, Encode_Bytes, 8);
        return Encode_Bytes;
    }
    
}
