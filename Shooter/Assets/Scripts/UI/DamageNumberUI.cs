using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamageNumberUI : MonoBehaviour, IPoolable
{
    [SerializeField] private float lifeTime;
    [SerializeField] private Vector2Int randomXRange, randomYRange;
    [SerializeField] private AnimationCurve anim;
    [SerializeField] private float animHeight;
    [SerializeField] private AnimationCurve transparent;

    public GameObject GameObject => gameObject;

    public event Action<IPoolable> Destroyed;
    private TextMeshProUGUI text;
    private float timer;
    private EnemyHealth enemy;
    private Vector3 randPos;
    private Vector2 canvasSizeDelta;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
            Reset();
        MoveText(Camera.main);
    }

    public void Init(int amount, EnemyHealth enemy, RectTransform enemyCanvas, Color color)
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = amount.ToString();
        this.enemy = enemy;
        canvasSizeDelta = enemyCanvas.sizeDelta;
        text.color = color;

        timer = 0;
        randPos = new Vector3(Random.Range(randomXRange.x, randomXRange.y), Random.Range(randomYRange.x, randomYRange.y), 0);
        MoveText(Camera.main);
    }

    Vector3 lastEnemyPos = new Vector3();
    public void MoveText(Camera camera)
    {
        if (enemy != null)
        {
            lastEnemyPos = enemy.GetDamageNumbersPos();
        }
        Vector3 viewportPos = camera.WorldToViewportPoint(lastEnemyPos);
        Vector3 newPos = new Vector3(viewportPos.x * canvasSizeDelta.x, (viewportPos.y * canvasSizeDelta.y) + (anim.Evaluate(timer) * animHeight), 0) + randPos;
        text.faceColor = new Color32(text.faceColor.r, text.faceColor.g, text.faceColor.b, (byte)transparent.Evaluate(timer));
        text.outlineColor = new Color32(text.outlineColor.r, text.outlineColor.g, text.outlineColor.b, (byte)transparent.Evaluate(timer));
        text.rectTransform.anchoredPosition = newPos;
    }

    public void Reset()
    { 
        Destroyed?.Invoke(this);
    }
}
