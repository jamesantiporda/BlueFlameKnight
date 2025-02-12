using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponToEnemy : MonoBehaviour
{
    public Enemy enemy;

    public Vector2 grabPositionScaling;

    private Vector3 grabPosition;

    // Start is called before the first frame update
    void Start()
    {
        grabPosition = enemy.transform.position + enemy.transform.forward * grabPositionScaling.y + enemy.transform.right * grabPositionScaling.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Enemy.Attack ReturnAttackType()
    {
        return enemy.ReturnAttackType();
    }

    public Vector3 ReturnGrabPosition()
    {
        return grabPosition;
    }
}
