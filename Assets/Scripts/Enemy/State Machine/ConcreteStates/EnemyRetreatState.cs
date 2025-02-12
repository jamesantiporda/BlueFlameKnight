using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRetreatState : EnemyState
{
    private float _speed = 5.0f;

    public EnemyRetreatState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();

        enemy.ResetAnimationTriggers();

        enemy.damageCounter = 0;

        enemy.animator.SetTrigger("Retreat");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        enemy.MoveEnemy(-enemy.transform.forward * _speed);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
