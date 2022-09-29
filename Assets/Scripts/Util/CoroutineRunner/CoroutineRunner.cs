using Util.GenericSingletons;

namespace Util.CoroutineRunner
{
    public class CoroutineRunner : GenericMonoSingleton<CoroutineRunner>,
        IGenericMonoSingletonSelfInstantiating,
        IGenericMonoSingletonDontDestroyOnLoad { }
}