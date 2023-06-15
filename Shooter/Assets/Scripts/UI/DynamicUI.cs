using UnityEngine;
using UnityEngine.UI;

public class DynamicUI : MonoBehaviour
{
    [SerializeField] private GameObject interfacePanel;
    [SerializeField] private GameObject[] panelsToHide;
    [SerializeField] private Image backgroundBlur;
    [SerializeField] private Weapon.Description[] localWeaponDescriptions;
    private bool _panelEnabled = false;
    private Player _player;

    private void Start()
    {
        interfacePanel.SetActive(false);
        backgroundBlur.gameObject.SetActive(false);
    }

    public void Init(Player player)
    {
        _player = player;
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
            }
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
        }
    }
}
