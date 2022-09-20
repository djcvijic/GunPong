using UnityEngine;

public class PersistedString
{
    private readonly string key;

    public PersistedString(string key)
    {
        this.key = key;
    }

    public string Get()
    {
        return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetString(key) : null;
    }

    public void Set(string value)
    {
        if (value != null) PlayerPrefs.SetString(key, value);
        else PlayerPrefs.DeleteKey(key);
    }
}