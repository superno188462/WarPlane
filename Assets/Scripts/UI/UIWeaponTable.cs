using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIWeaponTable : MonoBehaviour
{
    public int max;
    public string bullet;
    public Text bulletText;
    public Text cartridgeText;

    public Slider slider;

    void Awake()
    {
        Text[] tmp = transform.GetComponentsInChildren<Text>();
        bulletText = tmp[0];
        cartridgeText = tmp[1];
        //Debug.Log($"{bulletText == null} {cartridgeText == null}");

        slider = transform.GetComponentInChildren<Slider>();
    }

    public void RefreshProg(float value)//更新进度
    {
        slider.value = value;
    }

    public void FreshWeapon(EntityWeaponBase weapon)
    {
        //Debug.Log(weapon == null);
        bullet = weapon.attr.bullet;
        max = weapon.attr.cartridge;

        bulletText.text = bullet;
        UpdateBulletNum(max);
    }
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
