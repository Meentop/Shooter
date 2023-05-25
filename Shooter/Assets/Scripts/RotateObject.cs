using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private Vector3 rotateSpeed;
    void Update()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        Vector3 newRotation = currentRotation + (rotateSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(newRotation.x, newRotation.y, newRotation.z);
    }
}
