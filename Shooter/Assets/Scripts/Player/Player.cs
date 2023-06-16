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
    bool _isUpdated;
    private ICollectableItem _lastSavedWeapon;
    private InfoInterface _infoInterface;
    private DynamicUI _dynamicInterface;
    private UIManager _uiManager;

    [SerializeField] private List<Modifier> freeModifiers;
    [SerializeField] private Sprite testSprite;

    private void Start()
    {
        freeModifiers.Add(new Modifier(testSprite, "POISON", "+10% poison damage"));
        freeModifiers.Add(new Modifier(testSprite, "DARK", "+10% dark damage"));
        freeModifiers.Add(new Modifier(testSprite, "DARK", "+10% dark damage"));
        freeModifiers.Add(new Modifier(testSprite, "DARK", "+10% dark damage"));
        freeModifiers.Add(new Modifier(testSprite, "DARK", "+10% dark damage"));
        freeModifiers.Add(new Modifier(testSprite, "DARK", "+10% dark damage"));
        freeModifiers.Add(new Modifier(testSprite, "DARK", "+10% dark damage"));
        freeModifiers.Add(new Modifier(testSprite, "DARK", "+10% dark damage"));
    }

    private void Update()
    {
        if (!Pause.pause)
        {
            SelectWeapon();
            InputScrollWeapon();
            ChangeWeapon();
            TestButtonDetection();
        }
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
                    UpdateNewWeaponInfo(_lastSavedWeapon as Weapon);
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
                        UpdateNewWeaponInfo(_lastSavedWeapon as Weapon);
                    }
                    break;
            }
        }
    }

    public void Init(UIManager uiManager, CameraController cameraController)
    {
        _currentWeapon = Weapons[0];
        _infoInterface = uiManager.infoInterface;
        _dynamicInterface = uiManager.dinemicInterface;
        _uiManager = uiManager;
        _currentWeapon.Init(this, weaponHolder, targetLook, uiManager.infoInterface, _selectedSlot);
        _dynamicInterface.Init(this, cameraController);
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
        _currentWeapon.Init(this, weaponHolder, targetLook, _infoInterface, _selectedSlot);
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
                StartCoroutine(ScrollDelayRoutine());
            }
        }
    }
    
    private IEnumerator ScrollDelayRoutine()
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

    private void ChangeWeapon()
    {
        if(_isCollecting)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_lastSavedWeapon == null)
                    return;

                _currentWeapon.DiscardFromPlayer(_lastSavedWeapon as Weapon);

                _currentWeapon = _lastSavedWeapon as Weapon;

                Weapons[_selectedSlot] = _currentWeapon;

                _currentWeapon.Init(this, weaponHolder, targetLook, _infoInterface, _selectedSlot);
                _currentWeapon.ConectToPlayer();
            }
        }
    }

    private void UpdateNewWeaponInfo(Weapon CollectingWeapon)
    {
        if(_isCollecting)
        {
            _uiManager.SetActiveText(UIManager.TextTypes.SelectText, true);
            _uiManager.SetActiveText(UIManager.TextTypes.NewWeaponTextHolder, true);
            _uiManager.UpdateNewWeaponDescription(_lastSavedWeapon.GetType().Name, CollectingWeapon.GetDamage(), CollectingWeapon.GetFiringSpeed());
        }
        else
        {
            _uiManager.SetActiveText(UIManager.TextTypes.SelectText, false);
            _uiManager.SetActiveText(UIManager.TextTypes.NewWeaponTextHolder, false);
        }      
    }

    private void TestButtonDetection()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var hit, 4f))
        {
            if (hit.collider.TryGetComponent<TestEnemyButton>(out var testButton))
            {
                if (!_uiManager.GetActiveText(UIManager.TextTypes.SelectText))
                    _uiManager.SetActiveText(UIManager.TextTypes.SelectText, true);
                if (Input.GetKeyDown(KeyCode.F))
                    testButton.SpawnEnemy();
            }
        }
        else
        {
            if (_uiManager.GetActiveText(UIManager.TextTypes.SelectText))
                _uiManager.SetActiveText(UIManager.TextTypes.SelectText, false);
        }
    }


    public Weapon.Info[] GetWeaponsInfo()
    {
        Weapon.Info[] info = { Weapons[0].GetInfo(), Weapons[1].GetInfo() };
        return info;
    }

    public List<Modifier> GetFreeModifiers()
    {
        return freeModifiers;
    }
}
