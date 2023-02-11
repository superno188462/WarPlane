 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EntityColliderSlot : MonoBehaviour
{
    [SerializeField,Header("武器槽位的类型")]
    private EntitySlotType _type;
    [SerializeField, Header("武器槽位的大小")]
    private EntitySlotSize _size;

    [SerializeField, Header("槽位的角度限制，左右")]
    private float[] _angles;

    /// <summary>
    /// 武器槽位的类型
    /// </summary>
    public EntitySlotType Type => _type;
    /// <summary>
    /// 武器槽位的大小
    /// </summary>
    public EntitySlotSize Size => _size;
    /// <summary>
    /// 槽位的角度限制
    /// </summary>
    public float[] Angles => _angles;

    //测试
    public bool testShow;
    public Transform gjr;

    private void Update()
    {
        if(testShow) OnShow();
    }

    [Button("显示攻击范围")]
    public void OnShow()
    {
        gjr.eulerAngles = transform.eulerAngles;
        gjr.eulerAngles += new Vector3(0, 0, -_angles[0]);
        Debug.DrawLine(transform.position, gjr.position + gjr.up * 5, Color.cyan, testShow ? 0.01f :1);


        gjr.eulerAngles = transform.eulerAngles;
        gjr.eulerAngles += new Vector3(0, 0, _angles[1]);
        Debug.DrawLine(transform.position, gjr.position + gjr.up * 5, Color.yellow, testShow ? 0.01f : 1);
    }


}
