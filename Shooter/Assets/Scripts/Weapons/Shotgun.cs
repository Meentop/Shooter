using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] private int pelletsCount;
    [SerializeField] private float spread;

    public override void Shoot()
    {
        for (int i = 0; i < pelletsCount; i++)
        {
            Vector3 direction = Camera.main.transform.forward;
            Vector3 spread = Vector3.zero;
            spread += Camera.main.transform.up * Random.Range(-1f, 1f);
            spread += Camera.main.transform.right * Random.Range(-1f, 1f);
            direction += spread.normalized * Random.Range(0f, this.spread);

            RaycastShoot(direction);
        }

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
