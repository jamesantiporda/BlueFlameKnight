using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFlameKnight : Enemy
{
    [SerializeField] private AudioClip grabStabSFX;
    [SerializeField] private AudioClip swordHitGroundSFX;
    [SerializeField] private AudioClip jumpSFX;

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();

        IdleState = new BFKIdleState(this, StateMachine);
        ChaseState = new BFKChaseState(this, StateMachine);
        AttackState = new BFKAttackState(this, StateMachine);
        ReadyState = new BFKReadyState(this, StateMachine);
        RecoveryState = new BFKRecoveryState(this, StateMachine);
        RetreatState = new BFKRetreatState(this, StateMachine);
        DeactivatedState = new EnemyDeactivatedState(this, StateMachine);
        DieState = new EnemyDieState(this, StateMachine);
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;

        RB = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();

        _health = GetComponent<Health>();

        StateMachine.Initialize(DeactivatedState);

        Debug.Log(transform.forward);

        IsAggroed = false;

        IsDead = false;

        grabPosition = transform.position + transform.forward * 0.9f;
    }

    public void PlayGrabStabSFX()
    {
        SoundFXManager.instance.PlaySoundFXClip(grabStabSFX, transform, 1.0f);
    }

    public void PlaySwordHitGround()
    {
        SoundFXManager.instance.PlaySoundFXClip(swordHitGroundSFX, transform, 0.5f);
    }

    public void PlayJumpSFX()
    {
        SoundFXManager.instance.PlaySoundFXClip(jumpSFX, transform, 0.1f);
    }

    protected override void SetGrabPosition()
    {
        grabPosition = transform.position + transform.forward * 0.9f;
    }
}
