namespace Util.GenericSingletons
{
    public abstract class GenericSingleton<T> where T : GenericSingleton<T>, new()
    {
        private static T instance;

        public static T Instance => instance ??= new T();
    }
}