public interface ISelectableItem
{
    public SelectableItems ItemType { get; }

    public void OnSelect(Player player);
}

public enum SelectableItems
{
    Weapon,
    TestButton,
    Modifier,
    ActiveSkill,
    AwardButton,
    GoldAward,
    HealthAward,
    GoldButton
}
