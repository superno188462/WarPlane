using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EntityWeaponData 
{
    public string id;
    public string name;
    public EntitySlotType slotType;
    public EntitySlotSize slotSize;
    public EntityWeaponLoadType loadType;
    public int bulletCountMax;
    public float loadTimer;
    public float AttackTimer;

    public int bulletCount;
    public float loadTime;
    public float attackTime;

    public EntityWeaponData()
    {
        slotType = new EntitySlotType();
        slotSize = new EntitySlotSize();
        loadType = new EntityWeaponLoadType();
    }
    public void Init(string dataStr)
    {
        string[] datas = dataStr.Split('\t');
        if (datas.Length < 14)
        {
            Debug.Log("weapon data error");
            return;
        }
        id = datas[0];
        name = datas[1];
        slotType = Mathc.Instance.JudgeEntitySlotType(datas[2]);
        slotSize = Mathc.Instance.JudgeEntitySlotSize(datas[3]);
        loadType = (EntityWeaponLoadType)System.Enum.Parse(typeof(EntityWeaponLoadType), datas[4]);
        bulletCountMax = int.Parse(datas[5]);
        loadTimer = float.Parse(datas[6]);
        AttackTimer = float.Parse(datas[7]);

        bulletCount = bulletCountMax;
        loadTime = loadTimer;
        attackTime = AttackTimer;

     
}

}
