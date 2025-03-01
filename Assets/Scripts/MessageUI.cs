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

    private void Start()
    {
        gameOverText.SetActive(false);
        winText.SetActive(false);

        GameEventManager.Instance.OnPlayerDeath += ShowGameOver;
        GameEventManager.Instance.OnPlayerWin += ShowWin;
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
