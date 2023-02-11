/***********************************************
 * \file        UIWeaponTable.cs
 * \author      
 * \date        
 * \version     
 * \brief       ui武器表具体武器
 * \note        
 * \remarks     
 ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIWeaponTable : MonoBehaviour
{
    //最大子弹数
    public int max;
    //子弹类型
    //public string bullet;
    //子弹类型文本
    //public Text bulletText;
    //子弹数量文本
    public Text cartridgeText;
    //子弹装弹进度滚轮
    public Slider slider;

    void Awake()
    {
        Text[] tmp = transform.GetComponentsInChildren<Text>();
        //bulletText = tmp[0];
        cartridgeText = tmp[1];
        //Debug.Log($"{bulletText == null} {cartridgeText == null}");

        slider = transform.GetComponentInChildren<Slider>();
    }

    //更新子弹装弹进度滚轮
    public void RefreshProg(float value)//更新进度
    {
        slider.value = value;
    }

    //将武器数据传递给ui武器表具体武器项
    public void FreshWeapon(EntityWeaponBase weapon)
    {
        //Debug.Log(weapon == null);
        //bullet = weapon.attr.bullet;
        max = weapon.weaponData.bulletCountMax;

        //bulletText.text = bullet;
        UpdateBulletNum(max);
    }
    //更新子弹数文本
    public void UpdateBulletNum(int current)
    {
        string str = $"{current}/{max}";
        //Debug.Log(str);
        cartridgeText.text = str;
    }
    public void ChangeButtonNum(int x)
    {

    }
}
