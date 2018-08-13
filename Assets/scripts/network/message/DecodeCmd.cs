using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using ProtoBuf;
using ProtoBuf.Meta;

public class DecodeCmd
{
    public static T Deserialize<T>(byte[] data)
    {
        using (MemoryStream m = new MemoryStream(data))
        {
            return Serializer.Deserialize<T>(m);         
        }
    }

    public static object Deserialize(byte[] data,Type type)
    {
        using (MemoryStream m = new MemoryStream(data))
        {
            return RuntimeTypeModel.Default.Deserialize(m, null, type);
        }
    }
    public static msg_cmd Decode_Protobuf(byte[] package)
    {
        msg_cmd cmd = new msg_cmd();
        cmd.stype = package[0] | (package[1]<<8);
        cmd.ctype = package[2] | (package[3]<<8);
        cmd.body = new byte[package.Length - 8];
        BytesWriter.Write_Byte_to_bytes(package, 8, cmd.body, 0);
        return cmd;
    }
    public static msg_cmd Decode_Json(byte[] package)
    {
        msg_cmd cmd = new msg_cmd();
        cmd.stype = package[0] | (package[1] << 8);
        cmd.ctype = package[2] | (package[3] << 8);
        cmd.body = new byte[package.Length - 8];
        BytesWriter.Write_Byte_to_bytes(package, 8, cmd.body, 0);
        return cmd;
    }
}
