public interface ISelectableItem
{
    public SelectableItems ItemType { get; }

    public void OnSelect();
}

public enum SelectableItems
{
    Weapon,
    TestButton,
    Modifier
}
