using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierAward : MonoBehaviour
{
    [SerializeField] private Transform[] stands;
    [SerializeField] private List<Modifier> modifiers;
    [SerializeField] private float modifierHeight;

    private void Start()
    {
        foreach (var stand in stands)
        {
            int randomNumber = Random.Range(0, modifiers.Count);
            Modifier modifier = modifiers[randomNumber];
            modifiers.RemoveAt(randomNumber);
            Transform modifierTransform = Instantiate(modifier, stand).transform;
            modifierTransform.localPosition = new Vector3(0, modifierHeight, 0);
        }
    }

    public void DeleteOtherModifiers(Transform thisStand)
    {
        foreach (var stand in stands)
        {
            if (stand != thisStand && stand.childCount > 0)
                Destroy(stand.GetComponentInChildren<Modifier>().gameObject);
        }
    }
}
