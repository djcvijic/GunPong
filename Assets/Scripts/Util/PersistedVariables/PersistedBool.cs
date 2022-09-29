using UnityEngine;

namespace Util.PersistedVariables
{
    public class PersistedBool
    {
        private readonly string key;

        public PersistedBool(string key)
        {
            this.key = key;
        }

        public bool? Get()
        {
            return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) != 0 : null;
        }

        public void Set(bool? value)
        {
            if (value.HasValue) PlayerPrefs.SetInt(key, value.Value ? 1 : 0);
            else PlayerPrefs.DeleteKey(key);
        }
    }
}