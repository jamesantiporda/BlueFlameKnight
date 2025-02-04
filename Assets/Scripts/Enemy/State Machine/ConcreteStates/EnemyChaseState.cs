using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private Transform _playerTransform;
    private float _movementSpeed = 1.75f;

    private float _outOfRangeTimer = 0.0f;

    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
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

        Debug.Log("Chase State");

        enemy.SetAnimationTrigger("Run");

        _outOfRangeTimer = 0.0f;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        Vector3 targetPosition = new Vector3(_playerTransform.position.x, 0f, _playerTransform.position.z);

        Vector3 moveDirection = (targetPosition - enemy.transform.position).normalized;

        enemy.RotateEnemy(moveDirection, 5f);

        enemy.MoveEnemy(moveDirection * _movementSpeed * enemy.SprintSpeed);

        if(enemy.IsWithinStrikingDistance)
        {
            enemy.MoveEnemy(moveDirection * _movementSpeed * enemy.SprintSpeed/2);

            int run_attack_random = Random.Range(0, 5);

            if(run_attack_random == 0)
            {
                enemy.SetAnimationInt("AttackNo", -1);

                enemy.StateMachine.ChangeState(enemy.AttackState);
            }
            else
            {
                enemy.SetAnimationTrigger("WalkForward");

                enemy.StateMachine.ChangeState(enemy.ReadyState);
            }
        }
        else
        {
            _outOfRangeTimer += Time.deltaTime;

            if(_outOfRangeTimer > 5.0f)
            {
                int range_attack_random = Random.Range(0, 5);

                if(range_attack_random == 0)
                {
                    enemy.SetAnimationInt("AttackNo", -2);

                    enemy.StateMachine.ChangeState(enemy.AttackState);
                }
            }
        }

        if(!enemy.IsAggroed)
        {
            enemy.StateMachine.ChangeState(enemy.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
