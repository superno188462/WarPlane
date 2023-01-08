/***********************************************
 * \file        EntityBulletID002.cs
 * \author      
 * \date        
 * \version     
 * \brief       子弹ID002
 * \note        
 * \remarks     
 ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBulletID002 : EntityBulletBase
{
    //移动
    public override float PointToMove(float time)
    {
        float v = attr.speed / 50 * time;
        transform.position += v * transform.up;
        return v;
    }
    //修改生存周期
    public override void RefreshLife(float time)
    {
        life -= PointToMove(time);
        if (life < 0)
        {
            PushPool();
        }
    }
}
