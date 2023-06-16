using UnityEngine;

public class Pistol : Weapon
{
    private float shootTimer;
    private bool canShoot = true;

    protected override void Update()
    {
        base.Update();
        if (!Pause.pause && !canShoot)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= firingSpeed)
            {
                canShoot = true;
                shootTimer = 0f;
            }
        }
    }
    public override void Shoot()
    {
        if (!Pause.pause && canShoot)
        {
            RaycastShoot(Camera.main.transform.forward);

            shootEffect.Play();
            canShoot = false;
            shootTimer = 0;
        }  
    }

    public override void OnInit()
    {

    }

    public override void StopShooting()
    {
        
    }

    
}
