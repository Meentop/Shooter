using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifierUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text title, description;

    public void Set(Sprite sprite, string title, string description)
    {
        image.sprite = sprite;
        this.title.text = title;
        this.description.text = description;
    }
}
