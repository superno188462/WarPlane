using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class FunctionBase : MonoBehaviour
{
    [Header("是否启用")]
    public bool IsUse;

    [Header("触发的事件")]
    public UnityEvent EasyEvent;

    /// <summary>
    /// 开始执行事件触发器
    /// </summary>
    [Button]
    public virtual void StartFunctionEasy()
    {
        if (!IsUse) return;
        EasyEvent.Invoke();
    }

    /// <summary>
    /// 设置启用
    /// </summary>=
    public void SetState(bool isUse)
    {
        IsUse = isUse;
    }
}
