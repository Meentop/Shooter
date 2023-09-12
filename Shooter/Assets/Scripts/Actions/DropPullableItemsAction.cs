using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPullableItemsAction : ActionBase
{
    [SerializeField] private Agent agent;
    [SerializeField] private Gold gold;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private PullableConfig pullableConfig;
    [SerializeField] private GoldPriceConfig priceConfig;

    public override void ExecuteAction(params ActionParameter[] parametr)
    {
        DropGold();  
    }

    private void DropGold()
    {
        for (int i = 0; i < priceConfig.GetEnemiesPrice(agent); i++)
        {
            Gold gold = ObjectPool.Instance.GetObject(this.gold);
            PushItem(gold.transform);
        }
    }

    private void PushItem(Transform item)
    {
        item.position = (Random.insideUnitSphere * pullableConfig.randomInsideSphereSize) + dropPoint.position;
        Vector3 randVector = new Vector3(Random.Range(pullableConfig.XZMin, pullableConfig.XZMax), Random.Range(pullableConfig.YMin, pullableConfig.YMax), Random.Range(pullableConfig.XZMin, pullableConfig.XZMax));
        item.GetComponent<Rigidbody>().AddForce(randVector, ForceMode.Impulse);
    }
}
