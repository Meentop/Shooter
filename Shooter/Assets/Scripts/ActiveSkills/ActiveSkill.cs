using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ActiveSkill : MonoBehaviour, ISelectableItem
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private int damageToReload;

    public SelectableItems ItemType => SelectableItems.ActiveSkill;

    public string Text => "Press E to buy";

    private float curDamageTimer;
    protected Transform mainCamera;
    protected Player player;
    private InfoInterface _infoUI;

    [System.Serializable]
    public struct Info
    {
        public Image Image;
        public Text Title;
        public Text Description;
        public Text DamageToReload;
        public Text Price;
    }

    public void Init(InfoInterface infoInterface, Transform camera, Player player)
    {
        curDamageTimer = damageToReload;
        _infoUI = infoInterface;
        infoInterface.SetActiveSkillSprite(sprite);
        mainCamera = camera;
        this.player = player;
        UpdateUI();
    }

    public void Activate()
    {
        if (!player.Characteristics.bloodyActiveSkill && curDamageTimer >= damageToReload)
        {
            OnActivated();
            curDamageTimer = 0;
            UpdateUI();
        }
        else if(player.Characteristics.bloodyActiveSkill)
        {
            player.Health.GetDamage(new DamageData(player.Characteristics.activeSkillBloodyPrice));
            OnActivated();
        }
    }

    protected abstract void OnActivated();

    public void AddDamageToTimer(int damage)
    {
        curDamageTimer += damage;
        if(curDamageTimer > damageToReload)
            curDamageTimer = damageToReload;
        UpdateUI();
    }

    private void UpdateUI()
    {
        _infoUI.SetActiveSkillReload(curDamageTimer / damageToReload);
    }

    public Sprite GetSprite() => sprite;

    public string GetTitle() => title;

    public string GetDescription() => description;

    public int GetDamagaeToReturn() => damageToReload;

    public void OnSelect(Player player)
    {

    }
}
