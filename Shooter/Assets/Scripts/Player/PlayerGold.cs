using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    public static PlayerGold Instance;

    private float _gold;

    private void Awake()
    {
        Instance = this;
    }

    public void AddGold()
    {
        _gold++;
        print(_gold);
    }
}
