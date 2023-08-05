using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusDashCharge : BionicModuleBehaviour
{
    [SerializeField] private float addDashReload;

    public override PlayerMovement ApplyBehaviour(PlayerMovement movement, InfoForBionicModule info)
    {
        movement.dashCharges += 1;
        movement.dashReloadTime += addDashReload;
        return movement;
    }
}
