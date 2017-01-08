using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

    public static AudioController instance;

    public AudioClip[] theme;
    public AudioClip[] sfx;

    AudioSource[] musicSources;
        
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            musicSources = new AudioSource[3];
            for (int i = 0; i < 3; i++)
            {
                GameObject newMusicSource = new GameObject("Music Source " + (i + 1));
                musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                newMusicSource.transform.parent = transform;
            }

            PlayTheme("pacman_siren");
        }
    }

    private void Update()
    {
    }

    public void PlayTheme(string name)
    {
        for (int i = 0; i < theme.Length; i++)
        {
            if (theme[i].name == name)
            {
                musicSources[0].clip = theme[i];
                break;
            }
        }

        if (musicSources[0].clip != null)
        {
            if (musicSources[0].clip.name == "pacman_siren")
            {
                Invoke("LoopSiren", 0f);
            }
            else
                musicSources[0].Play();
        }
    }

    void LoopSiren()
    {
        musicSources[0].time = 0.5f;
        musicSources[0].Play();
        Invoke("LoopSiren", musicSources[0].clip.length - 0.6f);
    }

    public void PlaySFX(string name)
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (sfx[i].name == name)
            {
                musicSources[1].clip = sfx[i];
            }
        }

        if (musicSources[1].clip != null)
        {
            musicSources[1].Play();
        }
    }
}
