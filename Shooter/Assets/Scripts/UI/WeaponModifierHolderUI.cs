using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponModifierHolderUI : ModifierHolderUI
{
    [SerializeField] private Color activeColor, unactiveColor;
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    public override void OnEnter()
    {
        _image.color = activeColor;
    }

    public override void OnExit()
    {
        _image.color = unactiveColor;
    }
}
