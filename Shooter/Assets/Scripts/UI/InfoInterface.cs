using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoInterface : MonoBehaviour
{
    [SerializeField] private Image[] weaponsIcon;
    [SerializeField] private Image[] weaponsIconHoldear;
    [SerializeField] private Color activeWeaponColor;
    [SerializeField] private Color inactiveWeaponColor;
    [SerializeField] private Image dashReloadIcon;
    [SerializeField] private Text dashCharges;

    public void SetWeaponIcon(Sprite sprite, int number)
    {
        weaponsIcon[number].sprite = sprite;
    }

    public void SetActiveWeaponIcon(bool active, int number)
    {
        weaponsIconHoldear[number].color = active ? activeWeaponColor : inactiveWeaponColor;
    }

    public void SetDashInfo(float dashReload, int dashCharges)
    {
        dashReloadIcon.fillAmount = dashReload;
        this.dashCharges.text = dashCharges.ToString();
    }
}
