using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject selectText;
    [SerializeField] private GameObject newWeaponTextHolder;
    [SerializeField] private GameObject newModuleTextHolder;
    [SerializeField] private GameObject buyHealthHolder;
    [SerializeField] private GameObject newActiveSkillHolder;
    [SerializeField] private Color normalBuyColor, notEnoughColor;
    [SerializeField] private Weapon.Description newWeaponDescriptions;
    [SerializeField] private GameObject hollowModuleHolderPrefab;
    [SerializeField] private Module.Info newModuleInfo;
    [SerializeField] private ActiveSkill.Info newActiveSkillInfo;
    [SerializeField] private Image activeSkillReloadImage;
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
            case TextTypes.SelectText:
                selectText.SetActive(isActive);
                break;
            case TextTypes.NewWeaponTextHolder:
                newWeaponTextHolder.SetActive(isActive);
                break;
            case TextTypes.NewModuleTextHolder:
                newModuleTextHolder.SetActive(isActive);
                break;
            case TextTypes.NewActiveSkillTextHolder:
                newActiveSkillHolder.SetActive(isActive);
                break;
            case TextTypes.BuyHealthHolder: 
                buyHealthHolder.SetActive(isActive);
                break;
        }
    }

    public bool GetActiveText(TextTypes buttonTypes)
    {
        switch (buttonTypes)
        {
            case TextTypes.SelectText:
                return selectText.activeInHierarchy;
            case TextTypes.NewWeaponTextHolder:
                return newWeaponTextHolder.activeInHierarchy;
            case TextTypes.NewModuleTextHolder:
                return newModuleTextHolder.activeInHierarchy;
            case TextTypes.NewActiveSkillTextHolder:
                return newActiveSkillHolder.activeInHierarchy;
            case TextTypes.BuyHealthHolder:
                return buyHealthHolder.activeInHierarchy;
            default:
                return false;
        }
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

    public Image GetActiveSkillReloadImage() => activeSkillReloadImage;
}


public enum TextTypes
{
    SelectText,
    NewWeaponTextHolder,
    NewModuleTextHolder,
    NewActiveSkillTextHolder,
    BuyHealthHolder
}