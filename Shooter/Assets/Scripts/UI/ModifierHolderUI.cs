using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModifierHolderUI : MonoBehaviour
{
    private Transform _modifierDragHolder;
    private ModifiersManager _modifiersManager;

    public void Init(Transform modifierDragHolder, ModifiersManager modifiersManager)
    {
        _modifierDragHolder = modifierDragHolder;
        _modifiersManager = modifiersManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (_modifierDragHolder.childCount > 0 && _modifierDragHolder.GetChild(0) == other.transform)
        {
            if (_modifiersManager.activeHolder != null)
            {
                _modifiersManager.activeHolder.OnExit();
            }
            _modifiersManager.activeHolder = this;
            OnEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_modifierDragHolder.childCount > 0 && _modifierDragHolder.GetChild(0) == other.transform)
        {
            OnExit();
            if (_modifiersManager.activeHolder == this)
                _modifiersManager.activeHolder = null;
        }
    }

    public abstract void OnEnter();

    public abstract void OnExit();
}
