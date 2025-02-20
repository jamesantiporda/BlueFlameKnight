using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    public HealthBar staminaBar;

    public int maxStamina = 100;
    public int currentStamina;
    public int staminaRegenRate = 2;

    private float staminaCooldown = 1f;
    private float staminaTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
        staminaBar.SetMaxHealth(maxStamina);
    }

    // Update is called once per frame
    void Update()
    {
        if(staminaTimer > staminaCooldown)
        {
            if(currentStamina < maxStamina)
            {
                currentStamina += staminaRegenRate;
                staminaBar.SetHealth(currentStamina);
            }
        }
        else
        {
            staminaTimer += Time.deltaTime;
        }
    }

    public void DecreaseStamina(int val)
    {
        currentStamina -= val;

        staminaBar.SetHealth(currentStamina);

        staminaTimer = 0.0f;
    }
}
