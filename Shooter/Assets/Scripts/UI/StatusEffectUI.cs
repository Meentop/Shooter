using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text text;

    public void Init(Sprite sprite, int text, Color color)
    {
        image.sprite = sprite;
        this.text.text = text.ToString();
        this.text.color = color;
    }
}
