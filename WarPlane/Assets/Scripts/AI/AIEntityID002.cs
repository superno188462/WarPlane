/***********************************************
 * \file        AIEntityID002.cs
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

public class AIEntityID002 : AIEntityBase
{
    //单位2 攻击方式为撞向攻击目标
    public override void HowToAimPos()
    {
        int x = Order.CollisionToPosInRotate(ai.unit, ai.aimPos);
        int y = Order.CollisionToPosInLine(ai.unit, ai.aimPos);
        ai.unit.PointToMove(x, y, Time.deltaTime);
    }
}
