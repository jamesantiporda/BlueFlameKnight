using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageUI : MonoBehaviour
{
    public GameObject winText;
    public GameObject gameOverText;
    public MusicManager musicManager;
    public GameObject deathScreen;

    private float messageTime = 2.5f;
    private float messageTimer = 0.0f;

    private void Start()
    {
        messageTimer = 0.0f;

        gameOverText.SetActive(false);
        winText.SetActive(false);

        GameEventManager.Instance.OnPlayerDeath += ShowGameOver;
        GameEventManager.Instance.OnPlayerWin += ShowWin;
    }

    private void Update()
    {
        if (gameOverText.activeSelf || winText.activeSelf)
        {
            messageTimer += Time.deltaTime;

            if(messageTimer > messageTime && !deathScreen.activeSelf)
            {
                DisableMusic();
                deathScreen.SetActive(true);
            }
        }
    }

    private void ShowGameOver()
    {
        gameOverText.SetActive(true);
    }

    private void ShowWin()
    {
        winText.SetActive(true);

        UIManager.instance.HideBossHP();

        DisableMusic();
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.OnPlayerDeath -= ShowGameOver;
        GameEventManager.Instance.OnPlayerWin -= ShowWin;
    }

    public void DisableMusic()
    {
        musicManager.FadeOut();
    }
}
