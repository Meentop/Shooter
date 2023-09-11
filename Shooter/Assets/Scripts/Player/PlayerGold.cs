using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    private int _gold;
    private InfoInterface _infoUI;
    public void Init(InfoInterface infoInterface)
    {
        _infoUI = infoInterface;
    }

    public void Add(int addGold = 1)
    {
        _gold += addGold;
        _infoUI.SetGoldCount(_gold);
    }

    public bool HasCount(int count)
    {
        return _gold >= count;
    }

    public int GetCount()
    {
        return _gold;
    }

    public void Remove(int count)
    {
        _gold -= count;
        _infoUI.SetGoldCount(_gold);
    }
}
