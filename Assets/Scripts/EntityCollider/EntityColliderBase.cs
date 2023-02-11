/***********************************************
 * \file        EntityColliderBase.cs
 * \author      
 * \date        
 * \version     
 * \brief       碰撞类
 * \note        
 * \remarks     
 ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EntityColliderBase : MonoBehaviour
{
    //绑定实体触发器、碰撞体或刚体
    protected EntityBase entity;
    public Rigidbody2D rigidbody;

    //获取实体
    private void Awake()
    {
        rigidbody = GetComponentInChildren<Rigidbody2D>();
        entity = GetComponentInChildren<EntityBase>();
    }

    //碰撞成功函数，扣血+ui飘字
    public void ColliderSuccess(EntityBase entity,int hurt)
    {
        int tmpHp = entity.entityData.hp - hurt;
        //Debug.Log(tmpHp);
        entity.SetHp(tmpHp);

        Text text = UISystem.Instance.CreateText();
        text.transform.position = Camera.main.WorldToScreenPoint(entity.transform.position + new Vector3(0, 3, 0));
        text.text = hurt.ToString();
        Order.FlyToText(text);
    }

    //碰撞检测
    private void OnTriggerEnter2D(Collider2D other)
    {
        //当碰撞到子弹，判断子弹是否是自己发射的,最后计算伤害
        if(other.gameObject.layer == 9)
        {
            EntityBulletBase bullet = other.gameObject.GetComponentInParent<EntityBulletBase>();
            if (bullet == null) return;
            //自己发射的子弹
            if (bullet.attributionID == entity.unitID) return;

            //伤害为子弹伤害+增益
            int hurt = bullet.bulletData.bulletHurt;
            if(bullet.attributionID == PlayerControl.Instance.unit.unitID)
                hurt+=GameControl.Instance.gainAttack;

            //碰撞，单位掉血+飘字
            ColliderSuccess(entity, hurt);

            bullet.PushPool();
        }
        //判断碰到的也是实体，并且为单位ID002、敌对
        if (other.gameObject.layer == 8)
        {
            EntityBase enemy = other.gameObject.GetComponentInParent<EntityBase>();
            //Debug.Log($"{enemy == null} { entity == null}");
            if (enemy == null) return;
            if(enemy.group == EntityGroup.Player && entity.entityData.id == "ID002")
            {
                ColliderSuccess(entity, 1);
                entity.PushPool();
            }
        }
    }
}
