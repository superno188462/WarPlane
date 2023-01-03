using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EntityColliderBase : MonoBehaviour
{
    protected EntityBase entity;

    //protected Rigidbody rigidbody;

    private void Awake()
    {
        //rigidbody = GetComponentInChildren<Rigidbody>();
        entity = GetComponentInChildren<EntityBase>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == 9)
        {
            EntityBulletBase bullet = other.gameObject.GetComponentInParent<EntityBulletBase>();

            if (bullet.attributionID == entity.unitID) return;//自己发射的子弹
            int hurt = bullet.attr.hurt;
            if(bullet.attributionID == PlayerControl.Instance.unit.unitID)
                hurt+=GameControl.Instance.gainAttack;
            int tmpHp = entity.currentHP - hurt;
            Text text = UISystem.Instance.CreateText();
            
            
            text.transform.position = Camera.main.WorldToScreenPoint(entity.transform.position + new Vector3(0, 3, 0));
            //Debug.Log($"{hurt} {hurt.ToString()}");
            text.text = hurt.ToString();
            Order.FlyToText(text);
            entity.SetHp(tmpHp);
            //Debug.Log(entity.currentHP);
            bullet.PushPool();

        }
        if(other.gameObject.layer == 8)
        {
            EntityBase enemy = other.gameObject.GetComponentInParent<EntityBase>();
            if(enemy.group == Group.player && entity.attr.id == "ID002")
            {
                int tmpHp = enemy.currentHP - 1;
                
                enemy.SetHp(tmpHp);

                entity.PushPool();
            }
        }
    }
}
