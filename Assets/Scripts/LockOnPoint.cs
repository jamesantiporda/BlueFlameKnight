using itsSALT.FinalCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockOnPoint : MonoBehaviour
{
    public GameObject target;

    public PlayerLocomotionInput playerLocomotionInput;

    public GameObject point;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(playerLocomotionInput.LockToggledOn)
        {
            point.SetActive(true);
            transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
        }
        else
        {
            point.SetActive(false);
        }
    }
}
