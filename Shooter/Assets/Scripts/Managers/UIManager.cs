using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;

public partial class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoCurrentText;
    [SerializeField] private TextMeshProUGUI ammoLeftText;


    public enum TextFieldKeys
    {
        AmmoCurrentText,
        AmmoLeftText
    }


    public void SetText(TextFieldKeys textFieldKey, string textToSet)
    {
        switch (textFieldKey)
        {
            case TextFieldKeys.AmmoCurrentText:
                ammoCurrentText.text = textToSet;
                break;
            case TextFieldKeys.AmmoLeftText:
                ammoLeftText.text = textToSet;
                break;
        }
    }


}
