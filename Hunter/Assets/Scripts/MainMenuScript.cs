using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private int waitingTime = 2;
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject settingsCanvas;

    private void Awake()
    {
        mainMenuCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
    }
    public void Play()
    {
        StartCoroutine(StartScene(1));
    }
    public void ShowSettings()
    {
        mainMenuCanvas.SetActive(false);
        settingsCanvas.SetActive(true);
    }
    public void Exit()
    {
        Application.Quit();
    }
    IEnumerator StartScene(int sceneIndex)
    {
        yield return new WaitForSeconds(waitingTime);
        SceneManager.LoadScene(sceneIndex);
    }
}
