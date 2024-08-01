using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class FadeCanvas : MonoBehaviour
{

    public FadeEventSO fadeEvent;
    
    public Image fadeImage;

    private void OnEnable()
    {
        fadeEvent.OnActionLoad += OnFadeEvent;
    }

    private void OnDisable()
    {
        fadeEvent.OnActionLoad -= OnFadeEvent;
    }

    private void OnFadeEvent(Color target,float duration,bool isFade)
    {
        fadeImage.DOBlendableColor(target, duration);
    }
    
}
