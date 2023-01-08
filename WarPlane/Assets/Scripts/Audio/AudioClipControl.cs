/***********************************************
 * \file        AudioClipControl.cs
 * \author      
 * \date        
 * \version     
 * \brief       未使用
 * \note        
 * \remarks     
 ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//找文件需要以下两个内容
using System.IO;
using System.Text;

public class ClipFile
{
    AudioClip clip;
    public ClipFile(AudioClip tmp)
    {
        clip = tmp;
    }
    public void play(AudioSource tmpSource)
    {
        tmpSource.clip = clip;
        tmpSource.Play();
    }

}

public class AudioClipControl
{
    public const string Url = "Assets/Resources/Audio/";
   
    //public string[] clipName;//存储需要下载的资源
    //public ClipFile[] clipFile;//存储已经下载好的Clip文件，和clipName一一对应

    public Dictionary<string, ClipFile> clipDictory; 
    
    public AudioClipControl()
    {
        clipDictory = new Dictionary<string, ClipFile>();
        ReadConfig();
    }

    //通过文件名找到Clip文件
    public ClipFile ClipFindByName(string tmpName)
    {
        #region
        //for (int i = 0; i < clipName.Length; i++)
        //{
        //    if (clipName[i] == tmpName)
        //        return clipFile[i];
        //}
        //return null;
        #endregion
        if(clipDictory.ContainsKey(tmpName))
        {
            return clipDictory[tmpName];
        }
        return null;
    }

    //下载文件
    public ClipFile LoadClip(string fileName)
    {
        #region
        //clipFile = new ClipFile[clipName.Length];
        //for (int i = 0; i < clipName.Length; i++)
        //{
        //    AudioClip tmpClip = PoolSystem.Instance.GetAudioClip(clipName[i]);
        //    ClipFile tmpClipFile = new ClipFile(tmpClip);
        //    clipFile[i] = tmpClipFile;

        //}
        #endregion
        AudioClip tmpClip = PoolSystem.Instance.GetAudioClip(Url+fileName);
        ClipFile tmpClipFile = new ClipFile(tmpClip);
        return tmpClipFile;

        
    }

    //读取配置文件并下载
    public void ReadConfig()
    {
        #region
        ////unity中需要在Assets下有一个StreamingAssets文件夹，然后读取的是该文件夹下的文件
        //var fileAddress = System.IO.Path.Combine(Application.streamingAssetsPath, "Audio/AudioClipConfig.txt");
        //FileInfo file = new FileInfo(fileAddress);
        //if (file.Exists)
        //{
        //    StreamReader r = new StreamReader(fileAddress);
        //    string tmpLine = r.ReadLine();
        //    int lineCount;
        //    //TryParse 尝试解析，这里是把第一行的内容解析成int类型
        //    if (int.TryParse(tmpLine, out lineCount))
        //    {
        //        clipName = new string[lineCount];

        //        for (int i = 0; i < lineCount; i++)
        //        {
        //            string line = r.ReadLine();

        //            string[] tmps = line.Split(" ".ToCharArray());//()内需要字符，也可以单引号的形式提供
        //            clipName[i] = tmps[0];//配置文件一般格式如下 Collectable Collectable.wav我们取第一个参数（变量名）

        //        }
        //    }
        //    r.Close();
        //}
        //else
        //{
        //    Debug.Log("there are no file");
        //}
        #endregion
        string content = WebLoad.Load("Assets/Resources/Audio/AudioClipConfig.txt");
        FillDict(clipDictory, content);
    }

    public void FillDict(Dictionary<string, ClipFile> dict, string str)
    {
        string[] lines = str.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Trim().Length < 3) continue;
            string[] strs2 = lines[i].Split('\t');
            string name = strs2[0];
            string fileName = strs2[1];
            ClipFile tmpClipFile = LoadClip(fileName);
            dict.Add(name, tmpClipFile);
        }
    }

}
