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
        expSphere.transform.position = expDropPoint.position;
        Vector3 randVector = new Vector3(Random.Range(0f, 1f), 2, Random.Range(0f, 1f));
        expSphere.GetComponent<Rigidbody>().AddForce(randVector.normalized * 5f, ForceMode.Impulse);
    }
}
