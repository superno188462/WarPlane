using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//编号 hp	speed	rotatespeed	weapon
//ID001	5	20	20		ID002/ID001/ID001
public enum Group
{ 
    player,
    enemy,
    neutral,
}

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
    public string unitID;
    public string path;
    public bool isActive;

    //链接关系
    public EntityWeaponBase[] weapons;
    public EntityColliderBase ecb;
    public AIControl ai;
    public EntityEngineFXControl[] engines;
    public Slider hps;

    public EntityAttr attr;
    public Group group;
    public int currentHP;
    public float currentSpeed;

    //速度
    public float accelerationOfSpeed =0 ;//加速度
    public float changAccelerationOfSpeedTime;//修改加速时间
    public const float Timer = 1;

    public float gas = 0;//油门，决定加速度


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

    public void ChangeGroup(Group g)
    {

    }

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
 
    //初始化
    //参数：编号，路径，属性，坐标transform
    public void Init(string id,string url,string attrid, string[] str, Vector3 pos, float z)
    {
        if (id != null) unitID = id;
        path = url;

        //设置位置
        SetPosition(pos);
        SetDirection(z);

        //设置武器和属性
        string weaponstr = attr.SetAttr(attrid,str);
        SetWeapon(weaponstr);
        SetHp(attr.hp);
        gas = 0.2f;
    }

    //加速度 油门系列
    public float GetPower()
    {
        float a = 0;
        for(int i=0;i<engines.Length;i++)
        {
            a += engines[i].power;
        }
        return a;
    }
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
        changAccelerationOfSpeedTime = 0.1f;
    }
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
    public void SetPosition(Vector3 pos)
    {
        this.transform.position = pos;
    }

    public void SetDirection(float z)
    {
        transform.eulerAngles = new Vector3(0, 0, z);
    }

    public void SetHp(int x)
    {
        if (group == Group.player) return;
        
        if (x<=0)
        {
            PushPool();
            return;
        }
        currentHP = x;
        if (hps == null)
        {
            hps = UISystem.Instance.CreateSlider();
        }
        hps.value = (float)x / attr.hp;
    } 
    
    public virtual void SetWeapon(string str)//ID002/ID001/ID001/
    {
        string[] weaponTable = str.Split('/');
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].Init(weaponTable[i]);
            weapons[i].entity = this;
        }
    }

    public void SetGroup(Group g)
    {
        group = g;
    }
    #endregion



    public void PointToMove(float x,float y,float time)
    {
        if ((currentSpeed > -attr.speed && y == -1) || (currentSpeed < attr.speed && y == 1))
            ChanegGas(y, time);
        transform.Rotate(0, 0, 0 - x * attr.rotateSpeed  * time, Space.Self);
    }
    public void MoveToParallel(float x, float time)
    {
        transform.position += x * attr.speed  * transform.right * time;
    }


    public void Attack()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.GetComponent<EntityWeaponBase>().Attack();
        }
    }

    public void PushPool()
    {
        GameControl.Instance.Push(attr.id);

        UISystem.Instance.PushSlider(hps);
        hps = null;


        ParticleSystemSystem.Instance.TestPlayer(this.transform.position);
        EntitySystem.PushEntity(path, this.gameObject);
    }
    private void Update()
    {
        RefreshGas(Time.deltaTime);
        currentSpeed +=  accelerationOfSpeed * Time.deltaTime;
        float v = currentSpeed * 0.01f * Time.deltaTime;
        this.transform.position += v * transform.up;
        if(hps != null) hps.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0,2,0));

    }

}
