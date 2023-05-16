using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform targetLook;

    private UIManager _UIManager;

    private void Start()
    {

    }

    public void Init(UIManager uIManager)
    {
        _UIManager = uIManager;
        
        weapon.Init(uIManager, this, weaponHolder, targetLook);
    }
}
