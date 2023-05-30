using UnityEngine;
using UnityEngine.UI;

public class DinemicInterface : MonoBehaviour
{
    [SerializeField] private GameObject interfacePanel;
    [SerializeField] private Image backgroundBlur;

    private bool isInterfaceVisible = false;

    private void Start()
    {
        interfacePanel.SetActive(false);
        backgroundBlur.gameObject.SetActive(false);
        print("ddd");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            interfacePanel.SetActive(true);
            backgroundBlur.gameObject.SetActive(true);
            isInterfaceVisible = true;
            
        }
        else if (isInterfaceVisible)
        {
            interfacePanel.SetActive(false);
            backgroundBlur.gameObject.SetActive(false);
            isInterfaceVisible = false;
        }
    }
}
