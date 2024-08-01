using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioPlay : MonoBehaviour
{
    public AudioClip clip;
    public PlayAudioEventSO playAudio;
    public bool playOnEnable;
    public bool playOnStart;

    private void Start()
    {
        if(playOnStart) PlayAudio();
    }

    private void OnEnable()
    {
        if (playOnEnable) PlayAudio();
    }

    public void PlayAudio()
    {
        playAudio.PlayAudioAction(clip);
    }
    
}
