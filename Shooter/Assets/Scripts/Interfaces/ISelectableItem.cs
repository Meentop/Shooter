public interface ISelectableItem
{
    public SelectableItems ItemType { get; }

    public string Text { get; }

    public void OnSelect(Player player);
}

public enum SelectableItems
{
    Weapon,
    TestButton,
    Module,
    ActiveSkill,
    AwardButton,
    GoldAward,
    HealthAward,
    GoldButton,
    WeaponUpgrade,
    ModuleUpgrade,
    StartRun,
    Portal
}
