using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemSystem : Singleton<ParticleSystemSystem>
{
    private Dictionary<string, string> dict;


    protected override void Awake()
    {
        base.Awake();
        dict = new Dictionary<string, string>();
        Init();

    }

    private void Init()
    {
        string content = WebLoad.Load("Assets/Resources/FX/ParticleSystem.txt");
        //Debug.Log(content);
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

    public void PlayParticleSystem(string id,Vector3 point)
    {
        ParticleSystem ps = GetParticleSystem(id);
        ps.transform.position = point;
        //Instantiate(ps.transform,position,Quaternion.identity);
    }


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

    public void TestPlayer(Vector3 position)
    {
        GameObject obj = PoolSystem.Instance.GetObj("FX/Weapon/ID003" );
        ParticleSystem ps = obj.GetComponent<ParticleSystem>();
        ps.transform.SetPositionAndRotation(position, Quaternion.identity);

        //float time = ps.main.duration;
        //PoolSystem.Instance.PushObjWait("FX/Weapon/ID003", obj, time);
    }
}
