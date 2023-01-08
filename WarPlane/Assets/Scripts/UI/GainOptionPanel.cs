/***********************************************
 * \file        GainOptionPanel.cs
 * \author      
 * \date        
 * \version     
 * \brief       增益panel
 * \note        
 * \remarks     
 ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GainOptionPanel : UIPanel
{
    //记录了Panel下有的按钮
    public Dictionary<string, Button> btns;

    //控制该组件的隐藏和显示
    public bool isUsed;
    public Transform key;

    //不同增益效果
    List<string> gainString;
    List<UnityAction> gainAction;

    void Start()
    {
        base.Start();

        btns = new Dictionary<string, Button>();

        Button[] tmp = GetComponentsInChildren<Button>();
        for(int i=0;i< tmp.Length;i++)
        {
            btns.Add(tmp[i].name,tmp[i]);
        }

        isUsed = false;
        key = transform.GetChild(0);
        key.gameObject.SetActive(isUsed);

        gainString = new List<string>();
        gainAction = new List<UnityAction>();

        Init();

    }
    //通用增益后缀
    public void GainSuffix()
    {
        isUsed = false;
        key.gameObject.SetActive(isUsed);
        GameControl.Instance.NextWave();    
    }
    //增加攻击力增益
    public void Gain0()
    {
        //Debug.Log("Gain0");
        GameControl.Instance.gainAttack += 1;
        GainSuffix();
    }

    //初始化增益效果
    public void Init()
    {
        gainString.Add("攻击力+1");
        gainAction.Add(Gain0);
    }
    

    //增益表显示，并未按钮随机增加文本和事件
    public void ShowGainOption()
    {
        isUsed = true;
        key.gameObject.SetActive(isUsed);
        AddEvent();
    }

    //为按钮增加事件
    public void AddEvent()
    {
        //Debug.Log("AddEvent");
        KeyValuePair<string, UnityAction> p = new KeyValuePair<string, UnityAction>();
        //Debug.Log(btns.Count);
        foreach (Button btn in btns.Values)
        {
            p = RandomGain();
            //Debug.Log($"{p.Key} {p.Value}");
            UISystem.Instance.AddButtonFunction(btn,p.Key,p.Value );
        }
    }
    //增加随机增益
    public KeyValuePair<string, UnityAction> RandomGain()
    {
        int index = Random.Range(0, gainString.Count);
        return new KeyValuePair<string, UnityAction>(gainString[index], gainAction[index]);
    }



}
