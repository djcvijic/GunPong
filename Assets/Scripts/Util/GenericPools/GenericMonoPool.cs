using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class GenericMonoPool<T> : GenericSingleton<GenericMonoPool<T>> where T : MonoBehaviour
{
    private readonly Stack<T> inactiveObjects = new();

    public T GetOrCreate(T prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (inactiveObjects.TryPop(out var obj))
        {
            var t = obj.transform;
            t.position = position;
            t.rotation = rotation;
        }
        else
        {
            obj = Object.Instantiate(prefab, position, rotation, parent);
        }

        return obj;
    }

    public void Return(T obj)
    {
        if (inactiveObjects.Contains(obj))
        {
            throw new InvalidOperationException($"Cannot return an object which is already in the pool: {obj}");
        }

        inactiveObjects.Push(obj);
    }
}