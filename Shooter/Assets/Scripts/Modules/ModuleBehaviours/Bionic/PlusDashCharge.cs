using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusDashCharge : BionicModuleBehaviour
{
    [SerializeField] private float[] addDashReload;

    public override PlayerMovement ApplyBehaviour(PlayerMovement movement, InfoForBionicModule info)
    {
        movement.dashCharges += 1;
        movement.dashReloadTime += addDashReload[info.lvl];
        return movement;
    }

    public override string GetDescription(int lvl)
    {
        return $"Plus dash charge, but +{addDashReload[lvl]}s dash recharge time";
    }
}
