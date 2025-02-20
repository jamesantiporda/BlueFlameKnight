using itsSALT.FinalCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Flask : MonoBehaviour
{
    public Slider fill;
    public TMP_Text flasksText;
    public int maxFlasks = 10;
    public PlayerCombatInput _playerCombatInput;
    private int currentFlasks = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentFlasks = maxFlasks;

        fill.maxValue = maxFlasks;
        fill.value = maxFlasks;
        flasksText.text = "" + maxFlasks;
    }

    private void Update()
    {
        if(_playerCombatInput.FlaskInput)
        {
            UseFlask();
        }
    }

    public void UseFlask()
    {
        if(currentFlasks > 0)
        {
            currentFlasks--;

            fill.value = currentFlasks;
            flasksText.text = "" + currentFlasks;
        }
    }
}
