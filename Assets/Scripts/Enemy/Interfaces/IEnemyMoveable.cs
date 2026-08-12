using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMoveable
{
    Rigidbody RB { get; set; }

    CharacterController characterController { get; set; }

    void MoveEnemy(Vector3 velocity);
}
