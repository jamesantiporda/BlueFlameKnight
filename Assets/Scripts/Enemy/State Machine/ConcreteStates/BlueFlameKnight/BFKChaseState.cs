using itsSALT.FinalCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFKChaseState : EnemyChaseState
{
    private Transform _playerTransform;
    private float _movementSpeed = 1.75f;

    private PlayerController _playerController;

    private float _outOfRangeTimer = 0.0f;

    private float decisionCooldown = 1.0f;
    private float decisionTimer = 0.0f;

    public BFKChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _playerController = _playerTransform.GetComponent<PlayerController>();
    }

    public override void EnterState()
    {
        base.EnterState();

        Debug.Log("Chase State");

        enemy.SetAnimationTrigger("Run");

        _outOfRangeTimer = 0.0f;
        decisionTimer = 0.0f;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        decisionTimer += Time.deltaTime;

        Vector3 targetPosition = new Vector3(_playerTransform.position.x, 0f, _playerTransform.position.z);

        Vector3 moveDirection = (targetPosition - enemy.transform.position).normalized;

        enemy.RotateEnemy(moveDirection, 5f);

        enemy.MoveEnemy(moveDirection * _movementSpeed * enemy.SprintSpeed);

        if (enemy.IsWithinStrikingDistance)
        {
            enemy.MoveEnemy(moveDirection * _movementSpeed * enemy.SprintSpeed / 2);

            int run_attack_random = Random.Range(0, 5);

            if (run_attack_random == 0)
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

            if (_outOfRangeTimer > 2.5f || (_playerController.ReturnIsDrinking() && decisionTimer >= decisionCooldown))
            {
                int range_attack_random = Random.Range(0, 2);

                if (range_attack_random == 0)
                {
                    enemy.SetAnimationInt("AttackNo", -2);

                    enemy.StateMachine.ChangeState(enemy.AttackState);
                }

                decisionTimer = 0.0f;
            }
        }

        if (!enemy.IsAggroed)
        {
            enemy.StateMachine.ChangeState(enemy.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
