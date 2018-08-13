using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Threading;

namespace Compress.LZMAFile
{
    public delegate void ProgressDelegate(Int64 filesize, Int64 processSize);


    public class FileChangeInfo
    {
        public string inPath;
        public string outPath;
        public ProgressDelegate progress;
    }


    public class FileByteChangeInfo
    {
        public byte[] inPathByteArray;
        public string outPath;
        public ProgressDelegate progress;
        public System.Action codeComplete;

    }

    public class CodeProgress:SevenZip.ICodeProgress
    {
        public ProgressDelegate progress = null;
        public CodeProgress(ProgressDelegate del)
        {
            progress = del;
        }
        public void SetProgress(Int64 inSize,Int64 outSize)
        {

        }

        public void SetProgressPercent(Int64 fileSize,Int64 progressSize)
        {
            if(progress!=null)
            {
                progress(fileSize, progressSize);
            }
        }

    }

    public class LZMAFile
    {
        public static void Decompress(string inFile, string outFile, SevenZip.ICodeProgress progress = null)
        {
            SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
            using (var input = File.Create(inFile))
            {
                using (var output = File.Create(outFile))
                {
                    int propertiesSize = SevenZip.Compression.LZMA.Encoder.kPropSize;
                    byte[] properties = new byte[propertiesSize];
                    input.Read(properties, 0, propertiesSize);
                    byte[] fileLengthBytes = new byte[8];
                    input.Read(fileLengthBytes, 0, 8);
                    long fileLength = System.BitConverter.ToInt64(fileLengthBytes, 0);
                    decoder.SetDecoderProperties(properties);
                    decoder.Code(input, output, input.Length, fileLength, progress);

                }
            }
        }
        public static void Decompress(string inFile, string outFile, ProgressDelegate progress)
        {
            CodeProgress codeProgress = new CodeProgress(progress);
            Decompress(inFile, outFile, codeProgress);
        }
        public static void Decompress(byte[] inFileByteArray, string outFile, SevenZip.ICodeProgress progress = null, System.Action codeComplete = null)
        {
            SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
            using (var input = new MemoryStream(inFileByteArray))
            {
                using (var output = File.Create(outFile))
                {
                    byte[] properties = new byte[5];
                    input.Read(properties, 0, 5);
                    byte[] fileLengthBytes = new byte[8];
                    input.Read(fileLengthBytes, 0, 8);
                    long fileLength = System.BitConverter.ToInt64(fileLengthBytes, 0);
                    decoder.SetDecoderProperties(properties);
                    decoder.Code(input, output, input.Length, fileLength, progress);
                }
            }
            if (codeComplete != null)
            {
                codeComplete();
            }
        }

        public static void DecompressAsync(string inFile, string outFile, ProgressDelegate progress = null)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(DecompressInternal));
            FileChangeInfo info = new FileChangeInfo()
            {
                inPath = inFile,
                outPath = outFile,
                progress = progress
            };
            thread.Start(info);
        }

        public static void DecompressAsync(byte[] inFileByteArray,string outFile,ProgressDelegate progress = null,System.Action codeCompeleteFunc = null)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(DecompressByteInternal));
            FileByteChangeInfo info = new FileByteChangeInfo()
            {
                inPathByteArray = inFileByteArray,
                outPath = outFile,
                progress = progress,
                codeComplete = codeCompeleteFunc
            };
            thread.Start(info);
        }
        /// <summary>
        /// 同步压缩
        /// </summary>
        /// <param name="inFile"></param>
        /// <param name="outFile"></param>
        /// <param name="progress"></param>
        public static void Compress(string inFile,string outFile,SevenZip.ICodeProgress progress = null)
        {
            SevenZip.Compression.LZMA.Encoder coder = new SevenZip.Compression.LZMA.Encoder();
            using (var input = File.OpenRead(inFile))
            {
                using (var output = File.Create(outFile))
                {
                    coder.WriteCoderProperties(output);
                    output.Write(System.BitConverter.GetBytes(input.Length), 0, 8);
                    coder.Code(input, output, input.Length, -1, progress);
                }
            }
        }
        /// <summary>
        /// 同步压缩
        /// </summary>
        /// <param name="inFile"></param>
        /// <param name="outFile"></param>
        /// <param name="progress"></param>
        public static void Compress(string inFile, string outFile, ProgressDelegate progress)
        {
            CodeProgress codeProgress = new CodeProgress(progress);
            Compress(inFile, outFile, codeProgress);
        }

        /// <summary>
        /// 压缩内部实现
        /// </summary>
        /// <param name="obj"></param>
        public static void CompressInternal(object obj)
        {
            FileChangeInfo info = (FileChangeInfo)obj;
            string inpath = info.inPath;
            string outpath = info.outPath;
            CodeProgress codeProgress = null;
            if(info.progress!=null)
            {
                codeProgress = new CodeProgress(info.progress);
            }
            Compress(inpath, outpath, codeProgress);

        } 
        /// <summary>
        /// 解压缩内部实现
        /// </summary>
        /// <param name="obj"></param>
        private static void DecompressInternal(object obj)
        {
            FileChangeInfo info = (FileChangeInfo)obj;
            string inpath = info.inPath;
            string outpath = info.outPath;
            CodeProgress codeProgress = null;
            if (info.progress != null)
            {
                codeProgress = new CodeProgress(info.progress);
            }
            Decompress(inpath, outpath, codeProgress);
        }
        /// <summary>
        /// 解压缩内部实现
        /// </summary>
        /// <param name="obj"></param>
        private static void DecompressByteInternal(object obj)
        {
            FileByteChangeInfo info = (FileByteChangeInfo)obj;
            CodeProgress codeProgress = null;
            if (info.progress != null)
            {
                codeProgress = new CodeProgress(info.progress);
            }
            Decompress(info.inPathByteArray, info.outPath, codeProgress,info.codeComplete);
        }

    }

}

