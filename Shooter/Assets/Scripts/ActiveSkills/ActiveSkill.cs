using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public abstract class ActiveSkill : MonoBehaviour, ISelectableItem
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private int damageToReload;
    [SerializeField] private int price;

    public SelectableItems ItemType => SelectableItems.ActiveSkill;

    private float curDamageTimer;
    private Image reloadImage;
    protected Transform mainCamera;
    protected Player player;

    [System.Serializable]
    public struct Info
    {
        public Image Image;
        public Text Title;
        public Text Description;
        public Text DamageToReload;
        public Text Price;
    }

    public void Init(Image reloadImage, Transform camera, Player player)
    {
        curDamageTimer = damageToReload;
        this.reloadImage = reloadImage;
        reloadImage.sprite = sprite;
        mainCamera = camera;
        this.player = player;
        UpdateUI();
    }

    public void Activate()
    {
        if (curDamageTimer >= damageToReload)
        {
            OnActivated();
            curDamageTimer = 0;
            UpdateUI();
        }
        else
            print("need damage");
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
        reloadImage.fillAmount = curDamageTimer / damageToReload;
    }

    public Sprite GetSprite() => sprite;

    public string GetTitle() => title;

    public string GetDescription() => description;

    public int GetDamagaeToReturn() => damageToReload;

    public int GetPrice() => price;

    public void OnSelect(Player player)
    {

    }
}
