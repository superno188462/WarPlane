/***********************************************
 * \file        EntityBase.cs
 * \author      
 * \date        
 * \version     
 * \brief       单位基类
 * \note        
 * \remarks     
 ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//编号 hp	speed	rotatespeed	weapon
//ID001	5	300	60	ID002/ID001/ID001   
//ID002	2	200	45	null    
//ID003	10	300	60	ID001   


//队伍枚举类型
public enum Group
{ 
    player,
    enemy,
    neutral,
}

//基本属性类 ，包括单位类型id，血量，直线速度和角速度
public class EntityAttr
{
    public string id; 
    public int hp;
    public float speed;
    public float rotateSpeed;

    //设置基本属性并把武器返回给实体
    public string SetAttr(string attrid,string[] str)
    {
        id = attrid;
        hp = int.Parse(str[0]);
        speed = float.Parse(str[1]);
        rotateSpeed = float.Parse(str[2]);
        
        return str[3];
    }
}

public class EntityBase : MonoBehaviour
{
    //场景中entity的唯一id
    public string unitID;
    //回收存储位置
    public string path;
    
    //未使用
    public bool isActive;

    //武器
    public EntityWeaponBase[] weapons;
    //碰撞类
    public EntityColliderBase ecb;
    //ai控制器
    public AIControl ai;
    //引擎特效
    public EntityEngineFXControl[] engines;
    //血条
    public Slider hps;

    //基本属性、阵营、当前血量、速度
    public EntityAttr attr;
    public Group group;
    public int currentHP;
    public float currentSpeed;

    //加速度、加速度修正间隔
    public float accelerationOfSpeed =0 ;//加速度
    public float changAccelerationOfSpeedTime;//修改加速时间
    public const float Timer = 0.1f;

    //油门，决定加速度
    public float gas = 0;

    //获取攻击距离，从武器中获得最大的那个
    public float GetAttackDistance ()
    {
        float distance = 0;
        for (int i = 0;i<weapons.Length;i++)
        {
            if(weapons[i].attackDistance > distance)
            {
                distance = weapons[i].attackDistance;
            }
        }
        return distance;
    }

    //实例化相关类，设置默认速度0、阵营中立
    private void Awake()
    {
        attr = new EntityAttr();
        weapons = GetComponentsInChildren<EntityWeaponBase>();
        ecb = GetComponentInChildren<EntityColliderBase>();
        ai = GetComponentInChildren<AIControl>();
        engines = GetComponentsInChildren<EntityEngineFXControl>();
        //Debug.Log($"weapons number is: { weapons.Length}");
        currentSpeed = 0;
        group = Group.neutral;
        isActive = true;
    }
 
    //初始化，实体必须要初始化之后才能使用
    //参数：编号，路径，属性，坐标transform
    public void Init(string id,string url,string attrid, string[] str, Vector3 pos, float z)
    {
        //当id等于null时，说明unitID已经存在
        if (id != null) unitID = id;
        path = url;

        //设置位置和方向
        SetPosition(pos);
        SetDirection(z);

        //设置武器和属性
        string weaponstr = attr.SetAttr(attrid,str);
        SetWeapon(weaponstr);
        SetHp(attr.hp);
        gas = 0.2f;
    }

    //加速度 油门系列
    //获得当前所有引擎的总功率
    public float GetPower()
    {
        float a = 0;
        for(int i=0;i<engines.Length;i++)
        {
            a += engines[i].power;
        }
        return a;
    }

    //设置引擎的功率，不加参数为取属性gas作为总功率
    public void SetPower()
    {
        for (int i = 0; i < engines.Length; i++)
        {
            engines[i].SetEnginePower( gas /engines.Length);
        }
    }
    public void SetPower(float x)
    {
        for (int i = 0; i < engines.Length; i++)
        {
            engines[i].SetEnginePower(x / engines.Length);
        }
        //重置定时器，确保手动参数更新油门之后不会很快变化
        changAccelerationOfSpeedTime = Timer;
    }

    //设置加速度，计算公式为，假定当前速度为0，gas>0.2f，为加速向前，反之，则减速向后
    //当前速度越快，加速度越小
    public void SetAccelerationOfSpeed()
    {
        float power = GetPower();
        //if(power > 0.2f)
        //{
        //    accelerationOfSpeed = (power - 0.2f - currentSpeed / attr.speed)*100;
        //}
        //else
        //{
        //    accelerationOfSpeed = (0.2f - power + 0.2f * currentSpeed / attr.speed) * 100;
        //}
        accelerationOfSpeed = (power - 0.2f - currentSpeed / attr.speed) * 100;
    }

    //刷新油门定时器，当定时器达到时间，会重置功率和加速度
    public void RefreshGas(float time)
    {
        if(changAccelerationOfSpeedTime<0)
        {
            changAccelerationOfSpeedTime = Timer;
            SetPower();
            SetAccelerationOfSpeed();
            return;
        }
        changAccelerationOfSpeedTime -= time;
    }

    //改变油门，用于移动的时候对gas进行调整
    //当y=0时，表示不改变油门，油门会自动变化到0.2f附近，即缓慢减速到0
    public void ChanegGas(float y,float time)
    {
        gas += y * time;
        if (y>0)
        {
            if (gas > 1) gas = 1;
        }
        else if(y<0)
        {
            if (gas < 0) gas = 0;
        }
        else
        {
            if(gas>0.2f)
            {
                gas -= time;
            }
            else
            {
                gas += time;
            }
        }
    }

    #region 属性相关函数
    //设置位置
    public void SetPosition(Vector3 pos)
    {
        this.transform.position = pos;
    }

    //设置方向
    public void SetDirection(float z)
    {
        transform.eulerAngles = new Vector3(0, 0, z);
    }

    //设置血量，分配血条UI
    public void SetHp(int x)
    {
        //这里当前使得玩家无敌
        if (group == Group.player) return;
        
        //当血量小于0，阵亡
        if (x<=0)
        {
            PushPool();
            return;
        }

        //正常设置血量，需要从UISystem分配血条UI，再设置
        currentHP = x;
        if (hps == null)
        {
            hps = UISystem.Instance.CreateSlider();
        }
        hps.value = (float)x / attr.hp;
    } 
    
    //设置武器
    public virtual void SetWeapon(string str)//ID002/ID001/ID001/
    {
        string[] weaponTable = str.Split('/');
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].Init(weaponTable[i]);
            weapons[i].entity = this;
        }
    }

    //切换阵营
    public void SetGroup(Group g)
    {
        group = g;
    }
    #endregion

    //根据前进和转向参数，进行移动
    //参数x，决定旋转
    //参数y，决定前进油门调整
    public void PointToMove(float x,float y,float time)
    {
        if ((currentSpeed > -attr.speed && y == -1) || (currentSpeed < attr.speed && y == 1))
            ChanegGas(y, time);
        //transform.Rotate(0, 0, 0 - x * attr.rotateSpeed  * time, Space.Self);
        ecb.rigidbody.mass = 1;
        ecb.rigidbody.AddTorque(x * attr.rotateSpeed * 0.05f* time);
    }
    //力
    //public void PointToMove(float x, float y, float time)

    //单位平移，未使用
    public void MoveToParallel(float x, float time)
    {
        transform.position += x * attr.speed  * transform.right * time;
    }

    //单位攻击，所有武器同时攻击
    public void Attack()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.GetComponent<EntityWeaponBase>().Attack();
        }
    }

    //实体阵亡回收
    //需要先判断是不是GameControl刷的怪，来控制每一波敌人是否已经消灭完
    //然后回收血条，并置参数为null
    //然后产生爆炸特效并回收实体
    public void PushPool()
    {
        GameControl.Instance.Push(attr.id);

        UISystem.Instance.PushSlider(hps);
        hps = null;

        ParticleSystemSystem.Instance.TestPlayer(this.transform.position);
        EntitySystem.PushEntity(path, this.gameObject);
    }

    //实时更新油门
    //更新当前速度、当前位置变化
    //血条需要跟随实体移动
    private void Update()
    {
        RefreshGas(Time.deltaTime);

        //currentSpeed +=  accelerationOfSpeed * Time.deltaTime;
        //float v = currentSpeed * 0.01f * Time.deltaTime;
        //this.transform.position += v * transform.up;

        ecb.rigidbody.mass = 1;
        ecb.rigidbody.AddForce(-ecb.rigidbody.velocity.x / (0.01f * attr.speed) * transform.up);
        ecb.rigidbody.AddForce(-ecb.rigidbody.velocity.y / (0.01f * attr.speed) * transform.up);
        ecb.rigidbody.AddForce((gas-0.2f) * transform.up);
        
        if(group == Group.player)
        Debug.Log($"速度：{ ecb.rigidbody.velocity} {ecb.rigidbody.angularVelocity}");

        if (hps != null) hps.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0,2,0));
    }

}
