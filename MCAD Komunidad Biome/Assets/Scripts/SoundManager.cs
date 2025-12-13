using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] private AudioSource music;
    [SerializeField] private List<AudioClip> bgmList;
    private int currentTrack;

    [Header("SFX")]
    [SerializeField] private AudioSource audio;
    [SerializeField] private List<AudioClip> sfxList;

    // Convert to Singleton
    public static SoundManager instance = null; // public static means that it can be accessed

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentTrack = -1; // to allow index 0 to play
    }

    public void AdjustBGMVolume(float vol)
    {
        music.volume = vol;
    }

    public void AdjustSFXVolume(float vol)
    {
        audio.volume = vol;
    }

    public void PlayGameScreenTrack(int i) // Only play during Overworld Scene
    {
        if (currentTrack != i) // to prevent repeat in zone transition
        {
            currentTrack = i;

            music.Stop();
            music.clip = bgmList[i];
            // music.volume = 0.5f;
            music.loop = true;
            music.Play();
        }
    }

    public void PlayCombatScreenTrack(AudioClip clip) // Only play during Combat Scene
    {
        music.Stop();
        music.clip = clip;
        // music.volume = 0.5f;
        music.loop = true;
        music.Play();
    }

    public void StopCurrentGameScreenTrack()
    {
        music.Stop();
    }

    public void PlaySFX(int i)
    {
        audio.PlayOneShot(sfxList[i]);
    }
}