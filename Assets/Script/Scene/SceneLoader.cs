using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour,ISaveable
{
    public Transform playerTrans;
    public GameObject gameOverPlane;
    
    [Header("Listener")] 
    public SceneLoadEventSO loadEventSo;
    
    public FadeEventSO fadeEventSo;
    public VoidEventSO backToMenuEvent;
    public VoidEventSO StartGameSo;
    public VoidEventSO afterSceneLoadEvent;


    public Vector3 menuPosition;
    public GameSceneSO menuScene;
    public GameSceneSO firstGameScene;
    public GameSceneSO nowScene;
    private GameSceneSO willScene;
    public Vector3 firstPostion;
    private Vector3 willPostion;
    
    private bool isFade;
    private bool isLoading;
    public float FadeTime;
    
    private void Awake()
    {

        
        //todo:为什么使用第一种加载后面无法正确卸载
         // Addressables.LoadSceneAsync(FirstGameSceneSo.SceneReference, LoadSceneMode.Additive);
        //如果采用这种方式，后面无法使用nowScene对其卸载
        /*
         * ctrl点击Addressables的LoadSceneAsync和currentLoadScene.sceneReference的LoadSceneAsync就会发现在源码里是两个不同的方法，
         * 但是使用GetInstanceID查看firstLoadScene和currentLoadScene会得到同一个ID，所以应该是引用的同一个对象
         * ，只是Addressables的LoadSceneAsync与sceneReference的UnloadScene不能匹配产生的错误
         */
        // nowScene = FirstGameSceneSo;
        // nowScene.SceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        // NewGame();
    }

    private void Start()
    {
        loadEventSo.OnLoadSceneEvent(menuScene,menuPosition,true);
    }

    private void OnEnable()
    {
        loadEventSo.LoadSceneEvent+=OnLoadSceneEvent;
        StartGameSo.TakeOnAction += NewGame;
        backToMenuEvent.TakeOnAction += OnBackMenu;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }



    private void OnDisable()
    {
        loadEventSo.LoadSceneEvent-=OnLoadSceneEvent;
        StartGameSo.TakeOnAction -= NewGame;
        backToMenuEvent.TakeOnAction += OnBackMenu;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();

    }

    private void OnBackMenu()
    {
        willScene = menuScene;
        loadEventSo.OnLoadSceneEvent(willScene,menuPosition,true);
        gameOverPlane.SetActive(false);
    }


    private void NewGame()
    {
        willScene = firstGameScene;
        loadEventSo.OnLoadSceneEvent(willScene,firstPostion,true);
        
    }
    
    
    
    private void OnLoadSceneEvent(GameSceneSO goToScene, Vector3 goToPostion, bool fadeScreen)
    {
        if(isLoading) return;
        isLoading = true;
        willScene = goToScene;
        willPostion = goToPostion;
        isFade = fadeScreen;
        if(nowScene!= null) StartCoroutine(UnLoadNowScene());
        else LoadNewScene();

    }

    private IEnumerator UnLoadNowScene()
    {
        if (isFade)
        {
            fadeEventSo.FadeIn(FadeTime);
        }
        yield return new WaitForSeconds(FadeTime);
        yield return nowScene.SceneReference.UnLoadScene();
        playerTrans.gameObject.SetActive(false);
        LoadNewScene();

    }

    private void LoadNewScene()
    {
        var loadingOption = willScene.SceneReference.LoadSceneAsync(LoadSceneMode.Additive,true);
        loadingOption.Completed += OnLoadingCompleted;
    }
    //场景加载完成事件
    private void OnLoadingCompleted(AsyncOperationHandle<SceneInstance> obj)
    {
        nowScene = willScene;

        playerTrans.position = willPostion;
        playerTrans.gameObject.SetActive(true);
        if (isFade)
        {
            fadeEventSo.FadeOut(FadeTime);
        }

        isLoading = false;
        
        if(nowScene.sceneTag != SceneTag.Menu) afterSceneLoadEvent.OnEvent();

    }

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetSaveData(Data data)
    {
        data.SaveGameScene(nowScene);
    }

    public void LoadData(Data data)
    {
        var playerID = playerTrans.GetComponent<DataDefination>().ID;
        if (data.characterPosDict.ContainsKey(playerID))
        {
            willPostion = data.characterPosDict[playerID].ToVector3();
            willScene = data.GetSavedScene();
            
            OnLoadSceneEvent(willScene,willPostion,true);
            
        }
    }
}
