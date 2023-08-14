using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class MiniRoom : MonoBehaviour
{
    private readonly Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

    [SerializeField] private GameObject[] halls;
    [SerializeField] private Color unactiveColor;
    [SerializeField] private Color activeColor;
    [SerializeField] private GameObject awardTypeIcon;

    public Dictionary<Vector2Int, MiniRoom> Neighours = new Dictionary<Vector2Int, MiniRoom>();

    private bool _isActive;
    private bool _isActivated;
    private Image image;
    private Image awardImage;

    private void Update()
    {
        awardTypeIcon.transform.rotation = Quaternion.Euler(awardTypeIcon.transform.rotation.eulerAngles.x, awardTypeIcon.transform.rotation.eulerAngles.y, 0);
    }

    public void SetActiveHall(Vector2Int direction)
    {
        int index = Array.IndexOf(directions, direction);
        halls[index].SetActive(true);
    }

    public void SetAwardType(Sprite awardSprite)
    {
        awardImage = awardTypeIcon.GetComponent<Image>();
        awardImage.sprite = awardSprite;
        if (awardImage.sprite != null)
        {
            awardTypeIcon.SetActive(true);
        }
    }

    public void SetColor(bool isActive)
    {
        image = GetComponent<Image>();
        if (isActive)
        {
            image.color = activeColor;
        }    
        else
        {
            image.color = unactiveColor;
        }
        SetTransparency();
    }

    public void SetActiveMiniRoom(bool isActive)
    {
        _isActive = isActive;
        _isActivated = true;
        SetColor(_isActive);
    }

    public void SetTransparency()
    {
        image = GetComponent<Image>();
        if (!_isActivated)
        {
            Color color = image.color;
            color.a = 0.5f;
            image.color = color;
            awardImage.color = color;
        }
        else
        {
            Color color = image.color;
            color.a = 1f;
            image.color = color;
            awardImage.color = color;
        }
    }
}
