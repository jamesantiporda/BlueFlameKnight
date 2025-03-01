using itsSALT.FinalCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public PlayerLocomotionInput _playerLocomotionInput;

    public GameObject settingsPanel;

    public GameObject enemyHP;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        settingsPanel.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerLocomotionInput.SettingsInput)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }

        if(settingsPanel.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
    }

    public void HideBossHP()
    {
        enemyHP.SetActive(false);
    }
}
