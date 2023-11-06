using System.Collections;
using UnityEngine;

public class SemiAutomatic : Weapon
{
    public override void Shoot()
    {
        base.Shoot();
        RaycastShoot(Camera.main.transform.forward);
    }

    public override void OnInit()
    {

    }

    public override void StopShooting()
    {
        
    }
}
