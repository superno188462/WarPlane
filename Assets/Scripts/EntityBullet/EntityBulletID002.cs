using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBulletID002 : EntityBulletBase
{


    public override float PointToMove(float time)
    {
        float v = attr.speed / 50 * time;
        transform.position += v * transform.up;
        return v;
    }

    public override void RefreshLife(float time)
    {
        life -= PointToMove(time);
        if (life < 0)
        {
            PushPool();
        }
    }
}
