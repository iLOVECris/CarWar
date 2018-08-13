using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
using System.Security.Cryptography;

public class SecretInfo
{
    public static string ToMd5(string str)
    {
        byte[] result = Encoding.UTF8.GetBytes(str);
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] output = md5.ComputeHash(result);
        return (BitConverter.ToString(output).Replace("-", "")).ToLower();
    }

    public static string ToBase64(string text)
    {
        byte[] bys = Encoding.UTF8.GetBytes(text);
        return Convert.ToBase64String(bys);
    }

    public static string FromBase64(string text)
    {
        if (text.Contains("%")) return Uri.UnescapeDataString(text);
        byte[] bys = Convert.FromBase64String(text);
        return Encoding.UTF8.GetString(bys);
    }

    public static void Encrypt(string inFile, string outFile)
    {
        byte[] bytes = File.ReadAllBytes(inFile);
        short[] BitKeys = new short[] { 6, 2, 8, 0, 6, 8, 9, 1 };
        BitCryto m_bitCryto = new BitCryto(BitKeys);
        for (int i = 0, imax = bytes.Length; i < imax; ++i)
            bytes[i] = m_bitCryto.Encode(bytes[i]);
        File.WriteAllBytes(outFile, bytes);
    }

    public static void Encrypt(byte[] bytes)
    {
        short[] BitKeys = new short[] { 6, 2, 8, 0, 6, 8, 9, 1 };
        BitCryto m_bitCryto = new BitCryto(BitKeys);
        for (int i = 0, imax = bytes.Length; i < imax; ++i)
            bytes[i] = m_bitCryto.Encode(bytes[i]);
    }

    public static void Decrypt(string inFile, string outFile)
    {
        byte[] bytes = File.ReadAllBytes(inFile);
        short[] BitKeys = new short[] { 6, 2, 8, 0, 6, 8, 9, 1 };
        BitCryto m_bitCryto = new BitCryto(BitKeys);
        for (int i = 0, imax = bytes.Length; i < imax; ++i)
            bytes[i] = m_bitCryto.Decode(bytes[i]);
        File.WriteAllBytes(outFile, bytes);
    }

    public static void Decrypt(byte[] bytes)
    {
        short[] BitKeys = new short[] { 6, 2, 8, 0, 6, 8, 9, 1 };
        BitCryto m_bitCryto = new BitCryto(BitKeys);
        for (int i = 0, imax = bytes.Length; i < imax; ++i)
            bytes[i] = m_bitCryto.Decode(bytes[i]);
    }

    public static string EncryptToSHA256(string str)
    {
        SHA256 sha256 = new SHA256Managed();
        byte[] str1 = Encoding.UTF8.GetBytes(str);
        byte[] str2 = sha256.ComputeHash(str1);
        return Convert.ToBase64String(str2);
    }

}
