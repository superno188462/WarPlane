/***********************************************
 * \file        AIWeapon.cs
 * \author      
 * \date        
 * \version     
 * \brief       武器ai
 * \note        
 * \remarks     
 ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeapon : MonoBehaviour
{
    //单位ai控制器
    public AIControl ai;
    //挂载武器
    public EntityWeaponBase weapon;
    
    //创建单位时，为武器ai分配单位ai控制器和挂载武器
    private void Awake()
    {
        ai = GetComponent<AIControl>();
        weapon = GetComponent<EntityWeaponBase>();
    }

    //当ai激活时，会持续修正武器的转向，会持续攻击（当路径上有队友时暂时停止攻击）
    private void Update()
    {
        if (ai.isActive == false) return;

        int x = Order.MoveToPosInRotate(weapon, ai.aimPos);
        weapon.PointToMove(x, Time.deltaTime);

        if (ai.unitAI !=null && ai.unitAI.attackState == true)
        {
            if (Order.EmenyInPath(weapon) == false) return;
            Order.Attack(weapon);
        }

    }
}
