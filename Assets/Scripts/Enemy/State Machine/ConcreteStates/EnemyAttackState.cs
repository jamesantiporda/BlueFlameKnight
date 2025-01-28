using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private float _attackTimer;
    private Transform _playerTransform;

    public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();

        Debug.Log("Attack State");

        _attackTimer = 0.0f;

        enemy.MoveEnemy(Vector3.zero);

        enemy.SetAnimationTrigger("Attack");

        int randomAttack = 0;

        randomAttack = Random.Range(0, 6);

        enemy.SetAnimationInt("AttackNo", randomAttack);

        enemy.IsAttacking = true;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        _attackTimer += Time.deltaTime;

        if(enemy.IsTracking)
        {
            Vector3 targetPosition = new Vector3(_playerTransform.position.x, 0f, _playerTransform.position.z);

            Vector3 moveDirection = (targetPosition - enemy.transform.position).normalized;

            enemy.RotateEnemy(moveDirection, 7f);
        }

        if( !enemy.IsAttacking )
        {
            enemy.StateMachine.ChangeState(enemy.RecoveryState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
