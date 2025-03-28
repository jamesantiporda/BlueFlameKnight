using itsSALT.FinalCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-2)]
public class PlayerAnimationTriggers : MonoBehaviour
{
    public PlayerController _playerController;
    private Animator _animator;
    [SerializeField] private Flask flask;

    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private AudioClip rollSFX;
    [SerializeField] private AudioClip[] slashClips;
    [SerializeField] private AudioClip playerDamagedSFX;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseUpFlask()
    {
        flask.UseFlask();
    }

    public void StartAttack()
    {
        _animator.SetBool("isAttacking", true);
        _playerController.StartAttacking();
    }

    public void StopAttack()
    {
        _animator.SetBool("isAttacking", false);
        ResetAnimationTriggers();
        _playerController.StopAttacking();
    }

    public void StartDrinking()
    {
        _playerController.StartDrinking();
    }

    public void StopDrinking()
    {
        _playerController.StopDrinking();
    }

    public void StartMovingAttacking()
    {
        _playerController.StartMovingWhileAttacking();
    }

    public void StopMovingAttacking()
    {
        _playerController.StopMovingWhileAttacking();
    }

    public void StartRoll()
    {
        _playerController.StartMovingWhileRolling();
    }

    public void StopRoll()
    {
        _playerController.StopMovingWhileRolling();
    }

    public void StartFlinch()
    {
        _playerController.knockbackSpeed = 1.0f;
        _playerController.StartKnockback();
    }

    public void StartLaunchBack()
    {
        _playerController.knockbackSpeed = 3.5f;
        _playerController.StartKnockback();
    }

    public void StopKnockback()
    {
        _playerController.StopKnockback();
    }

    public void LockPlayer()
    {
        _playerController.LockInAnimation();
    }

    public void UnlockPlayer()
    {
        _playerController.UnlockOutOfAnimation();
    }

    public void Moveable()
    {
        _playerController.MakeMoveable();
    }

    public void Unmoveable()
    {
        _playerController.MakeUnmoveable();
    }

    public void TakeDamage(int damage)
    {
        _playerController.TakeDamage(damage);
    }

    public void StopGrab()
    {
        _animator.SetBool("isGrabbed", false);
    }

    public void ShowDeathMessage()
    {
        GameEventManager.Instance.PlayerDied();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Attack" && !_playerController.IsDead)
        {
            WeaponToEnemy attack = other.gameObject.GetComponent<WeaponToEnemy>();
            Vector3 attackDirection;

            int damage = 500;

            if (attack != null)
            {
                attackDirection = transform.position - attack.enemy.transform.position;
                attackDirection = new Vector3(attackDirection.x, 0.0f, attackDirection.z);
                attackDirection.Normalize();

                _playerController.knockbackDirection = attackDirection;

                ResetAnimationTriggers();

                if (attack.ReturnAttackType() == Enemy.Attack.Normal)
                {
                    _animator.SetInteger("DamageType", 0);
                }
                else if (attack.ReturnAttackType() == Enemy.Attack.Knockback)
                {
                    _animator.SetInteger("DamageType", 1);
                }
                else if (attack.ReturnAttackType() == Enemy.Attack.Launch)
                {
                    _animator.SetInteger("DamageType", 2);
                }
                else
                {
                    _animator.SetInteger("DamageType", 0);
                }

                damage = attack.enemy.CurrentAttackDamage;
            }
            else
            {
                attackDirection = transform.position - other.transform.position;
                attackDirection = new Vector3(attackDirection.x, 0.0f, attackDirection.z);
                attackDirection.Normalize();

                BFKRangedAttack bfkRangedAttack = other.gameObject.GetComponent<BFKRangedAttack>();

                if(bfkRangedAttack != null)
                {
                    attackDirection = bfkRangedAttack.ReturnAttackDirection();
                    attackDirection.Normalize();
                }

                _playerController.knockbackDirection = attackDirection;

                ResetAnimationTriggers();

                _animator.SetInteger("DamageType", 1);
            }

            _animator.SetTrigger("Hit");

            TakeDamage(damage);

            _animator.SetBool("isRolling", false);
            _playerController.Damage();
        }
        else if(other.gameObject.tag == "Grab" && !_playerController.IsDead)
        {
            WeaponToEnemy attack = other.gameObject.GetComponent<WeaponToEnemy>();

            _playerController.Damage();
            ResetAnimationTriggers();

            _playerController.MakeUnmoveable();
            _playerController.LockInAnimation();

            _playerController.MovePlayer(Vector3.zero, 0f);

            // SET PLAYER POSITION
            _playerController.SnapTowardsTarget(attack.enemy.transform.position);

            Vector3 new_pos = attack.enemy.ReturnGrabPosition();

            Vector3 new_player_pos = new Vector3(new_pos.x, _playerController.transform.position.y, new_pos.z);

            //new_player_pos += -_playerController.transform.right;

            _playerController.WarpToPosition(new_player_pos);

            _playerController.SnapTowardsTarget(attack.enemy.transform.position);

            _playerController.ForceLockOnDisable();

            _animator.SetTrigger("Grabbed");
            _animator.SetBool("isGrabbed", true);
            attack.enemy.animator.SetTrigger("Grab");
        }
    }

    private void ResetAnimationTriggers()
    {
        foreach (var param in _animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                _animator.ResetTrigger(param.name);
            }
        }
    }

    public void PlayFootstep()
    {
        if(_playerController.IsMoving())
        {
            SoundFXManager.instance.PlayRandomSoundFXClip(footstepClips, transform, 0.5f);
        }
    }

    public void PlayRollSFX()
    {
        SoundFXManager.instance.PlaySoundFXClip2D(rollSFX, transform, 0.5f);
    }

    public void PlaySlashSFX()
    {
        SoundFXManager.instance.PlayRandomSoundFXClip(slashClips, transform, 0.5f);
    }

    public void PlayDamageSound()
    {
        SoundFXManager.instance.PlaySoundFXClip(playerDamagedSFX, transform, 0.5f);
    }

    public void DecreaseStamina(int val)
    {
        _playerController.DecreaseStamina(val);
    }
}
