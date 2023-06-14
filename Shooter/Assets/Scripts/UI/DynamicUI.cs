using UnityEngine;
using UnityEngine.UI;

public class DynamicUI : MonoBehaviour
{
    [SerializeField] private GameObject interfacePanel;
    [SerializeField] private GameObject[] panelsToHide;
    [SerializeField] private GameObject[] weaponsDescriptionHolders;
    [SerializeField] private Image backgroundBlur;
    [SerializeField] private Weapon.WeaponDescription[] localWeaponDescriptions;

    private void Start()
    {
        interfacePanel.SetActive(false);
        backgroundBlur.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SetTabInterface(true);
        }
        if(Input.GetKeyUp(KeyCode.Tab))
        {
            SetTabInterface(false);
        }
    }

    private void SetTabInterface(bool value)
    {
        interfacePanel.SetActive(value);
        backgroundBlur.gameObject.SetActive(value);
        foreach (var infoPanel in panelsToHide)
            infoPanel.SetActive(!value);
    }

    public void UpdateWeaponInfo(int number, string weaponName, Vector2Int damage, float firingSpeed)
    {
        weaponsDescriptionHolders[number].SetActive(true);
        localWeaponDescriptions[number].WeaponNameText.text = weaponName;
        localWeaponDescriptions[number].DamageText.text = "Damage " + damage.ToString();
        localWeaponDescriptions[number].FiringSpeed.text = "FiringSpeed " + firingSpeed.ToString();
        weaponsDescriptionHolders[CollectionsExtensions.GetNextIndex(weaponsDescriptionHolders, number)].SetActive(false);
    }
}
