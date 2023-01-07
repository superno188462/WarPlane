/***********************************************
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
        float z = Vector3.Angle(v, Vector3.up);//距离目标的欧拉角

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

    //计算方向向量和相关角度
    public static int AngleOfRotation(Vector3 aimPos, Vector3 thisPos, Vector3 thisRotate)
    {
        Vector3 direction = aimPos - thisPos;//方向向量

        float z = DirectionZ(direction);
        float currentZ = CurrentZ(thisRotate.z);

        return (int)(z - currentZ + 360) % 360;
    }
    #endregion

    #region 速度变化
    //根据旋转角度确定加速度参数的变化
    public static int SetAcceleration(int angle)
    {
        int y = 0;//加速参数
        if (angle > 90 && angle < 180)
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
    public static int SetAcceleration(int angle,float keepDistance, float currentSpeed,Vector3 direction)
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
    public static int SetRotateDirection(int angle, float keepDistance, float currentSpeed, Vector3 direction)
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
        Debug.DrawLine(unit.transform.position, unit.transform.position + unit.transform.up * 5, Color.blue);
        Debug.DrawLine(unit.transform.position, unit.transform.position + direction.normalized * 5, Color.red);
        return SetAcceleration(angle,unit.GetAttackDistance(),unit.currentSpeed, direction);

    }

    //向目标旋转
    public static int MoveToPosInRotate(EntityBase unit, Vector3 aimPos)
    {
        //旋转判断
        int angle = AngleOfRotation(aimPos, unit.transform.position, unit.transform.eulerAngles);
        Vector3 direction = aimPos - unit.transform.position;
        return SetRotateDirection(angle,  unit.GetAttackDistance(), unit.currentSpeed,direction);
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
        return SetRotateDirection(angle);
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

}

