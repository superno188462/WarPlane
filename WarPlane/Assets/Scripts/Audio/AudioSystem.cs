/***********************************************
 * \file        AudioSystem.cs
 * \author      
 * \date        
 * \version     
 * \brief       音效系统
 * \note        
 * \remarks     
 ***********************************************/
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

public class AudioSystem : Singleton<AudioSystem>
{
    [Header("播放器")]
    public AudioSource audio;

    [Header("全局音量")]
    public float AudioVolume;

    //音效字典，根据名称和音效一一绑定
    private Dictionary<string, AudioClip> dict;

    //初始化音效系统，实例化字典，分配播放器
    protected override void Awake()
    {
        base.Awake();
        dict = new Dictionary<string, AudioClip>();
        audio = gameObject.AddComponent<AudioSource>();
        Init();

    }

    //初始化字典，根据文件内容，按照一定规则将音效说明文本和音效绑定
    private void Init()
    {
        string content = WebLoad.Load(Application.streamingAssetsPath+ "/Config/AudioClipConfig.txt");
        string[] lines = content.Split('\n');
        //Debug.Log(content);
        for (int i = 0; i < lines.Length-1; i++)
        {
            if (lines[i].Trim().Length < 3) continue;
            string[] strs2 = lines[i].Split('\t');
            string name = strs2[0];
            string fileName = strs2[1];
            AudioClip tmpClip = PoolSystem.Instance.GetAudioClip($"Audio/{fileName}");// + fileName
            dict.Add(name, tmpClip);
        }
    }

    //播放指定音效
    public void PlayAudio(string id)
    {
        audio.PlayOneShot(GetClip(id), 1);

    }

    //获得指定音效
    public AudioClip GetClip(string id)
    {
        return dict[id];
    }
}
