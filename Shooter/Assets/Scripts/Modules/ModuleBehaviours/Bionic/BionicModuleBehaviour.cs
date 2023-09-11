using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BionicModuleBehaviour : ModuleBehaviour
{
    public abstract PlayerCharacteristics ApplyBehaviour(PlayerCharacteristics movement, InfoForBionicModule info);
}
