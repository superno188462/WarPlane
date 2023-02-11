using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EntityBase : MonoBehaviour
{
    //场景中entity的唯一id
    public string unitID;
    //回收存储位置
    public string path;
    
    //判断是否正在使用
    public bool isActive;

    public AIControl ai;//ai控制器
    public EntityData entityData; //基本属性
    public EntityWeaponBase[] weapons;//武器
    public EntityColliderBase ECB;//碰撞类
    public EntityEngineFXControl[] engines;//引擎特效

    //血条
    //public Slider hps;

    public EntityGroup group;
    public float gasV = 0; //油门，决定动力比
    //加速度、加速度修正间隔
    //public float accelerationOfSpeed =0 ;//加速度
    //public float changAccelerationOfSpeedTime;//修改加速时间
    public float Timer = 0.1f; //加速特效保持时间
    private float time ; //加速特效保持时间


    //获取攻击距离，从武器中获得最大的那个
    public float GetAttackDistance ()
    {
        float distance = 0;
        for (int i = 0;i<weapons.Length;i++)
        {
            if(weapons[i].bulletData. attackDistance > distance)
            {
                distance = weapons[i].bulletData.attackDistance;
            }
        }

        return distance;
    }

    //实例化相关类，设置默认速度0、阵营中立
    private void Awake()
    {
        entityData = new EntityData();
        weapons = GetComponentsInChildren<EntityWeaponBase>();
        ECB = GetComponentInChildren<EntityColliderBase>();
        ai = GetComponentInChildren<AIControl>();
        engines = GetComponentsInChildren<EntityEngineFXControl>();
        time = 0;
        //Debug.Log($"weapons number is: { weapons.Length}");
        //currentSpeed = 0;
        //group = Group.neutral;
        //isActive = true;
    }
 
    //初始化，实体必须要初始化之后才能使用
    //参数：编号，路径，属性，坐标transform
    public void Init(string id,string url,string data, Vector3 pos, float z)
    {
        group = EntityGroup.Neutral;
        isActive = true;
        //当id等于null时，说明unitID已经存在
        if (id != null) unitID = id;
        path = url;

        //设置位置和方向
        SetPosition(pos);
        SetDirection(z);

        //设置属性、武器、血条
        entityData.Init(data);

        ECB.rigidbody.mass = entityData.mass;

        string[] strs = data.Split('\t');
        //Debug.Log(strs[strs.Length - 1]);
        SetEntityWeapon(strs[strs.Length-2]);

        UISystem.Instance.CreateHPSlider(this);



    }
    #region 速度 
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
            engines[i].SetEnginePower( gasV /engines.Length);
        }
    }
    public void SetPower(float x)
    {
        for (int i = 0; i < engines.Length; i++)
        {
            engines[i].SetEnginePower(x / engines.Length);
        }
        //重置定时器，确保手动参数更新油门之后不会很快变化
        time = Timer;
    }

    ////设置加速度，计算公式为，假定当前速度为0，gas>0.2f，为加速向前，反之，则减速向后
    ////当前速度越快，加速度越小
    //public void SetAccelerationOfSpeed()
    //{
    //    float power = GetPower();
    //    //if(power > 0.2f)
    //    //{
    //    //    accelerationOfSpeed = (power - 0.2f - currentSpeed / attr.speed)*100;
    //    //}
    //    //else
    //    //{
    //    //    accelerationOfSpeed = (0.2f - power + 0.2f * currentSpeed / attr.speed) * 100;
    //    //}
    //    accelerationOfSpeed = (power - 0.2f - currentSpeed / attr.speed) * 100;
    //}

    ////刷新油门定时器，当定时器达到时间，会重置功率和加速度
    //public void RefreshGas(float time)
    //{
    //    if(changAccelerationOfSpeedTime<0)
    //    {
    //        changAccelerationOfSpeedTime = Timer;
    //        SetPower();
    //        SetAccelerationOfSpeed();
    //        return;
    //    }
    //    changAccelerationOfSpeedTime -= time;
    //}

    //改变油门，用于移动的时候对gas进行调整
    //当y=0时，表示不改变油门，油门会自动变化到0.2f附近，即缓慢减速到0
    public void ChanegGas(float y,float time)
    {
        gasV += y * time;
        if (y>0)
        {
            if (gasV > 1) gasV = 1;
        }
        else if(y<0)
        {
            if (gasV < 0) gasV = 0;
        }
        else
        {
            if(gasV > 0.2f)
            {
                gasV -= time;
            }
            else
            {
                gasV += time;
            }
        }
    }

