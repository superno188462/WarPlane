using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EntityEngineFXControl : MonoBehaviour
{
    [Header("引擎特效")]
    public ParticleSystem engineFX;

    private ParticleSystem.MainModule main;
    private ParticleSystem.EmissionModule emis;
    private ParticleSystem.ShapeModule shape;

    [Header("引擎特效功率[0,1]")]
    public float power = 1;

    [Header("引擎特效速度")]
    public float speed = 8;
    [Header("引擎角度")]
    public float angle = 12;
    [Header("粒子数量")]
    public float number = 95;

    private void Awake()
    {
        engineFX = GetComponentInChildren<ParticleSystem>();
        main = engineFX.main;
        emis = engineFX.emission;
        shape = engineFX.shape;
    }

    [Button("改变动力")]
    public void SetEnginePower(float p)
    {
        power = p;
        main.startSpeed = speed * power;
        emis.rateOverTime = number * power;
        shape.angle = angle * (1 - power);
    }

}
