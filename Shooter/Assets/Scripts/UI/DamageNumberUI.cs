using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamageNumberUI : MonoBehaviour, IPoolable
{
    [SerializeField] private float lifeTime;
    [SerializeField] private Vector2Int randomXRange;
    [SerializeField] private AnimationCurve anim;
    [SerializeField] private float animHeight;
    [SerializeField] private AnimationCurve transparent;

    public GameObject GameObject => gameObject;

    public event Action<IPoolable> Destroyed;
    private TextMeshProUGUI text;
    private float timer;
    private EnemyHealth enemy;
    private Vector3 randPos;
    private RectTransform canvas;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
            Reset();
        MoveText(Camera.main);
    }

    public void Init(int amount, EnemyHealth enemy, RectTransform canvas)
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = amount.ToString();
        this.enemy = enemy;
        this.canvas = canvas;

        timer = 0;
        randPos = new Vector3(Random.Range(randomXRange.x, randomXRange.y), 0, 0);
        MoveText(Camera.main);
    }

    public void MoveText(Camera camera)
    {
        Vector3 newPos = new Vector3();
        if (enemy != null)
        {
            Vector3 viewportPos = camera.WorldToViewportPoint(enemy.GetDamageNumbersPos());
            newPos = new Vector3(viewportPos.x * canvas.sizeDelta.x, (viewportPos.y * canvas.sizeDelta.y) + (anim.Evaluate(timer) * animHeight), 0) + randPos;
            text.faceColor = new Color32(text.faceColor.r, text.faceColor.g, text.faceColor.b, (byte)transparent.Evaluate(timer));
            text.outlineColor = new Color32(text.outlineColor.r, text.outlineColor.g, text.outlineColor.b, (byte)transparent.Evaluate(timer));
        }
        text.rectTransform.anchoredPosition = newPos;
    }

    public void Reset()
    { 
        Destroyed?.Invoke(this);
    }
}