#endregion

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
        //Debug.Log(x);
        //Debug.Log($"die{isActive}");
        float spV;
        if (isActive == false) return;

        //这里当前使得玩家无敌
        if (group == EntityGroup.Player)
        {
            Debug.Log("group is player");
            return;
        } 
        
        //当血量小于0，阵亡
        if (x<=0)
        {
            //Debug.Log(x);
            PushPool();
            //Debug.Log("die");
           
            return;
        }
        //Debug.Log("set hp");
        //正常设置血量，需要从UISystem分配血条UI，再设置
        entityData.hp = x;
        //Debug.Log($"entityData.hp{entityData.hp}");
        //float hpV = entityData.hp/entityData.maxhp;
        //if (entityData.sp == 0)
        //{
        //    spV = 0;
        //}
        //else 
        //{
        //    spV = entityData.sp / entityData.maxsp; ;
        //}
        //UISystem.Instance.ChangeSliderShow(this, hpV, spV);
        
    } 
    
    //设置武器
    public virtual void SetEntityWeapon(string str)//ID002/ID001/ID001/
    {
        string[] weaponTable = str.Split(',');
        for (int i = 0; i < weapons.Length; i++)
        {
            //Debug.Log(weaponTable[i]);
            weapons[i].Init(weaponTable[i]);
            weapons[i].entity = this;
        }
    }

    //切换阵营
    public void SetGroup(EntityGroup g)
    {
        group = g;
    }
    #endregion

    //根据前进和转向参数，进行移动
    //参数x，决定旋转
    //参数y，决定前进油门调整
    public void PointToMove(float x,float y,float time)
    {
        /*
        if ((currentSpeed > -attr.speed && y == -1) || (currentSpeed < attr.speed && y == 1))
            ChanegGas(y, time);
        //transform.Rotate(0, 0, 0 - x * attr.rotateSpeed  * time, Space.Self);
        ecb.rigidbody.mass = 1;
        //ecb.rigidbody.AddTorque(x * attr.rotateSpeed * 0.05f* time);
        //if (group == Group.player)
            //Debug.Log(x * attr.rotateSpeed);
        ecb.rigidbody.MoveRotation(ecb.rigidbody.rotation - x  * attr.rotateSpeed* time);
        */
        ChanegGas( y , time);
        ECB.rigidbody.MoveRotation(ECB.rigidbody.rotation - x * entityData.rotateSpeed * time);
        //transform.Rotate(0, 0, 0 - x * entityData.rotateSpeed * time, Space.Self);
    }
    //力
    //public void PointToMove(float x, float y, float time)

    //单位平移，未使用
    public void MoveToParallel(float x, float time)
    {
        transform.position += x * 0.1f  * transform.right * time;
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
        GameControl.Instance.Push(entityData.id);
        UISystem.Instance.PushHPSlider(this);

        ParticleSystemSystem.Instance.TestPlayer(this.transform.position);
        EntitySystem.PushEntity(path, this.gameObject);
        isActive = false;
    }

    //实时更新油门
    //更新当前速度、当前位置变化
    //血条需要跟随实体移动
   
    

    public void FixedUpdate()
    {
        //RefreshGas(Time.deltaTime);
        //currentSpeed +=  accelerationOfSpeed * Time.deltaTime;
        //float v = currentSpeed * 0.01f * Time.deltaTime;
        //this.transform.position += v * transform.up;

        ECB.rigidbody.AddForce(3*(gasV) *entityData.power* transform.up);

        //if (group == Group.player)
        //Debug.Log($"速度：{ ecb.rigidbody.velocity} {ecb.rigidbody.angularVelocity}");

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].Refresh(Time.deltaTime);
        }
    }

}
