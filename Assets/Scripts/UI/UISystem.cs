/***********************************************
 * \file        UISystem.cs
 * \author      
 * \date        
 * \version     
 * \brief       UI系统
 * \note        
 * \remarks     
 ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UISystem : Singleton<UISystem>
{
    //UI类预制体存放路径
    public const string UILayoutUrl = "UI/";

    //布局
    public UILayout layout;

    //UI系统为全局创建一个布局预制体
    private void Awake()
    {
        base.Awake();
        InitUILayout();

    }
    
    public void InitUILayout()
    {
        //这里把波数的UpPanel相关数据内容放在GameControl的文本保存了
        //没有修改成新增一个UpPanel类 
        layout = PoolSystem.Instance.GetObj(UILayoutUrl + "Layout").GetComponent<UILayout>();
        Transform trans = layout.transform.GetChild(0);//Find("UpPanel");
        //Debug.Log(trans);
        GameControl.Instance.text = trans.GetComponentInChildren<Text>();
        //Debug.Log(GameControl.Instance.text);
    }

    public Transform FindChildByName(string str)
    {
        Transform[] allChildren = layout.transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < allChildren.Length; i++)
        {
            if (allChildren[i].name == str)
            {
                return allChildren[i];
            }
        }
        return null;
    }

    #region Get

    #endregion



    #region Create
    //分配血条
    public Slider CreateSlider()
    {
        Slider hps =  PoolSystem.Instance.GetObj("UI/Slider").GetComponent<Slider>();
        //Debug.Log(layout == null);
        hps.transform.parent =  layout.transform;
        hps.transform.localPosition = Vector3.zero;
        return hps;
    }
    //if(hps != null) hps.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0,2,0));


    //分配文本-用于掉血飘字
    public Text CreateText()
    {
        Text text = PoolSystem.Instance.GetObj("UI/Text").GetComponent<Text>();
        //Debug.Log(layout == null);
        text.transform.parent = layout.transform;
        text.transform.localPosition = Vector3.zero;
        return text;
    }

    //分配ui武器表
    public GameObject CreateWeaponTable()
    {
        GameObject obj = PoolSystem.Instance.GetObj("UI/WeaponTable");
        //Debug.Log(layout == null);
        //obj.transform.parent = layout.transform.GetChild(3).GetChild(0).GetChild(0).GetChild(0);
        //obj.transform.localPosition = Vector3.zero;
        return obj;
    }

    #endregion

    #region push
    public void PushSlider(Slider hps)
    {
        PoolSystem.Instance.PushObj("UI/Slider", hps.gameObject);
    }

    
    public void PushText(Text text)
    {
        PoolSystem.Instance.PushObj("UI/Text", text.gameObject);
    }
    public void PushWeaponTable(UIWeaponTable table)
    {
        PoolSystem.Instance.PushObj("UI/WeaponTable", table.gameObject);
    }
    #endregion

    #region 事件
    //按钮文本和实现
    public void AddButtonFunction(Button btn,string content, UnityAction action)
    {
        Text text = btn.GetComponentInChildren<Text>();
        //text.text = "123";
        //Debug.Log($"{btn.name } {content} {action}");
        ChangeTextContent(text, content);
        AddButtonListen( btn,  action);
    }
    public void AddButtonListen(Button btn,UnityAction action)
    {
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(action);
        }
    }
    public void ChangeTextContent(Text text,string content)
    {
        if (text != null)
        {
            text.text = content;
        }
    }
    #endregion
}
