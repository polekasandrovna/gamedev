using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour
{
    private void Awake()
    {
        HunterScript.onDie += RestartGame;
        Shooting.runOutOfAmmo += RestartGame;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    private void OnDisable()
    {
        HunterScript.onDie -= RestartGame;
    }
}
