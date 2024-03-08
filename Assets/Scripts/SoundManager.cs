using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public int minimumSoundSources = 8;
    public int minimumMusicSources = 2;
    public List<AudioSource> inactiveAudioSources = new();
    public List<AudioSource> inactiveMusicSources = new();
    public List<AudioSource> activeAudioSources = new();
    public List<AudioSource> activeMusicSources = new();
    public Dictionary<string, AudioClip> audioClips = new();
    public Dictionary<string, AudioClip> musicClips = new();
    void Start()
    {
        for (int i = 0; i < minimumSoundSources; i++)
        {
            inactiveAudioSources.Add(gameObject.AddComponent<AudioSource>());
        }
        for (int i = 0; i < minimumMusicSources; i++)
        {
            inactiveMusicSources.Add(gameObject.AddComponent<AudioSource>());
        }
    }

    public void Update()
    {
        for (int i = activeAudioSources.Count - 1; i >= 0; i--)
        {
            if(activeAudioSources[i].isPlaying == false)
            {
                AudioSource player = activeAudioSources[i];
                activeAudioSources.RemoveAt(i);
                if(inactiveAudioSources.Count > minimumSoundSources)
                {
                    Destroy(player);
                }
                else
                {
                    inactiveAudioSources.Add(player);
                }
            }
        }
        for (int i = activeMusicSources.Count - 1; i >= 0; i--)
        {
            AudioSource player = activeMusicSources[i];
            activeMusicSources.RemoveAt(i);
            if (activeMusicSources.Count > minimumMusicSources)
            {
                Destroy(player);
            }
            else
            {
                inactiveMusicSources.Add(player);
            }
        }
    }

    public void PlaySound(string soundName)
    {
        if(inactiveAudioSources.Count > 0)
        {
            inactiveAudioSources[0].clip = audioClips[soundName];
            inactiveAudioSources[0].loop = false;
            inactiveAudioSources[0].Play();
            activeAudioSources.Add(inactiveAudioSources[0]);
            inactiveAudioSources.RemoveAt(0);
        }
        else
        {
            AudioSource newPlayer = gameObject.AddComponent<AudioSource>();
            newPlayer.clip = audioClips[soundName];
            newPlayer.loop = false;
            newPlayer.Play();
            activeAudioSources.Add(newPlayer);
        }
    }

    public void PlayMusic(string musicName)
    {
        if (inactiveMusicSources.Count > 0)
        {
            inactiveMusicSources[0].clip = musicClips[musicName];
            inactiveMusicSources[0].loop = false;
            inactiveMusicSources[0].Play();
            activeMusicSources.Add(inactiveMusicSources[0]);
            inactiveMusicSources.RemoveAt(0);
        }
        else
        {
            AudioSource newPlayer = gameObject.AddComponent<AudioSource>();
            newPlayer.clip = musicClips[musicName];
            newPlayer.loop = false;
            newPlayer.Play();
            activeMusicSources.Add(newPlayer);
        }
    }
}
