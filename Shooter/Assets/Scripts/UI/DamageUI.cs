using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUI : MonoBehaviour
{
    public static DamageUI Instance { get; private set; }

    [SerializeField] private DamageNumberUI textPrefab;

    void Awake()
    {
        Instance = this;
    }

    public void AddText(int amount, EnemyHealth enemy, RectTransform enemyCanvas, Color color)
    {
        var item = ObjectPool.Instance.GetObject(textPrefab);
        item.transform.SetParent(transform);
        item.transform.localScale = Vector3.one;
        item.transform.localRotation = Quaternion.identity;
        item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y, 0);
        item.Init(amount, enemy, enemyCanvas, color);
    }
}
