using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerCheckable
{
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }

    public GameObject rangedAttackObject;

    public Transform rangedAttackEmissionPoint;
    public Rigidbody RB { get; set; }

    public Animator animator { get; set; }

    public bool IsAggroed { get; set; }
    public bool IsWithinStrikingDistance { get; set; }

    public bool IsAttacking { get; set; }
    public bool IsTracking { get; set; }

    public bool IsTurningClockwise { get; set; }
    public bool IsMovingWhileAttacking { get; set; }

    public bool isActivated = false;

    private bool isTouchingPlayer = false;

    public enum Attack { Normal, Knockback, Launch }

    private Attack currentAttackType = Attack.Normal;

    #region State Machine Variables

    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyReadyState ReadyState { get; set; }
    public EnemyAttackState AttackState { get; set; }
    public EnemyRecoveryState RecoveryState { get; set; }
    public EnemyRetreatState RetreatState { get; set; }

    public EnemyDeactivatedState DeactivatedState { get; set; }

    #endregion

    #region Idle Variables

    public float RandomMovementRange = 5f;
    public float RandomMovementSpeed = 1f;
    public float RotationSpeed = 1f;

    #endregion

    #region Chase Variables

    public float SprintSpeed = 2f;

    #endregion

    #region General Variables

    public int damageCounter = 0;
    public int damageThreshold = 5;

    #endregion

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
        ReadyState = new EnemyReadyState(this, StateMachine);
        RecoveryState = new EnemyRecoveryState(this, StateMachine);
        RetreatState = new EnemyRetreatState(this, StateMachine);
        DeactivatedState = new EnemyDeactivatedState(this, StateMachine);
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;

        RB = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();

        StateMachine.Initialize(DeactivatedState);

        Debug.Log(transform.forward);

        IsAggroed = false;
    }

    private void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    #region Health and Die Functions

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;

        if(CurrentHealth <= 0f )
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    #endregion

    #region Movement functions

    public void MoveEnemy(Vector3 velocity)
    {
        velocity = new Vector3(velocity.x, 0f, velocity.z);

        //Vector3 newPosition = Vector3.MoveTowards(transform.position, transform.position + velocity * 1000f, 7.5f * velocity.magnitude * Time.deltaTime);

        //RB.MovePosition(newPosition);

        //transform.position += velocity * Time.deltaTime;

        RB.velocity = velocity;
    }

    public void RotateEnemy(Vector3 targetDirection, float speed)
    {
        float singleStep = RotationSpeed * speed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        Debug.DrawRay(transform.position, newDirection, Color.red);

        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public void RotateEnemyClockwise(float speed)
    {
        transform.RotateAround(transform.position, Vector3.up, 4 * speed * Time.deltaTime);
    }
    #endregion

    #region Distance Checks

    public void SetAggroStatus(bool isAggroed)
    {
        IsAggroed = isAggroed;
    }

    public void SetWithinStrikingDistance(bool isWithinStrikingDistance)
    {
        IsWithinStrikingDistance = isWithinStrikingDistance;
    }

    #endregion

    #region Animation Triggers

    public void Flinch()
    {
        damageCounter++;

        animator.Play("Flinch", 1, 0);
    }

    public void SetAnimationBool(string name, bool value)
    {
        animator.SetBool(name, value);
    }

    public void SetAnimationInt(string name, int value)
    {
        animator.SetInteger(name, value);
    }

    public void SetAnimationFloat(string name, float value)
    {
        animator.SetFloat(name, value);
    }

    public void SetAnimationTrigger(string name)
    {
        animator.SetTrigger(name);
    }

    public void ResetAnimationTriggers()
    {
        foreach (var param in animator.parameters)
        {
            if(param.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(param.name);
            }
        }
    }

    public void SetIsNotAttacking()
    {
        IsAttacking = false;
    }

    public void StartTracking()
    {
        IsTracking = true;
    }

    public void StopTracking()
    {
        IsTracking = false;
    }

    public void StartMovingWhileAttacking()
    {
        IsMovingWhileAttacking = true;
    }

    public void StopMovingWhileAttacking()
    {
        IsMovingWhileAttacking = false;
    }

    public void StartTurningClockwise()
    {
        IsTurningClockwise = true;
    }

    public void StopTurningClockwise()
    {
        IsTurningClockwise = false;
    }

    public void SetAttackType(int type)
    {
        if(type == 0)
        {
            currentAttackType = Attack.Normal;
        }
        else if(type == 1)
        {
            currentAttackType = Attack.Knockback;
        }
        else if(type == 2)
        {
            currentAttackType = Attack.Launch;
        }
        else
        {
            currentAttackType = Attack.Normal;
        }
    }

    public void SetIsTouchingPlayer(bool val)
    {
        isTouchingPlayer = val;
    }

    public void ResetAttackType()
    {
        currentAttackType = Attack.Normal;
    }

    public Attack ReturnAttackType()
    {
        return currentAttackType;
    }

    public void ChangeToIdleState()
    {
        StateMachine.ChangeState(IdleState);
    }

    public void SpawnRangedAttack()
    {
        Instantiate(rangedAttackObject, rangedAttackEmissionPoint.position, Quaternion.identity);
    }

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType
    {
        EnemyDamaged,
        PlayFootstepSound
    }
    #endregion
}
