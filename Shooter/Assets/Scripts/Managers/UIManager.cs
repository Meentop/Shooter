using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image activeSkillReloadImage;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private InfoInterface infoInterface;
    [SerializeField] private ModulesPanelUI modulesPanel;
    [SerializeField] private SelectableUI selectadleUI;
    [SerializeField] private TerminalUI terminalUI;
    [SerializeField] private Settings settings;

    public InfoInterface InfoInterface { get { return infoInterface; } private set { infoInterface = value; } }
    public ModulesPanelUI ModulesPanel { get { return modulesPanel; } private set { modulesPanel = value; } }    
    public SelectableUI SelectadleUI { get { return selectadleUI; } private set { selectadleUI = value; } }

    public TerminalUI TerminalUI { get { return terminalUI; } private set { terminalUI = value; } }

    public Image GetActiveSkillReloadImage() => activeSkillReloadImage;

    public void SetActiveDeathPanel()
    {
        deathPanel.SetActive(true);
    }

    private void Start()
    {
        settings.StartVolume();
    }
}
