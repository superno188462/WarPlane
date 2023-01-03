using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEntityID002 : AIEntityBase
{
    public override void HowToAimPos()
    {
        int x = Order.CollisionToPosInRotate(ai.unit, ai.aimPos);
        int y = Order.CollisionToPosInLine(ai.unit, ai.aimPos);
        ai.unit.PointToMove(x, y, Time.deltaTime);
    }
}
