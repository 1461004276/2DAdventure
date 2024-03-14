using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{
    private Animator playerAnimator;
    private Rigidbody2D playerRigidbody2D;
    private PhysicsCheck physicsCheck;
    private PlayerController controller;
    
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        SwitchAnimation();
    }

    public void SwitchAnimation()
    {
        playerAnimator.SetFloat("speed",Mathf.Abs(playerRigidbody2D.velocity.x));
        playerAnimator.SetFloat("jumpSpeed",playerRigidbody2D.velocity.y);
        playerAnimator.SetBool("isGround",physicsCheck.isGround);
        playerAnimator.SetBool("isCrouch",controller.isCrouch);
        playerAnimator.SetBool("isAtk",controller.isAtk);
        
    }

    public void PlayHurt()
    {
        playerAnimator.SetTrigger("isHurt");
    }
    public void PlayDeath()
    {
        playerAnimator.SetBool("isDeadth",true);
    }

    public void PlayAtk()
    {
        playerAnimator.SetTrigger("atk");
    }


}
