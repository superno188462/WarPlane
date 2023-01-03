using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILayout : MonoBehaviour
{
    public Dictionary<string,UIPanel> panels;
    private void Awake()
    {
        panels = new Dictionary<string, UIPanel>();
    }
}
