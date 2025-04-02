using itsSALT.FinalCharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;

    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool CanSeePlayer { get; private set; }
    public GameObject targetObject;

    public PlayerLocomotionInput playerLocomotionInput;

    public float targetSwitchThreshold = 60f;

    private Collider[] rangeChecks = new Collider[10]; // Preallocate array to avoid GC
    private const float fovCheckInterval = 0.2f; // How often to check FOV

    private float targetSwitchCooldown = 0.1f;
    private float targetSwitchTimer = 0f;


    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(fovCheckInterval);

        while (true)
        {
            yield return wait;

            if (!playerLocomotionInput.LockToggledOn)
            {
                FieldOfViewCheck();
            }
        }
    }

    private void FieldOfViewCheck()
    {
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, radius, rangeChecks, targetMask);

        if (numColliders == 0)
        {
            CanSeePlayer = false;
            targetObject = null;
            return;
        }

        List<Transform> seenTargets = new List<Transform>();
        Transform target = null;

        float minDistance = Mathf.Infinity;
        int targetIndex = 0;

        for (int i = 0; i < numColliders; i++)
        {
            Transform potentialTarget = rangeChecks[i].transform;
            Vector3 directionToTarget = (potentialTarget.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, potentialTarget.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    seenTargets.Add(potentialTarget);
                    if(distanceToTarget < minDistance)
                    {
                        minDistance = distanceToTarget;
                        targetIndex = i;
                    }
                }
            }
        }

        Debug.Log("Target to lock: " + targetIndex);

        target = rangeChecks[targetIndex].transform;

        if (seenTargets.Count == 0)
        {
            CanSeePlayer = false;
            targetObject = null;
            return;
        }

        // Find best target based on priority
        /*
        Transform target = null;
        float maxPriority = float.MinValue;

        foreach (var t in seenTargets)
        {
            float targetDistance = Vector3.Distance(transform.position, t.position);
            float distancePrio = 1 / Mathf.Max(targetDistance, 1); // Prevent divide by zero

            Vector3 targetDirection = (t.position - transform.position).normalized;
            float dotPrio = Vector3.Dot(transform.forward, targetDirection);

            float priority = (dotPrio * 0.90f) + (distancePrio * 0.10f);

            if (priority > maxPriority)
            {
                target = t;
                maxPriority = priority;
            }
        }
        */

        targetObject = target != null ? target.gameObject : null;
        CanSeePlayer = targetObject != null;
    }

    private void SwitchTarget(bool right)
    {
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, radius, rangeChecks, targetMask);

        if (numColliders == 0)
        {
            CanSeePlayer = false;
            targetObject = null;
            return;
        }

        List<Transform> seenTargets = new List<Transform>();

        for (int i = 0; i < numColliders; i++)
        {
            Transform potentialTarget = rangeChecks[i].transform;
            Vector3 directionToTarget = (potentialTarget.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, potentialTarget.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    seenTargets.Add(potentialTarget);
                }
            }
        }

        if (seenTargets.Count == 0)
        {
            CanSeePlayer = false;
            targetObject = null;
            return;
        }

        Debug.Log("targets: " + seenTargets.Count);

        Dictionary<float, Transform> targetMap = new Dictionary<float, Transform>();

        float[] signedDots = new float[seenTargets.Count];

        // Get signed dot products of targets
        for(int i = 0; i < seenTargets.Count; i++)
        {
            Vector3 targetDirection = (seenTargets[i].position - transform.position).normalized;
            float dotForward = Vector3.Dot(transform.forward, targetDirection);
            float dotRight = Vector3.Dot(transform.right, targetDirection);

            float signedDot = dotRight;

            signedDots[i] = signedDot;

            //Debug.Log("target " + i + ": " + signedDot);

            targetMap.Add(signedDot, seenTargets[i]);
        }

        Debug.Log("Signed dots: " + signedDots.Length);

        Array.Sort(signedDots);

        int currentTargetIndex = 0;

        for(int i = 0; i < signedDots.Length; i++)
        {
            Debug.Log("target " + i + ": " + signedDots[i]);

            if (targetMap[signedDots[i]] == targetObject.transform)
            {
                currentTargetIndex = i;
            }
        }

        Debug.Log("target index: " + currentTargetIndex);

        if(right && currentTargetIndex < signedDots.Length - 1)
        {
            targetObject = targetMap[signedDots[currentTargetIndex + 1]].gameObject;
        }
        else if(!right && currentTargetIndex > 0)
        {
            targetObject = targetMap[signedDots[currentTargetIndex - 1]].gameObject;
        }

    }

    private void Update()
    {
        //if(CanSeePlayer)
        //{
        //    Debug.Log("SEES ENEMY");
        //}
        

        if(targetSwitchTimer <= 0f)
        {
            if (playerLocomotionInput.LookInput.magnitude >= targetSwitchThreshold && playerLocomotionInput.LockToggledOn)
            {
                Debug.Log("Switch input: " + playerLocomotionInput.LookInput.x);
                targetSwitchTimer = targetSwitchCooldown;
                SwitchTarget(playerLocomotionInput.LookInput.x >= 0);
            }
        }
        else
        {
            targetSwitchTimer -= Time.deltaTime;
        }
    }
}
