using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{

    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform targetLook;
    [SerializeField] private Weapon[] Weapons;
    [SerializeField] private float scrollDeley;

    private Weapon _currentWeapon;
    private int _selectedSlot;
    private bool _isScrolling;
    private bool _isCollecting;
    private ICollectableItem _lastSavedWeapon;
    private InfoInterface _infoInterface;
    private DynamicInterface _dinemicInterface;

    private void Update()
    {
        SelectWeapon();
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

    public void Init(InfoInterface infoInterface, DynamicInterface dinemicInterface)
    {
        _currentWeapon = Weapons[0];
        _infoInterface = infoInterface;
        _dinemicInterface = dinemicInterface;
        _currentWeapon.Init(this, weaponHolder, targetLook, _infoInterface, _dinemicInterface, _selectedSlot);
    }

    public void SelectWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _selectedSlot = 0;
            SwapWeapon(Weapons[_selectedSlot]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _selectedSlot = 1;
            SwapWeapon(Weapons[_selectedSlot]);
        }
    }
    
    private void SwapWeapon(Weapon weaponToSwap)
    {
        if (weaponToSwap == _currentWeapon)
            return;

        _currentWeapon.transform.gameObject.SetActive(false);
        _currentWeapon = weaponToSwap;
        _currentWeapon.transform.gameObject.SetActive(true);
        _currentWeapon.Init(this, weaponHolder, targetLook, _infoInterface, _dinemicInterface, _selectedSlot);
        _infoInterface.DiscardWeaponsIcon(CollectionsExtensions.GetNextIndex(Weapons, _selectedSlot));
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
            _selectedSlot = CollectionsExtensions.GetNextIndex(Weapons, _selectedSlot);
            SwapWeapon(Weapons[_selectedSlot]);
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

                _currentWeapon.DiscardWeaponFromPlayer(_lastSavedWeapon as Weapon);
                _currentWeapon = _lastSavedWeapon as Weapon;

                Weapons[_selectedSlot] = _currentWeapon;

                _currentWeapon.Init(this, weaponHolder, targetLook, _infoInterface, _dinemicInterface, _selectedSlot);
                _currentWeapon.ConectWeaponToPlayer();
            }
        }
    }  
}

