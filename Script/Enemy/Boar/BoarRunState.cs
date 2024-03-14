using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarRunState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        nowEnemy = enemy;
        nowEnemy.nowSpeed = nowEnemy.runSpeed;
    }

    public override void LogicUpdate()
    {
        if (nowEnemy.CheckPlayer()) nowEnemy.noPlayerTimeCount = 0;
        
        if (!nowEnemy.CheckPlayer())
        {
            nowEnemy.noPlayerTimeCount += Time.deltaTime;
            if (nowEnemy.noPlayerTimeCount > nowEnemy.noPlayerTime)
            {
                nowEnemy.switchState(NpcState.walk);
            }
        }
        
        if (!nowEnemy.phyCheck.isGround || !nowEnemy.isHurt && (nowEnemy.phyCheck.isLeftWall && nowEnemy.faceDir.x < 0 ) || (nowEnemy.phyCheck.isRightWall && nowEnemy.faceDir.x > 0 ) )
        {
            nowEnemy.transform.localScale = new Vector3(nowEnemy.faceDir.x,1,1);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        nowEnemy.rb.velocity = Vector2.zero;
        nowEnemy.noPlayerTimeCount = 0f;
    }
}
