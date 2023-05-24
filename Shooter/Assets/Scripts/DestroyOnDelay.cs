using UnityEngine;

public class DestroyOnDelay : MonoBehaviour
{
    [SerializeField] private float delayToDestroy;

    private void Start()
    {
        Destroy(gameObject, delayToDestroy);
    }
}
