public interface ICollectableItem
{
    public CollectableItems ItemType { get; }

    public void OnCollect();
}

public enum CollectableItems
{
    Weapon,
}
