using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Path
{
    [SerializeField]
    string[] path;

    public GameObject GetObjectAtPath(GameObject root)
    {
        Transform currentTransform = root.transform;

        for(int i = 0; i < path.Length; i++)
        {
            currentTransform = currentTransform.Find(path[i]);
        }

        return currentTransform.gameObject;
    }
}
