using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DownPanel : UIPanel
{
    Transform table;

    protected void Start()
    {
        base.Start();
        table = UISystem.Instance.FindChildByName("Content");
        //Debug.Log(table == null);
    }
    public override void PrintWeaponTable(EntityBase entity)
    {
        //Debug.Log("downpanel");
        for(int i=0;i<entity.weapons.Length;i++)
        {
            UIWeaponTable weaponTable = AddWeaponTable(entity.weapons[i]);
            entity.weapons[i].table = weaponTable;
            RectTransform rect = weaponTable.transform.GetComponent<RectTransform>();
            //weaponTable.transform.localPosition = Vector3.zero;
            weaponTable.transform.localPosition = new Vector3(rect.rect.width * weaponTable.transform.localScale.x * (i+0.5f), -rect.rect.height/2, 0);
        }
    }
    public UIWeaponTable AddWeaponTable(EntityWeaponBase weapon)
    {
        UIWeaponTable weaponTable = UISystem.Instance.CreateWeaponTable().GetComponent<UIWeaponTable>();
        weaponTable.transform.parent = table;
        //weaponTable.transform.localPosition = Vector3.zero;
        //Debug.Log(weaponTable == null);
        //Debug.Log(weaponTable.transform.name);
        weaponTable.FreshWeapon(weapon);
        return weaponTable;
    }
    public void DeleteAllWeaponTable()
    {
        UIWeaponTable[] allWeapon = table.GetComponentsInChildren<UIWeaponTable>();
        for(int i =0;i<allWeapon.Length;i++)
        {
            UISystem.Instance.PushWeaponTable(allWeapon[i]);
        }
    }

}
