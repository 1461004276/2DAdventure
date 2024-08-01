using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class PlayerEnter : MonoBehaviour
{
    public bool canUse;
    private SpriteRenderer sr;
    private Transform player;
    private Animator anim;
    private ICanUse canUseObj;
    public PlayerInputControl inputSystem;
    
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        player = transform.parent.GetComponent<Transform>();
        anim = GetComponent<Animator>();
        inputSystem = new PlayerInputControl();
        inputSystem.Enable();
    }
    
    private void OnEnable()
    {
        InputSystem.onActionChange += onActionChange;
        inputSystem.GamePlay.Enter.started += OnEnter;
    }

    private void OnEnter(InputAction.CallbackContext obj)
    {
        if(canUse) canUseObj.ToUse();
    }

    private void OnDisable()
    {
        canUse = false;
    }


    private void Update()
    {
        sr.enabled = canUse;
        transform.localScale = player.localScale;
    }
    
    
    

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("CanUse"))
        {
            canUse = true;
            canUseObj = other.GetComponent<ICanUse>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CanUse")) canUse = false;
    }
    
    
    private void onActionChange(object obj, InputActionChange actionChange)
    {
        if (actionChange == InputActionChange.ActionStarted)
        {
            var d = ((InputAction)obj).activeControl.device;
            switch (d)
            {
                case Keyboard : anim.Play("keyboard");
                    break;
                case Gamepad: anim.Play("gamepad");
                    break;
                default:
                    break;
            }
        }
    }
    
    
}
