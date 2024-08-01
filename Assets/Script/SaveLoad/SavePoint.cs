using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour,ICanUse
{
    public VoidEventSO saveGameEvent;
    
    
    private SpriteRenderer spriteRenderer;

    public Sprite darkSprite;
    public Sprite lightSprite;
    public bool isDone;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? lightSprite : darkSprite;
    }


    public void ToUse()
    {
        if (!isDone)
        {
            isDone = true;
            spriteRenderer.sprite = lightSprite;
            
            //todo:保存数据
            saveGameEvent.OnEvent();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(isDone) this.gameObject.tag = "Untagged";

    }
}
