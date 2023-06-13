using UnityEngine;

public class DropExpAction : ActionBase
{
    [SerializeField] private ExpSphere expSphere;
    [SerializeField] private Transform expDropPoint;
    [SerializeField] private int exp;

    public override void ExecuteAction(params ActionParameter[] parametr)
    {
        ExpSphere expSphere = ObjectPool.Instance.GetObject(this.expSphere);
        expSphere.exp = exp;
        expSphere.transform.position = (Random.insideUnitSphere * 0.2f) + expDropPoint.position;
        Vector3 randVector = new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 3f), Random.Range(-1f, 1f));
        expSphere.GetComponent<Rigidbody>().AddForce(randVector, ForceMode.Impulse);
    }
}
