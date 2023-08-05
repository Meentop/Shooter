using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoInterface : MonoBehaviour
{
    [SerializeField] private Image[] weaponsFillerIcon;
    [SerializeField] private Image[] weaponsIconHoldear;
    [SerializeField] private Image[] skillIcon;
    [SerializeField] private Image miniMap;
    [SerializeField] private Image activeWeaponsIcon;
    [SerializeField] private Image inactiveWeaponsIcon;
    [SerializeField] private Image dashReloadIcon;
    [SerializeField] private Text dashCharges;

    public void UpdateInfoIcon(InfoIconEnum infoIconEnum, Image newIcon, int number)
    {
        switch (infoIconEnum)
        {
            case InfoIconEnum.SelectWeaponsIcon:
                weaponsFillerIcon[number].sprite = newIcon.sprite;
                weaponsFillerIcon[number].color = newIcon.color;
                weaponsIconHoldear[number].color = activeWeaponsIcon.color;
                break;
            case InfoIconEnum.SkillIcon:
                skillIcon[number].sprite = newIcon.sprite;
                skillIcon[number].color = newIcon.color;
                break;
            case InfoIconEnum.DiscardWeaponsIcon:
                
                break;
        }
    }

    public void DiscardWeaponsIcon(int number)
    {
        weaponsIconHoldear[number].color = inactiveWeaponsIcon.color;
    }

    public void SetDashInfo(float dashReload, int dashCharges)
    {
        dashReloadIcon.fillAmount = dashReload;
        this.dashCharges.text = dashCharges.ToString();
    }

    public enum InfoIconEnum
    {
        SelectWeaponsIcon,
        DiscardWeaponsIcon,
        SkillIcon
    }
}
