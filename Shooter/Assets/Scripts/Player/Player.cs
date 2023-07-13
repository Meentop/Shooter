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

    public PlayerHealth health { get; private set; }
    public PlayerGold gold { get; private set; }

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
        health = GetComponent<PlayerHealth>();
        gold = GetComponent<PlayerGold>();
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
                    Weapon selectWeapon = _lastSavedSelectableItem as Weapon;
                    if (gold.HasCount(selectWeapon.GetPrice()) || selectWeapon.bought)
                    {
                        if(!selectWeapon.bought)
                            gold.Remove(selectWeapon.GetPrice());
                        _currentWeapon.DiscardFromPlayer(selectWeapon);
                        _currentWeapon = selectWeapon;
                        weapons[_selectedSlot] = _currentWeapon;
                        _currentWeapon.Init(this, weaponHolder, targetLook, _infoInterface, _selectedSlot);
                        _currentWeapon.ConectToPlayer();
                    }
                    break;
                case SelectableItems.Modifier:
                    Modifier selectModifier = _lastSavedSelectableItem as Modifier;
                    if (gold.HasCount(selectModifier.GetPrice()))
                    {
                        gold.Remove(selectModifier.GetPrice());
                        AddFreeModifier(selectModifier);
                        Destroy(selectModifier.gameObject);
                    }
                    break;
            }
            _lastSavedSelectableItem.OnSelect(this);
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

    private void OverChoosableItem()
    {
        if (!_uiManager.GetActiveText(TextTypes.SelectText))
            _uiManager.SetActiveText(TextTypes.SelectText, true);
        switch (_lastSavedSelectableItem.ItemType)
        {
            case SelectableItems.Weapon:
                Weapon selectingWeapon = _lastSavedSelectableItem as Weapon;
                _uiManager.UpdateNewWeaponDescription(gold.HasCount(selectingWeapon.GetPrice()), selectingWeapon);
                break;
            case SelectableItems.Modifier:
                Modifier selectingModifier = _lastSavedSelectableItem as Modifier; 
                _uiManager.UpdateNewModifierInfo(gold.HasCount(selectingModifier.GetPrice()), selectingModifier);
                break;
            case SelectableItems.HealthAward:
                HealthAward healthAward = _lastSavedSelectableItem as HealthAward;
                _uiManager.UpdateBuyHealthText(gold.HasCount(healthAward.GetPrice()), healthAward);
                break;
        }
        SetActiveTextTypes(true);
    }

    private void ExitSelectableItem()
    {
        if (_uiManager.GetActiveText(TextTypes.SelectText))
            _uiManager.SetActiveText(TextTypes.SelectText, false);
        if (_lastSavedSelectableItem != null)
        {
            SetActiveTextTypes(false);
            _lastSavedSelectableItem = null;
        }
    }

    private void SetActiveTextTypes(bool active)
    {
        switch (_lastSavedSelectableItem.ItemType)
        {
            case SelectableItems.Weapon:
                _uiManager.SetActiveText(TextTypes.NewWeaponTextHolder, active);
                break;
            case SelectableItems.Modifier:
                _uiManager.SetActiveText(TextTypes.NewModifierTextHolder, active);
                break;
            case SelectableItems.HealthAward:
                _uiManager.SetActiveText(TextTypes.BuyHealthHolder, active);
                break;
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
