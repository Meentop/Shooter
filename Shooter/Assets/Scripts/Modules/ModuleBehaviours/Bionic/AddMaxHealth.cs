using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMaxHealth : BionicModuleBehaviour
{
    [SerializeField] private int[] addMaxHealth;

    public override PlayerCharacteristics ApplyBehaviour(PlayerCharacteristics characteristics, InfoForBionicModule info)
    {
        characteristics.maxHealth += addMaxHealth[info.lvl];
        return characteristics;
    }

    public override string GetDescription(int lvl)
    {
        return $"+{addMaxHealth[lvl]} max health";
    }
}
