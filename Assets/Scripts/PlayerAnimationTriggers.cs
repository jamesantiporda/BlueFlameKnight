using itsSALT.FinalCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-2)]
public class PlayerAnimationTriggers : MonoBehaviour
{
    public PlayerController _playerController;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAttack()
    {
        _animator.SetBool("isAttacking", true);
        _playerController.StartAttacking();
    }

    public void StopAttack()
    {
        _animator.SetBool("isAttacking", false);
        _playerController.StopAttacking();
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Attack")
        {
            WeaponToEnemy attack = other.gameObject.GetComponent<WeaponToEnemy>();
            Vector3 attackDirection;

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
            }
            else
            {
                attackDirection = transform.position - other.transform.position;
                attackDirection = new Vector3(attackDirection.x, 0.0f, attackDirection.z);
                attackDirection.Normalize();

                _playerController.knockbackDirection = attackDirection;

                ResetAnimationTriggers();

                _animator.SetInteger("DamageType", 1);
            }

            _animator.SetTrigger("Hit");

            _animator.SetBool("isRolling", false);
            _playerController.Damage();
        }
        else if(other.gameObject.tag == "Grab")
        {
            WeaponToEnemy attack = other.gameObject.GetComponent<WeaponToEnemy>();

            _playerController.Damage();
            ResetAnimationTriggers();

            _playerController.MakeUnmoveable();
            _playerController.LockInAnimation();

            _playerController.MovePlayer(Vector3.zero, 0f);


            // HARD CODE GRAB POSITION
            _playerController.SnapTowardsTarget(attack.enemy.transform.position);

            Vector3 new_pos = attack.enemy.transform.position + attack.enemy.transform.forward * 0.9f;

            Vector3 new_player_pos = new Vector3(new_pos.x, _playerController.transform.position.y, new_pos.z);

            //new_player_pos += -_playerController.transform.right;

            _playerController.WarpToPosition(new_player_pos);
            _playerController.ForceLockOnDisable();

            _animator.SetTrigger("Grabbed");
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
}
