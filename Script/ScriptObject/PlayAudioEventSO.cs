using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Events/Play Audio Event")]
public class PlayAudioEventSO : ScriptableObject
{
    public UnityAction<AudioClip> PlayAudioAction;
    
    
    public void TakeAction(AudioClip clip)
    {
        PlayAudioAction?.Invoke(clip);
    }
    
}
