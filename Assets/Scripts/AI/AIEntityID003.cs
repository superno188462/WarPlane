using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEntityID003 : AIEntityBase
{
    public override void HowToAimPos()
    {
        if(attackState==false)
        {
            int x = Order.MoveToPosInRotate(ai.unit, ai.aimPos);
            int y = Order.MoveToPosInLine(ai.unit, ai.aimPos);
            ai.unit.PointToMove(x, y, Time.deltaTime);
        }
        else
        {
            int x, para;
            Order.AttackToAim(ai.unit, ai.attackAim,out  x,out para);
            int y = Order.MoveToPosInLine(ai.unit, ai.aimPos);
            ai.unit.PointToMove(x, y, Time.deltaTime);
            ai.unit.MoveToParallel(para, Time.deltaTime);
        }
        
    }
}
