using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform targetLook;
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private float scrollDelay;
    [SerializeField] private float selectDistance = 4f;
    [SerializeField] private WeaponConfig weaponConfig;
    [SerializeField] private WeaponModuleConfig weaponModuleConfig;
    [SerializeField] private BionicModuleConfig bionicModuleConfig;

    public PlayerHealth Health { get; private set; }
    public PlayerGold Gold { get; private set; }
    public PlayerController Controller { get; private set; }

    private Weapon _currentWeapon;
    private ActiveSkill _activeSkill;
    private int _selectedWeaponSlot;
    private bool _isScrolling;
    private ISelectableItem _lastSavedSelectableItem;
    private List<WeaponModule> _freeWeaponModules = new List<WeaponModule>();
    private List<BionicModule> _freeBionicModules = new List<BionicModule>();
    private List<BionicModule> _installedBionicModules = new List<BionicModule>();
    private InfoInterface _infoInterface;
    private ModulesPanelUI _dynamicInterface;
    private UIManager _uiManager;
    [SerializeField] private PlayerMovement _movement;

    private void Update()
    {
        if (!PauseManager.Pause)
        {
            SelectWeapon();
            InputScrollWeapon();
            UseActiveSkill();
            SelectableItemsDetection();
            SelectItem();
        }
    }

    public void Init(UIManager uiManager, CameraController cameraController, Camera mainCamera, RectTransform canvas)
    {
        Health = GetComponent<PlayerHealth>();
        Gold = GetComponent<PlayerGold>();
        Controller = GetComponent<PlayerController>();
        _currentWeapon = weapons[0];
        _infoInterface = uiManager.InfoInterface;
        _dynamicInterface = uiManager.ModulesPanel;
        _uiManager = uiManager;
        _dynamicInterface.Init(this, cameraController, mainCamera, canvas);
        Gold.Init(_infoInterface);
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

        if (_lastSavedSelectableItem != null && _lastSavedSelectableItem as ModuleUpgradeAward)
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

    private void SelectItem()
    {
        if (Input.GetKeyDown(KeyCode.E) && !PauseManager.Pause)
        {
            if (_lastSavedSelectableItem == null)
                return;
            
            switch (_lastSavedSelectableItem.ItemType)
            {
                case SelectableItems.Weapon:
                    Weapon selectWeapon = _lastSavedSelectableItem as Weapon;
                    if (Gold.HasCount(selectWeapon.GetPrice()) || selectWeapon.Bought)
                    {
                        if(!selectWeapon.Bought)
                            Gold.Remove(selectWeapon.GetPrice());
                        _currentWeapon.ConnectToStand(selectWeapon.transform.parent.parent);
                        _currentWeapon = selectWeapon;
                        weapons[_selectedWeaponSlot] = _currentWeapon;
                        _currentWeapon.Init(this, weaponHolder, targetLook, _infoInterface, _selectedWeaponSlot);
                        _currentWeapon.ConectToPlayer();
                    }
                    break;
                case SelectableItems.Module:
                    Module selectModule = _lastSavedSelectableItem as Module;
                    if (Gold.HasCount(selectModule.GetPrice()))
                    {
                        if (selectModule.transform.parent.parent.TryGetComponent<WeaponModuleAward>(out var modifierAward))
                            modifierAward.DeleteOtherModifiers(selectModule.transform.parent);
                        Gold.Remove(selectModule.GetPrice());
                        AddFreeModule(selectModule);
                        if (!selectModule.undestroyable)
                            Destroy(selectModule.gameObject);
                    }
                    break;
                case SelectableItems.ActiveSkill:
                    ActiveSkill selectSkill = _lastSavedSelectableItem as ActiveSkill;
                    if (Gold.HasCount(selectSkill.GetPrice()))
                    {
                        if (selectSkill.transform.parent.parent.TryGetComponent<ActiveSkillAward>(out var skillAward))
                            skillAward.DeleteOtherSkills(selectSkill.transform.parent);
                        Gold.Remove(selectSkill.GetPrice());
                        AddActiveSkill(selectSkill);
                        Destroy(selectSkill.gameObject);
                    }
                    break;
                case SelectableItems.WeaponUpgrade:
                    Weapon curWeapon = weapons[_selectedWeaponSlot];
                    WeaponUpgradeAward weaponUpgrader = _lastSavedSelectableItem as WeaponUpgradeAward;
                    if (Gold.HasCount(curWeapon.GetUpgradePrice()) && !weaponUpgrader.IsUsed() && curWeapon.CouldBeUpgraded())
                    {
                        Gold.Remove(curWeapon.GetUpgradePrice());
                        curWeapon.UpgradeWeapon();
                        weaponUpgrader.SetWasUsed();
                    }
                    break;
                case SelectableItems.ModuleUpgrade:
                    Module curModule = _uiManager.SelectadleUI.GetSelectedModule();
                    ModuleUpgradeAward moduleUpgrader = _lastSavedSelectableItem as ModuleUpgradeAward;
                    if (Gold.HasCount(10) && !moduleUpgrader.IsUsed() && curModule.CouldBeUpgraded())
                    {
                        Gold.Remove(10);
                        curModule.UpgradeModule();
                        moduleUpgrader.SetWasUsed();
                        _uiManager.SelectadleUI.EnableUpgradeModuleUI(Gold.HasCount(10), moduleUpgrader, GetAllModules());
                    }
                    break;
            }
            _lastSavedSelectableItem.OnSelect(this);
        }
    }

    private bool over = false;

    private void SelectableItemsDetection()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var hit, selectDistance))
        {
            if (hit.collider.TryGetComponent<ISelectableItem>(out var item))
            {
                _lastSavedSelectableItem = item;
                OverChoosableItem();
                if (!over)
                {
                    EnterSelectableItem();
                    over = true;
                }
            }
            else
            {
                NoOverSelectableItem();
                if (over)
                {
                    ExitSelectableItem();
                    over = false;
                }
            }
        }
        else
        {
            NoOverSelectableItem();
            if (over)
            {
                ExitSelectableItem();
                over = false;
            }
        }
    }

    private void OverChoosableItem()
    {
        if (!_uiManager.SelectadleUI.GetActiveSelectableUI(SelectableUIType.Select))
            _uiManager.SelectadleUI.SetActiveSelectableUI(SelectableUIType.Select, true);
        _uiManager.SelectadleUI.SetSelectText(_lastSavedSelectableItem.Text);
        switch (_lastSavedSelectableItem.ItemType)
        {
            case SelectableItems.Weapon:
                Weapon selectingWeapon = _lastSavedSelectableItem as Weapon;
                _uiManager.SelectadleUI.UpdateNewWeaponUI(Gold.HasCount(selectingWeapon.GetPrice()), selectingWeapon);
                break;
            case SelectableItems.Module:
                Module selectingModule = _lastSavedSelectableItem as Module; 
                _uiManager.SelectadleUI.UpdateNewModuleUI(Gold.HasCount(selectingModule.GetPrice()), selectingModule);
                break;
            case SelectableItems.ActiveSkill:
                ActiveSkill selectingSkill = _lastSavedSelectableItem as ActiveSkill;
                _uiManager.SelectadleUI.UpdateNewActiveSkillUI(Gold.HasCount(selectingSkill.GetPrice()), selectingSkill);
                break;
            case SelectableItems.HealthAward:
                HealthAward healthAward = _lastSavedSelectableItem as HealthAward;
                _uiManager.SelectadleUI.UpdateBuyHealthUI(Gold.HasCount(healthAward.GetPrice()), healthAward);
                break;
            case SelectableItems.WeaponUpgrade:
                Weapon curWeapon = weapons[_selectedWeaponSlot];
                WeaponUpgradeAward weapoUpgrader = _lastSavedSelectableItem as WeaponUpgradeAward;
                _uiManager.SelectadleUI.UpdateUpgradeWeaponUI(Gold.HasCount(curWeapon.GetUpgradePrice()), weapoUpgrader, curWeapon);
                break;
            case SelectableItems.ModuleUpgrade:
                ModuleUpgradeAward moduleUpgrader = _lastSavedSelectableItem as ModuleUpgradeAward;
                _uiManager.SelectadleUI.UpdateUpgradeModuleUI(moduleUpgrader);
                break;
        }
        SetActiveTextTypes(true);
    }

    private void NoOverSelectableItem()
    {
        if (_uiManager.SelectadleUI.GetActiveSelectableUI(SelectableUIType.Select))
            _uiManager.SelectadleUI.SetActiveSelectableUI(SelectableUIType.Select, false);
        if (_lastSavedSelectableItem != null)
        {
            SetActiveTextTypes(false);
            _lastSavedSelectableItem = null;
        }
    }

    private void EnterSelectableItem()
    {
        switch (_lastSavedSelectableItem.ItemType)
        {
            case SelectableItems.ModuleUpgrade:
                ModuleUpgradeAward moduleUpgrader = _lastSavedSelectableItem as ModuleUpgradeAward;
                _uiManager.SelectadleUI.EnableUpgradeModuleUI(Gold.HasCount(10), moduleUpgrader, GetAllModules());
                break;
        }
    }

    private void ExitSelectableItem()
    {

    }

    private void SetActiveTextTypes(bool active)
    {
        switch (_lastSavedSelectableItem.ItemType)
        {
            case SelectableItems.Weapon:
                _uiManager.SelectadleUI.SetActiveSelectableUI(SelectableUIType.NewWeapon, active);
                break;
            case SelectableItems.Module:
                _uiManager.SelectadleUI.SetActiveSelectableUI(SelectableUIType.NewModule, active);
                break;
            case SelectableItems.ActiveSkill:
                _uiManager.SelectadleUI.SetActiveSelectableUI(SelectableUIType.NewActiveSkill, active);
                break;
            case SelectableItems.HealthAward:
                _uiManager.SelectadleUI.SetActiveSelectableUI(SelectableUIType.BuyHealth, active);
                break;
            case SelectableItems.WeaponUpgrade:
                _uiManager.SelectadleUI.SetActiveSelectableUI(SelectableUIType.WeaponUpgrade, active);
                break;
            case SelectableItems.ModuleUpgrade:
                _uiManager.SelectadleUI.SetActiveSelectableUI(SelectableUIType.ModuleUpgrade, active);
                break;
        }
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
    public List<WeaponSave> GetWeaponSaves()
    {
        List<WeaponSave> weapons = new List<WeaponSave>();
        foreach (var weapon in this.weapons)
        {
            weapons.Add(weapon.GetSave());
        }
        return weapons;
    }

    public List<WeaponModuleSave> GetFreeWeaponModulesSave()
    {
        List<WeaponModuleSave> modules = new List<WeaponModuleSave>();
        foreach (var module in _freeWeaponModules)
        {
            modules.Add(module.GetSave());
        }
        return modules;
    }

    public List<BionicModuleSave> GetInstalledBionicModulesSave()
    {
        List<BionicModuleSave> modules = new List<BionicModuleSave>();
        foreach (var module in _installedBionicModules)
        {
            modules.Add(module.GetSave());
        }
        return modules;
    }
    public List<BionicModuleSave> GetFreeBionicModulesSave()
    {
        List<BionicModuleSave> modules = new List<BionicModuleSave>();
        foreach (var module in _freeBionicModules)
        {
            modules.Add(module.GetSave());
        }
        return modules;
    }



    public void LoadWeapons(List<WeaponSave> weaponSaves)
    {
        for (int i = 0; i < weaponSaves.Count; i++)
        {
            GameObject weaponHolder = Instantiate(weaponConfig.Weapons[weaponSaves[i].number]);
            Weapon weapon = weaponHolder.GetComponentInChildren<Weapon>();
            weapons[i] = weapon;
            weapon.SetLevel(weaponSaves[i].level);
            weapon.Init(this, this.weaponHolder, targetLook, _infoInterface, i);
            weapon.ConectToPlayer();

            foreach (var module in weaponSaves[i].modules)
            {
                WeaponModule weaponModule = weaponModuleConfig.Modules[module.number];
                weaponModule.SetLevel(module.level);
                weapon.AddModule(weaponModule);
            }
        }
        _currentWeapon = weapons[0];
        weapons[1].transform.gameObject.SetActive(false);
        _infoInterface.SetActiveWeaponIcon(true, 0);
    }

    public void LoadFreeWeaponModules(List<WeaponModuleSave> modules)
    {
        foreach (var module in modules)
        {
            WeaponModule weaponModule = weaponModuleConfig.Modules[module.number];
            weaponModule.SetLevel(module.level);
            _freeWeaponModules.Add(weaponModule);
        }
    }

    public void LoadInstalledBionicModules(List<BionicModuleSave> modules)
    {
        foreach (var module in modules)
        {
            BionicModule bionicnModule = bionicModuleConfig.Modules[module.number];
            bionicnModule.SetLevel(module.level);
            _installedBionicModules.Add(bionicnModule);
        }
    }

    public void LoadFreeBionicModules(List<BionicModuleSave> modules)
    {
        foreach (var module in modules)
        {
            BionicModule bionicnModule = bionicModuleConfig.Modules[module.number];
            bionicnModule.SetLevel(module.level);
            _freeBionicModules.Add(bionicnModule);
        }
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
    }

    public void RemoveInstalledBionicModule(BionicModule bionic)
    {
        _installedBionicModules.Remove(bionic);
    }

    private void AddActiveSkill(ActiveSkill skill)
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


    public PlayerMovement GetMovement()
    {
        PlayerMovement movement = new PlayerMovement(_movement);
        InfoForBionicModule info = new InfoForBionicModule();
        foreach (var module in _installedBionicModules)
        {
            info.lvl = module.Level;
            movement = module.ApplyBehaviour(movement, info);
        }
        return movement;
    }

    


    public void SetDashInfo(float dashReload, int dashCharges)
    {
        _infoInterface.SetDashInfo(dashReload, dashCharges);
    }
}