/***********************************************
 * \file        .cs
 * \author      
 * \date        
 * \version     
 * \brief       不使用
 * \note        
 * \remarks     
 ***********************************************/
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIWidget : MonoBehaviour
{
    //功能：主动上报
    private void Awake()
    {
        string panelName = GetComponentInParent<UIPanel>().name;
        UIManager.Instance.RegistWidget(panelName, name, gameObject);
    }

    //功能：添加按钮监听功能
    public void AddButtonListen(UnityAction action)
    {
        Button btn = GetComponent<Button>();
        if(btn != null)
        {
            btn.onClick.AddListener(action);
        }
    }

    //功能：修改文本
    public void ChangeTextContent(string content)
    {
        Text text = GetComponent<Text>();
        if (text != null)
        {
            text.text = content;
        }
    }

    //功能：修改滑轮
}
