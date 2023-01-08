/***********************************************
 * \file        EntityBulletBase.cs
 * \author      
 * \date        
 * \version     
 * \brief       子弹基类
 * \note        
 * \remarks     
 ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//编号 伤害	speed	attackDistance	描述
//ID001	1	800	10	physical
//ID002	1	650	10	laser

//子弹属性类，包括子弹id，子弹伤害，子弹速度，子弹攻击距离
public class EntityBulletAttr
{
    public string id;
    public int hurt;
    public float speed;
    public float attackDistance;
    public void SetAttr(string[] str)
    {
        hurt = int.Parse(str[0]);
        speed = float.Parse(str[1]);
        attackDistance = float.Parse(str[2]);
    }
}

public class EntityBulletBase : MonoBehaviour
{
    //场景中子弹唯一id
    public string unitID;
    //属于什么战斗单位
    public string attributionID;
    //回收存储路径
    public string path;

    //子弹基本属性
    public EntityBulletAttr attr;

    //子弹生存周期（攻击距离）
    public float life;

    //实例化子弹基本属性类
    private void Awake()
    {
        attr = new EntityBulletAttr();
    }

    //初始化
    //参数：编号，路径，属性，坐标transform，旋转角
    public void Init(string id, string belong, string url, string[] str, Vector3 pos, float z)
    {
        if(id != null) unitID = id;
        if(belong != null) attributionID = belong;
        path = url;

        //设置位置
        SetPosition(pos);
        SetDirection(z);

        //设置属性
        attr.SetAttr(str);
        SetLife();
    }

    #region 属性相关函数
    public void SetPosition(Vector3 pos)
    {
        this.transform.position = pos;
    }
    public void SetDirection(float z)
    {
        transform.eulerAngles = new Vector3(0, 0, z);
    }

    //设置攻击距离
    public void SetLife()
    {
        life = attr.attackDistance;
    }

    #endregion

    #region 动作相关函数
    //移动
    public virtual float PointToMove(float time)
    {
        return 0;
    } 
    //生存周期变化
    public virtual void RefreshLife(float time)
    {
    }

    #endregion
    //实时移动和修改生存周期
    private void Update()
    {
        PointToMove(Time.deltaTime);
        RefreshLife(Time.deltaTime);
    }

    //回收子弹
    public void PushPool()
    {
        EntitySystem.PushEntityBullet(path, this.gameObject);
    }
}
