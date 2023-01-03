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

public class EntityWeaponAttr
{
    public UIWeaponTable table;

    public string bullet;
    public AttackType attackType;
    public int cartridge;
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
    public EntityBase entity;
    public UIWeaponTable table;

    //属性
    public EntityWeaponAttr attr;
    public float shootTimer;
    public float reloadTimer;
    public int cartridgeCounter;

    public float attackDistance;

    private void Awake()
    {
        entity = GetComponentInParent<EntityBase>();
        attr = new EntityWeaponAttr();
    }

    public void Init(string str)//ID001
    {
        //Debug.Log($"武器编号: {str} 字符长度 {str.Length}");
        string[] weaponAttr = EntitySystem.entityWeapon[str];

        attr.SetAttr(weaponAttr);
        shootTimer = attr.shootTime;
        reloadTimer = attr.reloadTime;
        cartridgeCounter = attr.cartridge;
        //attr.Show();
        attackDistance = float.Parse(EntitySystem.entityBullet[attr.bullet][2]);
        //entity.ai.keepDistance = entity.GetAttackDistance();
    }
    
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
    public void ShowWeaponTable()
    {
        if(table != null)
        {
            //Debug.Log(cartridgeCounter);
            table.UpdateBulletNum(cartridgeCounter);
        }
    }


    public void PointToMove(int x,float time)
    {
       
        transform.Rotate(0, 0, 0 - x * 20 * time, Space.World);

    }

    private void Update()
    {
        RefreshShoot(Time.deltaTime);
        RefreshReload(Time.deltaTime);
    }
}
