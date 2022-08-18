using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Wraps a cached objects list and has a function to get game objects from the cache
//with a specified key
[System.Serializable]
public struct CachedObjectWrapper
{
    public List<CachedObjects> cache;

    public GameObject GetGameObjectFromCache(string key)
    {
        GameObject currentObject;

        for (int i = 0; i < cache.Count; i++)
        {
            currentObject = cache[i].GetValueFromKey(key);
            if (currentObject != null) return currentObject;
        }

        Debug.LogError("Key " + key + " not found in cache, did you forget to assign it?");
        return null;
    }
}

[System.Serializable]
public struct CachedObjects
{
    public GameObject cachedObject;
    public string key;

    public CachedObjects(GameObject cachedObject, string key)
    {
        this.key = key;
        this.cachedObject = cachedObject;
    }

    public GameObject GetValueFromKey(string key)
    {
        if (key == this.key)
        {
            return cachedObject;
        }

        return null;
    }
}