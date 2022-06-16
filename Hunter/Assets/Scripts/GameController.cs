using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private int rabbitCount = 3;
    [SerializeField] private int wolfCount = 3;
    [SerializeField] private int deerCount = 3;

    private static GameController gameController;
    private void Awake()
    {
        SerializeSingelton();
    }

    private void SerializeSingelton()
    {
        if (gameController != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            gameController = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public int RabbitCount
    {
        get { return rabbitCount; }
        set { rabbitCount = value; }
    }
    public int WolfCount
    {
        get { return wolfCount; }
        set { wolfCount = value; }
    }
    public int DeerCount
    {
        get { return deerCount; }
        set { deerCount = value; }
    }
}
