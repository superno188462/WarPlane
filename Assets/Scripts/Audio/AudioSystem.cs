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

    private Dictionary<string, AudioClip> dict;


    protected override void Awake()
    {
        base.Awake();
        dict = new Dictionary<string, AudioClip>();
        audio = gameObject.AddComponent<AudioSource>();
        Init();

    }

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

    public void PlayAudio(string id)
    {
        audio.PlayOneShot(GetClip(id), 1);

    }


    public AudioClip GetClip(string id)
    {
        return dict[id];
    }
}
