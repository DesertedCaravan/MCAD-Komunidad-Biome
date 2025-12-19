using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] private AudioSource bgm;
    [SerializeField] private List<AudioClip> bgmList;
    private int currentTrack;

    [Header("Narration")]
    [SerializeField] private AudioSource narration;
    [SerializeField] private List<AudioClip> narrationList;
    private int currentNarration;

    [Header("SFX")]
    [SerializeField] private AudioSource sfx;
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
        PlayBGM(0);

        currentNarration = -1;
    }

    public void AdjustBGMVolume(float vol)
    {
        bgm.volume = vol;
    }

    public void AdjustSFXVolume(float vol)
    {
        sfx.volume = vol;
    }

    public void AdjustNarrationVolume(float vol)
    {
        narration.volume = vol;
    }

    public void PlayBGM(int i)
    {
        if (currentTrack != i) // to prevent repeat in zone transition
        {
            currentTrack = i;

            StopCurrentBGM();

            bgm.clip = bgmList[currentTrack];
            AdjustBGMVolume(0.5f);
            bgm.loop = true;
            bgm.Play();
        }
    }

    public void StopCurrentBGM()
    {
        bgm.Stop();
    }

    public void PlayNarration(int i)
    {
        if (currentNarration != i) // to prevent repeat in zone transition
        {
            currentNarration = i;

            StopCurrentNarration();

            narration.clip = narrationList[currentNarration];
            AdjustBGMVolume(0.5f);
            narration.loop = false; // don't loop narration
            narration.Play();
        }
    }

    public void StopCurrentNarration()
    {
        narration.Stop();
    }

    public void PlaySFX(int i)
    {
        sfx.PlayOneShot(sfxList[i]);
    }
}