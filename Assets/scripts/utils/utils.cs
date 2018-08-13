using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class utils {

    public static string rand_str(int len) {
        byte[] b = new byte[4];
        new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
        System.Random r = new System.Random(System.BitConverter.ToInt32(b, 0));

        string str = null;
        str += "0123456789";
        str += "abcdefghijklmnopqrstuvwxyz";
        str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        string s = null;

        for (int i = 0; i < len; i++)
        {
            s += str.Substring(r.Next(0, str.Length - 1), 1);
        }
        return s;
    }

    public static string GenMd5(string str)
    {
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] md5_code = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));

        StringBuilder md5_str = new StringBuilder();
        for(int i = 0;i<md5_code.Length;i++)
        {
            md5_str.Append(md5_code[i].ToString("X2"));
        }
        return md5_str.ToString();
    }
}
