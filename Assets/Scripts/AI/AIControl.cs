using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControl : MonoBehaviour
{
    public bool isActive;

    public EntityBase unit;
    public AIEntityBase unitAI;
    public AIWeapon[] weaponsAI;


    public EntityBase attackAim;
    //public List<Transform> PosList;
    public Vector3 aimPos;//目标向量

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
