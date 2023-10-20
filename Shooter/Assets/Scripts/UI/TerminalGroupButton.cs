using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalGroupButton : MonoBehaviour
{
    [SerializeField] private TerminalButtonType type;

    [SerializeField] private TerminalUI terminal;

    public void SetGroup()
    {
        terminal.SetGroup(type);
    }
}
