using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ModulesPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject[] panelsToShow;
    [SerializeField] private GameObject[] panelsToHide;
    [SerializeField] private Image backgroundBlur;
    [SerializeField] private WeaponModuleHolderUI weaponModifierHolderPrefab;
    [SerializeField] private Weapon.Description[] localWeaponDescriptions;
    [SerializeField] private ModuleUI modulePrefab;
    [SerializeField] private Transform dragModuleHolder;
    [SerializeField] private FreeModulesHolderUI freeWeaponModulesHolderUI, freeBionicModulesHolderUI;
    [SerializeField] private List<BionicModuleHolderUI> installedBionicModuleHolders;
    [Header("Transition")]
    [SerializeField] private float bionicPos;
    [SerializeField] private float transitionSpeed;
    [SerializeField] private AnimationCurve toBionicTransition, toWeaponTransition;
    private bool _panelEnabled = false;
    private Player _player;
    private CameraController _cameraController;
    private Camera _mainCamera;
    private RectTransform _canvas;

    private void Start()
    {
        foreach (var panel in panelsToShow)
            panel.SetActive(false);
        backgroundBlur.gameObject.SetActive(false);
    }

    public void Init(Player player, CameraController cameraController, Camera mainCamera, RectTransform canvas)
    {
        _player = player;
        _cameraController = cameraController;
        _mainCamera = mainCamera;
        _canvas = canvas;
        freeWeaponModulesHolderUI.Init(dragModuleHolder, player);
        freeBionicModulesHolderUI.Init(dragModuleHolder, player);
        foreach (var holder in installedBionicModuleHolders)
        {
            holder.Init(player);
        }
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
                UpdateFreeModuleInfo(_player.GetFreeWeaponModules().Select(module => module as Module).ToList(), freeWeaponModulesHolderUI);
                UpdateFreeModuleInfo(_player.GetFreeBionicModules().Select(module => module as Module).ToList(), freeBionicModulesHolderUI);
                UpdateInstalledBionicModules(_player.GetInstalledBionicModules());
                _cameraController.UnlockCursor();
            }
            else
            {
                _cameraController.LockCursor();
                foreach (Transform child in freeWeaponModulesHolderUI.GetContent())
                {
                    Destroy(child.gameObject);
                }
                foreach (Transform child in freeBionicModulesHolderUI.GetContent())
                {
                    Destroy(child.gameObject);
                }
            }
            PauseManager.Pause = _panelEnabled;
        }
    }

    private void SetTabInterface(bool value)
    {
        foreach (var panel in panelsToShow)
            panel.SetActive(value);
        backgroundBlur.gameObject.SetActive(value);
        foreach (var infoPanel in panelsToHide)
            infoPanel.SetActive(!value);
    }

    public void UpdateWeaponsInfo(Weapon[] weapons)
    {       
        for (int i = 0; i < localWeaponDescriptions.Length; i++)
        {
            localWeaponDescriptions[i].Image.sprite = weapons[i].GetSprite();
            localWeaponDescriptions[i].NameText.text = weapons[i].GetName();
            localWeaponDescriptions[i].DamageText.text = "Damage " + weapons[i].GetDamage().ToString();
            localWeaponDescriptions[i].FiringSpeed.text = "FiringSpeed " + weapons[i].GetFiringSpeed().ToString();
            foreach (Transform modulesHolder in localWeaponDescriptions[i].ModulesHolder)
            {
                Destroy(modulesHolder.gameObject);
            }
            for (int j = 0; j < weapons[i].GetMaxNumbersOfModules(); j++)
            {          
                WeaponModuleHolderUI holder = Instantiate(weaponModifierHolderPrefab, localWeaponDescriptions[i].ModulesHolder);
                holder.Init(dragModuleHolder, _player);
                holder.SetWeapon(weapons[i]);

                if (weapons[i].GetModulesCount() > j)
                {
                    ModuleUI moduleUI = Instantiate(modulePrefab, holder.transform);
                    WeaponModule module = weapons[i].GetModule(j);
                    moduleUI.Init(module.GetSprite(), module.GetTitle(), module.GetDescription(), this, _mainCamera, _canvas, holder, module);
                    holder.SetPosition(moduleUI.transform);
                }
            }
        }
    }

    public void UpdateFreeModuleInfo(List<Module> freeModules, FreeModulesHolderUI freeModulesHolderUI)
    {
        foreach (var module in freeModules)
        {
            ModuleUI moduleUI = Instantiate(modulePrefab, freeModulesHolderUI.GetContent());
            moduleUI.Init(module.GetSprite(), module.GetTitle(), module.GetDescription(), this, _mainCamera, _canvas, freeModulesHolderUI, module);
        }
        freeModulesHolderUI.SetFreeModulesHolderSize(freeModules.Count, modulePrefab.GetComponent<RectTransform>());
    }

    public void UpdateInstalledBionicModules(List<BionicModule> modules)
    {
        foreach (var holder in installedBionicModuleHolders)
        {
            if (holder.transform.childCount > 0)
                Destroy(holder.transform.GetChild(0).gameObject);
        }
        for (int i = 0; i < modules.Count; i++)
        {
            ModuleUI moduleUI = Instantiate(modulePrefab, installedBionicModuleHolders[i].transform);
            BionicModule module = modules[i];
            moduleUI.Init(module.GetSprite(), module.GetTitle(), module.GetDescription(), this, _mainCamera, _canvas, installedBionicModuleHolders[i], module);
            installedBionicModuleHolders[i].SetPosition(moduleUI.transform);
        }
    }

    public void AddModule(ModuleUI module, ModuleHolderUI holder)
    {
        if (holder != null && holder.CanAddModule(module))
        {
            module.RemoveFromPastHolder();
            module.AddNewHolder(holder);

            holder.SetPosition(module.transform);
            holder.AddModule(module);
        }
        else
        {
            module.BackToPastHolder();
        }
    }

    public Transform GetDragModuleHolder()
    {
        return dragModuleHolder;
    }


    public void MoveToBionicModules()
    {
        StartCoroutine(MoveToAnim(toBionicTransition));
    }    

    public void MoveToWeaponModules()
    {
        StartCoroutine(MoveToAnim(toWeaponTransition));
    }

    IEnumerator MoveToAnim(AnimationCurve curve)
    {
        float time = 0f;
        while (time < 1f)
        {
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime * transitionSpeed;
            transform.localPosition = new Vector3(curve.Evaluate(time) * bionicPos, transform.localPosition.y);
        }
    }
}
