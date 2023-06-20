using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeModifierHolderUI : ModifierHolderUI
{
    [SerializeField] private GameObject backlight;
    [SerializeField] private Transform content;

    public override void OnEnter()
    {
        backlight.SetActive(true);
    }

    public override void OnExit()
    {
        backlight.SetActive(false);
    }

    public void SetFreeModifierHolderSize(int modifierCount, RectTransform modifier)
    {
        float sizeY = modifierCount * modifier.sizeDelta.y;
        sizeY += 5 * (modifierCount - 1);
        RectTransform rectTransform = content.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, sizeY);
    }

    public override void SetPosition(Transform modifier)
    {
        modifier.SetParent(content);
    }

    public override void AddModifier(ModifierUI modifier)
    {
        player.AddFreeModifier(modifier.modifier);
        SetFreeModifierHolderSize(content.childCount, modifier.GetComponent<RectTransform>());
    }

    public override bool CanAddModifier()
    {
        return true;
    }

    public override void RemoveModifier(ModifierUI modifier)
    {
        player.RemoveFreeModifier(modifier.modifier);
        SetFreeModifierHolderSize(content.childCount, modifier.GetComponent<RectTransform>());
    }
}
