using System;
using UnityEngine;

[Serializable]
public class AudioClipSettings
{
    public AudioType type;

    public AudioClip clip;

    [Range(0, 1)] public float defaultVolume = 1;

    public AudioLimitBehaviour limitBehaviour;
}