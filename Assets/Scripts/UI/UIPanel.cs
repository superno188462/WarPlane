using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIPanel : MonoBehaviour
{
    public virtual void PrintWeaponTable(EntityBase entity) 
    {
    }

    protected void Start()
    {
        //Debug.Log($"{this.name} {this.name.Length}");
        UISystem.Instance.layout.panels.Add(this.name, this);
        //Debug.Log(UISystem.Instance.layout.panels.Count);
    }
   

    //获取指定按钮组件
    public Button GetButton()
    {
        return null;
    }

    //功能：添加按钮监听功能
    public void AddButtonListen(Button btn, UnityAction action)
    {
        btn.onClick.AddListener(action);
    }

    //开启隐藏按钮显示
    public void ChangeButtonShow(Button btn, bool active)
    {
        btn.gameObject.SetActive(active);
    }
    //修改按钮显示
    public void ChangeButtonShow(Button btn,string content)
    {
        
    }

    //移除按钮功能
    public void RemoveButton(Button btn)
    {
        //
        btn.onClick.RemoveAllListeners();
    }

    //功能：修改文本
    public void ChangeTextContent()
    {
        
    }


}
