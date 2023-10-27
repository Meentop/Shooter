using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    [SerializeField] private float selectDistance = 4f;
    [SerializeField] private PriceConfig priceConfig;
    [SerializeField] private WeaponConfig weaponConfig;
    [SerializeField] private WeaponModuleConfig weaponModuleConfig;
    [SerializeField] private BionicModuleConfig bionicModuleConfig;
    [SerializeField] private ActiveSkillConfig activeSkillConfig;

    private ISelectableItem _lastSavedSelectableItem;
    private Player _player;
    private UIManager _uiManager;
    private CameraController _cameraController;

    public void Init(Player player, UIManager uIManager, CameraController cameraController)
    {
        _player = player;
        _uiManager = uIManager;
        _cameraController = cameraController;
    }

    private void Update()
    {
        if (!PauseManager.Pause)
        {
            SelectableItemsDetection();
            SelectItem();
        }
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
                    StandWeapon selectWeapon = _lastSavedSelectableItem as StandWeapon;
                    int weaponPrice = priceConfig.GetWeaponPrice(weaponConfig.GetIndex(selectWeapon.GetWeaponCharacteristics()), selectWeapon.GetWeapon().GetLevel());
                    if (_player.Gold.HasCount(weaponPrice) || selectWeapon.Bought)
                    {
                        if (!selectWeapon.Bought)
                            _player.Gold.Remove(weaponPrice);
                        //_player.AddWeaponFromStand(selectWeapon);
                    }
                    break;
                case SelectableItems.Module:
                    Module selectModule = _lastSavedSelectableItem as Module;
                    int modulePrice = 0;
                    if (selectModule is WeaponModule) modulePrice = priceConfig.GetWeaponModulePrice(selectModule.Level);
                    if (selectModule is BionicModule) modulePrice = priceConfig.GetBionicModulePrice(selectModule.Level);
                    if (_player.Gold.HasCount(modulePrice))
                    {
                        if (selectModule.transform.parent.parent.TryGetComponent<WeaponModuleAward>(out var weaponModuleAward))
                            weaponModuleAward.DeleteOtherModules(selectModule.transform.parent);
                        if (selectModule.transform.parent.parent.TryGetComponent<BionicModuleAward>(out var bionicModuleAward))
                            bionicModuleAward.DeleteOtherModules(selectModule.transform.parent);
                        _player.Gold.Remove(modulePrice);
                        _player.AddFreeModule(selectModule);
                        if (!selectModule.undestroyable)
                            Destroy(selectModule.gameObject);
                    }
                    break;
                case SelectableItems.ActiveSkill:
                    ActiveSkill selectSkill = _lastSavedSelectableItem as ActiveSkill;
                    int skillPrice = priceConfig.GetActiveSkillPrice(activeSkillConfig.GetIndex(selectSkill));
                    if (_player.Gold.HasCount(skillPrice))
                    {
                        if (selectSkill.transform.parent.parent.TryGetComponent<ActiveSkillAward>(out var skillAward))
                            skillAward.DeleteOtherSkills(selectSkill.transform.parent);
                        _player.Gold.Remove(skillPrice);
                        _player.AddActiveSkill(selectSkill);
                        Destroy(selectSkill.gameObject);
                    }
                    break;
                case SelectableItems.HealthAward:
                    HealthAward healthAward = _lastSavedSelectableItem as HealthAward;
                    int healingPrice = priceConfig.GetHealingPrice(0);
                    if (!healthAward.IsUsed() && _player.Gold.HasCount(healingPrice))
                    {
                        _player.Health.AddHealth(healthAward.GetAddHealth());
                        _player.Gold.Remove(healingPrice);
                        healthAward.SetWasUsed();
                    }
                    break;
                case SelectableItems.WeaponUpgrade:
                    Weapon curWeapon = _player.GetSelectedWeapon();
                    WeaponUpgradeAward weaponUpgrader = _lastSavedSelectableItem as WeaponUpgradeAward;
                    int weaponUpgradePrice = priceConfig.GetWeaponUpgradePrice(weaponConfig.GetIndex(curWeapon.GetWeaponCharacteristics()), curWeapon.GetLevel());
                    if (_player.Gold.HasCount(weaponUpgradePrice) && !weaponUpgrader.IsUsed() && curWeapon.CouldBeUpgraded())
                    {
                        _player.Gold.Remove(weaponUpgradePrice);
                        curWeapon.UpgradeWeapon();
                        weaponUpgrader.SetWasUsed();
                    }
                    break;
                case SelectableItems.ModuleUpgrade:
                    Module curModule = _uiManager.SelectadleUI.GetSelectedModule();
                    ModuleUpgradeAward moduleUpgrader = _lastSavedSelectableItem as ModuleUpgradeAward;
                    int upgradeModulePrice = 0;
                    if (curModule as WeaponModule) upgradeModulePrice = priceConfig.GetWeaponModuleUpgradePrice(curModule.Level);
                    if (curModule as BionicModule) upgradeModulePrice = priceConfig.GetBionicModuleUpgradePrice(curModule.Level);
                    if (_player.Gold.HasCount(upgradeModulePrice) && !moduleUpgrader.IsUsed() && curModule.CouldBeUpgraded())
                    {
                        _player.Gold.Remove(upgradeModulePrice);
                        curModule.UpgradeModule();
                        moduleUpgrader.SetWasUsed();
                        _uiManager.SelectadleUI.EnableUpgradeModuleUI(_player.Gold.HasCount(upgradeModulePrice), moduleUpgrader, _player.GetAllModules());
                        _player.SetCharacteristics();
                    }
                    break;
                case SelectableItems.TerminalButton:
                    _uiManager.TerminalUI.Init(_lastSavedSelectableItem as Terminal);
                    _uiManager.TerminalUI.SetActiveTerminalPanel(true);
                    _cameraController.UnlockCursor();
                    PauseManager.Pause = true;
                    break;
            }
            _lastSavedSelectableItem.OnSelect(_player);
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
                Weapon selectWeapon = _lastSavedSelectableItem as Weapon;
                int weaponPrice = priceConfig.GetWeaponPrice(weaponConfig.GetIndex(selectWeapon.GetWeaponCharacteristics()), selectWeapon.GetLevel());
                _uiManager.SelectadleUI.UpdateNewWeaponUI(weaponPrice, _player.Gold.HasCount(weaponPrice), selectWeapon);
                break;
            case SelectableItems.Module:
                Module selectModule = _lastSavedSelectableItem as Module;
                int modulePrice = 0;
                if (selectModule as WeaponModule) modulePrice = priceConfig.GetWeaponModulePrice(selectModule.Level);
                if (selectModule as BionicModule) modulePrice = priceConfig.GetBionicModulePrice(selectModule.Level);
                _uiManager.SelectadleUI.UpdateNewModuleUI(modulePrice, _player.Gold.HasCount(modulePrice), selectModule);
                break;
            case SelectableItems.ActiveSkill:
                ActiveSkill selectSkill = _lastSavedSelectableItem as ActiveSkill;
                int skillPrice = priceConfig.GetActiveSkillPrice(activeSkillConfig.GetIndex(selectSkill));
                _uiManager.SelectadleUI.UpdateNewActiveSkillUI(skillPrice, _player.Gold.HasCount(skillPrice), selectSkill);
                break;
            case SelectableItems.HealthAward:
                HealthAward healthAward = _lastSavedSelectableItem as HealthAward;
                int healingPrice = priceConfig.GetHealingPrice(0);
                _uiManager.SelectadleUI.UpdateBuyHealthUI(healingPrice, _player.Gold.HasCount(healingPrice), healthAward);
                break;
            case SelectableItems.WeaponUpgrade:
                Weapon curWeapon = _player.GetSelectedWeapon();
                WeaponUpgradeAward weapoUpgrader = _lastSavedSelectableItem as WeaponUpgradeAward;
                int weaponUpgradePrice = priceConfig.GetWeaponUpgradePrice(weaponConfig.GetIndex(curWeapon.GetWeaponCharacteristics()), curWeapon.GetLevel());
                _uiManager.SelectadleUI.UpdateUpgradeWeaponUI(weaponUpgradePrice, _player.Gold.HasCount(weaponUpgradePrice), weapoUpgrader, curWeapon);
                break;
            case SelectableItems.ModuleUpgrade:
                ModuleUpgradeAward moduleUpgrader = _lastSavedSelectableItem as ModuleUpgradeAward;
                _uiManager.SelectadleUI.UpdateUpgradeModuleUI(priceConfig, moduleUpgrader);
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
                _uiManager.SelectadleUI.EnableUpgradeModuleUI(_player.Gold.HasCount(10), moduleUpgrader, _player.GetAllModules());
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


    public bool IsOverModuleUpgrade()
    {
        return _lastSavedSelectableItem != null && _lastSavedSelectableItem as ModuleUpgradeAward;
    }
}
