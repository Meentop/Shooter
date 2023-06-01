using UnityEngine;
using UnityEngine.UI;

public class DinemicInterface : MonoBehaviour
{
    [SerializeField] private GameObject interfacePanel;
    [SerializeField] private GameObject[] infoPanelsToHighd;
    [SerializeField] private GameObject[] weaponsDescriptionHolldears;
    [SerializeField] private Image backgroundBlur;
    [SerializeField] private Weapon.WeaponsDescription[] localWeaponsDescriptions;

    private bool _isInterfaceVisible = false;
    private void Start()
    {
        interfacePanel.SetActive(false);
        backgroundBlur.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            interfacePanel.SetActive(true);
            backgroundBlur.gameObject.SetActive(true);
            foreach (var infoPanel in infoPanelsToHighd)
                infoPanel.SetActive(false);
            _isInterfaceVisible = true;
        }
        else if (_isInterfaceVisible)
        {
            interfacePanel.SetActive(false);
            backgroundBlur.gameObject.SetActive(false);
            foreach (var infoPanel in infoPanelsToHighd)
                infoPanel.SetActive(true);
            _isInterfaceVisible = false;
        }
    }

    public void UpdateWeaponInfo(int number, string weaponNameText, Vector2Int damageText, float firingRange, float firingSpeed)
    {
        weaponsDescriptionHolldears[number].SetActive(true);
        localWeaponsDescriptions[number].WeaponNameText.text = weaponNameText;
        localWeaponsDescriptions[number].DamageText.text = "Damage " + damageText.ToString();
        localWeaponsDescriptions[number].FiringRange.text = "FiringRange " + firingRange.ToString();
        localWeaponsDescriptions[number].FiringSpeed.text = "FiringSpeed " + firingSpeed.ToString();
        weaponsDescriptionHolldears[CollectionsExtensions.GetNextIndex(weaponsDescriptionHolldears, number)].SetActive(false);
    }
}
