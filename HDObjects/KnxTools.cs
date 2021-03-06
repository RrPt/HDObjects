﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace HomeData
{
    public class KnxTools
    {
        // Tools
        public static string BytesToString(byte[] receiveBytes)
        {
            if (receiveBytes==null) return "<nix>";
            String erg =  DateTime.Now.ToString("HH:mm:ss.fff")+":  ";
            for (int i = 0; i < receiveBytes.Length; i++) erg = erg + receiveBytes[i].ToString("X2") + " ";
            return erg;
        }


        public static string BytesToTrxString(byte[] receiveBytes)
        {
            if (receiveBytes == null) return "<nix>";
            String erg = DateTime.Now.ToString("HH:mm:ss.fff") + "\t";
            erg = erg + receiveBytes[0].ToString("X2") + " ";
            erg = erg + receiveBytes[2].ToString("X2") + " ";
            for (int i = 4; i < receiveBytes.Length; i++)
            {
                if (i == 8)
                {

                    erg = erg + (receiveBytes[i]+receiveBytes[3]).ToString("X2") + " ";
                }
                else
                    erg = erg + receiveBytes[i].ToString("X2") + " ";
            }
            return erg;
        }


    }
}
