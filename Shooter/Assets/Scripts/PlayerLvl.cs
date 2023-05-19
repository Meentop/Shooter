using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLvl : MonoBehaviour
{
    [SerializeField] private float maxLvlPoints;
    [SerializeField] private Transform lvlBar;
    [SerializeField] private Text curLvlText, nextLvlText;

    private float _curLvlPoints;
    private int _curLvl;

    private void Start()
    {
        UpdateLvlBar();
    }

    private void UpdateLvlBar()
    {
        lvlBar.localScale = new Vector3(_curLvlPoints / maxLvlPoints, 1, 1);
        curLvlText.text = _curLvl.ToString();
        nextLvlText.text = (_curLvl + 1).ToString();
    }

    public void AddExp(int addExp)
    {
        _curLvlPoints += addExp;
        if(_curLvlPoints >= maxLvlPoints)
        {
            _curLvlPoints -= maxLvlPoints;
            _curLvl++;
        }
        UpdateLvlBar();
    }
}
