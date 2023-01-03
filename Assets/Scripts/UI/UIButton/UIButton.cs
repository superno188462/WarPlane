
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
public class UIButton : MonoBehaviour
{
    public Button btn;
    private void Awake()
    {
        btn = GetComponentInChildren<Button>();
        btn.onClick.AddListener(Create);
    }
    public void Create()
    {
        string number = "ID001";

        Vector3 pos = PlayerControl.Instance.unit.transform.position;
        EntitySystem. CreateEntityInArea(number, pos ,20);
    }
    
}