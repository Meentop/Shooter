using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField] private ExpSphere expPrefab;

    private readonly List<ExpSphere> freeExp = new List<ExpSphere>();

    private void Awake()
    {
        Instance = this;
    }

    public ExpSphere GetExpSphere()
    {
        ExpSphere expSphere;
        if(freeExp.Count > 0)
        {
            expSphere = freeExp[0];
            freeExp.Remove(expSphere);
        }
        else
        {
            expSphere = Instantiate(expPrefab, transform);
        }
        expSphere.Destroyed += ReturnExpSphere;
        expSphere.StartTimer();
        return expSphere;
    }

    private void ReturnExpSphere(ExpSphere expSphere)
    {
        expSphere.Destroyed -= ReturnExpSphere;
        freeExp.Add(expSphere);
        expSphere.transform.position = transform.position;
    }
}
