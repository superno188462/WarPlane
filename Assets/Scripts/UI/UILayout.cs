/***********************************************
 * \file        UILayout.cs
 * \author      
 * \date        
 * \version     
 * \brief       布局类
 * \note        保存布局预制体下的所有需要经常使用的UIPanel类
 * \remarks     
 ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILayout : MonoBehaviour
{
    public Dictionary<string,UIPanel> panels;
    private void Awake()
    {
        panels = new Dictionary<string, UIPanel>();

    }


    //UIPanel    <=
    //DownPanel  PrintWeaponTable
    //(panels[" xxx"] as DownPanel).PrintWeaponTable(null);

}
