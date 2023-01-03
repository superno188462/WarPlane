using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class WebLoad : Singleton<WebLoad>
{

    protected override void Awake()
    {
        base.Awake();
    }

    public static IEnumerator Load(string path, UnityAction<string> action)
    {
        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();
        string result = request.downloadHandler.text;
        action(result);
    }
    public static string Load(string path)
    {
        StreamReader read = new StreamReader(path, System.Text.Encoding.UTF8);
        string str = read.ReadToEnd();
        read.Close();
        read.Dispose();
        return str;
    }
    public static void CreatePath(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

    }
    public static void Save(string str,string path)
    {
        StreamWriter file1 = new StreamWriter(path, false, System.Text.Encoding.UTF8); 
        file1.Write(str);    //保存数据到文件
        file1.Close();      //关闭文件
        file1.Dispose();  
    }
}
