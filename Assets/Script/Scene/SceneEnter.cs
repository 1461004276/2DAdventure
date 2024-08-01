using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEnter : MonoBehaviour,ICanUse
{
    private AudioPlay audioPlay;
    public Vector3 goToPostion;
    public GameSceneSO goToScene;
    public SceneLoadEventSO LoadEventSo;
    
    
    private void Awake()
    {
        audioPlay = GetComponent<AudioPlay>();
    }
    
    public void ToUse()
    {
        Debug.Log("Exit");
        audioPlay.PlayAudio();
        
        LoadEventSo.OnLoadSceneEvent(goToScene,goToPostion,true);
    }
}
