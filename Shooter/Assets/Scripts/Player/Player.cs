using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform targetLook;
    [SerializeField] private Weapon firstWeapon;
    [SerializeField] private Weapon secondWeapon;
    [SerializeField] private float scrollDeley;

    private Weapon _currentWeapon;
    private int _selWeapon;
    private bool _isScrolling;
    private bool _isCollecting;
    private ICollectableItem _lastSavedWeapon;

    private void Update()
    {
        InputSelectWeapon();
        InputScrollWeapon();
        ChengeWeapon();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<ICollectableItem>(out var item))
        {
            switch (item.ItemType)
            {
                case CollectableItems.Weapon:
                    _lastSavedWeapon = item;
                    _isCollecting = true;
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent<ICollectableItem>(out var item))
        {
            switch (item.ItemType)
            {
                case CollectableItems.Weapon:
                    if (item == _lastSavedWeapon)
                    {
                        _lastSavedWeapon = null;
                        _isCollecting = false;
                    }
                    break;
            }
        }
    }

    public void Init()
    {
        _currentWeapon = firstWeapon;
        _selWeapon = 1;
        _currentWeapon.Init(this, weaponHolder, targetLook);
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

    private void ChengeWeapon()
    {
        if(_isCollecting)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_lastSavedWeapon == null)
                    return;

                _currentWeapon.DisconnectWeaponFromPlayer(_lastSavedWeapon as Weapon);
                _currentWeapon = _lastSavedWeapon as Weapon;

                if (_selWeapon == 1)
                    firstWeapon = _currentWeapon;
                else if (_selWeapon == 2)
                    secondWeapon = _currentWeapon;

                _currentWeapon.Init(this, weaponHolder, targetLook);
                _currentWeapon.ConectWeaponToPlayer();
            }
        }
    }  
}

