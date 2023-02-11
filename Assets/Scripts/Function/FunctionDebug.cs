using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionDebug : MonoBehaviour
{
    public void Log(string str)
    {
        Debug.Log(str);
    }
    public void LogError(string str)
    {
        Debug.LogError(str);
    }
}
