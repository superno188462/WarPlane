using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeapon : MonoBehaviour
{
    public AIControl ai;
    public EntityWeaponBase weapon;
    private void Awake()
    {
        ai = GetComponent<AIControl>();
        weapon = GetComponent<EntityWeaponBase>();
    }
    private void Update()
    {
        if (ai.isActive == false) return;

        int x = Order.MoveToPosInRotate(weapon, ai.aimPos);
        weapon.PointToMove(x, Time.deltaTime);

        if (ai.unitAI !=null && ai.unitAI.attackState == true)
        {
            //Debug.Log(Order.EmenyInPath(weapon));
            //Debug.Log("判断敌人");
            if (Order.EmenyInPath(weapon) == false) return;
            Order.Attack(weapon);
        }

    }
}
