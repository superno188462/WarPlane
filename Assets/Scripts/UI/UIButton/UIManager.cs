using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    //功能：存储子控件 
    //参数：panel_name,widegt_name,gameObject
    public Dictionary<string, Dictionary<string, GameObject>> allWidgets;

    //功能：注册子控件
    //参数：
    public void RegistWidget(string panelName, string widgetName, GameObject obj)
    {
        if(!allWidgets.ContainsKey(panelName))
        {
            allWidgets[panelName] = new Dictionary<string, GameObject>();
        }
        allWidgets[panelName][widgetName] = obj;
    }


    //功能：查找子控件
    //参数：panel_name,widegt_name
    public GameObject GetWidgetGameObject(string panelName,string widgetName)
    {
        if(!allWidgets.ContainsKey(panelName))
        {
            return allWidgets[panelName][widgetName];
        }
        return null;
    }

}
