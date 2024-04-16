using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = .75f;

    [Range(.1f, 3f)]
    public float pitch = 1.0f;

    public bool loop = false;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public bool isMusicOn_;
    public bool areSoundEffectsOn_;

    [Header("Sounds")]
    public List<Sound> soundEffects_;
    public List<Sound> musicSongs_;

    [Header("Mixers")]
    public AudioMixerGroup musicAudioMixerGroup_;
    public AudioMixerGroup effectsAudioMixerGroup_;
    public AudioMixer audioMixer_;

    private int currentSongIndex_ = 0;

    public void PlaySong(string songName)
    {
        Sound s = GetSong(songName);
        if (s == null)
        {
            Debug.LogWarning("Song: " + name + " not found!");
            return;
        }
        if (isMusicOn_)
        {
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.Play();
        }
    }

    public bool IsCurrentSongPlaying()
    {
        Sound s = musicSongs_[currentSongIndex_];

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return false;
        }

        float totalDuration = s.clip.length;
        float currentDuration = s.source.time;
        return totalDuration - currentDuration > 0.3f;
    }

    public void PlayNextSong()
    {
        currentSongIndex_ = (currentSongIndex_ + 1) % musicSongs_.Count;

        Sound s = musicSongs_[currentSongIndex_];
        if (s == null)
        {
            Debug.LogWarning("Song: " + name + " not found!");
            return;
        }
        if (isMusicOn_)
        {
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.Play();
        }
    }

    public void PlaySoundEffect(string sound)
    {
        Sound s = GetSoundEffect(sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        if (areSoundEffectsOn_)
        {
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.Play();
        }
    }

    public void Stop(string sound)
    {
        Sound s = GetSound(sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();
    }

    public bool IsPlaying(string soundName)
    {
        Sound s = GetSound(soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return false;
        }

        return s.source.isPlaying;
    }

    public Sound GetSound(string sound)
    {
        Sound s = GetSoundEffect(sound);
        if (s == null)
        {
            s = GetSong(sound);
        }
        return s;
    }

    public Sound GetSoundEffect(string soundName)
    {
        return soundEffects_.Find(item => item.name == soundName);
    }

    public Sound GetSong(string songName)
    {
        return musicSongs_.Find(item => item.name == songName);
    }

    // Audio options management
    public bool IsMusicOn()
    {
        return isMusicOn_;
    }

    public bool AreSoundEffectsOn()
    {
        return areSoundEffectsOn_;
    }

    public void SetMusicOn(bool activate)
    {
        isMusicOn_ = activate;
        float volume_value = activate ? 0.0f : -80.0f;
        musicAudioMixerGroup_.audioMixer.SetFloat("MusicVolume", volume_value);

        if (activate)
        {
            if (!IsPlaying("Music"))
            {
                PlaySong("Music");
            }
        }
    }

    public void SetSoundEffectsOn(bool activate)
    {
        areSoundEffectsOn_ = activate;
        float volume_value = activate ? 0.0f : -80.0f;
        effectsAudioMixerGroup_.audioMixer.SetFloat("EffectsVolume", volume_value);
    }

    private void Awake()
    {
        foreach (Sound s in soundEffects_)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = effectsAudioMixerGroup_;
            s.source.clip = s.clip;
            s.source.loop = s.loop;
        }

        foreach (Sound s in musicSongs_)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = musicAudioMixerGroup_;
            s.source.clip = s.clip;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        currentSongIndex_ = -1;
        PlayNextSong();
    }

    private void Update()
    {
        if(!IsCurrentSongPlaying())
        {
            PlayNextSong();
        }
    }
}
