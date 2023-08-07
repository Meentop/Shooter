using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text selectText;
    [SerializeField] private GameObject newWeaponTextHolder;
    [SerializeField] private GameObject newModuleTextHolder;
    [SerializeField] private GameObject buyHealthHolder;
    [SerializeField] private GameObject newActiveSkillHolder;
    [SerializeField] private GameObject upgradeWeaponHolder;
    [SerializeField] private Color normalBuyColor, notEnoughColor;
    [SerializeField] private Weapon.Description newWeaponDescriptions;
    [SerializeField] private GameObject hollowModuleHolderPrefab;
    [SerializeField] private Module.Info newModuleInfo;
    [SerializeField] private ActiveSkill.Info newActiveSkillInfo;
    [SerializeField] private Image activeSkillReloadImage;
    [SerializeField] private Weapon.Description curWeaponDescriptions;
    [SerializeField] private Weapon.Description upgradedWeaponDescriptions;
    [SerializeField] private GameObject weaponUpgraderBlockPanel;
    [SerializeField] private Text weaponUpgraderBlockPanelText;
    [Header("Buy Health")]
    [SerializeField] private Text buyHealthPrice;
    [SerializeField] private Text buyHealthCount;
    [SerializeField] private GameObject buyHealthWasUsed;
    [Header("Other")]
    [SerializeField] private InfoInterface _infoInterface;
    [SerializeField] private ModulesPanelUI _dinemicInterface;

    public InfoInterface infoInterface { get { return _infoInterface; } private set { _infoInterface = value; } }
    public ModulesPanelUI dinemicInterface { get { return _dinemicInterface; } private set { _dinemicInterface = value; } }    

    public void SetActiveText(TextTypes buttonTypes, bool isActive)
    {
        switch (buttonTypes)
        {
            case TextTypes.Select:
                selectText.gameObject.SetActive(isActive);
                break;
            case TextTypes.NewWeapon:
                newWeaponTextHolder.SetActive(isActive);
                break;
            case TextTypes.NewModule:
                newModuleTextHolder.SetActive(isActive);
                break;
            case TextTypes.NewActiveSkill:
                newActiveSkillHolder.SetActive(isActive);
                break;
            case TextTypes.BuyHealth: 
                buyHealthHolder.SetActive(isActive);
                break;
            case TextTypes.UpdateWeapon:
                upgradeWeaponHolder.SetActive(isActive);
                break;
        }
    }

    public bool GetActiveText(TextTypes buttonTypes)
    {
        return buttonTypes switch
        {
            TextTypes.Select => selectText.gameObject.activeInHierarchy,
            TextTypes.NewWeapon => newWeaponTextHolder.activeInHierarchy,
            TextTypes.NewModule => newModuleTextHolder.activeInHierarchy,
            TextTypes.NewActiveSkill => newActiveSkillHolder.activeInHierarchy,
            TextTypes.BuyHealth => buyHealthHolder.activeInHierarchy,
            TextTypes.UpdateWeapon => upgradeWeaponHolder.activeInHierarchy,
            _ => false,
        };
    }

    public void SetSelectText(string text)
    {
        selectText.text = text;
    }

    public void UpdateNewWeaponDescription(bool hasGold, Weapon weapon)
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

    public void UpdateNewModuleInfo(bool hasGold, Module module)
    {
        if (module.GetType() == typeof(WeaponModule))
            newModuleInfo.Type.text = "WEAPON MODULE";
        else if (module.GetType() == typeof(BionicModule))
            newModuleInfo.Type.text = "BIONIC MODULE";
        newModuleInfo.Image.sprite = module.GetSprite();
        newModuleInfo.Title.text = module.GetTitle();
        newModuleInfo.Description.text = module.GetDescription();
        newModuleInfo.Price.text = module.GetPrice().ToString();
        newModuleInfo.Price.color = hasGold ? normalBuyColor : notEnoughColor;
    }

    public void UpdateNewActiveSkillInfo(bool hasGold, ActiveSkill skill)
    {
        newActiveSkillInfo.Image.sprite = skill.GetSprite();
        newActiveSkillInfo.Title.text = skill.GetTitle();
        newActiveSkillInfo.Description.text = skill.GetDescription();
        newActiveSkillInfo.DamageToReload.text = "Damage to reload: " + skill.GetDamagaeToReturn().ToString();
        newActiveSkillInfo.Price.text = skill.GetPrice().ToString();
        newActiveSkillInfo.Price.color = hasGold ? normalBuyColor : notEnoughColor;
    }

    public void UpdateBuyHealthText(bool hasGold, HealthAward healthAward)
    {
        buyHealthPrice.text = healthAward.GetPrice().ToString();
        buyHealthPrice.color = hasGold ? normalBuyColor : notEnoughColor;
        buyHealthCount.text = healthAward.GetAddHealth().ToString();
        buyHealthWasUsed.SetActive(healthAward.IsUsed());
    }

    public void UpdateUpgradeWeaponText(bool hasGold, UpgradeWeaponAward upgrader, Weapon weapon)
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
        if(!weapon.CouldBeUpgraded())
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

    public Image GetActiveSkillReloadImage() => activeSkillReloadImage;
}


public enum TextTypes
{
    Select,
    NewWeapon,
    NewModule,
    NewActiveSkill,
    BuyHealth,
    UpdateWeapon
}