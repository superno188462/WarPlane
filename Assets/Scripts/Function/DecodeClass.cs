using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecodeClass 
{
    public static void FillDict(Dictionary<string , string> dict,string str)
    {

        string[] strs = str.Split('\n');
        for (int i = 1; i < strs.Length; i++)
        {
            //strs[i].Trim();
            if (strs[i].Trim().Length < 3) continue;
            string[] strs2 = strs[i].Split('\t');
            dict.Add(strs2[0], strs2[1]);
        }
    }
    public static void FillDict(Dictionary<string, string> dict, string str, char split1, char split2)
    {
        dict.Clear();
        string[] strs = str.Split(split1);
        for (int i = 0; i < strs.Length; i++)
        {
            if (strs[i].Trim().Length < 3) continue;
            string[] strs2 = strs[i].Split(split2);
            dict.Add(strs2[0], strs2[1]);
        }
    }


    public static string Replace(string str)
    {
        str = str.Replace(',', '，');
        str = str.Replace(';', '；');
        return str;

    }
}
