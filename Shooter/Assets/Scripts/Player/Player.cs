using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform targetLook;
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private float scrollDeley;

    private Weapon _currentWeapon;
    private int _selectedSlot;
    private bool _isScrolling;
    private bool _isCollecting;
    bool _isUpdated;
    private ISelectableItem _lastSavedChoosableItem;
    private InfoInterface _infoInterface;
    private DynamicUI _dynamicInterface;
    private UIManager _uiManager;

    [SerializeField] private List<Modifier> freeModifiers;
    [SerializeField] private Sprite testSprite;

    private void Update()
    {
        if (!Pause.pause)
        {
            SelectWeapon();
            InputScrollWeapon();
            ChoosableItemsDetection();
            SelectItem();
        }
    }

    public void Init(UIManager uiManager, CameraController cameraController, Camera mainCamera, RectTransform canvas, Transform modifierDragHolder)
    {
        _currentWeapon = weapons[0];
        _infoInterface = uiManager.infoInterface;
        _dynamicInterface = uiManager.dinemicInterface;
        _uiManager = uiManager;
        _currentWeapon.Init(this, weaponHolder, targetLook, uiManager.infoInterface, _selectedSlot);
        _dynamicInterface.Init(this, cameraController, mainCamera, canvas, modifierDragHolder);
    }

    public void SelectWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _selectedSlot = 0;
            SwapWeapon(weapons[_selectedSlot]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _selectedSlot = 1;
            SwapWeapon(weapons[_selectedSlot]);
        }
    }
    
    private void SwapWeapon(Weapon weaponToSwap)
    {
        if (weaponToSwap == _currentWeapon)
            return;

        _currentWeapon.transform.gameObject.SetActive(false);
        _currentWeapon = weaponToSwap;
        _currentWeapon.transform.gameObject.SetActive(true);
        _currentWeapon.Init(this, weaponHolder, targetLook, _infoInterface, _selectedSlot);
        _infoInterface.DiscardWeaponsIcon(CollectionsExtensions.GetNextIndex(weapons, _selectedSlot));
    }

    private void InputScrollWeapon()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if(scrollInput != 0)
        {
            if (!_isScrolling)
            {
                _isScrolling = true;
                StartCoroutine(ScrollDelayRoutine());
            }
        }
    }
    
    private IEnumerator ScrollDelayRoutine()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        while (scrollInput != 0)
        {
            _selectedSlot = CollectionsExtensions.GetNextIndex(weapons, _selectedSlot);
            SwapWeapon(weapons[_selectedSlot]);
            yield return new WaitForSeconds(scrollDeley);
            scrollInput = Input.GetAxis("Mouse ScrollWheel");
        }
        _isScrolling = false;
    }

    private void SelectItem()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_lastSavedChoosableItem == null)
                return;
 
            switch (_lastSavedChoosableItem.ItemType)
            {
                case SelectableItems.Weapon:
                    _currentWeapon.DiscardFromPlayer(_lastSavedChoosableItem as Weapon);
                    _currentWeapon = _lastSavedChoosableItem as Weapon;
                    weapons[_selectedSlot] = _currentWeapon;
                    _currentWeapon.Init(this, weaponHolder, targetLook, _infoInterface, _selectedSlot);
                    _currentWeapon.ConectToPlayer();
                    break;
                case SelectableItems.TestButton:
                    _lastSavedChoosableItem.OnSelect();
                    break;
            }
        }
    }

    private void ChoosableItemsDetection()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var hit, 4f))
        {
            if (hit.collider.TryGetComponent<ISelectableItem>(out var item))
            {
                _lastSavedChoosableItem = item;
                OverChoosableItem();
            }
            else
                ExitChoosableItem();
        }
        else
            ExitChoosableItem();
    }

    void OverChoosableItem()
    {
        if (!_uiManager.GetActiveText(UIManager.TextTypes.SelectText))
            _uiManager.SetActiveText(UIManager.TextTypes.SelectText, true);
        switch (_lastSavedChoosableItem.ItemType)
        {
            case SelectableItems.Weapon:
                Weapon collectingWeapon = _lastSavedChoosableItem as Weapon;
                _uiManager.SetActiveText(UIManager.TextTypes.NewWeaponTextHolder, true);
                _uiManager.UpdateNewWeaponDescription(_lastSavedChoosableItem.GetType().Name, collectingWeapon.GetDamage(), collectingWeapon.GetFiringSpeed());
                break;
        }
    }

    void ExitChoosableItem()
    {
        if (_uiManager.GetActiveText(UIManager.TextTypes.SelectText))
            _uiManager.SetActiveText(UIManager.TextTypes.SelectText, false);
        if (_lastSavedChoosableItem != null)
        {
            switch (_lastSavedChoosableItem.ItemType)
            {
                case SelectableItems.Weapon:
                    _uiManager.SetActiveText(UIManager.TextTypes.NewWeaponTextHolder, false);
                    break;
            }
        }
        _lastSavedChoosableItem = null;
    }


    public Weapon[] GetWeapons()
    {
        return weapons;
    }

    public List<Modifier> GetFreeModifiers()
    {
        foreach (var item in freeModifiers)
        {
            print("current " + item.GetTitle());
        }
        return freeModifiers;
    }

    public void RemoveFreeModifier(Modifier modifier)
    {
        print("removed " + freeModifiers.IndexOf(modifier));
        freeModifiers.Remove(modifier);
        foreach (var item in freeModifiers)
        {
            print("after remove " + item.GetTitle());
        }
    }

    public void AddFreeModifier(Modifier modifier)
    {
        freeModifiers.Add(modifier);
    }
}
