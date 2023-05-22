using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageUI : MonoBehaviour
{
    public static DamageUI Instance { get; private set; }
    private class ActiveText
    {
        public TextMeshProUGUI UIText;
        public float maxTime;
        public float timer;
        public Transform enemy;
        public Vector3 randPos;
        public RectTransform canvas;

        public void MoveText(Camera camera)
        {
            Vector3 newPos = new Vector3();
            if (enemy != null)
            {
                Vector3 viewportPos = camera.WorldToViewportPoint(enemy.position);
                newPos = new Vector3(viewportPos.x * canvas.sizeDelta.x, viewportPos.y * canvas.sizeDelta.y, 0) + randPos;
            }
            UIText.rectTransform.anchoredPosition = newPos;
        }
    }
    [SerializeField] private TextMeshProUGUI textPrefab;

    private const int POOL_SIZE = 12;

    private Queue<TextMeshProUGUI> textPool = new Queue<TextMeshProUGUI>();
    private List<ActiveText> activeTexts = new List<ActiveText>();

    private Camera _camera;
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _camera = Camera.main;
        InitializeTextPool();
    }

    void Update()
    {
        for (int i = 0; i < activeTexts.Count; i++)
        {
            ActiveText activeText = activeTexts[i];
            activeText.timer -= Time.deltaTime;

            if (activeText.timer <= 0)
            {
                activeText.UIText.gameObject.SetActive(false);
                textPool.Enqueue(activeText.UIText);
                
                activeTexts.RemoveAt(i);
                --i;
            }
            else
            {
                var color = activeText.UIText.color;
                color.a = activeText.timer / activeText.maxTime;
                activeText.UIText.color = color;

                activeText.MoveText(_camera);
            }
        }
    }

    private void InitializeTextPool()
    {
        for (int i = 0; i < POOL_SIZE; i++)
        {
            TextMeshProUGUI temp = Instantiate(textPrefab, transform);
            temp.gameObject.SetActive(false);
            textPool.Enqueue(temp);
        }
    }

    public void AddText(int amount, Transform enemy, RectTransform canvas)
    {
        var item = textPool.Dequeue();
        item.text = amount.ToString();
        item.gameObject.SetActive(true);

        ActiveText activeText = new ActiveText()
        {
            maxTime = 1.0f,
            timer = 1.0f,
            UIText = item,
            enemy = enemy,
            randPos = new Vector3(Random.Range(-50, 50), Random.Range(50, 100), 0),
            canvas = canvas
        };

        activeText.MoveText(_camera);
        activeTexts.Add(activeText);
    }
}
