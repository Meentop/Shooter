using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BionicModule : Module
{
    [SerializeField] private List<BionicModuleBehaviour> behaviours;

    public PlayerMovement ApplyBehaviours(PlayerMovement movement, InfoForBionicModule info)
    {
        foreach (var behaviour in behaviours)
        {
            movement = behaviour.ApplyBehaviour(movement, info);
        }
        return movement;
    }

    public BionicModuleSave GetSave()
    {
        BionicModuleSave moduleSave = new BionicModuleSave
        {
            number = number,
            level = level
        };
        return moduleSave;
    }
}
