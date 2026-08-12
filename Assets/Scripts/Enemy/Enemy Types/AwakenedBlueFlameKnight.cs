using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakenedBlueFlameKnight : Enemy
{
    private void Awake()
    {
        StateMachine = new EnemyStateMachine();

        IdleState = new ABFKIdleState(this, StateMachine);
        ChaseState = new ABFKChaseState(this, StateMachine);
        AttackState = new ABFKAttackState(this, StateMachine);
        ReadyState = new ABFKReadyState(this, StateMachine);
        RecoveryState = new ABFKRecoveryState(this, StateMachine);
        RetreatState = new ABFKRetreatState(this, StateMachine);
        DeactivatedState = new EnemyDeactivatedState(this, StateMachine);
        DieState = new EnemyDieState(this, StateMachine);
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;

        RB = GetComponent<Rigidbody>();

        characterController = GetComponent<CharacterController>();

        animator = GetComponent<Animator>();

        _health = GetComponent<Health>();

        StateMachine.Initialize(DeactivatedState);

        //Debug.Log(transform.forward);

        IsAggroed = false;

        IsDead = false;
    }
}
