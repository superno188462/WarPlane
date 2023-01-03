using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : Singleton<PlayerControl>
{
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

    public void Attack()
    {
        unit.Attack();
        //foreach (string str in playerUnit.Keys)
        {
            //playerUnit[str].Attack();
        }
    }

    public void MoveStart()
    {
        unit.SetPower(1.5f);
    }

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

        if (Input.GetKeyDown(KeyCode.W)) MoveStart();
        if (Input.GetKeyDown(KeyCode.Q)) unit.MoveToParallel(1, Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.E)) unit.MoveToParallel(-1, Time.deltaTime); 

        float MoveX = Input.GetAxisRaw("Horizontal");
        float MoveY = Input.GetAxisRaw("Vertical");
        unit.PointToMove(MoveX, MoveY,Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0)) Attack();
        if (Input.GetMouseButton(1)) RightTrun(Input.mousePosition);


    }
}
