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

    private Weapon _currentWeapon;
    private ActiveSkill _activeSkill;
    private int _selectedSlot;
    private bool _isScrolling;
    private ISelectableItem _lastSavedSelectableItem;
    private List<WeaponModule> _freeWeaponModules = new List<WeaponModule>();
    private List<BionicModule> _freeBionicModules = new List<BionicModule>();
    private List<BionicModule> _installedBionicModules = new List<BionicModule>();
    private InfoInterface _infoInterface;
    private DynamicUI _dynamicInterface;
    private UIManager _uiManager;

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
        _currentWeapon = weapons[0];
        _infoInterface = uiManager.infoInterface;
        _dynamicInterface = uiManager.dinemicInterface;
        _uiManager = uiManager;
        _currentWeapon.Init(this, weaponHolder, targetLook, uiManager.infoInterface, _selectedSlot);
        _dynamicInterface.Init(this, cameraController, mainCamera, canvas);
    }

    public void SelectWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _selectedSlot = 0;
            SwapWeapon(weapons[_selectedSlot]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _selectedSlot = 1;
            SwapWeapon(weapons[_selectedSlot]);
        }
    }
    
    private void SwapWeapon(Weapon weaponToSwap)
    {
        if (weaponToSwap == _currentWeapon)
            return;

        _currentWeapon.transform.gameObject.SetActive(false);
        _currentWeapon = weaponToSwap;
        _currentWeapon.transform.gameObject.SetActive(true);
        _currentWeapon.Init(this, weaponHolder, targetLook, _infoInterface, _selectedSlot);
        _infoInterface.DiscardWeaponsIcon(CollectionsExtensions.GetNextIndex(weapons, _selectedSlot));
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
            _selectedSlot = CollectionsExtensions.GetNextIndex(weapons, _selectedSlot);
            SwapWeapon(weapons[_selectedSlot]);
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
                    if (Gold.HasCount(selectWeapon.GetPrice()) || selectWeapon.bought)
                    {
                        if(!selectWeapon.bought)
                            Gold.Remove(selectWeapon.GetPrice());
                        _currentWeapon.ConnectToStand(selectWeapon.transform.parent.parent);
                        _currentWeapon = selectWeapon;
                        weapons[_selectedSlot] = _currentWeapon;
                        _currentWeapon.Init(this, weaponHolder, targetLook, _infoInterface, _selectedSlot);
                        _currentWeapon.ConectToPlayer();
                    }
                    break;
                case SelectableItems.Module:
                    Module selectModule = _lastSavedSelectableItem as Module;
                    if (Gold.HasCount(selectModule.GetPrice()))
                    {
                        if (selectModule.transform.parent.parent.TryGetComponent<ModifierAward>(out var modifierAward))
                            modifierAward.DeleteOtherModifiers(selectModule.transform.parent);
                        Gold.Remove(selectModule.GetPrice());
                        AddFreeModule(selectModule);
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
        if (!_uiManager.GetActiveText(TextTypes.SelectText))
            _uiManager.SetActiveText(TextTypes.SelectText, true);
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
        }
        SetActiveTextTypes(true);
    }

    private void ExitSelectableItem()
    {
        if (_uiManager.GetActiveText(TextTypes.SelectText))
            _uiManager.SetActiveText(TextTypes.SelectText, false);
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
                _uiManager.SetActiveText(TextTypes.NewWeaponTextHolder, active);
                break;
            case SelectableItems.Module:
                _uiManager.SetActiveText(TextTypes.NewModuleTextHolder, active);
                break;
            case SelectableItems.ActiveSkill:
                _uiManager.SetActiveText(TextTypes.NewActiveSkillTextHolder, active);
                break;
            case SelectableItems.HealthAward:
                _uiManager.SetActiveText(TextTypes.BuyHealthHolder, active);
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
            print("error active skill not found");
        }
    }
}
