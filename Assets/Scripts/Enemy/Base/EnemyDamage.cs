using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerAttack")
        {
            Debug.Log("hit!");
            enemy.Flinch();
        }

        if (other.gameObject.tag == "Player")
        {
            //enemy.SetIsTouchingPlayer(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //enemy.SetIsTouchingPlayer(false);
        }
    }
}
