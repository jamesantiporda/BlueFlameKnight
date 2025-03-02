using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;

    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool CanSeePlayer { get; set; }

    public GameObject targetObject;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        float delay = 0.1f;

        WaitForSeconds wait = new WaitForSeconds(delay);

        while(true)
        {
            yield return wait;

            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    CanSeePlayer = true;
                    targetObject = target.gameObject;
                }
                else
                {
                    CanSeePlayer = false;
                    targetObject = null;
                }
            }
            else
            {
                CanSeePlayer = false;
                targetObject = null;
            }
        }
        else if (CanSeePlayer)
            CanSeePlayer = false;
    }

    private void Update()
    {
        //if(CanSeePlayer)
        //{
        //    Debug.Log("SEES ENEMY");
        //}
    }
}
