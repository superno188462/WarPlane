using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mathc : Singleton<Mathc>
{
    public EntityShipType JudgeShipType(string str)
    {
        if (str == "巨构") return EntityShipType.Leve16;
        if (str == "无畏舰") return EntityShipType.Level5;
        if (str == "战列舰") return EntityShipType.Level4;
        if (str == "巡洋舰") return EntityShipType.Level3;
        if (str == "驱逐舰") return EntityShipType.Level2;
        if (str == "护卫舰") return EntityShipType.Level1;
        if (str == "民用舰") return EntityShipType.Level0;
        Debug.Log("舰船类型格式错误");
        return EntityShipType.Level0;
    }

    public EntitySPType JudgeSPType(string str)
    {
        if (str == "指向性") return EntitySPType.HXV;
        if (str == "非指向性") return EntitySPType.All;
        Debug.Log("护盾类型格式错误");
        return EntitySPType.All;
    }

    public EntitySlotSize JudgeEntitySlotSize(string str)
    {
        if (str == "小型") return EntitySlotSize.Small;
        if (str == "中型") return EntitySlotSize.Medium;
        if (str == "大型") return EntitySlotSize.Large;
        Debug.Log("槽位大小格式错误");
        return EntitySlotSize.Small;
    }

    public EntitySlotType JudgeEntitySlotType(string str)
    {
        if (str == "物理") return EntitySlotType.Physics;
        if (str == "能量") return EntitySlotType.Energy;
        if (str == "导弹") return EntitySlotType.Missile;
        if (str == "复合") return EntitySlotType.Compound;
        Debug.Log("槽位类型格式错误");
        return EntitySlotType.Physics;
    }


    public EntityBulletAttackType JudgeEntityBulletAttackType(string str)
    {
        if (str == "直射") return EntityBulletAttackType.Line;
        if (str == "制导") return EntityBulletAttackType.Nav;
        if (str == "瞬发") return EntityBulletAttackType.Laser;
        Debug.Log("子弹攻击类型格式错误");
        return EntityBulletAttackType.Line;
    }

    public EntityBulletHurtType JudgeEntityBulletHurtType(string str)
    {
        if (str == "常规") return EntityBulletHurtType.Routine;
        if (str == "高爆") return EntityBulletHurtType.Explosive;
        if (str == "破甲") return EntityBulletHurtType.BrokenArmour;
        Debug.Log("子弹伤害类型格式错误");
        return EntityBulletHurtType.Routine;
    }
}

/// <summary>
/// 队伍枚举类型
/// </summary>
public enum EntityGroup
{
    Player,
    Enemy,
    Neutral,
}

/// <summary>
/// 舰船类型
/// </summary>
public  enum EntityShipType
{
    Leve16,//巨构
    Level5,//无畏舰
    Level4,//战列舰
    Level3,//巡洋舰
    Level2,//驱逐舰
    Level1,//护卫舰
    Level0,//民用舰

}

/// <summary>
/// 护盾类型
/// </summary>
public enum EntitySPType
{
    HXV,//指向性
    All,
}

/// <summary>
/// 槽位的大小
/// </summary>
public enum EntitySlotSize
{
    Small, Medium, Large
}

/// <summary>
/// 槽位的类型
/// </summary>
public enum EntitySlotType
{
    Physics,//物理
    Energy, //能量
    Missile, //导弹
    Compound, //复合
}

/// <summary>
///武器装弹类型
/// </summary>
public enum EntityWeaponLoadType
{
    All,//全部补充
    Add,//缓慢补充
}

/// <summary>
///子弹攻击类型
/// </summary>
public enum EntityBulletAttackType
{
    Line,//直射
    Nav,//制导
    Laser,//瞬发
}

/// <summary>
///伤害类型
/// </summary>
public enum EntityBulletHurtType
{
                        //对护盾   对装甲
    Routine,//常规        1	        1
    Explosive,//高爆      1.5	    1
    BrokenArmour,//破甲   1	        1.5
}

/// <summary>
///武器攻击类型
/// </summary>
public enum AttackType
{
    none,
    normal,
    scattering,
}