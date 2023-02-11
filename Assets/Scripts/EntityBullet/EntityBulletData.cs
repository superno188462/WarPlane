using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBulletData : MonoBehaviour
{
    public int offset = 8;
    public EntityBulletAttackType attackType;
    public float bulletSpeed;
    public int bulletHurt;
    public EntityBulletHurtType hurtType;
    public float boomAngle;
    public float attackDistance;

    public EntityBulletData()
    {
        attackType = new EntityBulletAttackType();
        hurtType = new EntityBulletHurtType();
    }
    public void Init(string dataStr)
    {
        string[] datas = dataStr.Split('\t');
        if (datas.Length < 14)
        {
            Debug.Log("bullet data error");
            return;
        }
        attackType = Mathc.Instance.JudgeEntityBulletAttackType(datas[offset + 0]);
        bulletSpeed = float.Parse(datas[offset + 1]);
        bulletHurt = int.Parse(datas[offset + 2]); 
        hurtType = Mathc.Instance.JudgeEntityBulletHurtType(datas[offset + 3]);
        boomAngle = float.Parse(datas[offset + 4]);
        attackDistance = float.Parse(datas[offset + 5]);
    }
}
