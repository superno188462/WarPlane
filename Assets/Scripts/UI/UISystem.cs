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
    public Dictionary<EntityBase, SliderState> allSliders;

    //布局
    public UILayout layout;

    //UI系统为全局创建一个布局预制体
    private void Awake()
    {
        base.Awake();
        InitUILayout();
        allSliders = new Dictionary<EntityBase, SliderState>();

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
    public void CreateHPSlider(EntityBase entity)
    {
        SliderState hps =  PoolSystem.Instance.GetObj("UI/SliderState").GetComponent<SliderState>();
        //Debug.Log(layout == null);
        hps.transform.parent = FindChildByName("HpSilderTF");
        hps.transform.localPosition = Vector3.zero;
        allSliders.Add(entity, hps);
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
    public void PushHPSlider(EntityBase entity)
    {
        PoolSystem.Instance.PushObj("UI/Slider", allSliders[entity].gameObject);
        allSliders.Remove(entity);
        
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
    public void ChangeSliderShow(EntityBase entity,float fill1,float fill2)
    {
        //Debug.Log($"{fill1} {fill2}");
    }
    #endregion

    private void Update()
    {
        float time = Time.deltaTime;
        float hpV, spV;
        foreach (KeyValuePair<EntityBase, SliderState> kv in allSliders)
        {
            if ((float)kv.Key.entityData.maxsp == 0)
            {
                spV = 0;
            }
            else 
            {
                spV = (float)kv.Key.entityData.sp / (float)kv.Key.entityData.maxsp;
            }
            Debug.Log($"{kv.Key.entityData.hp} {kv.Key.entityData.maxhp} {(float)kv.Key.entityData.hp / (float)kv.Key.entityData.maxhp}");
            hpV = (float)kv.Key.entityData.hp / (float)kv.Key.entityData.maxhp;
            kv.Value.refreshHps(hpV, spV);
            kv.Value.ParticleHps(time);
            kv.Value.refreshPostion(kv.Key.transform.position);
        }
    }
}
