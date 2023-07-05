using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGold : MonoBehaviour
{
    public static PlayerGold Instance;

    private int _gold;

    [SerializeField] private Text text;

    private void Awake()
    {
        Instance = this;
    }

    public void AddGold()
    {
        _gold++;
        text.text = _gold.ToString("D3");
    }
}
