using itsSALT.FinalCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
