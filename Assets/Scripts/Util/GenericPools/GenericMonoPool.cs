using System;
using System.Collections.Generic;
using UnityEngine;
using Util.GenericSingletons;
using Object = UnityEngine.Object;

namespace Util.GenericPools
{
    public class GenericMonoPool<T> : GenericSingleton<GenericMonoPool<T>> where T : MonoBehaviour
    {
        private readonly Stack<T> inactiveObjects = new();

        public T Get(T prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            if (inactiveObjects.TryPop(out var obj))
            {
                var t = obj.transform;
                t.SetParent(parent);
                t.position = position;
                t.rotation = rotation;
            }
            else
            {
                obj = Object.Instantiate(prefab, position, rotation, parent);
            }

            return obj;
        }

        public void Release(T obj)
        {
            if (inactiveObjects.Contains(obj))
            {
                throw new InvalidOperationException($"Cannot return an object which is already in the pool: {obj}");
            }

            inactiveObjects.Push(obj);
        }
    }
}