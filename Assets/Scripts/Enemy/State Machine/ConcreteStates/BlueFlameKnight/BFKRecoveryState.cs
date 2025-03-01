using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFKRecoveryState : EnemyRecoveryState
{
    private Transform _playerTransform;
    private Vector3 _direction;
    private float _lookSpeed;

    public BFKRecoveryState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
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

        Debug.Log("Recovery State");

        enemy.MoveEnemy(Vector3.zero);

        Vector3 targetPosition = new Vector3(_playerTransform.position.x, 0f, _playerTransform.position.z);

        Vector3 moveDirection = (targetPosition - enemy.transform.position).normalized;

        float dotProduct = Vector3.Dot(enemy.transform.forward, moveDirection);

        if (Mathf.Approximately(dotProduct, 0f))
        {
            // Target is straight ahead.
            enemy.StateMachine.ChangeState(enemy.IdleState);
        }
        else if (dotProduct > 0f)
        {
            // Target is to the right (head rotates clockwise when viewed from above).
            enemy.SetAnimationTrigger("WalkRight");
        }
        else if (dotProduct < 0f)
        {
            // Target is to the left (lead rotates counter-clockwise when viewed from above).
            enemy.SetAnimationTrigger("WalkLeft");
        }

        if (dotProduct < 0f)
        {
            int backswing_random = Random.Range(0, 4);

            if (backswing_random == 0)
            {
                enemy.ResetAnimationTriggers();

                enemy.SetAnimationInt("AttackNo", -3);

                enemy.StateMachine.ChangeState(enemy.AttackState);
            }
        }

        //enemy.RotateEnemy(moveDirection, 5f);

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

        float dotProduct = Vector3.Dot(enemy.transform.forward, moveDirection);

        enemy.RotateEnemy(moveDirection, 4f);

        if (dotProduct >= 0.75f)
        {
            // Target is straight ahead.
            enemy.StateMachine.ChangeState(enemy.ReadyState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
