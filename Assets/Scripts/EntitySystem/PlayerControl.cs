/***********************************************
 * \file        PlayerControl.cs
 * \author      
 * \date        
 * \version     
 * \brief       玩家控制器
 * \note        
 * \remarks     
 ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : Singleton<PlayerControl>
{
    //玩家阵营的控制单位
    //public Dictionary<string, EntityBase> playerUnit;
    public EntityBase unit;
    public EntityWeaponBase weapon;
    public LineRenderer line;

    protected override void Awake()
    {
        base.Awake();
        line = null;
        //playerUnit = new Dictionary<string, EntityBase>();
    }
    public void UnitEnter(EntityBase unitIn)
    {
        unit = unitIn;
        line = null;
        line = unit.gameObject.GetComponent<LineRenderer>();
        if (line == null) line=unit.gameObject.AddComponent<LineRenderer>();

        
        if (unit.weapons != null)
        {
            weapon = PlayerControl.Instance.unit.weapons[0];
        }
        else
        {
            weapon = null;
        }
        
        //Debug.Log($"{UISystem.Instance.layout.panels.Count}  ");
        (UISystem.Instance.layout.panels["DownPanel"] as DownPanel).PrintWeaponTable(unit);

    }

    //玩家控制的单位进行攻击
    public void Attack()
    {
        if(weapon != null)
            weapon.Attack();
        //foreach (string str in playerUnit.Keys)
        {
            //playerUnit[str].Attack();
        }
    }

    //当玩家按下加速瞬间会有引擎超功率效果
    public void MoveStart()
    {
        unit.SetPower(1.5f);
    }

    //旋转到指定方向
    public void UnitRightTrun(Vector3 aimPos) 
    {
        //Debug.Log($"{aimPos} {Camera.main.ScreenToWorldPoint(aimPos)}{unit.transform.position}");
        int x = Order.MoveToPosInRotate(unit, Camera.main.ScreenToWorldPoint(aimPos));
        unit.PointToMove(x, 0, Time.deltaTime);
    }
    public void WeaponRightTrun(Vector3 aimPos)
    {
        //Debug.Log($"{aimPos} {Camera.main.ScreenToWorldPoint(aimPos)}{unit.transform.position}");
        int x = Order.MoveToPosInRotate(weapon, Camera.main.ScreenToWorldPoint(aimPos));
        weapon.PointToMove(x, Time.deltaTime);
    }
    //切换控制武器
    public void ChangeWeapon(int i)
    {
        if (unit.weapons.Length>i)
        {
            Order.ClearDrawCircle();
            weapon = unit.weapons[i];
        }
    }
    void Update()
    {
        if (unit == null) return ;
        if (unit.ai.isActive == true) unit.ai.isActive = false;
        if (weapon != null) Order.DrawCircle(weapon);
        //加速
        if (Input.GetKeyDown(KeyCode.W)) MoveStart();
        
        //平移，不用
        if (Input.GetKeyDown(KeyCode.Q)) unit.MoveToParallel(1, Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.E)) unit.MoveToParallel(-1, Time.deltaTime); 

        //wasd移动
        float MoveX = Input.GetAxisRaw("Horizontal");
        float MoveY = Input.GetAxisRaw("Vertical");
        unit.PointToMove(MoveX, MoveY,Time.deltaTime);

        //j键或鼠标左键攻击，鼠标右键旋转到指定方向
        if (Input.GetKeyDown(KeyCode.J)) Attack();
        if (Input.GetMouseButtonDown(0)) Attack();
        if (Input.GetMouseButton(1)) WeaponRightTrun(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeWeapon(2);


    }
}
