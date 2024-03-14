using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu( menuName = "Events/Void Event")]
public class VoidEventSO : ScriptableObject
{
    public UnityAction TakeOnAction;

    public void OnEvent()
    {
        TakeOnAction?.Invoke();
    }

}
