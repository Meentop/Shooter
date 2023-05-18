using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    [SerializeField] private float shootInterval;
    private float shootTimer;
    private bool canShoot = true;

    protected override void Update()
    {
        base.Update();
        if (!canShoot)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                canShoot = true;
                shootTimer = 0f;
            }
        }
    }
    public override void Shoot()
    {
        if (canShoot)
        {
            Instantiate(bullet, shotPoint.position, shotPoint.rotation);
            shootEffect.Play();

            canShoot = false;
            shootTimer = 0;
        }
        
    }

    public override void OnInit()
    {

    }

}
