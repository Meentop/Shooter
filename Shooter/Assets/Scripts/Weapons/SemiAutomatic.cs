using System.Collections;
using UnityEngine;

public class SemiAutomatic : Weapon
{
    private bool _isShooting;
    private Coroutine _ShootCoroutine;

    public override void Shoot()
    {
        _isShooting = true;
        _ShootCoroutine = StartCoroutine(ShootCoroutine());
    }

    IEnumerator ShootCoroutine()
    {
        while (_isShooting && GetComponent<Weapon>().enabled == true)
        {
            RaycastShoot(Camera.main.transform.forward);
            shootEffect.Play();
            yield return new WaitForSeconds(firingSpeed);
        }
    }

    public override void OnInit()
    {

    }

    public override void StopShooting()
    {
        if (_ShootCoroutine != null)
        {
            StopCoroutine(_ShootCoroutine);
            _isShooting = false;
        }
    }
}
