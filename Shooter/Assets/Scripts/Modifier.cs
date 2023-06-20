using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Modifier : MonoBehaviour
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private string title, description;

    public Modifier(Sprite sprite, string title, string description)
    {
        this.sprite = sprite;
        this.title = title;
        this.description = description;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public string GetTitle()
    {
        return title;
    }

    public string GetDescription()
    {
        return description;
    }
}
