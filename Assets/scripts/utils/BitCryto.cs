using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitCryto {


    private short[] crytoKey;
    private int offsetOfKey;

    public BitCryto(short[] sKey)
    {
        crytoKey = sKey;
        offsetOfKey = 0;
    }
    public byte Encode(byte inputByte)
    {
        if(offsetOfKey>=crytoKey.Length)
        {
            offsetOfKey = 0;
        }
        short offset = (short)crytoKey[offsetOfKey++];
        short outputByte = (short)((short)inputByte - offset);
        if(outputByte<0)
        {
            outputByte += 256;
        }
        return (byte)outputByte;

    }
    public byte Decode(byte inputByte)
    {
        if(offsetOfKey>=crytoKey.Length)
        {
            offsetOfKey = 0;
        }

        short offset = (short)crytoKey[offsetOfKey++];
        short outputByte = (short)((short)inputByte + offset);

        if(outputByte>=256)
        {
            outputByte -= 256;
        }
        return (byte)outputByte;
    }

    public void Reset()
    {
        offsetOfKey = 0;
    }

}
