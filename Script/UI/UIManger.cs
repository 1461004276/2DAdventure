using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIManger : MonoBehaviour
{
    [Header("Event")] 
    public CharacterEventSO HealthEvent;


    public VoidEventSO backToMenu;
    public SceneLoadEventSO unLoadSceneEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO gameOverEvent;
    public FloatEventSO sameVolumeEvent;
    
    public VoidEventSO PauseEvent;
    
    
    [Header("UI")]
    public PlayerBar playerBar;
    public GameObject gameOverPlane;
    public GameObject resartButton;
    public Button settingsBtn;
    public GameObject pausePlane;
    public Slider volumeSlider;
    

    private void Awake()
    {
        settingsBtn.onClick.AddListener(TakePausePlane);
    }

    private void OnEnable()
    {
        HealthEvent.OnEventUP += OnHealthEvent;
        unLoadSceneEvent.LoadSceneEvent += UnLoadEvent;
        gameOverEvent.TakeOnAction += OnGameOver;
        loadDataEvent.TakeOnAction += OnLoadData;
        backToMenu.TakeOnAction += OnLoadData;
        sameVolumeEvent.OnEventRaised += SameVolume;
    }

    private void OnDisable()
    {
        HealthEvent.OnEventUP -= OnHealthEvent;
        unLoadSceneEvent.LoadSceneEvent -= UnLoadEvent;
        gameOverEvent.TakeOnAction -= OnGameOver;
        loadDataEvent.TakeOnAction -= OnLoadData;
        backToMenu.TakeOnAction -= OnLoadData;
        sameVolumeEvent.OnEventRaised -= SameVolume;

    }

    private void SameVolume(float amount)
    {
        volumeSlider.value = (amount + 80) / 100;
    }

    private void TakePausePlane()
    {
        if (pausePlane.activeInHierarchy)
        {
            pausePlane.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            PauseEvent.OnEvent();
            pausePlane.SetActive(true);
            Time.timeScale = 0;
            
        }
    }
    

    private void OnGameOver()
    {
        gameOverPlane.SetActive(true);
        EventSystem.current.SetSelectedGameObject(resartButton);
    }

    private void OnLoadData()
    {
        if (pausePlane.activeInHierarchy)
        {
            pausePlane.SetActive(false);
            Time.timeScale = 1;
        }
        gameOverPlane.SetActive(false);
        
    }

    private void UnLoadEvent(GameSceneSO scene, Vector3 arg1, bool arg2)
    {
        var isMenu = scene.sceneTag == SceneTag.Menu;
        playerBar.gameObject.SetActive(!isMenu);
        
    }

    private void OnHealthEvent(Character character)
    {
        var healthPercent = character.nowHeart / character.maxHeart;
        playerBar.HealthChange(healthPercent);
    }
}
