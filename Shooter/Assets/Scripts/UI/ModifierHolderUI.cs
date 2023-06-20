using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModifierHolderUI : MonoBehaviour
{
    protected Transform modifierDragHolder;
    protected Player player;

    public void Init(Transform modifierDragHolder, Player player)
    {
        this.modifierDragHolder = modifierDragHolder;
        this.player = player;
    }

    public abstract void OnEnter();

    public abstract void OnExit();

    public abstract void SetPosition(Transform modifier);

    public abstract void AddModifier(ModifierUI modifier);

    public abstract void RemoveModifier(ModifierUI modifier);

    public abstract bool CanAddModifier();
}
