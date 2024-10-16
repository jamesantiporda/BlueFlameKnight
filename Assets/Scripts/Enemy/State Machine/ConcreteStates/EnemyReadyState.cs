using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyReadyState : EnemyState
{
    private float _timer;
    private float _decisionTimer;
    private Transform _playerTransform;
    private int _decision;

    private float _speed = 0.5f;

    public EnemyReadyState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
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

        enemy.MoveEnemy(Vector3.zero);

        Debug.Log("Ready State");

        _decisionTimer = 0.0f;
        _decision = -1;

        enemy.SetAnimationTrigger("Idle");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        Vector3 targetPosition = new Vector3(_playerTransform.position.x, 0f, _playerTransform.position.z);

        Vector3 targetDirection = (targetPosition - enemy.transform.position).normalized;

        enemy.RotateEnemy(targetDirection, 5f);

        if(_decisionTimer < 0.0f)
        {
            enemy.MoveEnemy(Vector3.zero);

            _decision = Random.Range(0, 4);

            _decisionTimer = Random.Range(2.0f, 5.0f);

            switch (_decision)
            {
                case 0:
                    enemy.SetAnimationTrigger("WalkRight");
                    break;
                case 1:
                    enemy.SetAnimationTrigger("WalkLeft");
                    break;
                case 2:
                    enemy.SetAnimationTrigger("WalkForward");
                    break;
                case 3:
                    enemy.SetAnimationTrigger("WalkBackward");
                    break;
                default:
                    break;
            }
        }
        else
        {
            _decisionTimer -= Time.deltaTime;
        }

        Debug.Log(_decisionTimer);

        switch(_decision)
        {
            case 0:
                enemy.MoveEnemy(enemy.transform.right * _speed);
                break;
            case 1:
                enemy.MoveEnemy(-enemy.transform.right * _speed);
                break;
            case 2:
                enemy.MoveEnemy(enemy.transform.forward * _speed);
                break;
            case 3:
                enemy.MoveEnemy(-enemy.transform.forward * _speed);
                break;
            default:
                break;
        }



        if (!enemy.IsWithinStrikingDistance)
        {
            _timer -= Time.deltaTime;

            if(_timer < 0)
            {
                enemy.StateMachine.ChangeState(enemy.ChaseState);
            }
        }
        else
        {
            _timer = 2.0f;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
