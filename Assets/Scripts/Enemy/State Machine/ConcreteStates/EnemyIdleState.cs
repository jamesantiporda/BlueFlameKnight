using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private Vector3 _targetPos;
    private Vector3 _direction;
    private float _lookSpeed;
    private float _timer;

    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();

        _targetPos = GetRandomPointInCircle();

        _direction = (_targetPos - enemy.transform.position).normalized;

        _timer = 0.0f;

        Debug.Log("Idle State");

        enemy.SetAnimationTrigger("WalkForward");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        _timer += Time.deltaTime;

        if(enemy.IsAggroed)
        {
            enemy.RotateEnemy(_direction, 5f);

            enemy.StateMachine.ChangeState(enemy.ChaseState);
        }

        _direction = (_targetPos - enemy.transform.position).normalized;

        enemy.RotateEnemy(_direction, 5f);

        enemy.MoveEnemy(_direction * enemy.RandomMovementSpeed);

        if((enemy.transform.position - _targetPos).sqrMagnitude < 0.01f || _timer >= 5.0f)
        {
            _targetPos = GetRandomPointInCircle();
            _timer = 0.0f;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private Vector3 GetRandomPointInCircle()
    {
        Vector3 randomPoint = (Vector3)UnityEngine.Random.insideUnitCircle;

        randomPoint = new Vector3(randomPoint.x, 0f, randomPoint.y);

        return enemy.transform.position + randomPoint * enemy.RandomMovementRange;
    }
}
