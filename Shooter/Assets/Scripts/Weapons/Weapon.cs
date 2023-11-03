using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected ParticleSystem shootEffect;
    [SerializeField] protected ParticleSystem decalPrefab;
    [SerializeField] protected GameObject crossheir;
    [SerializeField] protected AudioClip[] shootSounds;
    [Space]
    [SerializeField] private WeaponConfig config;
    [SerializeField] protected WeaponCharacteristics characteristics;
    protected int maxNumberOfModules = 1;
    protected int level = 0;
    protected const int maxLevel = 2;

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
    protected AudioSource _audioSource;

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
            if (characteristics.CanSprayed)
                isSpraying = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopShooting();
            if (characteristics.CanSprayed)
                isSpraying = false;
        }

        if (isSpraying && !reload && !PauseManager.Pause)
            Shoot();
    }

    public void Init(Player player, Transform weaponHolder, Transform targetLook, InfoInterface infoInterface, int weaponPosition)
    {
        _player = player;
        _weaponHolder = weaponHolder;
        _targetLook = targetLook;
        gameObject.layer = LayerMask.NameToLayer("Weapon");
        _infoInterface = infoInterface;
        _infoInterface.SetWeaponIcon(characteristics.Sprite, weaponPosition);
        _infoInterface.SetWeaponCrossheir(crossheir);
        _playerDamage = _player.GetComponent<PlayerDamage>();
        _audioSource = Camera.main.GetComponent<AudioSource>();

        _isInited = true;
        OnInit();
    }

    private void Reload()
    {
        if (reload)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= characteristics.FiringSpeed)
            {
                reload = false;
                shootTimer = 0f;
            }
        }
    }

    public abstract void OnInit();

    public abstract void Shoot();

    public abstract void StopShooting();

    public Sprite GetSprite() => characteristics.Sprite;
    public string GetTitle() => characteristics.WeaponName;
    public string GetName() => characteristics.WeaponName + " " + (level + 1).ToString() + " lvl";
    public string GetUpgradedName() => characteristics.WeaponName + " " + (level + 2).ToString() + " lvl";
    public float GetDamage() => characteristics.Damage[level];
    public float GetUpgradedDamage() => characteristics.Damage[level + 1];
    public float GetFiringSpeed() => characteristics.FiringSpeed;
    public int GetMaxNumbersOfModules() => maxNumberOfModules;
    public int GetUpgradedMaxNumbersOfModules() => maxNumberOfModules + 1;
    public bool CouldBeUpgraded() => level < maxLevel;
    public int GetLevel() => level;
    public WeaponCharacteristics GetWeaponCharacteristics() => characteristics;

    protected int DamageModifired()
    {
        return (int)(characteristics.Damage[level] * _playerDamage.damagePower);
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

    public WeaponSave GetSave()
    {
        List<WeaponModuleSave> modules = new List<WeaponModuleSave>();
        foreach (var module in _weaponModules)
        {
            modules.Add(module.GetSave());
        }
        WeaponSave weaponSave = new WeaponSave
        {
            number = config.GetIndex(characteristics),
            level = level,
            modules = modules
        };
        return weaponSave;
    }
}
