/***********************************************
 * \file        ParticleSystemSystem.cs
 * \author      
 * \date        
 * \version     
 * \brief       特效系统
 * \note        
 * \remarks     
 ***********************************************/
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemSystem : Singleton<ParticleSystemSystem>
{
    //绑定特效名称和特效路径，目前初始化但是没有用它
    private Dictionary<string, string> dict;


    protected override void Awake()
    {
        base.Awake();
        dict = new Dictionary<string, string>();
        Init();

    }

    //初始化特效字典
    private void Init()
    {
        //string content = WebLoad.Load("Assets/Resources/FX/ParticleSystem.txt");
        string content = WebLoad.Load(Application.streamingAssetsPath+"/Config/ParticleSystem.txt");
       
        string[] lines = content.Split('\n');
        //Debug.Log(lines.Length);
        for (int i = 0; i < lines.Length ; i++)
        {
            //Debug.Log(lines[i]);
            if (lines[i].Trim().Length < 3) continue;
            string[] strs2 = lines[i].Split('\t');
            string name = strs2[0];
            string fileName = strs2[1];
            //Debug.Log("FX/" + fileName + "!");
            dict.Add(name, "FX/" + fileName);
        }
    }

    //在指定位置展示特效
    public void PlayParticleSystem(string id,Vector3 point)
    {
        ParticleSystem ps = GetParticleSystem(id);
        ps.transform.position = point;
        //Instantiate(ps.transform,position,Quaternion.identity);
    }

    //获取特效
    public ParticleSystem GetParticleSystem(string id)
    {
        Debug.Log(id);
        if (dict.ContainsKey(id))
        {
           // Debug.Log(dict[id]);
            return PoolSystem.Instance.GetParticleSystem(dict[id]);
        }
        return null;
    }

    //在指定位置展示特效
    public void TestPlayer(Vector3 position)
    {
        GameObject obj = PoolSystem.Instance.GetObj("FX/Weapon/ID003" );
        ParticleSystem ps = obj.GetComponent<ParticleSystem>();
        ps.transform.SetPositionAndRotation(position, Quaternion.identity);

        //float time = ps.main.duration;
        //PoolSystem.Instance.PushObjWait("FX/Weapon/ID003", obj, time);
    }
}
