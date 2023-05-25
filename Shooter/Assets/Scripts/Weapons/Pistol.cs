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
