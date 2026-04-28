using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Serializable]
    public class AudioEventClip
    {
        public AudioEvent audioEvent;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        public bool loop;
    }

    public static AudioManager Instance { get; private set; }

    [Header("Sources")]
    public AudioSource sfxSource;
    public AudioSource loopSource;
    public AudioSource musicSource;

    [Header("Library")]
    public AudioEventClip[] clips;

    [Header("Volumes")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;

    private readonly Dictionary<AudioEvent, AudioEventClip> clipMap = new Dictionary<AudioEvent, AudioEventClip>();
    private AudioEvent? currentMusicEvent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        RebuildMap();
    }

    private void OnValidate()
    {
        if (Application.isPlaying) return;
        RebuildMap();
    }

    private void RebuildMap()
    {
        clipMap.Clear();

        if (clips == null) return;

        for (int i = 0; i < clips.Length; i++)
        {
            AudioEventClip entry = clips[i];
            if (entry == null) continue;
            clipMap[entry.audioEvent] = entry;
        }
    }

    private bool TryGetEntry(AudioEvent audioEvent, out AudioEventClip entry)
    {
        if (clipMap.TryGetValue(audioEvent, out entry) && entry != null && entry.clip != null)
        {
            return true;
        }

        entry = null;
        return false;
    }

    public void PlaySfx(AudioEvent audioEvent)
    {
        if (sfxSource == null) return;
        if (!TryGetEntry(audioEvent, out AudioEventClip entry)) return;

        sfxSource.loop = entry.loop;
        sfxSource.volume = masterVolume * sfxVolume * entry.volume;

        if (entry.loop)
        {
            if (sfxSource.clip == entry.clip && sfxSource.isPlaying) return;
            sfxSource.clip = entry.clip;
            sfxSource.Play();
        }
        else
        {
            sfxSource.PlayOneShot(entry.clip, masterVolume * sfxVolume * entry.volume);
        }
    }

    public void PlayLoop(AudioEvent audioEvent)
    {
        if (loopSource == null) return;
        if (!TryGetEntry(audioEvent, out AudioEventClip entry)) return;

        loopSource.volume = masterVolume * sfxVolume * entry.volume;
        loopSource.loop = true;

        if (loopSource.clip == entry.clip && loopSource.isPlaying) return;

        loopSource.clip = entry.clip;
        loopSource.Play();
    }

    public void StopLoop(AudioEvent audioEvent)
    {
        if (loopSource == null) return;
        if (!TryGetEntry(audioEvent, out AudioEventClip entry)) return;

        if (loopSource.clip != entry.clip) return;
        loopSource.Stop();
        loopSource.clip = null;
        loopSource.loop = false;
    }

    public void StopSfxLoop()
    {
        if (sfxSource == null) return;
        if (!sfxSource.loop) return;
        sfxSource.Stop();
        sfxSource.clip = null;
        sfxSource.loop = false;
    }

    public void PlayMusic(AudioEvent audioEvent)
    {
        if (musicSource == null) return;
        if (!TryGetEntry(audioEvent, out AudioEventClip entry)) return;

        if (currentMusicEvent.HasValue && currentMusicEvent.Value == audioEvent && musicSource.isPlaying)
        {
            return;
        }

        currentMusicEvent = audioEvent;

        musicSource.clip = entry.clip;
        musicSource.loop = true;
        musicSource.volume = masterVolume * musicVolume * entry.volume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource == null) return;
        musicSource.Stop();
        musicSource.clip = null;
        currentMusicEvent = null;
    }
}
