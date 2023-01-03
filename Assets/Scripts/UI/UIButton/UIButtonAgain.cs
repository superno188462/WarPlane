
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
public class UIButtonAgain: MonoBehaviour
{
    public Button btn;
     
    private void Awake()
    {
        btn = GetComponentInChildren<Button>();
        btn.onClick.AddListener(Again);
    }
    public void Again()
    {
       
    }
    
}