using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{

    public Rigidbody2D rb;
    public PhysicsCheck phyCheck;
    public Animator anim;


    [Header("状态")] 
    public bool isDeath;

    public bool isHurt;
    public bool isWait;

    protected BaseState walkState;
    protected BaseState nowState;
    protected BaseState runState;
    
    [Header("基本参数")]
    //todo:nowSpeed
    public float nowSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float hurtForce;
    public Vector3 faceDir;
    public Transform willAtker;//将要攻击的对象
    
    
    [Header("计数器")] 
    public float waitTime;
    public float waitTimeCount;
    public float noPlayerTime;
    public float noPlayerTimeCount;
    
    [Header("检测玩家")] 
    public Vector2 checkOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask checkLayer;

    
    
    protected virtual void Awake()
    {
        //获取组件
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        phyCheck = GetComponent<PhysicsCheck>();
        
    }

    private void OnEnable()
    {
        nowState = walkState;
        waitTimeCount = waitTime;
        nowState.OnEnter(this);
    }


    private void Update()
    {
        anim.SetBool("isDeath",isDeath);
        faceDir = new Vector3(-transform.localScale.x,1,1);
        
        nowState.LogicUpdate();
        WaitTime();
        anim.SetFloat("speed",Mathf.Abs(rb.velocity.x));
        

        
    }

    private void FixedUpdate()
    {
        Move();
        nowState.PhysicsUpdate();
    }


    private void OnDisable()
    {
        nowState.OnExit();
    }

    public void Move()
    {
        if (!isDeath && !isWait && !isHurt) rb.velocity = new Vector2(faceDir.x * nowSpeed * Time.deltaTime, 0);
    }

    public void WaitTime()
    {
        if (isWait && !isHurt)
        {
            rb.velocity = new Vector2(0, 0);
            waitTimeCount -= Time.deltaTime;
            if (waitTimeCount <= 0)
            {
                isWait = false;
                waitTimeCount = waitTime;
                transform.localScale = new Vector3(faceDir.x,1,1);
            }
            
        }
    }

    public bool CheckPlayer()
    {
        return Physics2D.BoxCast(transform.position+(Vector3)checkOffset,checkSize,0,faceDir,checkDistance,checkLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)(transform.position+(Vector3)checkOffset+new Vector3(checkDistance*(-transform.localScale.x),0,0)), 0.4f);

    }

    public void switchState(NpcState state)
    {
        var newState = state switch
        {
            NpcState.walk => walkState,
            NpcState.run => runState,
            _ => null
        };
        
        nowState.OnExit();
        nowState = newState;
        nowState.OnEnter(this);
    }
    
    
    #region 事件

    public void OnHurt(Transform atker)
    {
        willAtker = atker;
        //转身
        if(atker.transform.position.x - transform.position.x > 0 ) transform.localScale = new Vector3(-1,1,1);
        if(atker.transform.position.x - transform.position.x < 0 ) transform.localScale = new Vector3(1,1,1);
        isHurt = true;
        anim.SetTrigger("hurt");
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(transform.position.x - atker.transform.position.x, 0).normalized;
        if (isWait)
        {
            isWait = false;
            waitTimeCount = -0.1f;
        }
        //通过协程实现
        StartCoroutine(waitHurt(dir));

    }
    
    //通过携程实现
    private IEnumerator waitHurt(Vector2 dir)
    {
        rb.AddForce(dir*hurtForce,ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.45f);
        isHurt = false;

    }
    
    
    
    public void OnDeath()
    {
        isDeath = true;
        gameObject.layer = 2;
    }

    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }


    #endregion

}
