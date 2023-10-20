using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TerminalUI : MonoBehaviour
{
    [SerializeField] private GameObject terminalPanel;
    [SerializeField] private Transform terminalButtonHolder;
    [SerializeField] private TerminalButton terminalButton;

    private Terminal terminal;

    public void Init(Terminal terminal)
    {
        this.terminal = terminal;
    }

    public void SetActiveTerminalPanel(bool active)
    {
        terminalPanel.SetActive(active);
    }

    public void SetGroup(TerminalButtonType type)
    {
        foreach (Transform item in terminalButtonHolder)
        {
            Destroy(item.gameObject);
        }
        List<string> names = terminal.GetButtonsName(type);
        for (int i = 0; i < names.Count; i++)
        {
            TerminalButton button = Instantiate(terminalButton, terminalButtonHolder);
            button.Init(type, terminal, i, names[i]);
        }
        RectTransform buttonHolderRect = terminalButtonHolder.GetComponent<RectTransform>();
        buttonHolderRect.sizeDelta = new Vector2(buttonHolderRect.sizeDelta.x, (terminalButton.GetComponent<RectTransform>().sizeDelta.y * names.Count) + (5 * names.Count - 1));
    }
}
