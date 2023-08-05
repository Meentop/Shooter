using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModuleHolderUI : MonoBehaviour
{
    protected Transform dragModuleHolder;
    protected Player player;

    public void Init(Transform dragModuleHolder, Player player)
    {
        this.dragModuleHolder = dragModuleHolder;
        this.player = player;
    }

    public abstract void OnEnter();

    public abstract void OnExit();

    public abstract void SetPosition(Transform module);

    public abstract void AddModule(ModuleUI module);

    public abstract void RemoveModule(ModuleUI module);

    public abstract bool CanAddModule(ModuleUI module);
}
