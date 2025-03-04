using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public static DeathScreen instance;

    public GameObject prompts;
    public float promptsTime = 1.5f;
    
    private float timer = 0.0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        prompts.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= promptsTime)
        {
            prompts.SetActive(true);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
