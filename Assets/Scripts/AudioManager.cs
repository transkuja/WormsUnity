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
    public AudioClip nextTurnFx;

    public AudioClip victorySound;   

    [SerializeField]
    private float volumeMusic;
    [SerializeField]
    private float volumeFXs;


    private float currentVolume;

    public static AudioManager Instance
    {
        get
        {
            return s_instance;
        }
    }

    void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
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

    public AudioSource Play(AudioClip clip, float volumeMultiplier = 1.0f)
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
        return sourceFX[sourceFXIndex];
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
