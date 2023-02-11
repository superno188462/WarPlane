/***********************************************
 * \file        EntityWeaponBase.cs
 * \author      
 * \date        
 * \version     
 * \brief       武器类，没有子类
 * \note        
 * \remarks     
 ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityWeaponBase : MonoBehaviour
{
    public EntityBase entity; //武器归属

    public GameObject Drawline;
    
    public UIWeaponTable table;//对应ui武器表中的武器，只有玩家单位会有

    public EntityWeaponData weaponData;
    public EntityBulletData bulletData;

    //初始化武器归属、实例化属性类
    private void Awake()
    {
        entity = GetComponentInParent<EntityBase>();
        weaponData = new EntityWeaponData();
        bulletData = new EntityBulletData();
    }

    //初始化属性，并重置定时器
    public void Init(string str)//ID001
    {
        //Debug.Log($"{str }");
        string dataStr = EntitySystem.entityWeapon[str];
        //Debug.Log($"{dataStr} {dataStr.Length}");
        weaponData.Init(dataStr);
        bulletData.Init(dataStr);
    }


    //刷新装弹定时器，实时控制ui武器表滚轮进度；时间清零子弹数+1，控制ai武器表文本变化
    //public void RefreshReload(float time)
    //{
    //    if (reloadTimer > 0)
    //    {
    //        reloadTimer -= time;
    //        if (table != null) table.RefreshProg(1-reloadTimer/ attr.reloadTime);
    //    }
    //    else
    //    {
    //        if(cartridgeCounter < attr.cartridge)
    //        {
    //            reloadTimer = attr.reloadTime;
    //            cartridgeCounter += 1;
    //            ShowWeaponTable();
    //        }
    //    }
    //}
    public void RefreshReload(float time)
    {
        if (weaponData.loadTime > 0)
        {
            weaponData.loadTime -= time;
        }
        else
        {
            if (weaponData.bulletCount < weaponData.bulletCountMax)
            {
                weaponData.loadTime = weaponData.loadTimer;
                weaponData.bulletCount += 1;
                ShowWeaponTable();
            }
        }
    }
    //刷新攻击间隔定时器
    public void RefreshShoot(float time)
    {
        if (weaponData.attackTime > 0)
        {
            weaponData.attackTime -= time;
        }
    }

    //不同攻击方式
    //public void Attack()
    //{
    //    if (shootTimer > 0) return;
    //    shootTimer = attr.shootTime;
    //    switch (attr.attackType)
    //    {
    //        case AttackType.none: break;
    //        case AttackType.normal:
    //            NormalAttack();
    //            break;
    //        case AttackType.scattering:
    //            ScatteringAttack();
    //            break;

    //    }

    //}

    public void Attack()
    {
        if (weaponData.attackTime > 0) return;
        
        if (weaponData.bulletCount >= 1)
        {
            weaponData.bulletCount -= 1;
            //Debug.Log(weaponData.bulletCount);
            ShowWeaponTable();
            EntitySystem.CreateEntityBulletInCondition(bulletData, entity.unitID, transform.position, transform.eulerAngles.z);
            //Debug.Log("NormalAttack");
            AudioSystem.Instance.PlayAudio("shooting");
        }
    }
    //普通攻击，生成一发子弹，减少子弹数量，修改ui武器表，播放音效
    //public void NormalAttack()
    //{
    //    if(cartridgeCounter >= 1)
    //    {
    //        cartridgeCounter -= 1;
    //        //Debug.Log(cartridgeCounter);
    //        ShowWeaponTable();
    //        EntitySystem.CreateEntityBulletInCondition(attr.bullet, entity.unitID, transform.position , transform.eulerAngles.z);
    //        //Debug.Log("NormalAttack");
    //        AudioSystem.Instance.PlayAudio("shooting");
    //    }
    //}
    //散射攻击
    //public void ScatteringAttack()
    //{
    //    if(cartridgeCounter >= 3)
    //    {
    //        cartridgeCounter -= 3;
    //        ShowWeaponTable();
    //        float[] offset = new float[3] { -10, 0, 10 };
    //        for (int i = 0; i < offset.Length;i++)
    //        {
    //            EntitySystem.CreateEntityBulletInCondition(attr.bullet, entity.unitID, transform.position, transform.eulerAngles.z + offset[i]);
    //        }

    //    }
    //}


    //当武器绑定了ui武器表，就会刷新ui武器表
    public void ShowWeaponTable()
    {
        if(table != null)
        {
            //Debug.Log(cartridgeCounter);
            table.UpdateBulletNum(weaponData.bulletCount);
        }
    }

    //旋转
    public void PointToMove(int x,float time)
    {
       
        transform.Rotate(0, 0, 0 - x * 360 * time, Space.World);

    }

    //实时更新射击定时器和装弹定时器
    public void Refresh(float time)
    {
        RefreshShoot(time);
        RefreshReload(time);
    }
}
