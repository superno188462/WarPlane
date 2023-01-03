using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//编号 子弹类型	伤害	speed	
//ID001	physical	2	100
//ID002	laser		1	80


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
    public string unitID;
    public string attributionID;//属于什么战斗单位
    public string path;

    public EntityBulletAttr attr;
    public float life;

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

    public void SetLife()
    {
        life = attr.attackDistance;
    }

    #endregion

    #region 动作相关函数

    public virtual float PointToMove(float time)
    {
        return 0;
    } 
    
    public virtual void RefreshLife(float time)
    {
    }

    #endregion

    private void Update()
    {
        PointToMove(Time.deltaTime);
        RefreshLife(Time.deltaTime);
    }

    public void PushPool()
    {
        EntitySystem.PushEntityBullet(path, this.gameObject);
    }
}
