using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeModulesHolderUI : ModuleHolderUI
{
    [SerializeField] private GameObject backlight;
    [SerializeField] private Transform content;
    [SerializeField] private string moduleType;

    public override void OnEnter()
    {
        backlight.SetActive(true);
    }

    public override void OnExit()
    {
        backlight.SetActive(false);
    }

    public void SetFreeModulesHolderSize(int moduleCount, RectTransform module)
    {
        float sizeY = moduleCount * module.sizeDelta.y;
        sizeY += 5 * (moduleCount - 1);
        RectTransform rectTransform = content.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, sizeY);
    }

    public override void SetPosition(Transform module)
    {
        module.SetParent(content);
    }

    public override void AddModule(ModuleUI module)
    {
        player.AddFreeModule(module.Module);
        SetFreeModulesHolderSize(content.childCount, module.GetComponent<RectTransform>());
    }

    public override bool CanAddModule(ModuleUI module)
    {
        return module.Module.GetType() == Type.GetType(moduleType);
    }

    public override void RemoveModule(ModuleUI module)
    {
        player.RemoveFreeModule(module.Module);
        SetFreeModulesHolderSize(content.childCount, module.GetComponent<RectTransform>());
    }

    public Transform GetContent() => content;

}
