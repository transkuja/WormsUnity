using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    private static AudioManager s_instance = null;

    public AudioSource sourceMusic;
    public List<AudioSource> sourceFX;

    public AudioClip musicGame;

    public AudioClip holyGFX;
    public AudioClip missileFX;
    public AudioClip explosionFX;
    public AudioClip crateFx;
    public AudioClip healCrateFx;
    public AudioClip boingFx;

    public AudioClip jumpVoiceFX;
    public AudioClip sheepFX;
    public AudioClip superSheepReleaseFx;
    public AudioClip superSheepFlightFx;

    public AudioClip planeFx;
    public AudioClip airStrikeFx;

    public AudioClip victorySound;   

    [SerializeField]
    private float volumeMusic = 0.015f;
    [SerializeField]
    private float volumeFXs = 0.01f;


    private float currentVolume;

    public static AudioManager Instance
    {
        get
        {
            return s_instance;
        }
    }

    public float VolumeMusic
    {
        get
        {
            return currentVolume;
        }

        set
        {
            currentVolume = value;
            sourceMusic.volume = currentVolume;
        }
    }

    public float VolumeFXs
    {
        get
        {
            return volumeFXs;
        }

        set
        {
            volumeFXs = value;
            foreach (AudioSource source in sourceFX)
                source.volume = volumeFXs;
        }
    }

    void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (s_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentVolume = volumeMusic;
    }

    private bool isFading = false;
    private float timerFade = 0.0f;

    public void PlayMusic()
    {
        sourceMusic.clip = musicGame;
        sourceMusic.volume = currentVolume;
        sourceMusic.Play();
    }

    public void PlayOneShot(AudioClip clip)
    {
        int sourceFXIndex = 0;
        if (sourceFX[sourceFXIndex].isPlaying)
        {
            sourceFXIndex++;
        }

        sourceFX[sourceFXIndex].PlayOneShot(clip, volumeFXs);
    }

    public void PlayOneShot(AudioClip clip, float volumeMultiplier)
    {
        int sourceFXIndex = 0;
        if (sourceFX[sourceFXIndex].isPlaying)
        {
            sourceFXIndex++;
        }

        sourceFX[sourceFXIndex].PlayOneShot(clip, volumeFXs * volumeMultiplier);
    }

    public void Play(AudioClip clip, float volumeMultiplier = 1.0f)
    {
        int sourceFXIndex = 0;
        if (sourceFX[sourceFXIndex].isPlaying)
        {
            sourceFXIndex++;
        }

        if (sourceFX[sourceFXIndex].clip != clip)
            sourceFX[sourceFXIndex].clip = clip;
        sourceFX[sourceFXIndex].volume = volumeFXs * volumeMultiplier;
        sourceFX[sourceFXIndex].Play();
    }


    public void ClearFX(AudioClip _fxToClear)
    {
        foreach (AudioSource source in sourceFX)
        {
            if (source.clip == _fxToClear)
            {
                source.Stop();
                source.clip = null;
            }
        }
    }

}
