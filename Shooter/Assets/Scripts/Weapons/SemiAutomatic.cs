using System.Collections;
using UnityEngine;

public class SemiAutomatic : Weapon
{
    public override void Shoot()
    {
        RaycastShoot(Camera.main.transform.forward);
        shootEffect.Play();
        reload = true;
        Camera.main.GetComponent<CameraController>().FireRecoil(weaponsRecoil, snappiness);
        shootTimer = 0;
    }

    public override void OnInit()
    {

    }

    public override void StopShooting()
    {
        
    }
}
