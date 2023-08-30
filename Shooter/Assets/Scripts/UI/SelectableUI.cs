using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableUI : MonoBehaviour
{
    [SerializeField] private Text selectText;
    [SerializeField] private GameObject[] selectablesUI;
    [SerializeField] private Color normalBuyColor, notEnoughColor;
    [SerializeField] private Weapon.Description newWeaponDescriptions;
    [SerializeField] private GameObject hollowModuleHolderPrefab;
    [SerializeField] private Module.Info newModuleInfo;
    [SerializeField] private ActiveSkill.Info newActiveSkillInfo;
    [SerializeField] private Weapon.Description curWeaponDescriptions;
    [SerializeField] private Weapon.Description upgradedWeaponDescriptions;
    [SerializeField] private GameObject weaponUpgraderBlockPanel;
    [SerializeField] private Text weaponUpgraderBlockPanelText;
    [SerializeField] private Transform modulesHolder;
    [SerializeField] private HollowModuleUI hollowModulePrefab;
    [SerializeField] private Module.Info upgardedModuleInfo;
    [SerializeField] private GameObject moduleUpgraderUsedPanel;
    [SerializeField] private GameObject moduleUpgraderMaxUpgradePanel;
    [SerializeField] private GameObject moduleUpgraderBuyPanel;
    [Header("Buy Health")]
    [SerializeField] private Text buyHealthPrice;
    [SerializeField] private Text buyHealthCount;
    [SerializeField] private GameObject buyHealthWasUsed;

    public void SetActiveSelectableUI(SelectableUIType buttonTypes, bool isActive)
    {
        if (!PauseManager.Pause)
            selectablesUI[(int)buttonTypes].SetActive(isActive);
    }

    public bool GetActiveSelectableUI(SelectableUIType buttonTypes)
    {
        return selectablesUI[(int)buttonTypes].activeInHierarchy;
    }

    public void DisableAllSelectablesUI()
    {
        foreach (var element in selectablesUI)
        {
            element.SetActive(false);
        }
    }

    public void SetSelectText(string text)
    {
        selectText.text = text;
    }

    public void UpdateNewWeaponUI(bool hasGold, Weapon weapon)
    {
        newWeaponDescriptions.BuyPanel.SetActive(!weapon.Bought);
        newWeaponDescriptions.Image.sprite = weapon.GetSprite();
        newWeaponDescriptions.NameText.text = weapon.GetName();
        newWeaponDescriptions.DamageText.text = "Damage " + weapon.GetDamage().ToString();
        newWeaponDescriptions.FiringSpeed.text = "FiringSpeed " + weapon.GetFiringSpeed().ToString();
        foreach (Transform modulesHolder in newWeaponDescriptions.ModulesHolder)
        {
            Destroy(modulesHolder.gameObject);
        }
        for (int j = 0; j < weapon.GetMaxNumbersOfModules(); j++)
        {
            Instantiate(hollowModuleHolderPrefab, newWeaponDescriptions.ModulesHolder);
        }
        newWeaponDescriptions.PriceText.text = weapon.GetPrice().ToString();
        newWeaponDescriptions.PriceText.color = hasGold ? normalBuyColor : notEnoughColor;
    }

    public void UpdateNewModuleUI(bool hasGold, Module module)
    {
        if (module.GetType() == typeof(WeaponModule))
            newModuleInfo.Type.text = "WEAPON MODULE";
        else if (module.GetType() == typeof(BionicModule))
            newModuleInfo.Type.text = "BIONIC MODULE";
        newModuleInfo.Image.sprite = module.GetSprite();
        newModuleInfo.Title.text = module.GetTitle(module.Level);
        newModuleInfo.Description.text = module.GetDescription(module.Level);
        newModuleInfo.Price.text = module.GetPrice().ToString();
        newModuleInfo.Price.color = hasGold ? normalBuyColor : notEnoughColor;
    }

    public void UpdateNewActiveSkillUI(bool hasGold, ActiveSkill skill)
    {
        newActiveSkillInfo.Image.sprite = skill.GetSprite();
        newActiveSkillInfo.Title.text = skill.GetTitle();
        newActiveSkillInfo.Description.text = skill.GetDescription();
        newActiveSkillInfo.DamageToReload.text = "Damage to reload: " + skill.GetDamagaeToReturn().ToString();
        newActiveSkillInfo.Price.text = skill.GetPrice().ToString();
        newActiveSkillInfo.Price.color = hasGold ? normalBuyColor : notEnoughColor;
    }

    public void UpdateBuyHealthUI(bool hasGold, HealthAward healthAward)
    {
        buyHealthPrice.text = healthAward.GetPrice().ToString();
        buyHealthPrice.color = hasGold ? normalBuyColor : notEnoughColor;
        buyHealthCount.text = healthAward.GetAddHealth().ToString();
        buyHealthWasUsed.SetActive(healthAward.IsUsed());
    }

    public void UpdateUpgradeWeaponUI(bool hasGold, WeaponUpgradeAward upgrader, Weapon weapon)
    {
        curWeaponDescriptions.Image.sprite = weapon.GetSprite();
        curWeaponDescriptions.NameText.text = weapon.GetName();
        curWeaponDescriptions.DamageText.text = "Damage " + weapon.GetDamage().ToString();
        curWeaponDescriptions.FiringSpeed.text = "FiringSpeed " + weapon.GetFiringSpeed().ToString();
        foreach (Transform modulesHolder in curWeaponDescriptions.ModulesHolder)
        {
            Destroy(modulesHolder.gameObject);
        }
        for (int j = 0; j < weapon.GetMaxNumbersOfModules(); j++)
        {
            Instantiate(hollowModuleHolderPrefab, curWeaponDescriptions.ModulesHolder);
        }

        bool cantUpgrade = upgrader.IsUsed() || !weapon.CouldBeUpgraded();
        upgradedWeaponDescriptions.BuyPanel.SetActive(!cantUpgrade);

        weaponUpgraderBlockPanel.SetActive(cantUpgrade);
        if (upgrader.IsUsed())
            weaponUpgraderBlockPanelText.text = "USED";
        if (!weapon.CouldBeUpgraded())
            weaponUpgraderBlockPanelText.text = "MAX UPGRADE";
        if (cantUpgrade)
        {
            upgradedWeaponDescriptions.Image.sprite = weapon.GetSprite();
            upgradedWeaponDescriptions.NameText.text = weapon.GetName();
            upgradedWeaponDescriptions.DamageText.text = "Damage " + weapon.GetDamage().ToString();
            upgradedWeaponDescriptions.FiringSpeed.text = "FiringSpeed " + weapon.GetFiringSpeed().ToString();
            foreach (Transform modulesHolder in upgradedWeaponDescriptions.ModulesHolder)
            {
                Destroy(modulesHolder.gameObject);
            }
            for (int j = 0; j < weapon.GetMaxNumbersOfModules(); j++)
            {
                Instantiate(hollowModuleHolderPrefab, upgradedWeaponDescriptions.ModulesHolder);
            }
            return;
        }

        upgradedWeaponDescriptions.Image.sprite = weapon.GetSprite();
        upgradedWeaponDescriptions.NameText.text = weapon.GetUpgradedName();
        upgradedWeaponDescriptions.DamageText.text = "Damage " + weapon.GetUpgradedDamage().ToString();
        upgradedWeaponDescriptions.FiringSpeed.text = "FiringSpeed " + weapon.GetFiringSpeed().ToString();
        foreach (Transform modulesHolder in upgradedWeaponDescriptions.ModulesHolder)
        {
            Destroy(modulesHolder.gameObject);
        }
        for (int j = 0; j < weapon.GetUpgradedMaxNumbersOfModules(); j++)
        {
            Instantiate(hollowModuleHolderPrefab, upgradedWeaponDescriptions.ModulesHolder);
        }
        upgradedWeaponDescriptions.PriceText.text = weapon.GetUpgradePrice().ToString();
        upgradedWeaponDescriptions.PriceText.color = hasGold ? normalBuyColor : notEnoughColor;
    }

    public void EnableUpgradeModuleUI(bool hasGold, ModuleUpgradeAward upgrader, List<Module> modules)
    {
        selectedModuleIndex = 0;
        foreach (Transform child in modulesHolder)
        {
            Destroy(child.gameObject);
        }
        hollowModules.Clear();
        foreach (var module in modules)
        {
            HollowModuleUI moduleUI = Instantiate(hollowModulePrefab, modulesHolder);
            hollowModules.Add(moduleUI);
            moduleUI.Init(module.GetSprite(), module.GetTitle(module.Level), module.GetDescription(module.Level), module);
        }

        moduleUpgraderUsedPanel.SetActive(upgrader.IsUsed());
    }

    int selectedModuleIndex;
    List<HollowModuleUI> hollowModules = new List<HollowModuleUI>();
    public void UpdateUpgradeModuleUI(ModuleUpgradeAward upgrader)
    {
        if (hollowModules.Count > 0 && !upgrader.IsUsed())
        {
            foreach (var module in hollowModules)
            {
                module.SetEnable(false);
            }
            hollowModules[selectedModuleIndex].SetEnable(true);

            if (Input.mouseScrollDelta.y > 0)
            {
                selectedModuleIndex--;
                if (selectedModuleIndex < 0)
                    selectedModuleIndex = hollowModules.Count - 1;
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                selectedModuleIndex++;
                if (selectedModuleIndex > hollowModules.Count - 1)
                    selectedModuleIndex = 0;
            }
            Module selectedModule = hollowModules[selectedModuleIndex].Module;
            upgardedModuleInfo.Image.sprite = selectedModule.GetSprite();
            if (selectedModule.CouldBeUpgraded())
            {
                upgardedModuleInfo.Title.text = selectedModule.GetTitle(selectedModule.Level + 1);
                upgardedModuleInfo.Description.text = selectedModule.GetDescription(selectedModule.Level + 1);
                upgardedModuleInfo.Price.text = "10";
            }
            else
            {
                upgardedModuleInfo.Title.text = selectedModule.GetTitle(selectedModule.Level);
                upgardedModuleInfo.Description.text = selectedModule.GetDescription(selectedModule.Level);
            }

            moduleUpgraderMaxUpgradePanel.SetActive(!selectedModule.CouldBeUpgraded());
            moduleUpgraderBuyPanel.SetActive(selectedModule.CouldBeUpgraded());
        }
        else
        {
            moduleUpgraderBuyPanel.SetActive(false);
        }
    }

    public Module GetSelectedModule() => hollowModules[selectedModuleIndex].Module;
}
