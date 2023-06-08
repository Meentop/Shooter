using UnityEngine;

public class Pistol : Weapon
{
    private float shootTimer;
    private bool canShoot = true;

    protected override void Update()
    {
        base.Update();
        if (!canShoot)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootDeley)
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
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
            {
                if (hit.transform.TryGetComponent<IDamageReceiver>(out var damageReceiver))
                {
                    damageReceiver.OnGetDamage(new DamageData(DamageModifired(), hit));
                }
            }
            else
            {
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, LayerMask.GetMask("Solid")))
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
