using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/FadeEventSO")]
public class FadeEventSO : ScriptableObject
{

    public UnityAction<Color, float, bool> OnActionLoad;
    
    /// <summary>
    /// 渐入
    /// </summary>
    /// <param name="duration"></param>
    public void FadeIn(float duration)
    {
        OnAction(Color.black,duration,true);
    }
    /// <summary>
    /// 渐出
    /// </summary>
    /// <param name="duration"></param>
    public void FadeOut(float duration)
    {
        OnAction(Color.clear,duration,true);
    }

    public void OnAction(Color target,float duration,bool isFade)
    {
        OnActionLoad?.Invoke(target,duration,isFade);
    }
    
    
}
