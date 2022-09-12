public abstract class GenericSingleton<T> : IGenericSelfReferencing
    where T : IGenericSelfReferencing, new()
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }

            return instance;
        }
    }
}