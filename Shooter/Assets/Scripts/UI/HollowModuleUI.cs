using UnityEngine;
using UnityEngine.UI;

public class HollowModuleUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text title, description;
    [SerializeField] private Image border;
    [SerializeField] private Color disableColor, enableColor;
    public Module Module { get; private set; }

    private bool isEnabled = false;

    public void Init(Sprite sprite, string title, string description, Module module)
    {
        image.sprite = sprite;
        this.title.text = title;
        this.description.text = description;
        Module = module;
    }

    public void SetEnable(bool enable)
    {
        isEnabled = enable;
        if(isEnabled) 
            border.color = enableColor;
        else
            border.color = disableColor;
    }
}
