﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//编号 hp	speed	rotatespeed	weapon
//ID001	5	20	20		ID002/ID001/ID001

public class EntitySystem : Singleton<EntitySystem>
{
    
    public static Dictionary<string, string[]> entity;
    public static Dictionary<string, string[]> entityWeapon;
    public static Dictionary<string, string[]> entityBullet;
    public static Dictionary<string, string> waveInfo;

    public const string EntityDataUrl = "Assets/Resources/EntityData/EntityData.txt";
    public const string EntityWeaponDataUrl = "Assets/Resources/EntityData/EntityWeaponData.txt";
    public const string EntityBulletDataUrl = "Assets/Resources/EntityData/EntityBulletData.txt";
    public const string waveInfoUrl = "Assets/Resources/EntityData/WaveData.txt";

    public const string EntityUrl = "Entity/";
    public const string EntityBulletUrl = "EntityBullet/";

    //表格中实体属性数据数
    public const int EntityAttrSize = 4;
    public const int EntityWeaponAttrSize = 5;
    public const int EntityBulletAttrSize = 3;

    public GameObject tmpObj;

    public static int entityID = 0;
    public static int entityBulletID = 0;

    public const string NULL = "0";

    public static void FillDict(Dictionary<string, string[]> dict, string str,int size)
    {
        string[] strs = str.Split('\n');
        for (int i = 1; i < strs.Length; i++)
        {
            if (strs[i].Trim().Length < 3) continue;
            string[] strs2 = strs[i].Split('\t');
            string id = strs2[0];
            string[] attr = { };//new string[5] { NULL, NULL , NULL , NULL, NULL };
            //Debug.Log("empty");
            
            //Array.ConstrainedCopy(strs, 1, attr, 0, size);
            ArrayList al = new ArrayList(strs2);
            al.RemoveAt(0);
            attr = (string[])al.ToArray(typeof(string));
            //Debug.Log("change");


            dict.Add(id,attr);
        }
    }
    protected override void Awake()
    {
        base.Awake();
        entity = new Dictionary<string, string[]>();
        entityWeapon = new Dictionary<string, string[]>();
        entityBullet = new Dictionary<string, string[]>();
        waveInfo = new Dictionary<string, string>();

        InitAttrData();
    }
    public static void InitAttrData()
    {
        string str = WebLoad.Load(EntityDataUrl);
        FillDict(entity, str, EntityAttrSize);
        str = WebLoad.Load(EntityWeaponDataUrl);
        FillDict(entityWeapon, str, EntityWeaponAttrSize);
        str = WebLoad.Load(EntityBulletDataUrl);
        FillDict(entityBullet, str, EntityBulletAttrSize);

        str = WebLoad.Load(waveInfoUrl);
        DecodeClass.FillDict(waveInfo, str);
    }

    //设置ID
    public static string SetBulletID(EntityBulletBase unit)
    {
        //Debug.Log(unit.unitID);
        if (unit.unitID == null || unit.unitID == "ID0000")
        {
            entityBulletID += 1;
            string str = "ID" + entityBulletID.ToString($"D{4}");
            return str;
        }
        return null;
    }
    public static string SetEntityID(EntityBase unit)
    {
        //Debug.Log(unit.unitID);
        if (unit.unitID == null || unit.unitID == "ID0000")
        {
            entityID += 1;
            string str = "ID" + entityID.ToString($"D{4}");
            return str;
        }
        return null;
    }

    //创建单位
    public static EntityBase CreateEntity(string number)
    {
        return PoolSystem.Instance.GetObj(EntityUrl + number).GetComponent<EntityBase>();
    }
    public static EntityBulletBase CreateEntityBullet(string number)
    {
        return PoolSystem.Instance.GetObj(EntityBulletUrl + number).GetComponent<EntityBulletBase>();
    }

    //删除单位
    public static void PushEntity(string path, GameObject obj)
    {
        PoolSystem.Instance.PushObj(path, obj);
    }
    public static void PushEntityBullet(string path, GameObject obj)
    {
        PoolSystem.Instance.PushObj(path, obj);
    }

    //在指定位置创建单位
    public static EntityBase CreateEntityInCondition(string number, Vector3 pos, float z)
    {
        string url = EntityUrl + number;

        EntityBase unit = CreateEntity(number);
        unit.Init(SetEntityID(unit), url, number ,entity[number], pos, z);
        return unit;
    }

    public static EntityBulletBase CreateEntityBulletInCondition(string number, string belong, Vector3 pos, float z)
    {
        string url = EntityBulletUrl + number;

        EntityBulletBase unit = CreateEntityBullet(number);
        unit.Init(SetBulletID(unit), belong,  url, entityBullet[number], pos, z);
        return unit;
    }

    //刷怪笼
    public static EntityBase CreateEntityInArea(string number, Vector3 pos,float radius)
    {
        float x = UnityEngine.Random.Range(-radius, radius);
        float y = UnityEngine.Random.Range(-radius, radius);
        pos += x * Vector3.right + y * Vector3.up;
        float z = UnityEngine.Random.Range(0, 360);

        return EntitySystem.CreateEntityInCondition(number, pos, z);
    }
}