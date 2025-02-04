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

    private int _decisionCount;

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
        _decisionCount = 0;

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
            _decisionCount++;

            if(_decisionCount >= 2)
            {
                enemy.StateMachine.ChangeState(enemy.AttackState);
                return;
            }

            enemy.MoveEnemy(Vector3.zero);

            _decision = Random.Range(0, 5);

            _decisionTimer = Random.Range(1.0f, 2f);

            switch (_decision)
            {
                case 0:
                    enemy.SetAnimationTrigger("WalkRight");
                    break;
                case 1:
                    enemy.SetAnimationTrigger("WalkLeft");
                    break;
                case 2:
                    if (Vector3.Distance(enemy.transform.position, _playerTransform.transform.position) < 1.0f)
                    {
                        enemy.SetAnimationTrigger("WalkBackward");
                    }
                    else
                    {
                        enemy.SetAnimationTrigger("WalkForward");
                    }
                    break;
                case 3:
                    enemy.SetAnimationTrigger("WalkBackward");
                    break;
                case 4:
                    enemy.StateMachine.ChangeState(enemy.AttackState);
                    break;
                default:
                    break;
            }
        }
        else
        {
            _decisionTimer -= Time.deltaTime;
        }

        //Debug.Log(_decisionTimer);

        switch(_decision)
        {
            case 0:
                enemy.MoveEnemy(enemy.transform.right * _speed);
                break;
            case 1:
                enemy.MoveEnemy(-enemy.transform.right * _speed);
                break;
            case 2:
                if (Vector3.Distance(enemy.transform.position, _playerTransform.transform.position) < 1.0f)
                {
                    enemy.MoveEnemy(-enemy.transform.forward * _speed);
                }
                else
                {
                    enemy.MoveEnemy(enemy.transform.forward * _speed);
                }
                break;
            case 3:
                enemy.MoveEnemy(-enemy.transform.forward * _speed);
                break;
            default:
                break;
        }

        if(Vector3.Distance(enemy.transform.position, _playerTransform.transform.position) < 1.0f)
        {
            if(_decision == 2)
            {
                enemy.SetAnimationTrigger("WalkBackward");
                enemy.MoveEnemy(-enemy.transform.forward * _speed);
            }
        }



        if (!enemy.IsWithinStrikingDistance)
        {
            _timer -= Time.deltaTime;

            if(_timer < 0)
            {
                enemy.ResetAnimationTriggers();
                enemy.StateMachine.ChangeState(enemy.ChaseState);
            }
        }
        else
        {
            _timer = 0.8f;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
