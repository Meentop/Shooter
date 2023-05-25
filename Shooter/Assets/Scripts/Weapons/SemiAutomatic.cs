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
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
            {
                if (hit.transform.TryGetComponent<IDamageReceiver>(out var damageReceiver))
                {
                    damageReceiver.OnGetDamage(new DamageData(Random.Range(damageRange.x, damageRange.y), hit));
                }
                else
                {
                    var decal = Instantiate(decalPrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                    decal.transform.SetParent(hit.transform);
                    Destroy(decal, 5);
                }
            }
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
