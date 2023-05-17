using System.Collections;
using UnityEngine;

public class DamageIndicatorAction : ActionBase
{
    [SerializeField] private Color originalColor;
    [SerializeField] private Color damageIndicatorColor;
    [SerializeField] private float delay;

    public override void ExecuteAction(params ActionParameter[] parametr)
    {
        ResetColor(damageIndicatorColor);
        StartCoroutine(ChangeColorInvoke());
    }

    private IEnumerator ChangeColorInvoke()
    {
        yield return new WaitForSeconds(delay);
        ResetColor(originalColor);
    }

    private void ResetColor(Color colorToReset)
    {
        gameObject.GetComponent<MeshRenderer>().material.color = colorToReset;
    }
}
