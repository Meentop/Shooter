using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableUI : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Text selectText;
    [SerializeField] private GameObject[] selectablesUI;
    [SerializeField] private Color normalBuyColor, notEnoughColor;
    [Header("New Weapon")]
    [SerializeField] private Weapon.Description newWeaponDescriptions;
    [SerializeField] private GameObject hollowModuleHolderPrefab;
    [Header("New Module")]
    [SerializeField] private Module.Info newModuleInfo;
    [Header("New Active Skill")]
    [SerializeField] private ActiveSkill.Info newActiveSkillInfo;
    [Header("Buy Health")]
    [SerializeField] private Text buyHealthPrice;
    [SerializeField] private Text buyHealthCount;
    [SerializeField] private GameObject buyHealthWasUsed;
    [Header("Weapon Upgrade")]
    [SerializeField] private Weapon.Description curWeaponDescriptions;
    [SerializeField] private Weapon.Description upgradedWeaponDescriptions;
    [SerializeField] private GameObject weaponUpgraderBlockPanel;
    [SerializeField] private Text weaponUpgraderBlockPanelText;
    [Header("Module Upgrade")]
    [SerializeField] private Transform modulesHolder;
    [SerializeField] private HollowModuleUI hollowModulePrefab;
    [SerializeField] private Module.Info upgradedModuleInfo;
    [SerializeField] private GameObject moduleUpgraderUsedPanel;
    [SerializeField] private GameObject moduleUpgraderMaxUpgradePanel;
    [SerializeField] private GameObject moduleUpgraderBuyPanel;
    [SerializeField] private RectTransform moduleHolderTopPoint, moduleHolderBottomPoint;
    
    int selectedModuleIndex;
    List<HollowModuleUI> hollowModules = new List<HollowModuleUI>();

    //General

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

    //New Weapon

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

    //New Module

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

    //New Active Skill

    public void UpdateNewActiveSkillUI(bool hasGold, ActiveSkill skill)
    {
        newActiveSkillInfo.Image.sprite = skill.GetSprite();
        newActiveSkillInfo.Title.text = skill.GetTitle();
        newActiveSkillInfo.Description.text = skill.GetDescription();
        newActiveSkillInfo.DamageToReload.text = "Damage to reload: " + skill.GetDamagaeToReturn().ToString();
        newActiveSkillInfo.Price.text = skill.GetPrice().ToString();
        newActiveSkillInfo.Price.color = hasGold ? normalBuyColor : notEnoughColor;
    }

    //Buy health

    public void UpdateBuyHealthUI(bool hasGold, HealthAward healthAward)
    {
        buyHealthPrice.text = healthAward.GetPrice().ToString();
        buyHealthPrice.color = hasGold ? normalBuyColor : notEnoughColor;
        buyHealthCount.text = healthAward.GetAddHealth().ToString();
        buyHealthWasUsed.SetActive(healthAward.IsUsed());
    }

    //Weapon Upgrade

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

    //Module Upgrade

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

        RectTransform modulesHolderRect = modulesHolder.GetComponent<RectTransform>();
        modulesHolderRect.sizeDelta = new Vector2(modulesHolderRect.sizeDelta.x, (hollowModulePrefab.GetComponent<RectTransform>().sizeDelta.y * modules.Count) + (5 * modules.Count - 1));

        moduleUpgraderUsedPanel.SetActive(upgrader.IsUsed());
        upgradedModuleInfo.Price.color = hasGold ? normalBuyColor : notEnoughColor;
    }

    
    public void UpdateUpgradeModuleUI(ModuleUpgradeAward upgrader)
    {
        if (hollowModules.Count > 0 && !upgrader.IsUsed())
        {
            DisableAllModules();
            SelectNextOrPreviousModule();
            DisplaySelectedModuleInfo(upgrader);

            SetModulesListPosition();

            UpdateModuleUpgraderPanels(upgrader);
        }
        else
        {
            moduleUpgraderBuyPanel.SetActive(false);
        }
    }

    private void DisableAllModules()
    {
        foreach (var module in hollowModules)
        {
            module.SetEnable(false);
        }
        hollowModules[selectedModuleIndex].SetEnable(true);
    }

    private void SelectNextOrPreviousModule()
    {
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
    }

    private void DisplaySelectedModuleInfo(ModuleUpgradeAward upgrader)
    {
        Module selectedModule = hollowModules[selectedModuleIndex].Module;
        upgradedModuleInfo.Image.sprite = selectedModule.GetSprite();

        if (selectedModule.CouldBeUpgraded())
        {
            upgradedModuleInfo.Title.text = selectedModule.GetTitle(selectedModule.Level + 1);
            upgradedModuleInfo.Description.text = selectedModule.GetDescription(selectedModule.Level + 1);
            upgradedModuleInfo.Price.text = "10";
        }
        else
        {
            upgradedModuleInfo.Title.text = selectedModule.GetTitle(selectedModule.Level);
            upgradedModuleInfo.Description.text = selectedModule.GetDescription(selectedModule.Level);
        }
    }

    private void UpdateModuleUpgraderPanels(ModuleUpgradeAward upgrader)
    {
        Module selectedModule = hollowModules[selectedModuleIndex].Module;
        moduleUpgraderMaxUpgradePanel.SetActive(!selectedModule.CouldBeUpgraded());
        moduleUpgraderBuyPanel.SetActive(selectedModule.CouldBeUpgraded());
    }

    private void SetModulesListPosition()
    {
        RectTransform modulesHolder = this.modulesHolder.GetComponent<RectTransform>();
        RectTransform module = hollowModules[selectedModuleIndex].GetComponent<RectTransform>();

        float modulePos = CalculatePositionInModuleList(module, modulesHolder);
        float topViewPos = CalculatePositionInView(moduleHolderTopPoint, modulesHolder);
        float bottomViewPos = CalculatePositionInView(moduleHolderBottomPoint, modulesHolder);

        if (Mathf.Abs(moduleHolderBottomPoint.anchoredPosition.y - moduleHolderTopPoint.anchoredPosition.y) < modulesHolder.sizeDelta.y)
            AdjustModulesListPosition(modulesHolder, module, modulePos, topViewPos, bottomViewPos);
    }

    private float CalculatePositionInModuleList(RectTransform module, RectTransform modulesHolder)
    {
        return Mathf.Abs((module.anchoredPosition.y / modulesHolder.sizeDelta.y) + 1);
    }

    private float CalculatePositionInView(RectTransform viewPoint, RectTransform modulesHolder)
    {
        float view = viewPoint.anchoredPosition.y - modulesHolder.anchoredPosition.y;
        return Mathf.Abs((view / modulesHolder.sizeDelta.y) + 1);
    }

    private void AdjustModulesListPosition(RectTransform modulesHolder, RectTransform module, float modulePos, float topViewPos, float bottomViewPos)
    {
        if (modulePos > topViewPos)
        {
            float delta = modulePos - topViewPos;
            float bigDelta = (modulesHolder.sizeDelta.y * delta) + (module.sizeDelta.y / 2);
            modulesHolder.anchoredPosition = new Vector2(modulesHolder.anchoredPosition.x, modulesHolder.anchoredPosition.y - bigDelta);
        }
        else if (modulePos < bottomViewPos)
        {
            float delta = bottomViewPos - modulePos;
            float bigDelta = (modulesHolder.sizeDelta.y * delta) + (module.sizeDelta.y / 2);
            modulesHolder.anchoredPosition = new Vector2(modulesHolder.anchoredPosition.x, modulesHolder.anchoredPosition.y + bigDelta);
        }
    }

    public Module GetSelectedModule() => hollowModules[selectedModuleIndex].Module;
}

public enum SelectableUIType
{
    Select,
    NewWeapon,
    NewModule,
    NewActiveSkill,
    BuyHealth,
    WeaponUpgrade,
    ModuleUpgrade
}