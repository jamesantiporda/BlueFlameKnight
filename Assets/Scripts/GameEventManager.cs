using itsSALT.FinalCharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance { get; private set; }

    public event Action OnPlayerDeath;
    public event Action OnPlayerWin;

    public PlayerLocomotionInput playerLocomotionInput;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayerDied()
    {
        OnPlayerDeath?.Invoke();
    }

    public void PlayerWon()
    {
        OnPlayerWin?.Invoke();

        playerLocomotionInput.ForceDisableLock();
    }
}
