using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TcpPacker
{
    public static byte[] Package(byte[] data)
    {
        int header_len = 2;
        int data_len = data.Length+header_len;
        if (data.Length > 65535 - 2)
        {
            return null;
        }
        byte[] package = new byte[data_len];
        BytesWriter.Write_short_to_bytes(package, 0, (short)data_len);
        BytesWriter.Write_Byte_to_bytes(data,0,package, 2);
        return package;
    }
     

    public static byte[] UnPackage(byte[] data,out int header_len)
    {
        if(data.Length<2)
        {
            header_len = 0;
            return null;
        }   
        header_len = data[0] | data[1] << 8;
        if(header_len>data.Length)//接收的数据不足
        {
            return null;
        }
        byte[] package = new byte[header_len-2];
        BytesWriter.Write_Byte_to_bytes(data,2, package, 0);
        return package;
    }
}
