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

    protected override void Awake()
    {
        base.Awake();
        //playerUnit = new Dictionary<string, EntityBase>();
    }
    public void UnitEnter(EntityBase unit)
    {
        //playerUnit.Add(unit.attr.id,unit);
    }

    //玩家控制的单位进行攻击
    public void Attack()
    {
        unit.Attack();
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
    public void RightTrun(Vector3 aimPos) 
    {
        //Debug.Log($"{aimPos} {Camera.main.ScreenToWorldPoint(aimPos)}{unit.transform.position}");
        int x = Order.MoveToPosInRotate(unit, Camera.main.ScreenToWorldPoint(aimPos));
        unit.PointToMove(x, 0, Time.deltaTime);
    }

    void Update()
    {
        if (unit == null) return ;
        if (unit.ai.isActive == true) unit.ai.isActive = false;

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
        if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0)) Attack();
        if (Input.GetMouseButton(1)) RightTrun(Input.mousePosition);


    }
}
