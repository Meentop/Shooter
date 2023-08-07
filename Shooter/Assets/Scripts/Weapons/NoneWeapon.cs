using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneWeapon : Weapon
{
    public override void OnInit()
    {
        maxNumberOfModules = 0;
    }

    public override void Shoot()
    {
        
    }

    public override void StopShooting()
    {
        
    }
}
