using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour,ISaveable
{

    public VoidEventSO NewGameEvent;
    
    [Header("基本属性")]
    public float maxHeart;
    public float nowHeart;

    [Header("无敌参数")] 
    public float noHurtTime;
    public float noHurtTimeCounter;
    public bool isNoHurt;

    [Header("Event")] 
    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Transform> OnHurt;
    public UnityEvent<Transform> OnDeadth;

    private void Start()
    {
        nowHeart = maxHeart;
    }

    private void NewGame()
    {
        nowHeart = maxHeart;
        OnHealthChange?.Invoke(this);
    }

    private void OnEnable()
    {
        NewGameEvent.TakeOnAction += NewGame;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        NewGameEvent.TakeOnAction -= NewGame;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    private void Update()
    {
        if (isNoHurt)
        {
            noHurtTimeCounter -= Time.deltaTime;
            if (noHurtTimeCounter <= 0)
            {
                isNoHurt = false;
            }
        }
        
    }
    
    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="attacker"></param>
    public void TakeAtk(Attack attacker)
    {
        if(isNoHurt) return;
        if (nowHeart - attacker.atk > 0)
        {
            nowHeart -= attacker.atk;
            TakeNoHurt();
            OnHurt?.Invoke(attacker.transform);
        }
        else
        {
            nowHeart = 0;
            OnDeadth?.Invoke(attacker.transform);
            
        }
        OnHealthChange?.Invoke(this);

    }
    /// <summary>
    /// 受伤无敌
    /// </summary>
    public void TakeNoHurt()
    {
        if (!isNoHurt)
        {
            isNoHurt = true;
            noHurtTimeCounter = noHurtTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water") && nowHeart > 0)
        {
            OnDeadth?.Invoke(this.transform);
            nowHeart = 0;
            OnHealthChange?.Invoke(this);
        }
    }

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetSaveData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            data.characterPosDict[GetDataID().ID] = new SerializeVector3(transform.position);
            data.floatSavedData[GetDataID().ID + "health"] = this.nowHeart;
        }
        else
        {
            data.characterPosDict.Add(GetDataID().ID,new SerializeVector3(transform.position));
            data.floatSavedData.Add(GetDataID().ID+"health",this.nowHeart);
        }
    }

    public void LoadData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            transform.position = data.characterPosDict[GetDataID().ID].ToVector3();
            this.nowHeart = data.floatSavedData[GetDataID().ID + "health"];
            OnHealthChange?.Invoke(this);
        }
    }
}
