using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class PauseInput : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private ModulesPanelUI modules;
    [SerializeField] private GameObject pausePanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || (Input.GetKeyDown(KeyCode.Escape) && modules.PanelEnabled))
        {
            modules.SetEnablePanel();
            PauseManager.Pause = modules.PanelEnabled;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && !modules.PanelEnabled)
        {
            PauseManager.Pause = !pausePanel.activeInHierarchy;
            pausePanel.SetActive(!pausePanel.activeInHierarchy);
            if (pausePanel.activeInHierarchy)
                cameraController.UnlockCursor();
            else
                cameraController.LockCursor();
        }
    }
    
    public void DisablePauseMenu()
    {
        PauseManager.Pause = false;
        pausePanel.SetActive(false);
        cameraController.LockCursor();
    }
}
