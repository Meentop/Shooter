using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour, ISelectableItem
{
    [SerializeField] protected ParticleSystem shootEffect;
    [SerializeField] protected ParticleSystem decalPrefab;
    [SerializeField] private Collider boxCollider;
    [SerializeField] protected GameObject crossheir;
    [Space]
    [SerializeField] protected Sprite sprite;
    [SerializeField] protected string weaponName;
    [SerializeField] private bool canSprayed;
    [SerializeField] protected int[] damage = new int[3];
    [SerializeField] protected float firingSpeed;
    [SerializeField] protected Vector3 weaponOnCollectRot;
    [SerializeField] private WeaponConfig config;

    protected int maxNumberOfModules = 1;
    protected int level = 0;
    protected const int maxLevel = 2;

    public SelectableItems ItemType => SelectableItems.Weapon;

    public string Text => Bought ? "Press E to buy" : "Press E to equip";
    
    private Player _player;
    private InfoInterface _infoInterface;
    private PlayerDamage _playerDamage;
    private Transform _targetLook;
    private Transform _weaponHolder;
    private bool _isInited;
    private List<WeaponModule> _weaponModules = new List<WeaponModule>();
    protected bool isSpraying;
    protected float shootTimer;
    protected bool reload = true;
    public bool Bought { get; private set; }

    [System.Serializable]
    public struct Description
    {
        public Image Image;
        public Text NameText;
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

        if(!PauseManager.Pause)
            Reload();

        if (Input.GetMouseButtonDown(0) && !reload && !PauseManager.Pause)
        {
            Shoot();
            if (canSprayed)
                isSpraying = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopShooting();
            if (canSprayed)
                isSpraying = false;
        }

        if (isSpraying && !reload && !PauseManager.Pause)
            Shoot();
    }

    public void Init(Player player, Transform weaponHolder, Transform targetLook, InfoInterface infoInterface, int selectedWeapon)
    {
        _player = player;
        _weaponHolder = weaponHolder;
        _targetLook = targetLook;
        gameObject.layer = LayerMask.NameToLayer("Weapon");
        _infoInterface = infoInterface;
        _infoInterface.SetWeaponIcon(sprite, selectedWeapon);
        _infoInterface.SetWeaponCrossheir(crossheir);
        _playerDamage = _player.GetComponent<PlayerDamage>();

        _isInited = true;
        Bought = true;
        OnInit();
    }

    private void Reload()
    {
        if (reload)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= firingSpeed)
            {
                reload = false;
                shootTimer = 0f;
            }
        }
    }

    public abstract void OnInit();

    public abstract void Shoot();

    public abstract void StopShooting();

    public void ConnectToStand(Transform stand)
    {
        enabled = false;
        if (transform.TryGetComponent<RotateObject>(out var component))
            component.enabled = true;
        transform.parent.parent = stand;
        transform.parent.localRotation = Quaternion.identity;
        transform.parent.position = transform.parent.parent.position + new Vector3(0, 1f, 0) + (transform.parent.position - transform.position);
        if (boxCollider != null)
            boxCollider.enabled = true;
        foreach (var module in _weaponModules)
            _player.AddFreeModule(module);
        _weaponModules.Clear();
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void ConectToPlayer()
    {
        enabled = true;
        if (transform.GetComponentInParent<WeaponAward>())
            transform.GetComponentInParent<WeaponAward>().DeleteOtherWeapons(transform.parent.parent);
        if (boxCollider != null)
            boxCollider.enabled = false;
        if (transform.TryGetComponent<RotateObject>(out var component))
            component.enabled = false;
        transform.localRotation = Quaternion.Euler(weaponOnCollectRot);
        transform.parent.parent = _weaponHolder.transform;
        transform.parent.localPosition = Vector3.zero;
        transform.parent.localRotation = Quaternion.identity;
        gameObject.layer = LayerMask.NameToLayer("Weapon");
    }

    public Sprite GetSprite() => sprite;
    public string GetTitle() => weaponName;
    public string GetName() => weaponName + " " + (level + 1).ToString() + " lvl";
    public string GetUpgradedName() => weaponName + " " + (level + 2).ToString() + " lvl";
    public float GetDamage() => damage[level];
    public float GetUpgradedDamage() => damage[level + 1];
    public float GetFiringSpeed() => firingSpeed;
    public int GetMaxNumbersOfModules() => maxNumberOfModules;
    public int GetUpgradedMaxNumbersOfModules() => maxNumberOfModules + 1;
    public bool CouldBeUpgraded() => level < maxLevel;
    public int GetLevel() => level;

    public void OnSelect(Player player)
    {
        
    }

    protected int DamageModifired()
    {
        return (int)(damage[level] * _playerDamage.damagePower);
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

    public List<WeaponModule> GetModules() => _weaponModules;

    public int GetModulesCount() => _weaponModules.Count;

    public WeaponModule GetModule(int index) => _weaponModules[index];

    public GameObject GetWeaponCrossheir()
    {
        return crossheir;
    }

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
        InfoForWeaponModule info = new InfoForWeaponModule
        {
            enemyStatusEffects = enemy.GetStatusEffects()
        };
        foreach (var module in _weaponModules)
        {
            info.lvl = module.Level;
            damageData = module.ApplyBehaviour(damageData, info);
        }
        return damageData;
    }


    public void UpgradeWeapon()
    {
        level++;
        maxNumberOfModules++;
    }

    public void SetLevel(int level)
    {
        this.level = level;
        maxNumberOfModules = level + 1;
    }

    public void SetBought()
    {
        Bought = true;
    }

    public WeaponSave GetSave()
    {
        List<WeaponModuleSave> modules = new List<WeaponModuleSave>();
        foreach (var module in _weaponModules)
        {
            modules.Add(module.GetSave());
        }
        WeaponSave weaponSave = new WeaponSave
        {
            number = config.GetIndex(this),
            level = level,
            modules = modules
        };
        return weaponSave;
    }
}
