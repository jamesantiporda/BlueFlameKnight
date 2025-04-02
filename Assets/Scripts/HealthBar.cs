using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public Slider yellowBar;

    public int yellowShowThreshold = 1000;

    public int yellowCatchupRate = 2000;

    private float yellowCatchupTime = 0.5f;

    private float yellowTimer = 0f;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        yellowBar.maxValue = health;
        yellowBar.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        if(yellowBar.value - slider.value < yellowShowThreshold)
        {
            yellowBar.value = slider.value;
        }

        yellowTimer = 0f;
    }

    public void SetBothRedAndYellow(int val)
    {
        slider.value = val;
        yellowBar.value = val;
    }

    public void AddToBothRedAndYellow(int val)
    {
        slider.value += val;
        yellowBar.value += val;
    }

    private void Update()
    {
        if(yellowBar.value > slider.value)
        {
            yellowTimer += Time.deltaTime;

            if(yellowTimer > yellowCatchupTime)
            {
                yellowBar.value -= yellowCatchupRate;
            }
        }
    }
}
