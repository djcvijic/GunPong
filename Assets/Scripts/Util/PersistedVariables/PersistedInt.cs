using UnityEngine;

namespace Util.PersistedVariables
{
    public class PersistedInt
    {
        private readonly string key;

        public PersistedInt(string key)
        {
            this.key = key;
        }

        public int? Get()
        {
            return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) : null;
        }

        public void Set(int? value)
        {
            if (value.HasValue) PlayerPrefs.SetInt(key, value.Value);
            else PlayerPrefs.DeleteKey(key);
        }
    }
}