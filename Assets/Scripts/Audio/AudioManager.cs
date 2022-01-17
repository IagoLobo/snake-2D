using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSource;

    [Header("Soundtracks")]
    public AudioClip mainMenuClip;
    public AudioClip gameplayClip;
    public AudioClip victoryJingleClip;

    [Header("SFX")]
    public AudioClip gameOverJingleClip;
    public AudioClip appleEatenClip;
    public AudioClip buttonClickClip;

    private void Awake()
    {
        if (Instance != null)   Destroy(gameObject);
        else                    Instance = this;
    }

    public void PlayMainMenuMusic()
    {
        musicAudioSource.clip = mainMenuClip;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void PlayGameplayMusic()
    {
        musicAudioSource.clip = gameplayClip;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void PlayVictoryJingleMusic()
    {
        musicAudioSource.clip = victoryJingleClip;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void PlayGameOverJingleSFX()
    {
        sfxAudioSource.PlayOneShot(gameOverJingleClip);
    }

    public void PlayAppleEatenSFX()
    {
        sfxAudioSource.PlayOneShot(appleEatenClip);
    }

    public void PlayButtonClickSFX()
    {
        sfxAudioSource.PlayOneShot(buttonClickClip);
    }
}
