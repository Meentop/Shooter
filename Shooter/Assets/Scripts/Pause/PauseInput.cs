using UnityEngine;

public class PauseInput : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || (Input.GetKeyDown(KeyCode.Escape) && uiManager.ModulesPanel.PanelEnabled))
        {
            if (!pausePanel.activeInHierarchy)
            {
                uiManager.ModulesPanel.ToggleAndRefreshPanel();
                PauseManager.Pause = uiManager.ModulesPanel.PanelEnabled;
                uiManager.SelectadleUI.DisableAllSelectablesUI();
            }
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && settingsPanel.activeInHierarchy)
        {
            settingsPanel.SetActive(false);
            pausePanel.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && !uiManager.ModulesPanel.PanelEnabled)
        {
            PauseManager.Pause = !pausePanel.activeInHierarchy;
            pausePanel.SetActive(!pausePanel.activeInHierarchy);
            if (pausePanel.activeInHierarchy)
                cameraController.UnlockCursor();
            else
                cameraController.LockCursor();
            uiManager.SelectadleUI.DisableAllSelectablesUI();
        }
    }
    
    //ui button
    public void DisablePauseMenu()
    {
        PauseManager.Pause = false;
        pausePanel.SetActive(false);
        cameraController.LockCursor();
    }

    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        settingsPanel.SetActive(false);
    }
}
