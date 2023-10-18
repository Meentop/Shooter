using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPullableItemsAction : ActionBase
{
    [SerializeField] private Agent agent;
    [SerializeField] private Gold oneGold, tenGold;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private PullableConfig pullableConfig;
    [SerializeField] private GoldProfitConfig priceConfig;

    public override void ExecuteAction(params ActionParameter[] parametr)
    {
        DropGold();  
    }

    private void DropGold()
    {
        int price;
        if (agent != null)
            price = priceConfig.GetEnemiesPrice(agent);
        else
            price = priceConfig.GetChestGold();
        for (int i = 0; i < price / 10; i++)
        {
            Gold gold = ObjectPool.Instance.GetObject(tenGold);
            PushItem(gold.transform);
        }
        for (int i = 0; i < price % 10; i++)
        {
            Gold gold = ObjectPool.Instance.GetObject(oneGold);
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
