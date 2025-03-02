using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionApplier : MonoBehaviour
{
    public Animator animator;
    public Transform rootBone; // Assign the root bone manually

    private Vector3 previousRootPosition;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (rootBone != null)
            previousRootPosition = rootBone.position;
    }

    void OnAnimatorMove()
    {
        if (animator && rootBone)
        {
            // Get the root motion delta
            Vector3 deltaPosition = rootBone.position - previousRootPosition;
            deltaPosition.y = 0; // Optional: Remove vertical motion if not needed

            // Apply it to the character's transform
            transform.position += deltaPosition;

            // Store the new position for the next frame
            previousRootPosition = rootBone.position;
        }
    }
}
