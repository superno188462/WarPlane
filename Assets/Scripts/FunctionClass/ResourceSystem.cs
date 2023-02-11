using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourceSystem : Singleton<ResourceSystem>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public T Load<T>(string path) where T : Object
    {
        T obj = Resources.Load<T>(path);
        if (obj is GameObject)
        {
            return Instantiate(obj);
        }
        return obj;
    }

    public void LoadAsyn<T>(string path, UnityAction<T> action) where T : Object
    {
        StartCoroutine(LoadAysnIE<T>(path, action));
    }

    private IEnumerator LoadAysnIE<T>(string path ,UnityAction<T> action)where T : Object
    {
        ResourceRequest re = Resources.LoadAsync<T>(path);
        yield return re;
        if(re.asset is GameObject)
        {
            action(Instantiate(re.asset) as T);
        }
        action(re.asset as T);
    }

}
