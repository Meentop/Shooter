using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private CameraConfig cameraConfig;

    private float xRotation, yRotation;

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        if (!PauseManager.Pause)
            Rotate();
    }

    private void FixedUpdate()
    {
        
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * cameraConfig.sensivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * cameraConfig.sensivity * Time.deltaTime;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        player.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
