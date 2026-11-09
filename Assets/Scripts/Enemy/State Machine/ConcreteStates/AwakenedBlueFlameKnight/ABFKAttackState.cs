using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABFKAttackState : EnemyAttackState
{
    private float _attackTimer;
    private Transform _playerTransform;
    private float _movementSpeed = 1.75f;
    private BossAttack _currentAttack;

    private bool openerUsed = false;

    public ABFKAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
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

        if (enemy.AttackSelector == null)
        {
            Debug.LogError("AttackSelector is null on " + enemy.name, enemy);
            return;
        }

        var ctx = AttackContext.Build(enemy, _playerTransform);

        if (enemy.hasOpeningAttack && !openerUsed)
        {
            _currentAttack = enemy.AttackSelector.ReturnOpeningAttack(enemy.openingAttackIndex);
            openerUsed = true;
        }
        else
        {
            _currentAttack = enemy.AttackSelector.Choose(ctx);
        }

        Debug.Log($"Chose: {(_currentAttack == null ? "NOTHING" : _currentAttack.name)} | distance {ctx.distance:F1} | hp {ctx.hpPercent:F2}");


        if (_currentAttack == null)
        {
            // Nothing valid right now: reposition instead of forcing an attack
            Debug.Log("No valid attack, going to ChaseState");
            enemy.StateMachine.ChangeState(enemy.ChaseState);
            return;
        }

        enemy.SetAnimationInt("AttackNo", _currentAttack.attackNo);
        enemy.SetAnimationTrigger("Attack");
        enemy.IsAttacking = true;
    }

    public override void ExitState()
    {
        if (_currentAttack != null)
        {
            enemy.AttackSelector.RegisterUsed(_currentAttack);
            _currentAttack = null;
        }

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
            enemy.MoveEnemy(moveDirection * _movementSpeed / 2 * enemy.SprintSpeed);
        }
        else
        {
            enemy.MoveEnemy(Vector3.zero);
        }

        // Backswing attack
        if (enemy.IsTurningClockwise)
        {
            enemy.RotateEnemyClockwise(180f);
        }

        // Rotate towards player while attacking
        if (enemy.IsTracking)
        {
            enemy.RotateEnemy(moveDirection, 7f);
        }

        if (!enemy.IsAttacking)
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