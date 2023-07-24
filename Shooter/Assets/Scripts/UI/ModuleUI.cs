using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ModuleUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Image image;
    [SerializeField] private Text title, description;
    public Module Module { get; private set; }

    private DynamicUI _dynamicUI;
    private Camera _mainCamera;
    private RectTransform _canvas;
    private Vector3 _offset;
    private ModuleHolderUI _holder;
    private bool _drag = false;
    private List<ModuleHolderUI> _holders = new List<ModuleHolderUI>();
    private ModuleHolderUI _activeHolder;

    public void Init(Sprite sprite, string title, string description, DynamicUI dynamicUI, Camera mainCamera, RectTransform canvas, ModuleHolderUI holder, Module module)
    {
        image.sprite = sprite;
        this.title.text = title;
        this.description.text = description;
        _dynamicUI = dynamicUI;
        _mainCamera = mainCamera;
        _canvas = canvas;
        _holder = holder;
        this.Module = module;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(_dynamicUI.GetDragModuleHolder());
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
        _dynamicUI.AddModule(this, _activeHolder);
        _holders.Clear();
        if (_activeHolder != null)
            _activeHolder.OnExit();
        _activeHolder = null;
        _drag = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_drag && other.TryGetComponent<ModuleHolderUI>(out var holder))
        {
            _holders.Add(holder);
            UpdateActiveHolder(holder);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_drag && other.TryGetComponent<ModuleHolderUI>(out var holder))
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

    private void UpdateActiveHolder(ModuleHolderUI holder)
    {
        if(_activeHolder != null)
            _activeHolder.OnExit();
        _activeHolder = holder;
        _activeHolder.OnEnter();
    }

    public void RemoveFromPastHolder()
    {
        _holder.RemoveModule(this);
    }

    public void AddNewHolder(ModuleHolderUI holder)
    {
        _holder = holder;
    }

    public void BackToPastHolder()
    {
        _holder.SetPosition(transform);
    }
}
