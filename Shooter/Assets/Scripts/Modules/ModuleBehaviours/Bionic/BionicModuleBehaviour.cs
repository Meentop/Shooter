using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BionicModuleBehaviour : ModuleBehaviour
{
    public abstract PlayerMovement ApplyBehaviour(PlayerMovement movement, InfoForBionicModule info);
}
