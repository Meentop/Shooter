using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToRunDoor : MonoBehaviour, ISelectableItem
{
    [SerializeField] private Save save;

    public SelectableItems ItemType => SelectableItems.StartRun;

    public string Text => "Press E to start run";

    public void OnSelect(Player player)
    {
        save.SaveToFile("saveData.json");
        SceneManager.LoadScene(1);
    }
}
