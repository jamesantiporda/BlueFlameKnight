using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageUI : MonoBehaviour
{
    public GameObject winText;
    public GameObject gameOverText;

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
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.OnPlayerDeath -= ShowGameOver;
        GameEventManager.Instance.OnPlayerWin -= ShowWin;
    }
}
