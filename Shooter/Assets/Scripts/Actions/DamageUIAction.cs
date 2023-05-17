//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class DamageUIAction : ActionBase
//{
//    public static DamageUIAction Instance { get; private set; }
//    private class ActiveText
//    {
//        public Text UIText;
//        public float maxTime;
//        public float timer;
//        public Vector3 unitPosition;

//        public void MoveText(Camera camera)
//        {
//            float delta = 1.0f - (timer / maxTime);
//            Vector3 pos = unitPosition + new Vector3(delta, delta, 0);
//            pos = camera.WorldToScreenPoint(pos);
//            pos.z = 0;

//            UIText.transform.position = pos;
//        }
//    }
//    public Text textPrefab;

//    const int POOL_SIZE = 64;

//    Queue<Text> textPool = new Queue<Text>();
//    List<ActiveText> activeTexts = new List<ActiveText>();

//    private Camera _camera;
//    private Transform _transformPos;
    
//    void Awake()
//    {
//        Instance = this;
//    }

//    private void Start()
//    {
//        for (int i = 0; i < POOL_SIZE; i++)
//        {
//            Text temp = Instantiate(textPrefab, _transformPos);
//            temp.gameObject.SetActive(false);
//            textPool.Enqueue(temp);
//        }
//    }

//    void Update()
//    {
//        for (int i = 0; i < activeTexts.Count; i++)
//        {
//            ActiveText activeText = activeTexts[i];
//            activeText.timer -= Time.deltaTime;

//            if (activeText.timer <= 0)
//            {
//                activeText.UIText.gameObject.SetActive(false);
//                textPool.Enqueue(activeText.UIText);
//                activeTexts.RemoveAt(i);
//                --i;
//            }
//            else
//            {
//                var color = activeText.UIText.color;
//                color.a = activeText.timer / activeText.maxTime;
//                activeText.UIText.color = color;

//                activeText.MoveText(_camera);            
//            }
//        }
//    }

//    public void AddText(int amount, Vector3 unitPos)
//    {
//        var item = textPool.Dequeue();
//        item.text = amount.ToString();
//        item.gameObject.SetActive(true);

//        ActiveText activeText = new ActiveText() { maxTime = 1.0f};
//        activeText.timer = activeText.maxTime;
//        activeText.UIText = item;
//        activeText.unitPosition = unitPos + Vector3.up;

//        activeText.MoveText(_camera);
//        activeTexts.Add(activeText);
//    }

//    public override void ExecuteAction(params ActionParameter[] parametr)
//    {

//    }
//}
