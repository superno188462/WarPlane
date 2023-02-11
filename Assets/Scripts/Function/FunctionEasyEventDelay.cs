using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionEasyEventDelay : FunctionBase
{

    [Header("延迟时间")]
    public float delayTimer;


    public override void StartFunctionEasy()
    {
        if (!IsUse) return;
        Invoke(nameof(StartFunctionEasyDelay), delayTimer);
    }
    private void StartFunctionEasyDelay()
    {
        EasyEvent.Invoke();
    }
}
