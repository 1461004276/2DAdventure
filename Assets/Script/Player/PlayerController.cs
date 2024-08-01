using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputControl inputControl;
    private Rigidbody2D playerRigidbody;
    private PhysicsCheck physicsCheck;
    private CapsuleCollider2D playerColl;
    private PlayerAnimationControl myAnimControl;
    private AudioPlay audioPlay;
    public PlayerBar playerBar;
    
    
    [Header("基本参数")]
    
    public float jumpForce;//跳跃力

    public float hurtForce;
    public Vector2 inputDirection;//输入方向
    //速度
    public float speed;
    private float walkSpeed;
    private float runSpeed;
    //碰撞体数据
    private Vector2 originalSize;
    private Vector2 originalOffet;
    

    [Header("状态")] 
    public bool isCrouch;
    public bool isHurt;
    public bool isDeadth;
    public bool isAtk;

    [Header("物理材质")] 
    public PhysicsMaterial2D groundMaterial;

    public PhysicsMaterial2D wallMaterial;

    [Header("监听事件")] 
    public SceneLoadEventSO loadEventSo;
    public VoidEventSO afterLoad;
    public VoidEventSO restartEvent;
    public VoidEventSO bakcToMenu;
    
    private void Awake()
    {

        #region 组件获取初始值
        playerRigidbody = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputControl();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerColl = GetComponent<CapsuleCollider2D>();
        myAnimControl = GetComponent<PlayerAnimationControl>();
        audioPlay = GetComponent<AudioPlay>();
        
        #endregion

        #region 获取初始记录值
        originalSize = playerColl.size;
        originalOffet = playerColl.offset;
        walkSpeed = speed / 2;
        runSpeed = speed;
        #endregion
        
        inputControl.Enable();

        
        #region 注册事件

        inputControl.GamePlay.Jump.started += Jump;
        inputControl.GamePlay.Atk.started += Atk;

        #region 下蹲事件注册

        inputControl.GamePlay.Crouch.performed += ctx =>{
            if (physicsCheck.isGround)
            {
                isCrouch = true;
                playerColl.offset = new Vector2(-0.06f,0.7f);
                playerColl.size = new Vector2(0.6f,1.4f);
            }};
        inputControl.GamePlay.Crouch.canceled += ctx =>{
            if (physicsCheck.isGround)
            {
                isCrouch = false;
                playerColl.offset = originalOffet;
                playerColl.size = originalSize;
                
            }};

        #endregion
        
        #endregion
        

        #region 行走与跑步切换
        inputControl.GamePlay.Walk.performed += ctx => { if( physicsCheck.isGround ) speed = walkSpeed; };
        inputControl.GamePlay.Walk.canceled += ctx => { if( physicsCheck.isGround ) speed = runSpeed; };
        #endregion
        

    }




    #region  组件启用

    private void OnEnable()
    {
        loadEventSo.LoadSceneEvent += OnLoadEvent;
        afterLoad.TakeOnAction += afterEvent;
        restartEvent.TakeOnAction += OnRestart;
        bakcToMenu.TakeOnAction += OnRestart;
    }


    private void OnDisable()
    {
        inputControl.Disable();
        loadEventSo.LoadSceneEvent -= OnLoadEvent;
        afterLoad.TakeOnAction -= afterEvent;
        restartEvent.TakeOnAction -= OnRestart;
        bakcToMenu.TakeOnAction -= OnRestart;


    }
 

    #endregion

    private void Update()
    {
        inputDirection = inputControl.GamePlay.Move.ReadValue<Vector2>();
        CheckState();
    }

    private void FixedUpdate()
    {
      if(!isHurt && !isDeadth && !isAtk)  Move();
    }
    //加载场景过程停止控制
    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.GamePlay.Disable();
        playerBar.gameObject.SetActive(false);
    }
    //加载结束启用人物控制
    private void afterEvent()
    {
        inputControl.GamePlay.Enable();
        playerBar.gameObject.SetActive(true);
    }

    //返回上一次记录点
    private void OnRestart()
    {
        isDeadth = false;
    }
    
    public void Move()
    {
        
        //移动
        //Time.deltaTime=1/每秒帧数
        if(!isCrouch) playerRigidbody.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, playerRigidbody.velocity.y);
        
        //人物翻转
        int faceDir = (int)transform.localScale.x;
        if(inputDirection.x != 0) faceDir = (inputDirection.x > 0) ? 1 : -1;
        //人物朝向
        transform.localScale = new Vector3(faceDir, 1, 1);
        

    }
    
    private void Jump(InputAction.CallbackContext obj)
    {
        if (physicsCheck.isGround && !isHurt)
        {
            audioPlay.PlayAudio();
            playerRigidbody.AddForce(transform.up*jumpForce, ForceMode2D.Impulse);
        }
    }
    
    private void Atk(InputAction.CallbackContext obj)
    {
        if (physicsCheck.isGround && !isHurt) isAtk = true;
        myAnimControl.PlayAtk();
        
    }
    


    #region Unity事件

    public void GetHurt(Transform atker)
    {
        isHurt = true;
        playerRigidbody.velocity = Vector2.zero;
        Vector2 dir = new Vector2(this.transform.position.x - atker.transform.position.x, 0).normalized;
        playerRigidbody.AddForce(dir*hurtForce,ForceMode2D.Impulse);
    }

    public void ToDeadth(Transform atker)
    {
        isDeadth = true;
        playerRigidbody.velocity = Vector2.zero;
        inputControl.GamePlay.Disable();
        Vector2 dir = new Vector2(this.transform.position.x - atker.transform.position.x, 0).normalized;
        playerRigidbody.AddForce(dir*hurtForce,ForceMode2D.Impulse);
    }

    #endregion

    


    private void CheckState()
    {
        playerColl.sharedMaterial = physicsCheck.isGround ?groundMaterial:wallMaterial;
        if (isDeadth) gameObject.layer = LayerMask.NameToLayer("Enemy");
        else gameObject.layer = LayerMask.NameToLayer("Player");
    }
    

}
