using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour, ISelectableItem
{
    [SerializeField] protected ParticleSystem shootEffect;
    [SerializeField] protected ParticleSystem decalPrefab;
    [SerializeField] protected Image weaponsIcon;
    [SerializeField] private Collider boxCollider;
    [Space]
    [SerializeField] protected int damage;
    [SerializeField] protected float firingSpeed;
    [SerializeField] protected Vector3 weaponOnCollectRot;
    [SerializeField] protected int maxNumberOfModules = 1;
    [SerializeField] private int price;
    
    public SelectableItems ItemType => SelectableItems.Weapon;

    private Player _player;
    private InfoInterface _infoInterface;
    private PlayerDamage _playerDamage;
    private Transform _targetLook;
    private Transform _weaponHolder;
    private bool _isInited;
    private List<WeaponModule> _weaponModules = new List<WeaponModule>();
    public bool bought { get; private set; }

    [System.Serializable]
    public struct Description
    {
        public Text WeaponNameText;
        public Text DamageText;
        public Text FiringSpeed;
        public Transform ModulesHolder;
        public GameObject BuyPanel;
        public Text PriceText;
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
        gameObject.layer = LayerMask.NameToLayer("Weapon");
        _infoInterface = infoInterface;
        _infoInterface.UpdateInfoIcon(InfoInterface.InfoIconEnum.SelectWeaponsIcon, weaponsIcon, selectedWeapon);
        _playerDamage = _player.GetComponent<PlayerDamage>();

        _isInited = true;
        bought = true;
        OnInit();
    }

    public abstract void OnInit();

    public abstract void Shoot();

    public abstract void StopShooting();

    public void ConnectToStand(Transform stand)
    {
        enabled = false;
        transform.GetComponent<RotateObject>().enabled = true;
        transform.parent.parent = stand;
        transform.parent.localRotation = Quaternion.identity;
        transform.parent.position = transform.parent.parent.position + new Vector3(0, 1f, 0) + (transform.parent.position - transform.position);
        boxCollider.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void ConectToPlayer()
    {
        enabled = true;
        if (transform.parent.parent.parent.TryGetComponent<WeaponAward>(out var weaponAward))
            weaponAward.DeleteOtherWeapons(transform.parent.parent);
        boxCollider.enabled = false;
        transform.GetComponent<RotateObject>().enabled = false;
        transform.localRotation = Quaternion.Euler(weaponOnCollectRot);
        transform.parent.parent = _weaponHolder.transform;
        transform.parent.localPosition = Vector3.zero;
        transform.parent.localRotation = Quaternion.identity;
        gameObject.layer = LayerMask.NameToLayer("Weapon");
    }

    public string GetName() => GetType().Name;

    public float GetDamage() => damage;

    public float GetFiringSpeed() => firingSpeed;

    public int GetMaxNumbersOfModules() => maxNumberOfModules;

    public int GetPrice() => price;

    public void OnSelect(Player player)
    {
        
    }

    protected int DamageModifired()
    {
        return (int)(damage * _playerDamage.damagePower);
    }

    public void RaycastShoot(Vector3 direction)
    {
        Physics.Raycast(Camera.main.transform.position, direction, out RaycastHit enemyHit, Mathf.Infinity, LayerMask.GetMask("Enemy"));
        Physics.Raycast(Camera.main.transform.position, direction, out RaycastHit solidHit, Mathf.Infinity, LayerMask.GetMask("Solid"));

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
            damageReceiver.GetDamage(GetDamageData(damageReceiver as EnemyHealth));
    }

    private void SpawnDecal(RaycastHit solidHit)
    {
        var decal = Instantiate(decalPrefab, solidHit.point + solidHit.normal * 0.001f, Quaternion.LookRotation(solidHit.normal));
        decal.transform.SetParent(solidHit.transform);
        Destroy(decal, 5);
    }


    public int GetModulesCount() => _weaponModules.Count;

    public WeaponModule GetModule(int index) => _weaponModules[index];

    public void AddModule(WeaponModule module)
    {
        _weaponModules.Add(module);
    }

    public void RemoveModule(WeaponModule module)
    {
        _weaponModules.Remove(module);
    }


    private DamageData GetDamageData(EnemyHealth enemy)
    {
        DamageData damageData = new DamageData(damage: DamageModifired());
        foreach (var modifier in _weaponModules)
        {
            damageData = modifier.ApplyBehaviours(damageData, enemy);
        }
        return damageData;
    }
}
