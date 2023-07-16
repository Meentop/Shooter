using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject selectText;
    [SerializeField] private GameObject newWeaponTextHolder;
    [SerializeField] private GameObject newModifierTextHolder;
    [SerializeField] private GameObject buyHealthHolder;
    [SerializeField] private GameObject newActiveSkillHolder;
    [SerializeField] private Color normalBuyColor, notEnoughColor;
    [SerializeField] private Weapon.Description newWeaponDescriptions;
    [SerializeField] private Modifier.Info newModifierInfo;
    [SerializeField] private ActiveSkill.Info newActiveSkillInfo;
    [Header("Buy Health")]
    [SerializeField] private Text buyHealthPrice;
    [SerializeField] private Text buyHealthCount;
    [SerializeField] private GameObject buyHealthWasUsed;
    [Header("Other")]
    [SerializeField] private InfoInterface _infoInterface;
    [SerializeField] private DynamicUI _dinemicInterface;

    public InfoInterface infoInterface { get { return _infoInterface; } private set { _infoInterface = value; } }
    public DynamicUI dinemicInterface { get { return _dinemicInterface; } private set { _dinemicInterface = value; } }    

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
            case TextTypes.NewModifierTextHolder:
                newModifierTextHolder.SetActive(isActive);
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
            case TextTypes.NewModifierTextHolder:
                return newModifierTextHolder.activeInHierarchy;
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
        newWeaponDescriptions.BuyPanel.SetActive(!weapon.bought);
        newWeaponDescriptions.WeaponNameText.text = weapon.GetName();
        newWeaponDescriptions.DamageText.text = "Damage " + weapon.GetDamage().ToString();
        newWeaponDescriptions.FiringSpeed.text = "FiringSpeed " + weapon.GetFiringSpeed().ToString();
        newWeaponDescriptions.PriceText.text = weapon.GetPrice().ToString();
        newWeaponDescriptions.PriceText.color = hasGold ? normalBuyColor : notEnoughColor;
    }

    public void UpdateNewModifierInfo(bool hasGold, Modifier modifier)
    {
        newModifierInfo.Image.sprite = modifier.GetSprite();
        newModifierInfo.Title.text = modifier.GetTitle();
        newModifierInfo.Description.text = modifier.GetDescription();
        newModifierInfo.Price.text = modifier.GetPrice().ToString();
        newModifierInfo.Price.color = hasGold ? normalBuyColor : notEnoughColor;
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
}


public enum TextTypes
{
    SelectText,
    NewWeaponTextHolder,
    NewModifierTextHolder,
    NewActiveSkillTextHolder,
    BuyHealthHolder
}