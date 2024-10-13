using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField]
    private float speed = 1f;

    PlayerInput playerInput;
    InputAction moveAction;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Debug.Log(moveAction.ReadValue<Vector2>());

        Vector2 direction = moveAction.ReadValue<Vector2>();
        Vector2 move;

        //direction.Normalize();

        move = direction * speed;

        //controller.velocity = new Vector3(move.x, 0.0f, move.y);
    }
}
