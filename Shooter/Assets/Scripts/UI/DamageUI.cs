using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageUI : MonoBehaviour
{
    public static DamageUI Instance { get; private set; }

    [SerializeField] private DamageNumberUI textPrefab;

    private Camera _camera;
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    public void AddText(int amount, Transform enemy, RectTransform canvas)
    {
        var item = ObjectPool.Instance.GetObject(textPrefab);
        item.transform.SetParent(transform);
        item.transform.localScale = Vector3.one;
        item.transform.localRotation = Quaternion.identity;
        item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y, 0);
        item.Init(amount, enemy, canvas);
    }
}
