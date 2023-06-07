using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLvl : MonoBehaviour
{
    [SerializeField] private float maxLvlPoints;
    [SerializeField] private int healthLvlStep;
    [SerializeField] private float damageLvlStep;
    [SerializeField] private Transform lvlBar;
    [SerializeField] private Text curLvlText, nextLvlText;
    [SerializeField] private PlayerDamage playerDamage;

    private float _curLvlPoints;
    private int _curLvl;
    private PlayerHealth playerHealth;

    private void Start()
    {
        UpdateLvlBar();
        playerHealth = GetComponent<PlayerHealth>();
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
            NewLvl();
        }
        UpdateLvlBar();
    }

    private void NewLvl()
    {
        playerHealth.AddMaxHealth(healthLvlStep);
        playerDamage.AddDamagePower(damageLvlStep);
    }
}
