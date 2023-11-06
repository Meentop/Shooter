using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private CameraConfig cameraConfig;

    private Vector3 targetRotation = new Vector3();
    private Vector3 currentRotation;
    private float _snappiness;
    private float _returnSpeed = 2;

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

        transform.rotation = Quaternion.Euler(xRotation + currentRotation.x, yRotation + currentRotation.y, 0 + currentRotation.z);
        player.rotation = Quaternion.Euler(0, yRotation, 0);

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, _returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, _snappiness * Time.fixedDeltaTime);
    }

    public void FireRecoil(Vector3 recoilXYZ, float snappiness, float returnSpeed)
    {
        targetRotation += new Vector3(recoilXYZ.x, Random.Range(-recoilXYZ.y, recoilXYZ.y), Random.Range(-recoilXYZ.z, recoilXYZ.z));
        _snappiness = snappiness;
        _returnSpeed = returnSpeed;
    }
}
