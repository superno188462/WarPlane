/***********************************************
 * \file        AIEntityBase.cs
 * \author      
 * \date        
 * \version     
 * \brief       单位ai基类
 * \note        
 * \remarks     
 ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEntityBase : MonoBehaviour
{
    //单位ai控制器
    public AIControl ai;

    //持续攻击后会切换状态，暂时没有实现
    public float attackTimer = 5;
    public const float Timer = 5;
    public bool attackState;

    //创建单位时，获取单位ai总控制 ai，并初始化
    private void Awake()
    {
        ai = GetComponent<AIControl>();
        Init();
    }

    //初始化，设置攻击目标和攻击状态
    public void Init()
    {
        SetAttackAim();
        attackState = true;
    }

    //设置攻击目标
    public void SetAttackAim()
    {
        ai.attackAim = PlayerControl.Instance.unit;
    }

    //设置移动位置，有攻击目标时，ai总控制器的移动目标即攻击目标
    public void SetCurrentPosToMove()
    {
        if (ai.attackAim != null) ai.aimPos = ai.attackAim.transform.position;
    }

    //根据函数计算单位向目标移动需要进行的转向和前进移动，并调用单位的移动函数
    // virtual ：不同单位的移动方式可能会有区别
    public virtual void HowToAimPos()
    {
            int x = Order.MoveToPosInRotate(ai.unit, ai.aimPos);
            int y = Order.MoveToPosInLine(ai.unit, ai.aimPos);
            ai.unit.PointToMove(x, y, Time.deltaTime);
    }

    //操作攻击
    public void BeInAttack(float time)
    {
        //if(attackTimer<0)
        //{
        //    attackTimer = Timer;
        //    attackState = !attackState;
        //}
        //attackTimer -= time;
    }

    //当ai控制是激活状态，会持续修正移动目标并进行移动
    void Update()
    {
        if (ai.isActive == false) return;

        SetCurrentPosToMove();
        HowToAimPos();

        //暂未实现攻击状态改变
        if (ai.attackAim != null)
            BeInAttack(Time.deltaTime);
    }
}
