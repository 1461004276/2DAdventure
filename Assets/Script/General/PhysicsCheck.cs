using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [Header("检测参数")]
    // 地面图层
    public LayerMask groundLayer;
    // 检测半径
    public float checkRidus;
    //碰撞体检测原点偏移量
    public Vector2 buttomOffset;
    public Vector2 rightOffset;
    public Vector2 leftOffset;
    
    [Header("状态参数")]
    // 是否在地面上
    public bool isGround;

    public bool isRightWall;
    public bool isLeftWall;
    
    void Update()
    {
        Check();
    }

    void Check()
    {
        //检测地面
        isGround = Physics2D.OverlapCircle((Vector2)transform.position+buttomOffset*transform.localScale, checkRidus, groundLayer);
        //检测左右墙体
        isRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRidus, groundLayer);
        isLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRidus, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position+buttomOffset*transform.localScale.x, checkRidus);
        
        Gizmos.DrawWireSphere((Vector2)transform.position+rightOffset, checkRidus);
        Gizmos.DrawWireSphere((Vector2)transform.position+leftOffset, checkRidus);

        
    }
}
