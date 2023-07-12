using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGold : MonoBehaviour
{
    private int _gold;

    [SerializeField] private Text text;

    public void Add()
    {
        _gold++;
        UpdateUI();
    }

    public bool HasCount(int count)
    {
        return _gold >= count;
    }

    public void Remove(int count)
    {
        _gold -= count;
        UpdateUI();
    }

    private void UpdateUI()
    {
        text.text = _gold.ToString("D3");
    }
}
