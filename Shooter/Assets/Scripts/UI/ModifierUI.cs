using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ModifierUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Image image;
    [SerializeField] private Text title, description;
    private DynamicUI _dynamicUI;
    private Camera _mainCamera;
    private RectTransform _canvas;
    private Vector3 _offset;

    public void Init(Sprite sprite, string title, string description, DynamicUI dynamicUI, Camera mainCamera, RectTransform canvas)
    {
        image.sprite = sprite;
        this.title.text = title;
        this.description.text = description;
        _dynamicUI = dynamicUI;
        _mainCamera = mainCamera;
        _canvas = canvas;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(_dynamicUI.GetDragModifierHolder());
        Vector3 viewportMouse = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
        Vector3 mousePos = new Vector3(viewportMouse.x * _canvas.sizeDelta.x, viewportMouse.y * _canvas.sizeDelta.y, 0);

        Vector3 viewportThis = _mainCamera.WorldToViewportPoint(transform.position);
        Vector3 thisPos = new Vector3(viewportThis.x * _canvas.sizeDelta.x, viewportThis.y * _canvas.sizeDelta.y, 0);
        _offset = thisPos - mousePos;
    }

    public void OnDrag(PointerEventData eventData)
    {   
        Vector3 viewport = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
        transform.localPosition = new Vector3(viewport.x * _canvas.sizeDelta.x, viewport.y * _canvas.sizeDelta.y, 0) + _offset;
    }   

    public void OnEndDrag(PointerEventData eventData)
    {
        print("end drag");
    }
}
