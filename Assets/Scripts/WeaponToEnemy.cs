using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponToEnemy : MonoBehaviour
{
    public Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Enemy.Attack ReturnAttackType()
    {
        return enemy.ReturnAttackType();
    }
}
