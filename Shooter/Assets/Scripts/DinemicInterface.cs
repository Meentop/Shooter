using UnityEngine;
using UnityEngine.UI;

public class DinemicInterface : MonoBehaviour
{
    [SerializeField] private GameObject interfacePanel;
    [SerializeField] private GameObject[] infoPanelsToHighd;
    [SerializeField] private Image backgroundBlur;

    private bool isInterfaceVisible = false;

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
            isInterfaceVisible = true;
        }
        else if (isInterfaceVisible)
        {
            interfacePanel.SetActive(false);
            backgroundBlur.gameObject.SetActive(false);
            foreach (var infoPanel in infoPanelsToHighd)
                infoPanel.SetActive(true);
            isInterfaceVisible = false;
        }
    }
}
