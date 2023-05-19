using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform targetLook;
    [SerializeField] private Weapon firstWeapon;
    [SerializeField] private Weapon secondWeapon;

    private int _selWeapon;
    private Weapon _currentWeapon;

    private void Start()
    {

    }

    private void Update()
    {
        InputSelectWeapon();
    }


    public void Init()
    {
        _currentWeapon = weapon;
        
        weapon.Init(this, weaponHolder, targetLook);
    }

    public void InputSelectWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && _selWeapon != 1)
        {
            _selWeapon = 1;
            SelectWeapon(_selWeapon);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && _selWeapon != 2)
        {
            _selWeapon = 2;
            SelectWeapon(_selWeapon);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && _selWeapon != 3)
        {
            _selWeapon = 3;
        }
    }

    public void SelectWeapon(int selectWeapon)
    {
        if (selectWeapon == 1)
        {
            _currentWeapon.transform.gameObject.SetActive(false);
            _currentWeapon = firstWeapon;
            _currentWeapon.transform.gameObject.SetActive(true);
            _currentWeapon.Init(this, weaponHolder, targetLook);
        }
        if (selectWeapon == 2)
        {
            _currentWeapon.transform.gameObject.SetActive(false);
            _currentWeapon = secondWeapon;
            _currentWeapon.transform.gameObject.SetActive(true);
            _currentWeapon.Init(this, weaponHolder, targetLook);
        }
        if (selectWeapon == 3)
        {

        }
    }
}
