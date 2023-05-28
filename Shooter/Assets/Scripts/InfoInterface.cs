using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoInterface : MonoBehaviour
{
    [SerializeField] private Image[] weaponsIcon;
    [SerializeField] private Image[] skillIcon;
    [SerializeField] private Image mapMini;

    public void UpdateInfoIcon(InfoIconEnum infoIconEnum ,Image newIcon, int number)
    {
        switch (infoIconEnum)
        {
            case InfoIconEnum.WeaponsIcon:
                weaponsIcon[number].sprite = newIcon.sprite;
                weaponsIcon[number].color = newIcon.color;
                break;
            case InfoIconEnum.SkillIcon:
                skillIcon[number].sprite = newIcon.sprite;
                skillIcon[number].color = newIcon.color;
                break;
        }
    }

    public enum InfoIconEnum
    {
        WeaponsIcon,
        SkillIcon
    }
}
