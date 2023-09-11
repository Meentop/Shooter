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
    [SerializeField] private Text goldCount;
    [SerializeField] private Image activeSkill;
    [SerializeField] private Image activeSkillReloadIcon;
    [SerializeField] private GameObject activeSkillBlood;
    [SerializeField] private Text activeSkillBloodPrice;

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

    public void SetGoldCount(int count)
    {
        goldCount.text = count.ToString("D4");
    }

    public void SetActiveSkillSprite(Sprite sprite)
    {
        activeSkill.sprite = sprite;
    }

    public void SetActiveSkillReload(float reload)
    {
        activeSkillReloadIcon.fillAmount = Mathf.Abs(reload - 1);
    }

    public void SetBloodyActiveSkill(bool bloody)
    {
        activeSkillReloadIcon.gameObject.SetActive(!bloody);
        activeSkillBlood.SetActive(bloody);
    }

    public void SetActiveSkillBloodyPrice(int price)
    {
        activeSkillBloodPrice.text = price.ToString();
    }
}
