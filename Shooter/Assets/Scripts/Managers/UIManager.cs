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
    [SerializeField] private Weapon.Description newWeaponDescriptions;
    [SerializeField] private Modifier.Info newModifierInfo;
    [Header("Buy Health")]
    [SerializeField] private Text buyHealthPrice;
    [SerializeField] private Text buyHealthCount;
    [SerializeField] private GameObject buyHealthWasUsed;
    [SerializeField] private Color normalColor, notEnoughColor;
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
            case TextTypes.BuyHealthHolder:
                return buyHealthHolder.activeInHierarchy;
            default:
                return false;
        }
    }

    public void UpdateNewWeaponDescription(string weaponName, float damage, float firingSpeed)
    {
        newWeaponDescriptions.WeaponNameText.text = weaponName;
        newWeaponDescriptions.DamageText.text = "Damage " + damage.ToString();
        newWeaponDescriptions.FiringSpeed.text = "FiringSpeed " + firingSpeed.ToString();
    }

    public void UpdateNewModifierInfo(Sprite sprite, string title, string description)
    {
        newModifierInfo.Image.sprite = sprite;
        newModifierInfo.Title.text = title;
        newModifierInfo.Description.text = description;
    }

    public void UpdateBuyHealthText(bool hasGold, int price, int healthCount, bool isUsed)
    {
        buyHealthPrice.text = price.ToString();
        buyHealthPrice.color = hasGold ? normalColor : notEnoughColor;
        buyHealthCount.text = healthCount.ToString();
        buyHealthWasUsed.SetActive(isUsed);
    }
}


public enum TextTypes
{
    SelectText,
    NewWeaponTextHolder,
    NewModifierTextHolder,
    BuyHealthHolder
}