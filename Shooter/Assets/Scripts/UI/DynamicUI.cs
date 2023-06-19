using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicUI : MonoBehaviour
{
    [SerializeField] private GameObject interfacePanel;
    [SerializeField] private GameObject[] panelsToHide;
    [SerializeField] private Image backgroundBlur;
    [SerializeField] private ModifierHolderUI modifierHolder;
    [SerializeField] private Weapon.Description[] localWeaponDescriptions;
    [SerializeField] private RectTransform freeModifierHolder;
    [SerializeField] private ModifierUI modifierPrefab;
    [SerializeField] private Transform dragModifierHolder;
    [SerializeField] private FreeModifierHolderUI FreeModifierHolderUI;
    private bool _panelEnabled = false;
    private Player _player;
    private CameraController _cameraController;
    private Camera _mainCamera;
    private RectTransform _canvas;
    private Transform _modifierDragHolder;
    private ModifiersManager _modifiersManager;

    private void Start()
    {
        interfacePanel.SetActive(false);
        backgroundBlur.gameObject.SetActive(false);
    }

    public void Init(Player player, CameraController cameraController, Camera mainCamera, RectTransform canvas, Transform modifierDragHolder, ModifiersManager modifiersManager)
    {
        _player = player;
        _cameraController = cameraController;
        _mainCamera = mainCamera;
        _canvas = canvas;
        _modifierDragHolder = modifierDragHolder;
        _modifiersManager = modifiersManager;
        FreeModifierHolderUI.Init(dragModifierHolder, modifiersManager);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {      
            _panelEnabled = !_panelEnabled;
            SetTabInterface(_panelEnabled);
            if (_panelEnabled)
            {
                UpdateWeaponsInfo(_player.GetWeaponsInfo());
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

    public void UpdateWeaponsInfo(Weapon.Info[] info)
    {       
        for (int i = 0; i < localWeaponDescriptions.Length; i++)
        {
            localWeaponDescriptions[i].WeaponNameText.text = info[i].Name;
            localWeaponDescriptions[i].DamageText.text = "Damage " + info[i].Damage.ToString();
            localWeaponDescriptions[i].FiringSpeed.text = "FiringSpeed " + info[i].FiringSpeed.ToString();
            for (int j = 0; j < info[i].MaxNumbersOfModifiers; j++)
            {
                ModifierHolderUI holder = Instantiate(modifierHolder, localWeaponDescriptions[i].ModifiersHolder);
                holder.Init(_modifierDragHolder, _modifiersManager);
            }
        }
    }

    public void UpdateFreeModifierInfo(List<Modifier> freeModifier)
    {
        foreach (var modifier in freeModifier)
        {
            ModifierUI modifierUI = Instantiate(modifierPrefab, freeModifierHolder);
            modifierUI.Init(modifier.GetSprite(), modifier.GetTitle(), modifier.GetDescription(), this, _mainCamera, _canvas);
        }
        float sizeY = freeModifier.Count * modifierPrefab.GetComponent<RectTransform>().sizeDelta.y;
        sizeY += 5 * (freeModifier.Count - 1);
        freeModifierHolder.sizeDelta = new Vector2(freeModifierHolder.sizeDelta.x, sizeY);
    }

    public Transform GetDragModifierHolder()
    {
        return dragModifierHolder;
    }
}
