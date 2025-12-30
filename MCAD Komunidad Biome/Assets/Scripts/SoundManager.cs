using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] private AudioSource bgm;
    [SerializeField] private List<AudioClip> bgmList;
    private int _currentTrack;

    [Header("Narration")]
    [SerializeField] private AudioSource narration;
    [SerializeField] private List<AudioClip> narrationList;
    private int _currentNarration;

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

    public void InitializeTracks()
    {
        _currentTrack = -1; // to allow index 0 to play
        _currentNarration = -1;
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
        if (_currentTrack != i) // to prevent repeat in zone transition
        {
            _currentTrack = i;

            StopCurrentBGM();

            bgm.clip = bgmList[_currentTrack];
            AdjustBGMVolume(0.25f);
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
        if (_currentNarration != i) // to prevent repeat in zone transition
        {
            _currentNarration = i;

            StopCurrentNarration();

            narration.clip = narrationList[_currentNarration];
            AdjustBGMVolume(1.0f);
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