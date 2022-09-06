using UnityEngine;

public interface IGenericSelfReferencing { }

public interface IGenericMonoSingletonSelfInstantiating { }

public interface IGenericMonoSingletonDontDestroyOnLoad { }

public abstract class GenericMonoSingleton<T> : MonoBehaviour, IGenericSelfReferencing
    where T : MonoBehaviour, IGenericSelfReferencing
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null && typeof(T).IsImplementationOf(typeof(IGenericMonoSingletonSelfInstantiating)))
            {
                var go = new GameObject($"{typeof(T).Name}");
                instance = go.AddComponent<T>();
                ApplyDontDestroyOnLoadIfNeeded();
            }

            return instance;
        }
    }

    private static void ApplyDontDestroyOnLoadIfNeeded()
    {
        if (typeof(T).IsImplementationOf(typeof(IGenericMonoSingletonDontDestroyOnLoad)))
        {
            DontDestroyOnLoad(instance);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            ApplyDontDestroyOnLoadIfNeeded();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (this == instance)
        {
            instance = null;
        }
    }
}
