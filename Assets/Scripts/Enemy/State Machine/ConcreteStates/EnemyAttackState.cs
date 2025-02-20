using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private float _attackTimer;
    private Transform _playerTransform;
    private float _movementSpeed = 1.75f;

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

        if(enemy.animator.GetInteger("AttackNo") >= 0)
        {
            int randomAttack = 0;

            randomAttack = Random.Range(0, 7);

            if(randomAttack == 6)
            {
                randomAttack = -2;
            }

            enemy.SetAnimationInt("AttackNo", randomAttack);
        }

        // GRAB FORCE
        //enemy.SetAnimationInt("AttackNo", 5);

        enemy.SetAnimationTrigger("Attack");

        enemy.IsAttacking = true;
    }

    public override void ExitState()
    {
        enemy.SetAnimationInt("AttackNo", 0);

        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        _attackTimer += Time.deltaTime;

        Vector3 targetPosition = new Vector3(_playerTransform.position.x, 0f, _playerTransform.position.z);

        Vector3 moveDirection = (targetPosition - enemy.transform.position).normalized;

        // Running attack
        if (enemy.IsMovingWhileAttacking)
        {
            enemy.MoveEnemy(moveDirection * _movementSpeed/2 * enemy.SprintSpeed);
        }
        else
        {
            enemy.MoveEnemy(Vector3.zero);
        }

        // Backswing attack
        if(enemy.IsTurningClockwise)
        {
            enemy.RotateEnemyClockwise(180f);
        }

        // Rotate towards player while attacking
        if(enemy.IsTracking)
        {
            enemy.RotateEnemy(moveDirection, 7f);
        }

        if( !enemy.IsAttacking )
        {
            enemy.ResetAttackType();
            enemy.StateMachine.ChangeState(enemy.RecoveryState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
