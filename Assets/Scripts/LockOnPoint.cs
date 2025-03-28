using itsSALT.FinalCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockOnPoint : MonoBehaviour
{
    private GameObject target;

    public PlayerLocomotionInput playerLocomotionInput;

    public GameObject point;

    public PlayerController playerController;

    private Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        target = null;
        enemy = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerLocomotionInput.LockToggledOn && !playerController.IsDead)
        {
            if(playerController.ReturnTargetEnemy() != null)
            {
                target = playerController.ReturnTargetEnemy();

                enemy = target.GetComponent<ComponentToEnemy>().enemy;

                point.SetActive(true);
                transform.position = Camera.main.WorldToScreenPoint(target.GetComponent<ComponentToEnemy>().lockOnPointUI.position);
            }
        }
        else
        {
            target = null;
            enemy = null;

            point.SetActive(false);
        }
    }
}
