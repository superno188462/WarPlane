/***********************************************
 * \file        AIEntityID003.cs
 * \author      
 * \date        
 * \version     
 * \brief       单位ai子类
 * \note        
 * \remarks     
 ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEntityID003 : AIEntityBase
{
    //单位3 攻击方式为正常，会移动向目标位置，指定距离后会环绕去移动
    public override void HowToAimPos()
    {
        //攻击状态始终为true
        if(attackState==false)
        {
            int x = Order.MoveToPosInRotate(ai.unit, ai.aimPos);
            int y = Order.MoveToPosInLine(ai.unit, ai.aimPos);
            ai.unit.PointToMove(x, y, Time.deltaTime);
        }
        else //会在单位附近横跳
        {
            int x, para;
            Order.AttackToAim(ai.unit, ai.attackAim,out  x,out para);
            int y = Order.MoveToPosInLine(ai.unit, ai.aimPos);
            ai.unit.PointToMove(x, y, Time.deltaTime);
            ai.unit.MoveToParallel(para, Time.deltaTime);
        }
        
    }
}
