using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//基本属性类 ，包括单位类型id，血量，直线速度和角速度
//ID Name	UnitType	HP	SP	SPX	SPType	SPAnge	Speed	SpeedR	SpA	SpAr	Mass	Weapons
    //ID001	哨兵级	护卫舰	2000	0	0	指向性	120	3	2	1	1	10	ID001, ID002

public class EntityData
{
    public string id;
    public string name;
    public EntityShipType shipGroup;
    public int maxhp; 
    public int maxsp;
    public float spRecover;
    public EntitySPType spType;
    public float spAngle;
    public float power;
    public float rotateSpeed;
    //public float speedAcc;
    //public float rotateSpeedAcc;
    public float mass;

    //旋转角度
    public float leftR = 60;
    public float rightR = 60;

    public int hp;
    public int sp;

    public EntityData()
    {
        shipGroup = new EntityShipType();
        spType = new EntitySPType();
    }
    public void Init(string dataStr)
    {
        //Debug.Log("init");
        string[] datas = dataStr.Split('\t');
        if (datas.Length < 12)
        {
            Debug.Log(datas.Length);
            Debug.Log("entity data error");
            return;
        }
        id = datas[0];
        name = datas[1];
        shipGroup = Mathc.Instance.JudgeShipType(datas[2]);
        maxhp = int.Parse(datas[3]);
        maxsp = int.Parse(datas[4]);
        spRecover = float.Parse(datas[5]);
        spType = Mathc.Instance.JudgeSPType(datas[6]);
        spAngle = float.Parse(datas[7]);
        power = float.Parse(datas[8])/100;
        //Debug.Log(power);
        rotateSpeed = float.Parse(datas[9])/10;
        mass = float.Parse(datas[10]);
        hp = maxhp;
        sp = maxsp;
    }
}

