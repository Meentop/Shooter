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
    private ModifierHolderUI _holder;
    public Modifier modifier { get; private set; }
    private bool _drag = false;
    private List<ModifierHolderUI> _holders = new List<ModifierHolderUI>();
    private ModifierHolderUI _activeHolder;

    public void Init(Sprite sprite, string title, string description, DynamicUI dynamicUI, Camera mainCamera, RectTransform canvas, ModifierHolderUI holder, Modifier modifier)
    {
        image.sprite = sprite;
        this.title.text = title;
        this.description.text = description;
        _dynamicUI = dynamicUI;
        _mainCamera = mainCamera;
        _canvas = canvas;
        _holder = holder;
        this.modifier = modifier;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(_dynamicUI.GetDragModifierHolder());
        Vector3 viewportMouse = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
        Vector3 mousePos = new Vector3(viewportMouse.x * _canvas.sizeDelta.x, viewportMouse.y * _canvas.sizeDelta.y, 0);

        Vector3 viewportThis = _mainCamera.WorldToViewportPoint(transform.position);
        Vector3 thisPos = new Vector3(viewportThis.x * _canvas.sizeDelta.x, viewportThis.y * _canvas.sizeDelta.y, 0);
        _offset = thisPos - mousePos;
        _drag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {   
        Vector3 viewport = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
        transform.localPosition = new Vector3(viewport.x * _canvas.sizeDelta.x, viewport.y * _canvas.sizeDelta.y, 0) + _offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _dynamicUI.AddModifier(this, _activeHolder);
        _holders.Clear();
        if (_activeHolder != null)
            _activeHolder.OnExit();
        _activeHolder = null;
        _drag = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_drag && other.TryGetComponent<ModifierHolderUI>(out var holder))
        {
            _holders.Add(holder);
            UpdateActiveHolder(holder);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_drag && other.TryGetComponent<ModifierHolderUI>(out var holder))
        {
            _holders.Remove(holder);
            _activeHolder = null;
            holder.OnExit();
            for (int i = 0; i < _holders.Count; i++)
            {
                if(_holders[i] != holder)
                    UpdateActiveHolder(_holders[i]);
            }
        }
    }

    private void UpdateActiveHolder(ModifierHolderUI holder)
    {
        if(_activeHolder != null)
            _activeHolder.OnExit();
        _activeHolder = holder;
        _activeHolder.OnEnter();
    }

    public void RemoveFromPastHolder()
    {
        _holder.RemoveModifier(this);
    }

    public void AddNewHolder(ModifierHolderUI holder)
    {
        _holder = holder;
    }

    public void BackToPastHolder()
    {
        _holder.SetPosition(transform);
    }
}
