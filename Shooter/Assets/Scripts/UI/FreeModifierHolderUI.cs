using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeModifierHolderUI : ModifierHolderUI
{
    [SerializeField] private GameObject backlight;

    public override void OnEnter()
    {
        backlight.SetActive(true);
    }

    public override void OnExit()
    {
        backlight.SetActive(false);
    }
}
