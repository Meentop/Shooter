using UnityEngine;

public class DestroyOnDelayAction : ActionBase
{
    [SerializeField] private float delay;
    public override void ExecuteAction(params ActionParameter[] parametr)
    {
        Destroy(gameObject, delay);
    }
}
