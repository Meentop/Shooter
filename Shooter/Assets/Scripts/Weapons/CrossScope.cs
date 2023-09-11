using UnityEngine;

public class CrossScope : MonoBehaviour
{
    [SerializeField] private Parts[] parts;

    [SerializeField] private float speedSpread;

    public float CurrentSpread;

    private float _currentSpreadInternal;

    void Update()
    {
        CrossheirUpdate();
    }

    public void CrossheirUpdate()
    {
        float t = 0.005f * speedSpread;
        _currentSpreadInternal = Mathf.Lerp(_currentSpreadInternal, CurrentSpread, t);

        for (int i = 0; i < parts.Length; i++)
        {
            Parts p = parts[i];
            p.trans.anchoredPosition = p.pos * _currentSpreadInternal;
        }
    }

    [System.Serializable]
    public class Parts
    {
        public RectTransform trans;
        public Vector2 pos;
    }
}

