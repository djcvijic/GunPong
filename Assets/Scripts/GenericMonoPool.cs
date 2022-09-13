using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class GenericMonoPool<T> : GenericSingleton<GenericMonoPool<T>> where T : MonoBehaviour
{
    private readonly Stack<T> inactiveObjects = new();

    public T GetOrCreate(T prefab, Vector3 position, Quaternion rotation)
    {
        if (inactiveObjects.TryPop(out var obj))
        {
            var t = obj.transform;
            t.position = position;
            t.rotation = rotation;
            obj.gameObject.SetActive(true);
        }
        else
        {
            obj = Object.Instantiate(prefab, position, rotation, GameView.Instance.transform);
        }

        return obj;
    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        inactiveObjects.Push(obj);
    }
}