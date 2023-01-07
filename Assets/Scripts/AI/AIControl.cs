/***********************************************
 * \file        AIControl.cs
 * \author      
 * \date        
 * \version     
 * \brief       单位 ai控制器
 * \note        
 * \remarks     
 ***********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControl : MonoBehaviour
{
    //是否启用该脚本函数
    public bool isActive;

    //记录控制的单位ai
    public EntityBase unit;
    //使用的单位ai基类
    public AIEntityBase unitAI;
    //单位下的武器控制ai
    public AIWeapon[] weaponsAI;

    //ai的攻击目标或移动目标
    public EntityBase attackAim;
    //public List<Transform> PosList;
    public Vector3 aimPos;//目标向量

    //创建一个单位后
    //获取控制单位 unit 和单位ai基类 unitAI ，分配武器控制ai weaponsAI，默认设置单位由ai控制 isActive
    private void Awake()
    {
        unitAI = this.gameObject.GetComponent<AIEntityBase>();
        EntityWeaponBase[] weapons = this.gameObject.GetComponentsInChildren<EntityWeaponBase>();
        for(int i=0;i<weapons.Length;i++)
        {
            AIWeapon tmp = weapons[i].gameObject.AddComponent<AIWeapon>();
            tmp.ai = this;
        }
        weaponsAI = this.gameObject.GetComponentsInChildren<AIWeapon>();

        isActive = true;

        unit = GetComponentInChildren<EntityBase>();
    }

}
