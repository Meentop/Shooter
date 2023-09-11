using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform targetLook;
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private float scrollDelay;
    [SerializeField] private ActiveSkillConfig activeSkillConfig;
    [SerializeField] private WeaponConfig weaponConfig;
    [SerializeField] private WeaponModuleConfig weaponModuleConfig;
    [SerializeField] private BionicModuleConfig bionicModuleConfig;
    [SerializeField] private PlayerCharacteristics baseCharacteristics;

    public PlayerHealth Health { get; private set; }
    public PlayerGold Gold { get; private set; }
    public PlayerController Controller { get; private set; }
    public PlayerCharacteristics Characteristics { get; private set; }
    public PlayerInteractionManager InteractionManager { get; private set; }
    public PlayerSaveLoadManager SaveLoadManager { get; private set; }

    private Weapon _currentWeapon;
    private ActiveSkill _activeSkill;
    private int _selectedWeaponSlot;
    private bool _isScrolling;
    private List<WeaponModule> _freeWeaponModules = new List<WeaponModule>();
    private List<BionicModule> _freeBionicModules = new List<BionicModule>();
    private List<BionicModule> _installedBionicModules = new List<BionicModule>();
    private InfoInterface _infoInterface;
    private ModulesPanelUI _dynamicInterface;
    private UIManager _uiManager;

    private void Update()
    {
        if (!PauseManager.Pause)
        {
            SelectWeapon();
            InputScrollWeapon();
            UseActiveSkill();
        }
    }

    public void Init(UIManager uiManager, CameraController cameraController, Camera mainCamera, RectTransform canvas)
    {
        Health = GetComponent<PlayerHealth>();
        Gold = GetComponent<PlayerGold>();
        Controller = GetComponent<PlayerController>();
        InteractionManager = GetComponent<PlayerInteractionManager>();
        SaveLoadManager = GetComponent<PlayerSaveLoadManager>();
        _currentWeapon = weapons[0];
        _infoInterface = uiManager.InfoInterface;
        _dynamicInterface = uiManager.ModulesPanel;
        _uiManager = uiManager;
        _dynamicInterface.Init(this, cameraController, mainCamera, canvas);
        Gold.Init(_infoInterface);
        SetCharacteristics();
        InteractionManager.Init(this, _uiManager);
        SaveLoadManager.Init(this);
    }

    public void SelectWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _selectedWeaponSlot = 0;
            SwapWeapon(weapons[_selectedWeaponSlot]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _selectedWeaponSlot = 1;
            SwapWeapon(weapons[_selectedWeaponSlot]);
        }
    }
    
    private void SwapWeapon(Weapon weaponToSwap)
    {
        if (weaponToSwap == _currentWeapon)
            return;

        if (InteractionManager.IsOverModuleUpgrade())
            return;

        _currentWeapon.transform.gameObject.SetActive(false);
        _currentWeapon = weaponToSwap;
        _currentWeapon.transform.gameObject.SetActive(true);
        _infoInterface.SetActiveWeaponIcon(false, CollectionsExtensions.GetNextIndex(weapons, _selectedWeaponSlot));
        _infoInterface.SetActiveWeaponIcon(true, _selectedWeaponSlot);
    }

    private void InputScrollWeapon()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if(scrollInput != 0)
        {
            if (!_isScrolling)
            {
                _isScrolling = true;
                StartCoroutine(ScrollDelayRoutine());
            }
        }
    }
    
    private IEnumerator ScrollDelayRoutine()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        while (scrollInput != 0)
        {
            _selectedWeaponSlot = CollectionsExtensions.GetNextIndex(weapons, _selectedWeaponSlot);
            SwapWeapon(weapons[_selectedWeaponSlot]);
            yield return new WaitForSeconds(scrollDelay);
            scrollInput = Input.GetAxis("Mouse ScrollWheel");
        }
        _isScrolling = false;
    }

    private void UseActiveSkill()
    {
        if (Input.GetMouseButtonDown(1) && !PauseManager.Pause)
        {
            try
            {
                _activeSkill.Activate();
            }
            catch 
            {
                print("error active skill not found");
            }
        }
    }


    public Weapon[] GetWeapons() => weapons;
    public Weapon GetSelectedWeapon() => weapons[_selectedWeaponSlot];
    


    public void AddWeaponFromStand(Weapon selectWeapon)
    {
        _currentWeapon.ConnectToStand(selectWeapon.transform.parent.parent);
        _currentWeapon = selectWeapon;
        weapons[_selectedWeaponSlot] = _currentWeapon;
        _currentWeapon.Init(this, weaponHolder, targetLook, _infoInterface, _selectedWeaponSlot);
        _currentWeapon.ConectToPlayer();
    }

    public void AddWeapon(Weapon weapon, int index)
    {
        weapons[index] = weapon;
        weapon.Init(this, this.weaponHolder, targetLook, _infoInterface, index);
    }

    public void SetCurrentWeapon()
    {
        _currentWeapon = weapons[0];
        weapons[1].transform.gameObject.SetActive(false);
        _infoInterface.SetActiveWeaponIcon(true, 0);
    }

    public List<WeaponModule> GetFreeWeaponModules() => _freeWeaponModules;
    public List<BionicModule> GetFreeBionicModules() => _freeBionicModules;
    public List<BionicModule> GetInstalledBionicModules() => _installedBionicModules;

    public List<Module> GetAllModules()
    {
        List<Module> modules = new List<Module>();
        modules.AddRange(_freeWeaponModules);
        modules.AddRange(_freeBionicModules);
        modules.AddRange(_installedBionicModules);
        foreach (var weapon in GetWeapons())
        {
            modules.AddRange(weapon.GetModules());
        }
        return modules;
    }

    public void RemoveFreeModule(Module module)
    {
        if (module.GetType() == typeof(WeaponModule))
        {
            _freeWeaponModules.Remove(module as WeaponModule);
        }
        else if (module.GetType() == typeof(BionicModule))
        {
            _freeBionicModules.Remove(module as BionicModule);
        }
    }

    public void AddFreeModule(Module module)
    {
        if(module.GetType() == typeof(WeaponModule))
        {
            _freeWeaponModules.Add(module as WeaponModule);
        }
        else if(module.GetType() == typeof(BionicModule))
        {
            _freeBionicModules.Add(module as BionicModule);
        }
    }

    public void AddInstalledBionicModule(BionicModule module)
    {
        _installedBionicModules.Add(module);
        SetCharacteristics();
    }

    public void RemoveInstalledBionicModule(BionicModule bionic)
    {
        _installedBionicModules.Remove(bionic);
        SetCharacteristics();
    }

    public ActiveSkill GetActiveSkill()
    {
        return _activeSkill;
    }

    public void AddActiveSkill(ActiveSkill skill)
    {
        _activeSkill = skill;
        _activeSkill.Init(_uiManager.InfoInterface, Camera.main.transform, this);
    }

    public void AddDamageToActiveSkill(int damage)
    {
        try
        {
            _activeSkill.AddDamageToTimer(damage);
        }
        catch
        {
            
        }
    }


    public void SetCharacteristics()
    {
        PlayerCharacteristics characteristics = new PlayerCharacteristics(baseCharacteristics);
        InfoForBionicModule info = new InfoForBionicModule();
        foreach (var module in _installedBionicModules)
        {
            info.lvl = module.Level;
            characteristics = module.ApplyBehaviour(characteristics, info);
        }
        Characteristics = characteristics;
        Health.SetMaxHealth(characteristics.maxHealth);
        _infoInterface.SetBloodyActiveSkill(characteristics.bloodyActiveSkill);
        if (characteristics.bloodyActiveSkill)
        {
            _infoInterface.SetActiveSkillBloodyPrice(characteristics.activeSkillBloodyPrice);
        }
    }

    


    public void SetDashInfo(float dashReload, int dashCharges)
    {
        _infoInterface.SetDashInfo(dashReload, dashCharges);
    }
}