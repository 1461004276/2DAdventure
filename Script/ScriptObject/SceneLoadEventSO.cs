using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

//传递要切换的下一个场景的信息
[CreateAssetMenu(menuName = "Events/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneSO,Vector3,bool> LoadSceneEvent;
    
    /// <summary>
    /// 呼叫场景切换事件
    /// </summary>
    /// <param name="goToScene">要去的场景</param>
    /// <param name="goToPosition">下一个场景的初始位置</param>
    /// <param name="fadeScreen">是否要启用渐入渐出</param>
    public void OnLoadSceneEvent(GameSceneSO goToScene,Vector3 goToPosition,bool fadeScreen)
    {
        LoadSceneEvent?.Invoke(goToScene,goToPosition,fadeScreen);
    }
}
