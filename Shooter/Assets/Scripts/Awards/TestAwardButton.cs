using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAwardButton : MonoBehaviour, ISelectableItem
{
    [SerializeField] private Room room;
    [SerializeField] private AwardType award;

    public SelectableItems ItemType => SelectableItems.TestButton;

    public string Text => "E";

    public void OnSelect(Player player)
    {
        room.SpawnAward(award);
    }
}
