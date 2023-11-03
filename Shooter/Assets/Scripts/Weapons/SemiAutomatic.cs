using System.Collections;
using UnityEngine;

public class SemiAutomatic : Weapon
{
    public override void Shoot()
    {
        RaycastShoot(Camera.main.transform.forward);
        shootEffect.Play();
        _audioSource.PlayOneShot(shootSounds[Random.Range(0, shootSounds.Length)]);
        reload = true;
        Camera.main.GetComponent<CameraController>().FireRecoil(characteristics.WeaponsRecoil, characteristics.Snappiness);
        shootTimer = 0;
    }

    public override void OnInit()
    {

    }

    public override void StopShooting()
    {
        
    }
}
