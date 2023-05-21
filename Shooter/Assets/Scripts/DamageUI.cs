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
        public Vector3 unitPosition;

        public void MoveText(Camera camera)
        {
            float delta = 1.0f - (timer / maxTime);
            float modefiedDelta = delta * 2;
            Vector3 newPos = unitPosition + new Vector3(delta, modefiedDelta, 0);
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

    public void AddText(int amount, Vector3 unitPos)
    {
        var item = textPool.Dequeue();
        item.text = amount.ToString();
        item.gameObject.SetActive(true);

        ActiveText activeText = new ActiveText()
        {
            maxTime = 1.0f,
            timer = 1.0f,
            UIText = item,
            unitPosition = _camera.WorldToScreenPoint(unitPos) + new Vector3(0, Random.Range(0.5f, 1f), Random.Range(-1f, 1f))
        };

        activeText.MoveText(_camera);
        activeTexts.Add(activeText);
        
    }
}
