﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SevenZip.Compression.LZMA;
using System.IO;
using System;

public class LZMAUtil
{
    public static void CompressFileLZMA(string inFile, string outFile)
    {
        SevenZip.Compression.LZMA.Encoder coder = new SevenZip.Compression.LZMA.Encoder();
        FileStream input = new FileStream(inFile, FileMode.Open);
        FileStream output = new FileStream(outFile, FileMode.Create);

        // Write the encoder properties
        coder.WriteCoderProperties(output);

        // Write the decompressed file size.
        output.Write(BitConverter.GetBytes(input.Length), 0, 8);

        // Encode the file.
        coder.Code(input, output, input.Length, -1, null);
        output.Flush();
        output.Close();
        input.Close();
    }

    public static void DecompressFileLZMA(string inFile, string outFile)
    {
        SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
        FileStream input = new FileStream(inFile, FileMode.Open);
        FileStream output = new FileStream(outFile, FileMode.Create);

        // Read the decoder properties
        byte[] properties = new byte[5];
        input.Read(properties, 0, 5);

        // Read in the decompress file size.
        byte[] fileLengthBytes = new byte[8];
        input.Read(fileLengthBytes, 0, 8);
        long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

        // Decompress the file.
        coder.SetDecoderProperties(properties);
        coder.Code(input, output, input.Length, fileLength, null);
        output.Flush();
        output.Close();
        input.Close();
    }
    public static void DecompressFileLZMA(byte[] inFileByteArray,string outFile,SevenZip.ICodeProgress progress = null,System.Action codeCompelete = null)
    {
        SevenZip.Compression.LZMA.Decoder decoder = new Decoder();
        using (var input = new MemoryStream(inFileByteArray))
        {
            using (var output = File.Create(outFile))
            {
                byte[] properties = new byte[5];
                input.Read(properties, 0, 5);

                byte[] fileLengthBytes = new byte[8];
                input.Read(fileLengthBytes, 0, 8);
                long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

                decoder.SetDecoderProperties(properties);
                decoder.Code(input, output, input.Length, fileLength, progress);
            }
        }
        if (codeCompelete != null)
            codeCompelete();
    }
}
