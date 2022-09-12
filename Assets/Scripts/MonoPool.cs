using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class MonoPool<T> : GenericSingleton<MonoPool<T>> where T : PooledMonoBehaviour
{
    private readonly Stack<T> inactiveObjects = new();

    public void GetOrCreate(T prefab, Vector3 position, Quaternion rotation, PlayerEnum owner, Vector3 velocity)
    {
        if (inactiveObjects.TryPop(out var obj))
        {
            obj.gameObject.SetActive(true);
            var t = obj.transform;
            t.position = position;
            t.rotation = rotation;
            obj.Activate(owner, velocity, null);
        }
        else
        {
            obj = Object.Instantiate(prefab, position, rotation, GameView.Instance.transform);
            obj.Activate(owner, velocity, () => ReturnToPool(obj));
        }
    }

    private void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        inactiveObjects.Push(obj);
    }
}