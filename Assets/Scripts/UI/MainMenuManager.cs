using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    public GameObject fadeFromBlack;
    public GameObject fadeToBlack;

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
        fadeFromBlack.SetActive(true);
        fadeToBlack.SetActive(false);

        StartCoroutine(DisableFadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartScene(int scene)
    {
        StartCoroutine(LoadSceneAfterFade(scene));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator LoadSceneAfterFade(int scene)
    {
        fadeToBlack.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(scene);
    }

    private IEnumerator DisableFadeIn()
    {
        yield return new WaitForSeconds(1.1f);

        fadeFromBlack.SetActive(false);
    }
}
