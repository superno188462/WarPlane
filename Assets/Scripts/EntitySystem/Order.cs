﻿/***********************************************
 * \file        Order.cs
 * \author      
 * \date        
 * \version     
 * \brief       中间函数，算法公式
 * \note        
 * \remarks     
 ***********************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Order : Singleton<Order>
{
    //从列表中获取路径点向量，未使用
    public static Vector3 Pop(List<Vector3> list)
    {
        if (list.Count == 0) return Vector3.zero;
        Vector3 tmp = list[0];
        list.RemoveAt(0);
        return tmp;
    }

    #region 计算公式
    //需要旋转到的角度
    public static float DirectionZ(Vector3 v)
    {
        Vector3 tmp =  new Vector3(v.x, v.y, 0);
        float z = Vector3.Angle(tmp,  Vector3.up);//距离目标的欧拉角

        if (v.x > 0)
        {
            z = 360 - z;
        }

        return z;
    }

    //当前角度
    public static float CurrentZ(float p)
    {
        while (p < 0)
        {
            p = p + 360;
        }
        while (p >= 360)
        {
            p -= 360;
        }
        return p;
    }
    //归一化角度，使用角度之前将其设置成指定格式,均为顺时针旋转0-360度
    public static float NoramlZ(float p)
    {
        while (p < 0)
        {
            p = p + 360;
        }
        while (p >= 360)
        {
            p -= 360;
        }
        return p;
    }

    //计算方向向量和相关角度
    public static int AngleOfRotation(Vector3 aimPos, Vector3 thisPos, Vector3 thisRotate)
    {
        Vector3 direction = aimPos - thisPos;//方向向量


        float z = DirectionZ(direction);
        float currentZ = NoramlZ(thisRotate.z);

        if(z>=currentZ) return (int)(z - currentZ) % 360;
        return (int)( 360+z- currentZ) % 360;
    }
    #endregion

    #region 速度变化
    //根据旋转角度确定加速度参数的变化
    public static int SetAcceleration(int angle)
    {
        int y = 0;//加速参数
        if (angle > 90 && angle < 270)
        {
            y = -1;
        }
        else
        {
            y = 1;
        }
        return y;
    }

    //根据旋转角度、保持距离、当前速度 确定加速度参数的变化（接近目标变化）
    public static int SetAcceleration(int angle,float keepDistance,Vector3 direction)
    {
        int y = SetAcceleration(angle);
        return y;
    }

    //根据旋转角度确定旋转方向参数的变化
    public static int SetRotateDirection(int angle)
    {
        if (angle == 0)
        {
            return 0;
        }
        else if (angle < 180)
        {
            return -1;
        }
        else if (angle >= 180)
        {
            return 1;
        }
        return 0;
    }

    //根据旋转角度、保持距离、当前速度 确定旋转方向参数的变化(接近目标变化)
    public static int SetRotateDirection(int angle, float keepDistance, Vector3 direction)
    {
        int x = SetRotateDirection(angle);
        if (direction.magnitude < keepDistance)
        {
            x = SetRotateDirection((angle + 90) % 360);
        }
        return x;
    }
    #endregion

    #region Entity
    //向目标直线加速
    public static int  MoveToPosInLine(EntityBase unit, Vector3 aimPos)
    {
        int angle = AngleOfRotation(aimPos, unit.transform.position, unit.transform.eulerAngles);
        Vector3 direction = aimPos - unit.transform.position;
        //Debug.DrawLine(unit.transform.position, unit.transform.position + unit.transform.up * 5, Color.blue);
        //Debug.DrawLine(unit.transform.position, unit.transform.position + direction.normalized * 5, Color.red);
        return SetAcceleration(angle,unit.GetAttackDistance(), direction);

    }

    //向目标旋转
    public static int MoveToPosInRotate(EntityBase unit, Vector3 aimPos)
    {
        //旋转判断
        int angle = AngleOfRotation(aimPos, unit.transform.position, unit.transform.eulerAngles);
        Vector3 direction = aimPos - unit.transform.position;
        //if (unit.group == EntityGroup.Player)
            //Debug.Log($"{unit.transform.eulerAngles} {angle} {direction} ");
        return SetRotateDirection(angle,  unit.GetAttackDistance(),direction);
    }

    public static int MoveToPosInRotate(EntityBase unit, Vector3 aimPos,float x)
    {
        //旋转判断
        int angle = AngleOfRotation(aimPos, unit.transform.position, unit.transform.eulerAngles);
        Vector3 direction = aimPos - unit.transform.position;
       
        return SetRotateDirection(angle, x, direction);
    }

    //向预定位置移动
    public static int CollisionToPosInLine(EntityBase unit, Vector3 aimPos)
    {
        int angle = AngleOfRotation(aimPos, unit.transform.position, unit.transform.eulerAngles);
        return SetAcceleration(angle);
    }
    //向预定位置旋转
    public static int CollisionToPosInRotate(EntityBase unit, Vector3 aimPos)
    {
        int angle = AngleOfRotation(aimPos, unit.transform.position, unit.transform.eulerAngles);
        return SetRotateDirection(angle);
    }

    //向目标攻击，未使用
    public static void AttackToAim(EntityBase unit, EntityBase aim,out int x,out int para)
    {
        
        int angle = AngleOfRotation(aim.transform.position, unit.transform.position, unit.transform.eulerAngles);
        x = SetRotateDirection(angle);

        //按照和玩家旋转方向相悖的位置平移
        int angle2 = AngleOfRotation(unit.transform.position, aim.transform.position, aim.transform.eulerAngles);
        para = -SetRotateDirection(angle2);
    }
    //向目标旋转
    public static int AttackToPosInRotate(EntityBase unit, Vector3 aimPos)
    {
        int angle = AngleOfRotation(aimPos, unit.transform.position, unit.transform.eulerAngles);
        return SetRotateDirection(angle);
    }
    #endregion

    #region 武器
    //向目标旋转
    public static int MoveToPosInRotate(EntityWeaponBase weapon, Vector3 aimPos)
    {
        //旋转判断
        int angle = AngleOfRotation(aimPos, weapon.transform.position, weapon.transform.eulerAngles);
        //Debug.Log($"draw{weapon.transform.position} {weapon.transform.up * 5} {weapon.transform.position + weapon.transform.up * 5}");
        Debug.DrawLine(weapon.transform.position, weapon.transform.position + Vector3.Normalize( new Vector3(weapon.transform.up.x, weapon.transform.up.y,0))  * 10, Color.blue);
        //Debug.Log(Vector3.magnitude);
        //if (weapon.entity.group == Group.player)
        //    Debug.Log($"{aimPos} {weapon.transform.position} {weapon.transform.eulerAngles} {weapon.entity.transform.eulerAngles}");
        
        //预旋转方向/当前旋转方向
        int x = SetRotateDirection(angle);
        
        float tmp = Order.NoramlZ(weapon.transform.eulerAngles.z - weapon.entity.transform.eulerAngles.z);
        //Debug.Log($"{weapon.transform.eulerAngles.z} {weapon.entity.transform.eulerAngles.z} {tmp}");
        if (x == 1)
        {
            if (tmp > 180 && tmp < 360 - weapon.entity.entityData.rightR)
            {
                return 0;
            }
        }
        else if(x==-1)
        {
            if (tmp > weapon.entity.entityData.leftR && tmp <= 180)
            {
                return 0;
            }
        }
        
        //Debug.Log($"{weapon.transform.eulerAngles} {x}");
        return x;
    }


    //向目标攻击
    public static void Attack(EntityWeaponBase weapon)
    {
        weapon.Attack();
    }
    #endregion

    //检测前行路径有没有敌人
    public static bool EmenyInPath( EntityWeaponBase weapon)
    {
        //RaycastHit2D hit = Physics2D.Raycast(weapon.transform.position, weapon.transform.up, 100, 1 << 8 | 1 << 9);
        RaycastHit2D[] hits = Physics2D.RaycastAll(weapon.transform.position, weapon.transform.up, 100, 1 << 8 | 1 << 9);

        //Debug.DrawLine(weapon.transform.position, hit.point, Color.red, 0.01f);
        //Debug.DrawLine(weapon.transform.position,weapon.transform.position + weapon.transform.up * 10, Color.yellow, 0.01f);
        
        if (hits.Length>=2)
        {
            //Debug.Log(hits[0].transform.GetComponent<EntityBase>() == null);
            //Debug.Log(hits[0].collider.gameObject.GetComponent<EntityBase>() == null);
            if (hits[0].transform.GetComponent<EntityBase>() != null &&
                hits[0].transform.GetComponent<EntityBase>().group != weapon.entity.group)
            {
                //Debug.Log($"{hits[0].transform.name}打");
                return true;
            }
                
            if(hits[1].transform.GetComponent<EntityBase>()!=null &&
                hits[1].transform.GetComponent<EntityBase>().group == weapon.entity.group)
            {
                //Debug.Log($"{hits[0].transform.name} {hits[1].transform.name}不打");
                return false;
            }
            //Debug.Log($"{hits[0].transform.name} {hits[1].transform.name}打");

        }
        return true;
    }

    #region dotween
    //伤害飘字
    public static void FlyToText(Graphic text)
    {
        RectTransform rt = text.rectTransform;
        Color c = text.color;
        c.a = 0;
        text.color = c;
        Sequence seq = DOTween.Sequence();
        Tweener move1 = rt.DOMoveY(rt.position.y + 0.05f, 0.5f);
        Tweener move2 = rt.DOMoveY(rt.position.y + 20f, 0.5f);
        Tweener move3 = rt.DOMoveY(rt.position.y, 0.5f);
        Tweener alpha1 = text.DOColor(new Color(c.r, c.g, c.b, 1), 0.5f);
        Tweener alpha2 = text.DOColor(new Color(c.r, c.g, c.b, 0), 0.5f);
        seq.Append(move1);
        seq.Join(alpha1);
        seq.AppendInterval(0.2f);
        seq.Append(move2);
        seq.Join(alpha2);
        seq.Append(move3);
        



    }
    #endregion


    #region 画画
    public static void Draw(EntityWeaponBase weapon)
    {
        //Debug.Log("draw");
        ClearDrawCircle(weapon);
        DrawLine(weapon);
        DrawCircle(weapon);
    }
    //画圆,可以提供材质
    public static void DrawCircle(EntityWeaponBase weapon, Material m_material)
    {

    }
    public static void ClearDrawCircle (EntityWeaponBase weapon)
    {
        for(int i=0;i<weapon.entity.weapons.Length;i++)
        {
            weapon.entity.weapons[i].Drawline. SetActive(false);
        }
         return;
        
    }
    public static void DrawLine(EntityWeaponBase weapon)
    {
        //Vector3.Normalize()
        List<Vector3> vPath = new List<Vector3>();
        float R = weapon.bulletData.attackDistance;
        float W = 0.01f;                            //宽度
        int count = 60;
        Vector3 v = weapon.transform.position;// - weapon.entity.transform.position;
        Vector3 up = weapon.transform.up;// + new Vector3(weapon.entity.transform.up.x, weapon.entity.transform.up.y, 0);
        Vector3 go = R * up;
        //Debug.Log(go.magnitude);
        //Debug.Log(DirectionZ(weapon.transform.up));
        //Debug.Log($"RRRRRR: {R}");
        weapon.Drawline.SetActive(true);
        GameObject obj = weapon.Drawline.transform.GetChild(0).gameObject;
        LineRenderer dynamicLine = obj.GetComponent<LineRenderer>();
        if (dynamicLine == null) dynamicLine = obj.AddComponent<LineRenderer>();


        for (int i = 1; i < count; i++)
        {
            
                float x = (i) * go.x / count + v.x;
                float y = (i) * go.y / count + v.y;
                float z = 0;
                vPath.Add(new Vector3(x, y, z));
            if (i == count-1)
            {
                //Debug.Log($"{DirectionZ(weapon.entity.transform.up)}");
                //Debug.Log($"{DirectionZ(weapon.transform.up)} {weapon.transform.position}{10*weapon.transform.up} {weapon.transform.position+10* weapon.transform.up}");
                //Debug.Log($"{DirectionZ(new Vector3(x, y, z)- v)} {v} {new Vector3(x, y, z) - v} {new Vector3(x, y, z)}");
            }
        }
            dynamicLine.useWorldSpace = true;
            dynamicLine.positionCount = vPath.Count;
            dynamicLine.startWidth = W;
            dynamicLine.endWidth = W;
            dynamicLine.SetPositions(vPath.ToArray());
        
    }
    public static void DrawCircle(EntityWeaponBase weapon)
    {
        
        //Debug.Log(weapon.transform.name);
        List<Vector3> vPath = new List<Vector3>();
        Vector3 v = weapon.transform.position - weapon.entity.transform.position;      //圆心偏移
        float R = weapon.bulletData.attackDistance;            //半径
        float W = 0.01f;                            //宽度
        int count = 60;                             //完成一个圆的总点数，
        float angleL = weapon.entity.entityData.leftR;                           // 准备画线的弧度
        float angleR = weapon.entity.entityData.leftR;
        float angleL2 = 2 * Mathf.PI / (count - 1) * angleL/360;   //当前点转角，三个点形成的两段线之间的夹角
        float angleR2 = 2 * Mathf.PI / (count - 1) * angleR / 360;

        //Debug.Log(R);
        weapon.Drawline.SetActive(true);
        GameObject obj = weapon.Drawline.transform.GetChild(1).gameObject;
        LineRenderer line = obj.GetComponent<LineRenderer>();
        if (line == null) line = obj.AddComponent<LineRenderer>();

        float x, y, z;
        for (int i = 0; i <= (count + 1); i++)
        {

            //Debug.Log($"RRRRRR: {R}");
            x = Mathf.Sin(angleR2 * (60 - i)) * R + v.x;
            y = Mathf.Cos(angleR2 * (60 - i)) * R + v.y;
            z = 0;
            //if (i == 0)
                //Debug.Log($"{v.x} {v.y} {x} {y} {z}");
            vPath.Add(new Vector3(x, y, z));
        }
        for (int i = 0; i <= (count + 1); i++)
        {
                x =   v.x-Mathf.Sin(angleL2 * (i))* R ;
                y =  v.y +Mathf.Cos(angleL2 * (i)) * R;
                z = 0;
            vPath.Add(new Vector3(x, y, z));
            

        }

        line.useWorldSpace = false;
        line.positionCount = vPath.Count;
        line.startWidth = W;
        line.endWidth = W;
        line.SetPositions(vPath.ToArray());
        
    }
    #endregion 
}

