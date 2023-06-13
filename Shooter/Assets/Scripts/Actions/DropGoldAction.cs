using UnityEngine;

public class DropGoldAction : ActionBase
{
    [SerializeField] private Gold gold;
    [SerializeField] private Transform goldDropPoint;
    [SerializeField] private int count;

    public override void ExecuteAction(params ActionParameter[] parametr)
    {
        for (int i = 0; i < count; i++)
        {
            Gold gold = ObjectPool.Instance.GetObject(this.gold);
            gold.transform.position = (Random.insideUnitSphere * 0.2f) + goldDropPoint.position;
            Vector3 randVector = new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 3f), Random.Range(-1f, 1f));
            gold.GetComponent<Rigidbody>().AddForce(randVector, ForceMode.Impulse);
        }
    }
}
