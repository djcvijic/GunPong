using UnityEngine;

public class PersistedFloat
{
    private readonly string key;

    public PersistedFloat(string key)
    {
        this.key = key;
    }

    public float? Get()
    {
        return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetFloat(key) : null;
    }

    public void Set(float? value)
    {
        if (value.HasValue) PlayerPrefs.SetFloat(key, value.Value);
        else PlayerPrefs.DeleteKey(key);
    }
}