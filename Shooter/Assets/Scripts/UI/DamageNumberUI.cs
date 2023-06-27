using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamageNumberUI : MonoBehaviour, IPoolable
{
    [SerializeField] private float lifeTime;

    public GameObject GameObject => gameObject;

    public event Action<IPoolable> Destroyed;
    private TextMeshProUGUI text;
    private float timer;
    private Transform enemy;
    private Vector3 randPos;
    private RectTransform canvas;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            Reset();
        MoveText(Camera.main);
    }

    public void Init(int amount, Transform enemy, RectTransform canvas)
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = amount.ToString();
        this.enemy = enemy;
        this.canvas = canvas;

        timer = lifeTime;
        randPos = new Vector3(Random.Range(-50, 50), Random.Range(100, 150), 0);
        MoveText(Camera.main);
    }

    public void MoveText(Camera camera)
    {
        Vector3 newPos = new Vector3();
        if (enemy != null)
        {
            Vector3 viewportPos = camera.WorldToViewportPoint(enemy.position);
            float distance = Vector3.Distance(camera.transform.position, enemy.position);
            float yFarOffset = 400f;
            float yNearOffset = 1300f;
            float yOffset = Mathf.Lerp(yFarOffset, yNearOffset, distance / camera.farClipPlane);
            float multiplier = Mathf.Lerp(1f, 3f, distance / camera.farClipPlane);
            newPos = new Vector3(viewportPos.x * canvas.sizeDelta.x, viewportPos.y + yOffset, 0) + randPos * multiplier;
        }
        text.rectTransform.anchoredPosition = newPos;
    }

    public void Reset()
    { 
        Destroyed?.Invoke(this);
    }
}
