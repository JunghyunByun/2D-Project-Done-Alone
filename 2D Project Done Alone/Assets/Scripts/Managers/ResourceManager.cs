using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Vector3 parent)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");

        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab : {path}");

            return null;
        }
        return Object.Instantiate(prefab, parent, Quaternion.identity);
    }

    public void Destroy(GameObject go)
    {
        if (go == null) return;

        Object.Destroy(go);
    }
}
