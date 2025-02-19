using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BFKRangedAttack : MonoBehaviour
{
    public float lifetime = 5f;
    public float speed = 5f;
    private float timer;

    private GameObject target;
    private Vector3 targetDirection = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        target = targets[0];

        targetDirection = (target.transform.position + Vector3.up * 0.25f) - transform.position;

        targetDirection = targetDirection.normalized;

        transform.LookAt(target.transform.position);

        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += targetDirection * speed * Time.deltaTime;

        timer += Time.deltaTime;

        if (timer > lifetime)
        {
            Destroy(gameObject);
        }
    }
}
