/***********************************************
 * \file        AudioSourceControl.cs
 * \author      
 * \date        
 * \version     
 * \brief       未使用
 * \note        
 * \remarks     
 ***********************************************/
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioSourceControl 
{
    //使用列表存储所有的AudioSource组件
    List<AudioSource> allSources;
    GameObject own;
    public const int FreeCountMax = 1;//最大空闲数

    //构造函数，需要一个GameObject，用于挂载AudioSource组件
    public AudioSourceControl(GameObject owner)
    {
        own = owner;
        Initial();
    }

    public void Initial()
    {
        allSources = new List<AudioSource>();

        for (int i = 0; i < FreeCountMax; i++)
        {
            //由于AudioSource继承了mono，因此必须挂载在GameObject上，而不能直接new
            AudioSource source = own.AddComponent<AudioSource>();
            allSources.Add(source);
        }
    }

    //获取空闲的AudioSource，当列表中没有空闲的AudioSource，会再创建一个
    public AudioSource GetFreeSoure()
    {
        //Debug.Log(allSources.Count);
        for (int i = 0; i < allSources.Count; i++)
        {
            if (!allSources[i].isPlaying)
            {
                return allSources[i];
            }
        }
        AudioSource tmpSource = own.AddComponent<AudioSource>();
        allSources.Add(tmpSource);
        return tmpSource;
    }

    //释放多余的空闲的AudioSource，保证维持在3个空闲
    public void ReleaseFreeSource()
    {
        int freeCount = 0;//用来计数多少个空闲
        List<AudioSource> delSource = new List<AudioSource>();//用来存放要释放的多余的空闲组件
        for (int i = 0; i < allSources.Count; i++)
        {
            if (!allSources[i].isPlaying)
            {
                freeCount++;
                if (freeCount > FreeCountMax)
                {
                    delSource.Add(allSources[i]);
                }
            }
        }

        for (int i = 0; i < delSource.Count; i++)
        {
            //释放需要把列表中的元素删除，同时也需要将物体下的组件销毁（关键）
            allSources.Remove(delSource[i]);
            GameObject.Destroy(delSource[i]);
        }
        //清空临时列表，其实不清空问题也不大
        delSource.Clear();
        delSource = null;

    }

}
