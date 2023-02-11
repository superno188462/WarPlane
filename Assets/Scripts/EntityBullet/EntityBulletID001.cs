/***********************************************
 * \file        EntityBulletID001.cs
 * \author      
 * \date        
 * \version     
 * \brief       子弹ID001
 * \note        
 * \remarks     
 ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBulletID001 : EntityBulletBase
{ 
    //移动
    public override float PointToMove(float time)
    {
        float v = bulletData.bulletSpeed * time;
        //Debug.Log(v/time);
        transform.position += v * transform.up;
        //transform.position=Vector3.MoveTowards(transform.position,目标点，每个逻辑帧能走的距离)；
        return v;
    }

    //修改生存周期
    public override void RefreshLife(float time)
    {
        //Debug.Log($"{life} {PointToMove(time)}");
        life -= time;
        if (life < 0)
        {
            PushPool();
        }
    }


    



}
