using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PoolData
{
    public GameObject parentObj;
    public List<GameObject> poolList;

    public PoolData(GameObject obj,GameObject poolObj)
    {
        parentObj = new GameObject(obj.name);
        parentObj.transform.SetParent(poolObj.transform);
        poolList = new List<GameObject>();
        PushObj(obj);

    }

    public GameObject GetObj()
    {
        GameObject obj = poolList[0];
        poolList.RemoveAt(0);
        obj.SetActive(true);
        //obj.transform.SetParent(null);
        return obj;
    }
    public void PushObj(GameObject obj)
    {
        poolList.Add(obj);
        obj.transform.SetParent(parentObj.transform);
        obj.SetActive(false);
    }
}

public class PoolSystem : Singleton<PoolSystem>
{
    private Dictionary<string, PoolData> poolDict = new Dictionary<string, PoolData>();
    private GameObject poolObj;

    protected override void Awake()
    {
        base.Awake();
    }
    public void Recycle()
    {
        StopAllCoroutines();
        poolDict.Clear();
        poolObj = null;
    }

    public void GetObjAsyn(string path ,UnityAction<GameObject > action)
    {
        if (poolDict.ContainsKey(path) && poolDict[path].poolList.Count > 0)
        {
            action(poolDict[path].GetObj());
        }
        else
        {
            ResourceSystem.Instance.LoadAsyn<GameObject>(path, (f) =>
            {
                if (f is null) Debug.LogError("无对象");
                else
                {
                    f.name = path;
                    action(f);
                }
            });
        }
    }
    public GameObject GetObj(string path)
    {
        if (poolDict.ContainsKey(path) && poolDict[path].poolList.Count > 0)
        {
            return poolDict[path].GetObj();
        }
        return ResourceSystem.Instance.Load<GameObject>(path);
    }
    public AudioClip GetAudioClip(string path)
    {
        return ResourceSystem.Instance.Load<AudioClip>(path);
    }
    public Sprite GetSprite(string path)
    {
        return ResourceSystem.Instance.Load<Sprite>(path);
    }

    public ParticleSystem GetParticleSystem(string path)
    {
        return ResourceSystem.Instance.Load<ParticleSystem>(path);
    }

    public void PushObj(string path,GameObject obj)
    {
        if (poolObj == null) poolObj = new GameObject("pool");
        if (poolDict.ContainsKey(path))
        {
            poolDict[path].PushObj(obj);
        }
        else
        {
            poolDict.Add(path, new PoolData(obj, poolObj));
        }
    }

    public void PushObjWait(string path ,GameObject obj,float time)
    {
        StartCoroutine(PushObjWaitIE(path, obj, time));
    }
    private IEnumerator PushObjWaitIE(string path, GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        if (poolObj == null) poolObj = new GameObject("pool");
        if (poolDict.ContainsKey(path))
        {
            poolDict[path].PushObj(obj);
        }
        else
        {
            poolDict.Add(path, new PoolData(obj, poolObj));
        }

    }
}
