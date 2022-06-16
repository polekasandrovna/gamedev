using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private TMP_InputField rabbitsInputField;
    [SerializeField] private TMP_InputField wolfsInputField;
    [SerializeField] private TMP_InputField deerInputField;

    public void SetRabbitsCount()
    {
        int.TryParse(rabbitsInputField.text, out int result);
        FindObjectOfType<GameController>().RabbitCount = result;
    }
    
    public void SetWolfsCount()
    {
        int.TryParse(wolfsInputField.text, out int result);
        FindObjectOfType<GameController>().WolfCount = result;
    }
    
    public void SetDeerCount()
    {
        int.TryParse(deerInputField.text, out int result);
        FindObjectOfType<GameController>().DeerCount = result;
    }

    public void BackToMenu()
    {
        settingsCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }
}
