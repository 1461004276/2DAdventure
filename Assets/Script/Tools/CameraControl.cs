using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource shakePlay;
    public VoidEventSO cameraShake;
    public VoidEventSO afterLoadEventListen;
    
    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }

    // private void Start()
    // {
    //     GetEdge();
    // }

    private void OnEnable()
    {
        cameraShake.TakeOnAction += TakeCameraShaek;
        afterLoadEventListen.TakeOnAction += OnAfter;
    }

    private void OnDisable()
    {
        cameraShake.TakeOnAction -= TakeCameraShaek;
        afterLoadEventListen.TakeOnAction -= OnAfter;
    }

    private void OnAfter()
    {
        GetEdge();
    }

    private void TakeCameraShaek()
    {
        shakePlay.GenerateImpulse();
    }

    private void GetEdge()
    {
        var obj = GameObject.FindGameObjectWithTag("Edge");
        if (obj != null)
        {
            confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
            confiner2D.InvalidateCache();
        }
        else return;
    }
    
    
}
