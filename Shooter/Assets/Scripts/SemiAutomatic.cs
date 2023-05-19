using System.Collections;
using UnityEngine;

public class SemiAutomatic : Weapon
{
    [SerializeField] private float shootDeley;

    private bool _isShooting;
    private Coroutine _ShootCoroutine;

    public override void Shoot()
    {
        _isShooting = true;
        _ShootCoroutine = StartCoroutine(ShootCoroutine());
    }

    IEnumerator ShootCoroutine()
    {
        while (_isShooting)
        {
            Instantiate(bullet, shotPoint.position, shotPoint.rotation);
            shootEffect.Play();
            yield return new WaitForSeconds(shootDeley);
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
