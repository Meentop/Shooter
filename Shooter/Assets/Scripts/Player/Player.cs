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
        if (!Pause.pause)
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
        _infoInterface = uiManager.infoInterface;
        _dynamicInterface = uiManager.dinemicInterface;
        _uiManager = uiManager;
        _currentWeapon.Init(this, weaponHolder, targetLook, uiManager.infoInterface, _selectedWeaponSlot);
        _dynamicInterface.Init(this, cameraController, mainCamera, canvas);
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

        _currentWeapon.transform.gameObject.SetActive(false);
        _currentWeapon = weaponToSwap;
        _currentWeapon.transform.gameObject.SetActive(true);
        _currentWeapon.Init(this, weaponHolder, targetLook, _infoInterface, _selectedWeaponSlot);
        _infoInterface.DiscardWeaponsIcon(CollectionsExtensions.GetNextIndex(weapons, _selectedWeaponSlot));
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
        if (Input.GetKeyDown(KeyCode.E))
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
                case SelectableItems.UpgradeWeapon:
                    Weapon curWeapon = weapons[_selectedWeaponSlot];
                    UpgradeWeaponAward upgrader = _lastSavedSelectableItem as UpgradeWeaponAward;
                    if (Gold.HasCount(curWeapon.GetUpgradePrice()) && !upgrader.IsUsed() && curWeapon.CouldBeUpgraded())
                    {
                        Gold.Remove(curWeapon.GetUpgradePrice());
                        curWeapon.UpgradeWeapon();
                        upgrader.SetWasUsed();
                    }
                    break;
            }
            _lastSavedSelectableItem.OnSelect(this);
        }
    }

    private void SelectableItemsDetection()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var hit, selectDistance))
        {
            if (hit.collider.TryGetComponent<ISelectableItem>(out var item))
            {
                _lastSavedSelectableItem = item;
                OverChoosableItem();
            }
            else
                ExitSelectableItem();
        }
        else
            ExitSelectableItem();
    }

    private void OverChoosableItem()
    {
        if (!_uiManager.GetActiveText(TextTypes.Select))
            _uiManager.SetActiveText(TextTypes.Select, true);
        _uiManager.SetSelectText(_lastSavedSelectableItem.Text);
        switch (_lastSavedSelectableItem.ItemType)
        {
            case SelectableItems.Weapon:
                Weapon selectingWeapon = _lastSavedSelectableItem as Weapon;
                _uiManager.UpdateNewWeaponDescription(Gold.HasCount(selectingWeapon.GetPrice()), selectingWeapon);
                break;
            case SelectableItems.Module:
                Module selectingModule = _lastSavedSelectableItem as Module; 
                _uiManager.UpdateNewModuleInfo(Gold.HasCount(selectingModule.GetPrice()), selectingModule);
                break;
            case SelectableItems.ActiveSkill:
                ActiveSkill selectingSkill = _lastSavedSelectableItem as ActiveSkill;
                _uiManager.UpdateNewActiveSkillInfo(Gold.HasCount(selectingSkill.GetPrice()), selectingSkill);
                break;
            case SelectableItems.HealthAward:
                HealthAward healthAward = _lastSavedSelectableItem as HealthAward;
                _uiManager.UpdateBuyHealthText(Gold.HasCount(healthAward.GetPrice()), healthAward);
                break;
            case SelectableItems.UpgradeWeapon:
                Weapon curWeapon = weapons[_selectedWeaponSlot];
                UpgradeWeaponAward upgrader = _lastSavedSelectableItem as UpgradeWeaponAward;
                _uiManager.UpdateUpgradeWeaponText(Gold.HasCount(curWeapon.GetUpgradePrice()), upgrader, curWeapon);
                break;
        }
        SetActiveTextTypes(true);
    }

    private void ExitSelectableItem()
    {
        if (_uiManager.GetActiveText(TextTypes.Select))
            _uiManager.SetActiveText(TextTypes.Select, false);
        if (_lastSavedSelectableItem != null)
        {
            SetActiveTextTypes(false);
            _lastSavedSelectableItem = null;
        }
    }

    private void SetActiveTextTypes(bool active)
    {
        switch (_lastSavedSelectableItem.ItemType)
        {
            case SelectableItems.Weapon:
                _uiManager.SetActiveText(TextTypes.NewWeapon, active);
                break;
            case SelectableItems.Module:
                _uiManager.SetActiveText(TextTypes.NewModule, active);
                break;
            case SelectableItems.ActiveSkill:
                _uiManager.SetActiveText(TextTypes.NewActiveSkill, active);
                break;
            case SelectableItems.HealthAward:
                _uiManager.SetActiveText(TextTypes.BuyHealth, active);
                break;
            case SelectableItems.UpgradeWeapon:
                _uiManager.SetActiveText(TextTypes.UpdateWeapon, active);
                break;
        }
    }

    private void UseActiveSkill()
    {
        if (Input.GetMouseButtonDown(1))
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

    public List<WeaponModule> GetFreeWeaponModules() => _freeWeaponModules;
    public List<BionicModule> GetFreeBionicModules() => _freeBionicModules;
    public List<BionicModule> GetInstalledBionicModules() => _installedBionicModules;

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
        _activeSkill.Init(_uiManager.GetActiveSkillReloadImage(), Camera.main.transform, this);
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
            movement = module.ApplyBehaviours(movement, info);
        }
        return movement;
    }

    


    public void SetDashInfo(float dashReload, int dashCharges)
    {
        _infoInterface.SetDashInfo(dashReload, dashCharges);
    }
}