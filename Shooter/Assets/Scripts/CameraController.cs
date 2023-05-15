using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private CameraConfig cameraConfig;

    private float _mouseX, _mouseY;
    private float xRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        _mouseX = Input.GetAxis("Mouse X") * cameraConfig.sensivity * Time.deltaTime;
        _mouseY = Input.GetAxis("Mouse Y") * cameraConfig.sensivity * Time.deltaTime;

        xRotation -= _mouseY;
        xRotation = Mathf.Clamp(xRotation, cameraConfig.minAngle, cameraConfig.maxAngle);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(-_mouseY * new Vector3(1, 0, 0) * Time.deltaTime);

        player.Rotate(_mouseX * new Vector3(0, 1, 0));

    }
}
