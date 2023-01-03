using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEntityBase : MonoBehaviour
{
    public AIControl ai;
    public float attackTimer = 5;
    public const float Timer = 5;
    public bool attackState;

    private void Awake()
    {
        ai = GetComponent<AIControl>();
        Init();
    }

    public void Init()
    {
        SetAttackAim();
        attackState = true;
    }

    //设置攻击目标
    public void SetAttackAim()
    {
        ai.attackAim = PlayerControl.Instance.unit;
    }

    //设置移动位置
    public void SetCurrentPosToMove()
    {
        if (ai.attackAim != null) ai.aimPos = ai.attackAim.transform.position;
    }

    public virtual void HowToAimPos()
    {
       
            int x = Order.MoveToPosInRotate(ai.unit, ai.aimPos);
            int y = Order.MoveToPosInLine(ai.unit, ai.aimPos);
            ai.unit.PointToMove(x, y, Time.deltaTime);
        
    }

    //操作攻击
    public void BeInAttack(float time)
    {
        //if(attackTimer<0)
        //{
        //    attackTimer = Timer;
        //    attackState = !attackState;
        //}
        //attackTimer -= time;
    }
    void Update()
    {
        if (ai.isActive == false) return;

        SetCurrentPosToMove();
        HowToAimPos();

        if (ai.attackAim != null)
            BeInAttack(Time.deltaTime);
    }
}
