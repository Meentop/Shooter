using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float _mouseX, _mouseY;

    [SerializeField] private float sensivity = 200f;

    [SerializeField] private Transform player;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    float xRotation = 0f;

    private void Update()
    {
        _mouseX = Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
        _mouseY = Input.GetAxis("Mouse Y") * sensivity * Time.deltaTime;

        xRotation -= _mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(-_mouseY * new Vector3(1, 0, 0) * Time.deltaTime);

        player.Rotate(_mouseX * new Vector3(0, 1, 0));

    }
}
