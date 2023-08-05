using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BionicModuleHolderUI : ModuleHolderUI
{
    [SerializeField] private Color activeColor, unactiveColor;
    private Image _image;
    private Player _player;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    public void Init(Player player)
    {
        _player = player;
    }

    public override void OnEnter()
    {
        _image.color = activeColor;
    }

    public override void OnExit()
    {
        _image.color = unactiveColor;
    }

    public override void SetPosition(Transform module)
    {
        module.SetParent(transform);
        module.position = transform.position;
    }

    public override void AddModule(ModuleUI module)
    {
        _player.AddInstalledBionicModule(module.Module as BionicModule);
    }

    public override bool CanAddModule(ModuleUI module)
    {
        return transform.childCount == 0 && module.Module.GetType() == typeof(BionicModule);
    }

    public override void RemoveModule(ModuleUI module)
    {
        _player.RemoveInstalledBionicModule(module.Module as BionicModule);
    }
}
