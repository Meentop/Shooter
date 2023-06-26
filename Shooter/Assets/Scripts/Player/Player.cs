using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform targetLook;
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private float scrollDeley;
    [SerializeField] private Sprite testSprite;
    [SerializeField] private float selectDistance = 4f;

    private Weapon _currentWeapon;
    private int _selectedSlot;
    private bool _isScrolling;
    private ISelectableItem _lastSavedSelectableItem;
    private List<Modifier> freeModifiers = new List<Modifier>();
    private InfoInterface _infoInterface;
    private DynamicUI _dynamicInterface;
    private UIManager _uiManager;

    private void Update()
    {
        if (!Pause.pause)
        {
            SelectWeapon();
            InputScrollWeapon();
            SelectableItemsDetection();
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
            if (_lastSavedSelectableItem == null)
                return;
 
            switch (_lastSavedSelectableItem.ItemType)
            {
                case SelectableItems.Weapon:
                    _currentWeapon.DiscardFromPlayer(_lastSavedSelectableItem as Weapon);
                    _currentWeapon = _lastSavedSelectableItem as Weapon;
                    weapons[_selectedSlot] = _currentWeapon;
                    _currentWeapon.Init(this, weaponHolder, targetLook, _infoInterface, _selectedSlot);
                    _currentWeapon.ConectToPlayer();
                    break;
                case SelectableItems.TestButton:
                    _lastSavedSelectableItem.OnSelect();
                    break;
                case SelectableItems.Modifier:
                    AddFreeModifier(_lastSavedSelectableItem as Modifier);
                    _lastSavedSelectableItem.OnSelect();
                    break;
            }
        }
    }

    private void SelectableItemsDetection()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var hit, selectDistance))
        {
            if (hit.collider.TryGetComponent<ISelectableItem>(out var item))
            {
                _lastSavedSelectableItem = item;
                OverChoosableItem();
            }
            else
                ExitSelectableItem();
        }
        else
            ExitSelectableItem();
    }

    void OverChoosableItem()
    {
        if (!_uiManager.GetActiveText(UIManager.TextTypes.SelectText))
            _uiManager.SetActiveText(UIManager.TextTypes.SelectText, true);
        switch (_lastSavedSelectableItem.ItemType)
        {
            case SelectableItems.Weapon:
                Weapon collectingWeapon = _lastSavedSelectableItem as Weapon;
                _uiManager.SetActiveText(UIManager.TextTypes.NewWeaponTextHolder, true);
                _uiManager.UpdateNewWeaponDescription(collectingWeapon.GetName(), collectingWeapon.GetDamage(), collectingWeapon.GetFiringSpeed());
                break;
            case SelectableItems.Modifier:
                Modifier collectingModifier = _lastSavedSelectableItem as Modifier;
                _uiManager.SetActiveText(UIManager.TextTypes.NewModifierTextHolder, true);
                _uiManager.UpdateNewModifierInfo(collectingModifier.GetSprite(), collectingModifier.GetTitle(), collectingModifier.GetDescription());
                break;
        }
    }

    void ExitSelectableItem()
    {
        if (_uiManager.GetActiveText(UIManager.TextTypes.SelectText))
            _uiManager.SetActiveText(UIManager.TextTypes.SelectText, false);
        if (_lastSavedSelectableItem != null)
        {
            switch (_lastSavedSelectableItem.ItemType)
            {
                case SelectableItems.Weapon:
                    _uiManager.SetActiveText(UIManager.TextTypes.NewWeaponTextHolder, false);
                    break;
                case SelectableItems.Modifier:
                    _uiManager.SetActiveText(UIManager.TextTypes.NewModifierTextHolder, false);
                    break;
            }
            _lastSavedSelectableItem = null;
        }
    }


    public Weapon[] GetWeapons()
    {
        return weapons;
    }

    public List<Modifier> GetFreeModifiers()
    {
        return freeModifiers;
    }

    public void RemoveFreeModifier(Modifier modifier)
    {
        freeModifiers.Remove(modifier);
    }

    public void AddFreeModifier(Modifier modifier)
    {
        freeModifiers.Add(modifier);
    }
}
