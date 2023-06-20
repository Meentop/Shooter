using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicUI : MonoBehaviour
{
    [SerializeField] private GameObject interfacePanel;
    [SerializeField] private GameObject[] panelsToHide;
    [SerializeField] private Image backgroundBlur;
    [SerializeField] private WeaponModifierHolderUI weaponModifierHolder;
    [SerializeField] private Weapon.Description[] localWeaponDescriptions;
    [SerializeField] private RectTransform freeModifierHolder;
    [SerializeField] private ModifierUI modifierPrefab;
    [SerializeField] private Transform dragModifierHolder;
    [SerializeField] private FreeModifierHolderUI freeModifierHolderUI;
    private bool _panelEnabled = false;
    private Player _player;
    private CameraController _cameraController;
    private Camera _mainCamera;
    private RectTransform _canvas;
    private Transform _modifierDragHolder;

    private void Start()
    {
        interfacePanel.SetActive(false);
        backgroundBlur.gameObject.SetActive(false);
    }

    public void Init(Player player, CameraController cameraController, Camera mainCamera, RectTransform canvas, Transform modifierDragHolder)
    {
        _player = player;
        _cameraController = cameraController;
        _mainCamera = mainCamera;
        _canvas = canvas;
        _modifierDragHolder = modifierDragHolder;
        freeModifierHolderUI.Init(dragModifierHolder, player);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {      
            _panelEnabled = !_panelEnabled;
            SetTabInterface(_panelEnabled);
            if (_panelEnabled)
            {
                UpdateWeaponsInfo(_player.GetWeapons());
                UpdateFreeModifierInfo(_player.GetFreeModifiers());
                _cameraController.UnlockCursor();
            }
            else
            {
                _cameraController.LockCursor();
                foreach (Transform child in freeModifierHolder)
                {
                    Destroy(child.gameObject);
                }
            }
            Pause.pause = _panelEnabled;
        }
    }

    private void SetTabInterface(bool value)
    {
        interfacePanel.SetActive(value);
        backgroundBlur.gameObject.SetActive(value);
        foreach (var infoPanel in panelsToHide)
            infoPanel.SetActive(!value);
    }

    public void UpdateWeaponsInfo(Weapon[] weapons)
    {       
        for (int i = 0; i < localWeaponDescriptions.Length; i++)
        {
            localWeaponDescriptions[i].WeaponNameText.text = weapons[i].GetName();
            localWeaponDescriptions[i].DamageText.text = "Damage " + weapons[i].GetDamage().ToString();
            localWeaponDescriptions[i].FiringSpeed.text = "FiringSpeed " + weapons[i].GetFiringSpeed().ToString();
            foreach (Transform modifierHolder in localWeaponDescriptions[i].ModifiersHolder)
            {
                Destroy(modifierHolder.gameObject);
            }
            for (int j = 0; j < weapons[i].GetMaxNumbersOfModifiers(); j++)
            {          
                WeaponModifierHolderUI holder = Instantiate(weaponModifierHolder, localWeaponDescriptions[i].ModifiersHolder);
                holder.Init(_modifierDragHolder, _player);
                holder.SetWeapon(weapons[i]);

                if (weapons[i].GetModifiersCount() > j)
                {
                    ModifierUI modifierUI = Instantiate(modifierPrefab, holder.transform);
                    Modifier modifier = weapons[i].GetModifier(j);
                    modifierUI.Init(modifier.GetSprite(), modifier.GetTitle(), modifier.GetDescription(), this, _mainCamera, _canvas, holder, modifier);
                    holder.SetPosition(modifierUI.transform);
                }
            }
        }
    }

    public void UpdateFreeModifierInfo(List<Modifier> freeModifier)
    {
        foreach (var modifier in freeModifier)
        {
            ModifierUI modifierUI = Instantiate(modifierPrefab, freeModifierHolder);
            modifierUI.Init(modifier.GetSprite(), modifier.GetTitle(), modifier.GetDescription(), this, _mainCamera, _canvas, freeModifierHolderUI, modifier);
        }
        freeModifierHolderUI.SetFreeModifierHolderSize(freeModifier.Count, modifierPrefab.GetComponent<RectTransform>());
    }

    public void AddModifier(ModifierUI modifier, ModifierHolderUI holder)
    {
        if (holder != null && holder.CanAddModifier())
        {
            modifier.RemoveFromPastHolder();
            modifier.AddNewHolder(holder);

            holder.SetPosition(modifier.transform);
            holder.AddModifier(modifier);
        }
        else
        {
            modifier.BackToPastHolder();
        }
    }

    public Transform GetDragModifierHolder()
    {
        return dragModifierHolder;
    }
}
