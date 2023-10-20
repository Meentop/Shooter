using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalButton : MonoBehaviour
{
    [SerializeField] private Text text;

    private TerminalButtonType type;
    private Terminal terminal;
    private int index;

    public void Init(TerminalButtonType type, Terminal terminal, int index, string name)
    {
        this.type = type;
        this.terminal = terminal;
        this.index = index;
        text.text = name;
    }

    public void Apply()
    {
        terminal.ApplyTerminalButton(type, index);
    }
}
