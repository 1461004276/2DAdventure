using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    [Header("AudioSource")]
    public AudioSource bgm;
    public AudioSource fx;
    [Header("接收")]
    public PlayAudioEventSO playAudioEvent;
    public PlayAudioEventSO playBgmAudioEvent;
    public FloatEventSO volumeChangeEvent;
    public VoidEventSO pauseEvent;

    public AudioMixer mixer;

    public FloatEventSO SameVolumeEvent;
    private void OnEnable()
    {
        playAudioEvent.PlayAudioAction += playFxAudio;
        playBgmAudioEvent.PlayAudioAction += playBgmAudio;
        volumeChangeEvent.OnEventRaised += OnVolumeChange;
        pauseEvent.TakeOnAction += OnPause;
    }
    
    private void OnDisable()
    {
        playAudioEvent.PlayAudioAction -= playFxAudio;
        playBgmAudioEvent.PlayAudioAction -= playBgmAudio;
        volumeChangeEvent.OnEventRaised -= OnVolumeChange;
        pauseEvent.TakeOnAction -= OnPause;


    }

    private void OnPause()
    {
        float amount;
        mixer.GetFloat("MasterVolume",out amount);
        SameVolumeEvent.RaiseEvent(amount);
    }

    private void OnVolumeChange(float amount)
    {
        mixer.SetFloat("MasterVolume", amount*100-80);
    }

    private void playBgmAudio(AudioClip bgmAudio)
    {
        bgm.clip = bgmAudio;
        bgm.Play();
    }
    private void playFxAudio(AudioClip fxAudio)
    {
        fx.clip = fxAudio;
        fx.Play();
    }
    
}
