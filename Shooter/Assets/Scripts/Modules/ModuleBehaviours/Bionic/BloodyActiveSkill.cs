using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodyActiveSkill : BionicModuleBehaviour
{
    [SerializeField] private int[] bloodPrice;

    public override PlayerCharacteristics ApplyBehaviour(PlayerCharacteristics characteristics, InfoForBionicModule info)
    {
        characteristics.bloodyActiveSkill = true;
        characteristics.activeSkillBloodyPrice = bloodPrice[info.lvl];
        return characteristics;
    }

    public override string GetDescription(int lvl)
    {
        return $"The active skill doesn't have a reload, but it costs {bloodPrice[lvl]} health";
    }
}
