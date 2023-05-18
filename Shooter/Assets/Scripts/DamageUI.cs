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
            UIText.transform.position = newPos;
        }
    }
    public TextMeshProUGUI textPrefab;

    const int POOL_SIZE = 12;

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
                if (activeText.UIText != null)
                { 
                activeText.UIText.gameObject.SetActive(false);
                textPool.Enqueue(activeText.UIText);
                }
                activeTexts.RemoveAt(i);
                --i;
            }
            else
            {
                if (activeText.UIText != null)
                {
                    var color = activeText.UIText.color;
                    color.a = activeText.timer / activeText.maxTime;
                    activeText.UIText.color = color;
                }

                activeText.MoveText(_camera);
            }
        }
    }

    public void AddText(int amount, Vector3 unitPos)
    {
        TextMeshProUGUI item;
        if (textPool.Count > 0)
        { 
        item = textPool.Dequeue();
        }
        else
        {
            item = Instantiate(textPrefab, _transformPos);
        }
        item.text = amount.ToString();
        item.gameObject.SetActive(true);

        ActiveText activeText = new ActiveText() { maxTime = 1.0f };
        activeText.timer = activeText.maxTime;
        activeText.UIText = item;
        activeText.unitPosition = unitPos + new Vector3(0, Random.Range(0f, 1f), Random.Range(-1f, 1f));

        activeText.MoveText(_camera);
        activeTexts.Add(activeText);
        
    }
}
