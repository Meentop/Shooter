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
    [SerializeField] private float scrollDeley;

    private int _selWeapon;
    private Weapon _currentWeapon;
    private bool _isScrolling;

    private void Start()
    {

    }

    private void Update()
    {
        InputSelectWeapon();
        InputScrollWeapon();
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
            SwapWeapon(firstWeapon);
        }
        if (selectWeapon == 2)
        {
            SwapWeapon(secondWeapon);
        }
        if (selectWeapon == 3)
        {

        }
    }
    
    private void SwapWeapon(Weapon weaponToSwap)
    {
        _currentWeapon.transform.gameObject.SetActive(false);
        _currentWeapon = weaponToSwap;
        _currentWeapon.transform.gameObject.SetActive(true);
        _currentWeapon.Init(this, weaponHolder, targetLook);
    }

    private void InputScrollWeapon()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if(scrollInput != 0)
        {
            if (!_isScrolling)
            {
                _isScrolling = true;
                StartCoroutine(ScrollDeleyRoutine());
            }
        }
    }

    private IEnumerator ScrollDeleyRoutine()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        while (scrollInput != 0)
        {
            if (scrollInput > 0)
            {
                _selWeapon++;
                if (_selWeapon > 2) 
                {
                    _selWeapon = 1;
                }
            }
            else if(scrollInput < 0)
            {
                _selWeapon--;
                if (_selWeapon < 1) 
                {
                    _selWeapon = 2;
                }
            }
            SelectWeapon(_selWeapon);
            yield return new WaitForSeconds(scrollDeley);
            scrollInput = Input.GetAxis("Mouse ScrollWheel");
        }
        _isScrolling = false;
    }
}
