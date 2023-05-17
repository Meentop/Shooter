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
            Vector3 startPos = unitPosition;
            Vector3 offset = unitPosition + Vector3.up * 3;
            Vector3 newPos = startPos + offset * delta;
            UIText.transform.position = newPos;
        }
    }
    public TextMeshProUGUI textPrefab;

    const int POOL_SIZE = 64;

    Queue<TextMeshProUGUI> textPool = new Queue<TextMeshProUGUI>();
    List<ActiveText> activeTexts = new List<ActiveText>();

    private Camera _camera;
    private Transform _transformPos;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _camera = Camera.main;
        _transformPos = transform;
        for (int i = 0; i < POOL_SIZE; i++)
        {
            Vector3 calibratedTransforPos = new Vector3(_transformPos.position.x + Random.Range(-1f, 1f), _transformPos.position.y + Random.Range(-1f, 1f), _transformPos.position.z);
            TextMeshProUGUI temp = Instantiate(textPrefab, _transformPos);
            temp.gameObject.SetActive(false);
            textPool.Enqueue(temp);
        }
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

    public void AddText(int amount, Vector3 unitPos)
    {
        var item = textPool.Dequeue();
        item.text = amount.ToString();
        item.gameObject.SetActive(true);

        ActiveText activeText = new ActiveText() { maxTime = 1.0f };
        activeText.timer = activeText.maxTime;
        activeText.UIText = item;
        activeText.unitPosition = unitPos + Vector3.up;

        activeText.MoveText(_camera);
        activeTexts.Add(activeText);
    }
}
