using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPacket
{
    private Dictionary<string, string> dataDict;
    public Dictionary<string, string> DataDict
    {
        get { return dataDict; }
    }

    public DataPacket(string str)
    {
        dataDict = new Dictionary<string, string>();
        DecodeClass.FillDict(dataDict, str);
    }
    public DataPacket(string str, char split1, char split2)
    {
        dataDict = new Dictionary<string, string>();
        DecodeClass.FillDict(dataDict, str, split1, split2);
    }

    public string GetData<T>(T t)
    {
        return dataDict[t.ToString()];
    }
    public float GetDataF<T>(T t)
    {
        return float.Parse(dataDict[t.ToString()]);
    }

    public string GetDataStr()
    {
        string str = "";
        foreach (KeyValuePair<string, string> i in DataDict)
        {
            str += $"{i.Key },{ i.Value};";
        }
        return str;
    }
}
