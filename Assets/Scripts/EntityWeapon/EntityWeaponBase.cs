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

//编号 子弹类型	攻击类型	弹夹数	装弹时间	子弹射击间隔	
//ID001	ID001		normal	5x1        	1		0.2	

public enum AttackType
{
    none,
    normal,
    scattering,
}

//武器属性
public class EntityWeaponAttr
{
    public UIWeaponTable table;

    //子弹ID、攻击类型、子弹数量上限
    public string bullet;
    public AttackType attackType;
    public int cartridge;
   //装弹时间、射击间隔
    public float reloadTime;
    public float shootTime;
    
    public void SetAttr(string[] str)
    {
        bullet = str[0];
        attackType = (AttackType)AttackType.Parse(typeof(AttackType), str[1]);
        string[] tmp = str[2].Split('x');
        cartridge = int.Parse(tmp[0])* int.Parse(tmp[1]);
        reloadTime = float.Parse(str[3]);
        shootTime = float.Parse(str[4]);
        
    }
    public void Show()
    {
        Debug.Log($"{bullet} {attackType} {cartridge} {reloadTime} {shootTime}");
    }
}

public class EntityWeaponBase : MonoBehaviour
{
    
    //链接关系
    //武器归属
    public EntityBase entity;
    //对应ui武器表中的武器，只有玩家单位会有
    public UIWeaponTable table;

    //基本属性，射击定时器，装弹定时器
    public EntityWeaponAttr attr;
    public float shootTimer;
    public float reloadTimer;
    //子弹数量
    public int cartridgeCounter;

    //攻击距离
    public float attackDistance;

    //初始化武器归属、实例化属性类
    private void Awake()
    {
        entity = GetComponentInParent<EntityBase>();
        attr = new EntityWeaponAttr();
    }

    //初始化属性，并重置定时器
    public void Init(string str)//ID001
    {
        //Debug.Log($"武器编号: {str} 字符长度 {str.Length}");
        string[] weaponAttr = EntitySystem.entityWeapon[str];

        attr.SetAttr(weaponAttr);
        shootTimer = attr.shootTime;
        reloadTimer = attr.reloadTime;
        cartridgeCounter = attr.cartridge;
        //attr.Show();
        //武器攻击距离等于子弹攻击距离
        attackDistance = float.Parse(EntitySystem.entityBullet[attr.bullet][2]);
        //entity.ai.keepDistance = entity.GetAttackDistance();
    }
    
    //刷新装弹定时器，实时控制ui武器表滚轮进度；时间清零子弹数+1，控制ai武器表文本变化
    public void RefreshReload(float time)
    {
        if (reloadTimer > 0)
        {
            reloadTimer -= time;
            if (table != null) table.RefreshProg(1-reloadTimer/ attr.reloadTime);
        }
        else
        {
            if(cartridgeCounter < attr.cartridge)
            {
                reloadTimer = attr.reloadTime;
                cartridgeCounter += 1;
                ShowWeaponTable();
            }
        }
    }

    //刷新攻击间隔定时器
    public void RefreshShoot(float time)
    {
        if (shootTimer > 0)
        {
            shootTimer -= time;
        }
    }

    //不同攻击方式
    public void Attack()
    {
        if (shootTimer > 0) return;
        shootTimer = attr.shootTime;
        switch (attr.attackType)
        {
            case AttackType.none: break;
            case AttackType.normal:
                NormalAttack();
                break;
            case AttackType.scattering:
                ScatteringAttack();
                break;

        }

    }

    //普通攻击，生成一发子弹，减少子弹数量，修改ui武器表，播放音效
    public void NormalAttack()
    {
        if(cartridgeCounter >= 1)
        {
            cartridgeCounter -= 1;
            //Debug.Log(cartridgeCounter);
            ShowWeaponTable();
            EntitySystem.CreateEntityBulletInCondition(attr.bullet, entity.unitID, transform.position , transform.eulerAngles.z);
            //Debug.Log("NormalAttack");
            AudioSystem.Instance.PlayAudio("shooting");
        }
    }
    //散射攻击
    public void ScatteringAttack()
    {
        if(cartridgeCounter >= 3)
        {
            cartridgeCounter -= 3;
            ShowWeaponTable();
            float[] offset = new float[3] { -10, 0, 10 };
            for (int i = 0; i < offset.Length;i++)
            {
                EntitySystem.CreateEntityBulletInCondition(attr.bullet, entity.unitID, transform.position, transform.eulerAngles.z + offset[i]);
            }

        }
    }

    //当武器绑定了ui武器表，就会刷新ui武器表
    public void ShowWeaponTable()
    {
        if(table != null)
        {
            //Debug.Log(cartridgeCounter);
            table.UpdateBulletNum(cartridgeCounter);
        }
    }

    //旋转
    public void PointToMove(int x,float time)
    {
       
        transform.Rotate(0, 0, 0 - x * 20 * time, Space.World);

    }

    //实时更新射击定时器和装弹定时器
    private void Update()
    {
        RefreshShoot(Time.deltaTime);
        RefreshReload(Time.deltaTime);
    }
}
