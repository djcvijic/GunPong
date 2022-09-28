using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : GenericMonoSingleton<AudioManager>,
	IGenericMonoSingletonDontDestroyOnLoad,
	IGenericMonoSingletonSelfInstantiating
{
	private readonly Dictionary<AudioClipSettings, Coroutine> fadeCoroutines = new();

	private readonly List<AudioSource> musicSources = new();

	private readonly List<AudioSource> soundSources = new();

    private bool? musicEnabled;
    
    private bool? soundEnabled;

	public bool MusicEnabled
    {
        get => musicEnabled.GetValueOrDefault(true);
        set
        {
	        if (value != musicEnabled)
	        {
		        musicEnabled = value;
		        musicSources.ForEach(s => s.mute = !value);
	        }
        }
    }

	public bool SoundEnabled
    {
        get => soundEnabled.GetValueOrDefault(true);
        set
        {
	        if (value != soundEnabled)
	        {
		        soundEnabled = value;
		        soundSources.ForEach(s => s.mute = !value);
	        }
        }
    }

	public void PlayAudio(AudioClipSettings settings)
	{
		var sourceList = DetermineSourceList(settings.type);
		AudioSource source = null;
		switch (settings.limitBehaviour)
		{
			case AudioLimitBehaviour.DoNotLimit:
				break;
			case AudioLimitBehaviour.DiscardOldInstance:
				source = FindSourceByClip(sourceList, settings.clip);
				if (source != null) source.Stop();
				break;
			case AudioLimitBehaviour.DiscardNewInstance:
				source = FindSourceByClip(sourceList, settings.clip);
				if (source != null) return;
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		if (source == null) source = FindInactiveSource(sourceList);

		if (source == null) source = InstantiateAudioSource(settings.type);

		SetupAndPlayClip(settings, source);
	}

	private List<AudioSource> DetermineSourceList(AudioType audioType)
	{
		switch (audioType)
		{
			case AudioType.Music:
				return musicSources;
			case AudioType.Sound:
				return soundSources;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private bool DetermineSourceMuteValue(AudioType audioType)
	{
		switch (audioType)
		{
			case AudioType.Music:
				return !MusicEnabled;
			case AudioType.Sound:
				return !SoundEnabled;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private static AudioSource FindSourceByClip(List<AudioSource> sourceList, AudioClip clip)
	{
		return sourceList.Find(s => s.clip == clip);
	}

	private static AudioSource FindInactiveSource(List<AudioSource> sourceList)
	{
		return sourceList.Find(s => !s.isPlaying);
	}

	private AudioSource InstantiateAudioSource(AudioType audioType)
	{
		var sourceList = DetermineSourceList(audioType);
		var obj = new GameObject($"{nameof(AudioSource)}{sourceList.Count + 1}");
		obj.transform.SetParent(transform);

		var source = obj.AddComponent<AudioSource>();
		source.mute = DetermineSourceMuteValue(audioType);
		source.playOnAwake = false;
		sourceList.Add(source);
		return source;
	}

	private static void SetupAndPlayClip(AudioClipSettings settings, AudioSource source)
	{
		source.loop = settings.type == AudioType.Music;
		source.clip = settings.clip;
		source.volume = settings.defaultVolume;
		source.Play();
	}

	public void FadeAudio(AudioClipSettings settings, float fadeTo, float fadeDuration)
	{
		var sourceList = DetermineSourceList(settings.type);
		var source = FindSourceByClip(sourceList, settings.clip);
		if (source == null) return;

		if (fadeCoroutines.TryGetValue(settings, out var coroutine)) StopCoroutine(coroutine);
		coroutine = StartCoroutine(FadeAudioFlow(source, source.volume, fadeTo, fadeDuration));
		fadeCoroutines[settings] = coroutine;
	}

	private static IEnumerator FadeAudioFlow(AudioSource source, float fadeFrom, float fadeTo, float fadeDuration)
	{
		if (fadeDuration <= 0f)
		{
			source.volume = fadeTo;
			yield break;
		}

		var progress = 0f;
		while (progress < 1f)
		{
			yield return null;

			progress += Time.deltaTime / fadeDuration;
			source.volume = Mathf.Lerp(fadeFrom, fadeTo, progress);
		}
	}
}