public class DropExpAction : ActionBase
{
    public override void ExecuteAction(params ActionParameter[] parametr)
    {
        ExpSphere expSphere = ObjectPool.Instance.GetExpSphere();
        expSphere.transform.position = transform.position;
    }
}
