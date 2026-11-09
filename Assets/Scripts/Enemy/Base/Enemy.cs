using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Enemy;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerCheckable
{
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public Transform LockOnPoint;
    public float CurrentHealth { get; set; }

    public GameObject rangedAttackObject;

    public Transform rangedAttackEmissionPoint;
    public Rigidbody RB { get; set; }

    public Animator animator { get; set; }

    public CharacterController characterController { get; set; }

    private Transform playerTransform;

    public bool IsAggroed { get; set; }
    public bool IsWithinStrikingDistance { get; set; }

    public bool IsAttacking { get; set; }
    public bool IsTracking { get; set; }

    public bool IsTurningClockwise { get; set; }
    public bool IsMovingWhileAttacking { get; set; }

    public bool isActivated = false;
    public bool UsesCharacterController = false;

    public bool IsDead { get; set; }

    public int CurrentAttackDamage { get; private set; } = 0;

    public float TargetOffset { get; private set; } = 0f;

    protected bool isTouchingPlayer = false;

    private Transform currentTarget;
    private Vector3 currentTargetDirection;
    private Vector3 currentTargetPosition;
    private bool isMovingToDirection = false;
    private bool isMovingToTarget = false;
    private bool isMovingToPosition = false;

    private float moveToPointSpeed = 2f;
    
    private Vector2 targetPositionOffset = Vector2.zero;
    private Vector3 startingPosition = Vector3.zero;
    private float moveToPointTime = 0f;
    private float moveToPointTimer = 0f;
    private float maxDistance = 1000f;

    public enum Attack { Normal, Knockback, Launch }

    protected Attack currentAttackType = Attack.Normal;

    protected Health _health;

    private float damageCDTimer = 0.0f;
    private float damageCD = 0.1f;

    #region State Machine Variables

    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyReadyState ReadyState { get; set; }
    public EnemyAttackState AttackState { get; set; }
    public EnemyRecoveryState RecoveryState { get; set; }
    public EnemyRetreatState RetreatState { get; set; }
    public EnemyDeactivatedState DeactivatedState { get; set; }
    public EnemyDieState DieState { get; set; }

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

    protected Vector3 grabPosition = Vector3.zero;

    #endregion

    [Header("SFX")]
    [SerializeField] private AudioClip damagedSFX;
    [SerializeField] private AudioClip[] footstepsSFX;
    [SerializeField] private AudioClip[] bladeSwingSFX;
    [SerializeField] private AudioClip[] bladeSlamSFX;
    [SerializeField] private AudioClip deathSFX;

    #region Attack Logic

    // With your other fields:
    [SerializeField] private List<BossAttack> attacks;

    private AttackSelector _attackSelector;
    public AttackSelector AttackSelector => _attackSelector ??= new AttackSelector(attacks);

    public bool hasOpeningAttack = false;
    public int openingAttackIndex;

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
        DieState = new EnemyDieState(this, StateMachine);
    }

    private void Start()
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

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        currentTargetDirection = transform.forward;
        currentTargetDirection = new Vector3(currentTargetDirection.x, 0.0f, currentTargetDirection.z);
        currentTargetDirection.Normalize();

        currentTargetPosition = Vector3.zero;

        targetPositionOffset = Vector2.zero;
        startingPosition = transform.position;

        isMovingToDirection = false;
        isMovingToTarget = false;
        isMovingToPosition = false;

        moveToPointTime = 0;
        moveToPointTimer = 0;
        maxDistance = 1000f;
    }

    private void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();

        SetGrabPosition();

        if(_health.currentHealth <= 0f && !IsDead)
        {
            IsDead = true;
            SoundFXManager.instance.PlaySoundFXClip(deathSFX, transform, 0.5f);
            StateMachine.ChangeState(DieState);
        }

        if(damageCDTimer > 0)
        {
            damageCDTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();

        // Hard targeting (tracking player movement)
        if(isMovingToTarget && currentTarget != null)
        {
            Vector3 move_direction = currentTarget.position - transform.position;

            move_direction = new Vector3(move_direction.x, 0.0f, move_direction.z);

            move_direction.Normalize();

            if (UsesCharacterController)
            {
                characterController.Move(move_direction * moveToPointSpeed * Time.deltaTime);
            }
            else
            {
                RB.velocity = move_direction * moveToPointSpeed;
            }
        }

        // Soft targeting (only goes toward a set direction)
        if(isMovingToDirection)
        {
            MoveEnemy(currentTargetDirection * moveToPointSpeed);
        }

        // Move to a target point
        if(isMovingToPosition && moveToPointTime > 0)
        {
            float t = moveToPointTimer / moveToPointTime;

            t = Mathf.Clamp01(t);

            transform.position = Vector3.Lerp(startingPosition, currentTargetPosition, t);
            
            if(t >= 1f)
            {
                EndMoveEnemyToPoint();
            }

            moveToPointTimer += Time.fixedDeltaTime;
        }
    }

    #region Helper Functions

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;

        if (CurrentHealth <= 0f )
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void SetAttackDamage(int damage)
    {
        CurrentAttackDamage = damage;
    }

    #endregion

    #region Movement functions

    public void MoveEnemy(Vector3 velocity)
    {
        velocity = new Vector3(velocity.x, 0f, velocity.z);

        //Vector3 newPosition = Vector3.MoveTowards(transform.position, transform.position + velocity * 1000f, 7.5f * velocity.magnitude * Time.deltaTime);

        //RB.MovePosition(newPosition);

        //transform.position += velocity * Time.deltaTime;

        //Debug.Log("MOVE");


        if (UsesCharacterController)
        {
            characterController.Move(velocity * Time.deltaTime);
        }
        else
        {
            RB.velocity = velocity;
        }
    }

    public void RotateEnemy(Vector3 targetDirection, float speed)
    {
        targetDirection.y = 0f;

        if (targetDirection.sqrMagnitude < 0.001f)
            return;

        targetDirection.Normalize();

        Vector3 offsetDirection =
            Quaternion.AngleAxis(TargetOffset, Vector3.up) * targetDirection;

        float singleStep = RotationSpeed * speed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(
            transform.forward,
            offsetDirection,
            singleStep,
            0f
        );

        float newAngle = Mathf.Atan2(newDirection.x, newDirection.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, newAngle, 0f);
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

    protected virtual void SetGrabPosition()
    {

    }

    public void StartMoveEnemyTowardsTarget(float speed)
    {
        isMovingToTarget = true;

        moveToPointSpeed = speed;
    }

    public void EndMoveEnemyTowardsTarget()
    {
        currentTarget = null;
        isMovingToTarget = false;
    }

    public void StartMoveEnemyTowardsDirection(float speed)
    {
        isMovingToDirection = true;

        moveToPointSpeed = speed;
    }

    public void EndMoveEnemyTowardsDirection()
    {
        currentTargetDirection = transform.forward;
        currentTargetDirection = new Vector3(currentTargetDirection.x, 0.0f, currentTargetDirection.z);
        currentTargetDirection.Normalize();

        isMovingToDirection = false;
    }

    public void StartMoveEnemyToPoint(float moveTime)
    {
        isMovingToPosition = true;

        moveToPointTime = moveTime;

        moveToPointTimer = 0f;

        startingPosition = transform.position;
    }

    public void EndMoveEnemyToPoint()
    {
        isMovingToPosition = false;
        targetPositionOffset = Vector2.zero;
        moveToPointTime = 0f;
        maxDistance = 1000f;
    }

    public void SetTargetToPlayer()
    {
        if(playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        currentTarget = playerTransform;
    }

    public void SetTargetDirectionToPlayer()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        currentTargetDirection = playerTransform.position - transform.position;
        currentTargetDirection = new Vector3(currentTargetDirection.x, 0.0f, currentTargetDirection.z);
        currentTargetDirection.Normalize();
    }

    public void SetTargetPointToPlayer()
    {
        if(playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        currentTargetPosition = playerTransform.position;

        currentTargetPosition = new Vector3(currentTargetPosition.x, transform.position.y, currentTargetPosition.z);

        Vector3 direction = currentTargetPosition - transform.position;

        // Clamp to max distance
        currentTargetPosition = transform.position + Vector3.ClampMagnitude(direction, maxDistance);
    }

    public void SetMaxDistanceToTargetPoint(float distance)
    {
        maxDistance = distance;
    }

    public void SetTargetPositionOffsetX(float x)
    {
        targetPositionOffset = new Vector2(x, targetPositionOffset.y);
    }

    public void SetTargetPositionOffsetZ(float z)
    {
        targetPositionOffset = new Vector2(targetPositionOffset.x, z);
    }

    public void SetTargetToDirectionX(float x)
    {
        currentTargetDirection = new Vector3(x, 0.0f, currentTargetDirection.z);

        currentTargetDirection.Normalize();
    }

    public void SetTargetToDirectionZ(float z)
    {
        currentTargetDirection = new Vector3(currentTargetDirection.x, 0.0f, z);

        currentTargetDirection.Normalize();
    }

    #endregion

    #region Animation Triggers

    public void Flinch()
    {
        if ((damageCDTimer <= 0.0f))
        {
            damageCounter++;

            SoundFXManager.instance.PlaySoundFXClip(damagedSFX, transform, 0.5f);

            _health.TakeDamage(1000);

            animator.Play("Flinch", 1, 0);

            damageCDTimer = damageCD;
        }
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

        ResetTargetOffset();
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

    public void SetTargetOffset(float offset)
    {
        TargetOffset = offset;
    }

    public void ResetTargetOffset()
    {
        TargetOffset = 0;
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

    public void ConfirmDeath()
    {
        GameEventManager.Instance.PlayerWon();
    }

    public void StartDeathSequence()
    {
        GameEventManager.Instance.BossDeath();
    }

    public Vector3 ReturnGrabPosition()
    {
        return grabPosition;
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

    #region SFX methods

    public void PlayFootstep()
    {
        SoundFXManager.instance.PlayRandomSoundFXClip(footstepsSFX, transform, 0.1f);
    }

    public void PlayWeaponSwing()
    {
        SoundFXManager.instance.PlayRandomSoundFXClip(bladeSwingSFX, transform, 0.5f);
    }

    public void PlayWeaponSlam()
    {
        SoundFXManager.instance.PlayRandomSoundFXClip(bladeSlamSFX, transform, 0.75f);
    }

    #endregion
}
