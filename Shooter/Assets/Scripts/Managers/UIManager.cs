using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI selectText;
    [SerializeField] private Image newWeaponTextHolder;
    [SerializeField] private Weapon.Description newWeaponDescriptions;
    [SerializeField] private InfoInterface _infoInterface;
    [SerializeField] private DynamicUI _dinemicInterface;

    public InfoInterface infoInterface { get { return _infoInterface; } private set { _infoInterface = value; } }
    public DynamicUI dinemicInterface { get { return _dinemicInterface; } private set { _dinemicInterface = value; } }

    public enum TextTypes
    {
        SelectText,
        NewWeaponTextHolder
    }

    public void SetActiveText(TextTypes buttonTypes, bool isActive)
    {
        switch (buttonTypes)
        {
            case TextTypes.SelectText:
                selectText.gameObject.SetActive(isActive);
                break;
            case TextTypes.NewWeaponTextHolder:
                newWeaponTextHolder.gameObject.SetActive(isActive);
                break;
        }
    }

    public bool GetActiveText(TextTypes buttonTypes)
    {
        switch (buttonTypes)
        {
            case TextTypes.SelectText:
                return selectText.gameObject.activeInHierarchy;
            case TextTypes.NewWeaponTextHolder:
                return newWeaponTextHolder.gameObject.activeInHierarchy;
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
}
