/***********************************************
 * \file        EntitySystem.cs
 * \author      
 * \date        
 * \version     
 * \brief       实体工厂
 * \note        
 * \remarks     
 ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//编号 hp	speed	rotatespeed	weapon
//ID001	5	20	20		ID002/ID001/ID001

public class EntitySystem : Singleton<EntitySystem>
{
    //WebLoad.Load(Application.streamingAssetsPath+ "/Config/AudioClipConfig.txt");
    //字典：实体数据、实体武器数据、实体子弹数据、波数数据，存储
    public static Dictionary<string, string[]> entity;
    public static Dictionary<string, string[]> entityWeapon;
    public static Dictionary<string, string[]> entityBullet;
    public static Dictionary<string, string> waveInfo;

    //数据文本下载路径
    public const string EntityDataUrl = "/Config/EntityData.txt";
    public const string EntityWeaponDataUrl = "/Config//EntityWeaponData.txt";
    public const string EntityBulletDataUrl = "/Config//EntityBulletData.txt";
    public const string waveInfoUrl = "/Config/WaveData.txt";

    //实体下载路径
    public const string EntityUrl = "Entity/";
    public const string EntityBulletUrl = "EntityBullet/";

    //表格中实体属性数据数
    public const int EntityAttrSize = 4;
    public const int EntityWeaponAttrSize = 5;
    public const int EntityBulletAttrSize = 3;

    //临时使用
    public GameObject tmpObj;
    public static string str;

    //为新生成的实体分配id
    public static int entityID = 0;
    public static int entityBulletID = 0;

    public const string NULL = "0";

    //将文本中的内容按规则传入字典
    public static void FillDict(Dictionary<string, string[]> dict, string str,int size)
    {
        //ID001   5   300 60  ID002/ID001/ID001 
        string[] strs = str.Split('\n');
        for (int i = 1; i < strs.Length; i++)
        {
            if (strs[i].Trim().Length < 3) continue;

            string[] strs2 = strs[i].Split('\t');
            //第一项为实体id
            string id = strs2[0];
            //后面指定数量项为属性，将其变成字符串数组传入字典
            string[] attr = { };
            ArrayList al = new ArrayList(strs2);
            al.RemoveAt(0);
            attr = (string[])al.ToArray(typeof(string));

            dict.Add(id,attr);
        }
    }

    //实例化并初始化字典
    protected override void Awake()
    {
        base.Awake();
        entity = new Dictionary<string, string[]>();
        entityWeapon = new Dictionary<string, string[]>();
        entityBullet = new Dictionary<string, string[]>();
        waveInfo = new Dictionary<string, string>();

        InitAttrData();
    }

    //初始化字典
    public static void InitAttrData()
    {
        
        str = WebLoad.Load(Application.streamingAssetsPath+EntityDataUrl);
        FillDict(entity, str, EntityAttrSize);
        str = WebLoad.Load(Application.streamingAssetsPath + EntityWeaponDataUrl);
        FillDict(entityWeapon, str, EntityWeaponAttrSize);
        str = WebLoad.Load(Application.streamingAssetsPath + EntityBulletDataUrl);
        FillDict(entityBullet, str, EntityBulletAttrSize);

        str = WebLoad.Load(Application.streamingAssetsPath + waveInfoUrl);
        DecodeClass.FillDict(waveInfo, str);
    }

    //设置ID,如果创建的实体是新的，则为其分配新的id号，否则返回null
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
