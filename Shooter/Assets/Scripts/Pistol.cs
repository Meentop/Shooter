using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public override void Shoot()
    {
        if (CurrentAmmo > 0)
        {
            Instantiate(bullet, shotPoint.position, shotPoint.rotation);
            shootEffect.Play();
            SpendAmmo(ammoPerShoot);
        }

        if (CurrentAmmo == 0)
            Reload();
    }

    public override void OnInit()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
}
