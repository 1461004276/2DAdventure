using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarWalkState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        nowEnemy = enemy;
        nowEnemy.nowSpeed = nowEnemy.walkSpeed;
    }

    public override void LogicUpdate()
    {
        if (nowEnemy.CheckPlayer())
        {
            nowEnemy.switchState(NpcState.run);
        }
        
        if (!nowEnemy.phyCheck.isGround || !nowEnemy.isHurt && (nowEnemy.phyCheck.isLeftWall && nowEnemy.faceDir.x < 0 ) || (nowEnemy.phyCheck.isRightWall && nowEnemy.faceDir.x > 0 ) )
        {
            nowEnemy.isWait = true;
        }
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        nowEnemy.rb.velocity = Vector2.zero;
    }
}
