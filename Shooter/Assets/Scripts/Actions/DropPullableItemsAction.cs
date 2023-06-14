using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPullableItemsAction : ActionBase
{
    [SerializeField] private ExpSphere expSphere;
    [SerializeField] private Gold gold;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private PullableConfig config;
    [SerializeField] private int exp;
    [SerializeField] private int goldCount;

    public override void ExecuteAction(params ActionParameter[] parametr)
    {
        DropExp();
        DropGold();  
    }

    private void DropExp()
    {
        ExpSphere expSphere = ObjectPool.Instance.GetObject(this.expSphere);
        expSphere.exp = exp;
        PushItem(expSphere.transform);
    }

    private void DropGold()
    {
        for (int i = 0; i < goldCount; i++)
        {
            Gold gold = ObjectPool.Instance.GetObject(this.gold);
            PushItem(gold.transform);
        }
    }

    private void PushItem(Transform item)
    {
        item.position = (Random.insideUnitSphere * config.randomInsideSphereSize) + dropPoint.position;
        Vector3 randVector = new Vector3(Random.Range(config.XZMin, config.XZMax), Random.Range(config.YMin, config.YMax), Random.Range(config.XZMin, config.XZMax));
        item.GetComponent<Rigidbody>().AddForce(randVector, ForceMode.Impulse);
    }
}
