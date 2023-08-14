using UnityEngine;

public class Pistol : Weapon
{
    public override void Shoot()
    {
        RaycastShoot(Camera.main.transform.forward);

        shootEffect.Play();
        reload = true;
        shootTimer = 0;
    }

    public override void OnInit()
    {

    }

    public override void StopShooting()
    {
        
    }
}
