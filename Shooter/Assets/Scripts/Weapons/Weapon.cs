using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour, ICollectableItem
{
    [SerializeField] protected ParticleSystem shootEffect;
    [SerializeField] protected ParticleSystem decalPrefab;
    [SerializeField] protected Image weaponsIcon;
    [Space]
    [SerializeField] protected float damage;
    [SerializeField] protected float firingSpeed;
    [SerializeField] protected Vector3 weaponOnCollectRot;
    [SerializeField] protected int maxNumberOfModifiers = 1;
    
    public CollectableItems ItemType => CollectableItems.Weapon;

    private Player _player;
    private Collider _collider;
    private InfoInterface _infoInterface;
    private PlayerDamage _playerDamage;

    private Transform _targetLook;
    private Transform _weaponHolder;

    private bool _isInited;
    private List<Modifier> _modifiers;

    [System.Serializable]
    public struct Description
    {
        public Text WeaponNameText;
        public Text DamageText;
        public Text FiringSpeed;
        public Transform ModifiersHolder;
    }

    public struct Info
    {
        public Info(string name, float damage, float firingSpeed, int maxNumbersOfModifiers)
        {
            Name = name;
            Damage = damage;
            FiringSpeed = firingSpeed;
            MaxNumbersOfModifiers = maxNumbersOfModifiers;
        }

        public string Name;
        public float Damage;
        public float FiringSpeed;
        public int MaxNumbersOfModifiers;
    }

    protected virtual void Update()
    {
        if (!_isInited)
            return;

        if (Input.GetMouseButtonDown(0))
            Shoot();
        else if (Input.GetMouseButtonUp(0))
            StopShooting();
    }

    public void Init(Player player, Transform weaponHolder, Transform targetLook, InfoInterface infoInterface, int selectedWeapon)
    {
        _player = player;
        _weaponHolder = weaponHolder;
        _targetLook = targetLook;
        _collider = GetComponent<Collider>();
        gameObject.layer = LayerMask.NameToLayer("Weapon");
        _infoInterface = infoInterface;
        _infoInterface.UpdateInfoIcon(InfoInterface.InfoIconEnum.SelectWeaponsIcon, weaponsIcon, selectedWeapon);
        _playerDamage = _player.GetComponent<PlayerDamage>();

        _isInited = true;
        OnInit();
    }

    public abstract void OnInit();

    public abstract void Shoot();

    public abstract void StopShooting();

    public Info GetInfo()
    {
        return new Info(GetName(), GetDamage(), GetFiringSpeed(), GetMaxNumbersOfModifiers());
    }

    public void DiscardFromPlayer(Weapon savedWeapon)
    {
        GetComponent<Weapon>().enabled = false;
        transform.GetComponent<RotateObject>().enabled = true;
        transform.parent.parent = savedWeapon.transform.parent.parent;
        transform.parent.localRotation = Quaternion.identity;
        transform.parent.position = (transform.parent.parent.position + new Vector3(0, 1f, 0)) + (transform.parent.position - transform.position);
        _collider.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void ConectToPlayer()
    {
        GetComponent<Weapon>().enabled = true;
        _collider.enabled = false;
        transform.GetComponent<RotateObject>().enabled = false;
        transform.localRotation = Quaternion.Euler(weaponOnCollectRot);
        transform.parent.transform.parent = _weaponHolder.transform;
        transform.parent.localPosition = new Vector3(0, 0, 0);
        transform.parent.localRotation = Quaternion.identity;
        gameObject.layer = LayerMask.NameToLayer("Weapon");
    }

    public string GetName()
    {
        return GetType().Name;
    }

    public float GetDamage()
    {
        return damage;
    }

    public float GetFiringSpeed()
    {
        return firingSpeed;
    }

    public int GetMaxNumbersOfModifiers()
    {
        return maxNumberOfModifiers;
    }

    public void OnCollect()
    {
        
    }

    protected float DamageModifired()
    {
        return damage * _playerDamage.damagePower;
    }

    public void RaycastShoot(Vector3 direction)
    {
        Physics.Raycast(Camera.main.transform.position, direction, out RaycastHit enemyHit, Mathf.Infinity, LayerMask.GetMask("Enemy"));
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit solidHit, Mathf.Infinity, LayerMask.GetMask("Solid"));

        if (solidHit.collider == null && enemyHit.collider != null)
            SetDamageToEnemy(enemyHit);
        else if (solidHit.collider != null && enemyHit.collider == null)
            SpawnDecal(solidHit);
        else if (solidHit.collider != null && enemyHit.collider != null)
        {
            if (solidHit.distance > enemyHit.distance)
                SetDamageToEnemy(enemyHit);
            else
                SpawnDecal(solidHit);
        }
    }

    private void SetDamageToEnemy(RaycastHit enemyHit)
    {
        if (enemyHit.transform.TryGetComponent<IDamageReceiver>(out var damageReceiver))
            damageReceiver.OnGetDamage(new DamageData(DamageModifired(), enemyHit));
    }

    private void SpawnDecal(RaycastHit solidHit)
    {
        var decal = Instantiate(decalPrefab, solidHit.point + solidHit.normal * 0.001f, Quaternion.LookRotation(solidHit.normal));
        decal.transform.SetParent(solidHit.transform);
        Destroy(decal, 5);
    }
}
