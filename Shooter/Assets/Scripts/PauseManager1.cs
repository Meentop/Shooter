using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager1 : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Settings settingsPanel;
    public void ShowSettings()
    {
        settingsPanel.gameObject.SetActive(true);
        pauseMenu.SetActive(false);
    }
    public void BackToMenu()
    {
        settingsPanel.gameObject.SetActive(false);
        pauseMenu.SetActive(true);
    }
}
