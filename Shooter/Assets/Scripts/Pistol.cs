using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public override void Shoot()
    {
        Instantiate(bullet, shotPoint.position, shotPoint.rotation);
        shootEffect.Play();
    }

    public override void OnInit()
    {

    }

}
