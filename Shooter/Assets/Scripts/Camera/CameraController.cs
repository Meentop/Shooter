using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private CameraConfig cameraConfig;
    [SerializeField] private float returnSpeed;

    private Vector3 targetRotation = new Vector3();
    private Vector3 currentRotation;
    private float _snappiness;
    private float _fireTime = 2;

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
        if(_fireTime >= 0)
            _fireTime -= Time.deltaTime;

        float mouseX = Input.GetAxisRaw("Mouse X") * cameraConfig.sensivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * cameraConfig.sensivity * Time.deltaTime;

        yRotation += mouseX;

        xRotation -= mouseY;

        if(_fireTime >= 0)
            currentRotation = Vector3.Slerp(currentRotation, targetRotation, _snappiness * Time.deltaTime);
        else
            currentRotation = Vector3.Slerp(currentRotation, Vector3.zero, _snappiness * Time.deltaTime);

        xRotation = Mathf.Clamp(xRotation, -90f - currentRotation.x, 90f - currentRotation.x);

        transform.rotation = Quaternion.Euler(xRotation + currentRotation.x, yRotation + currentRotation.y, 0);
        player.rotation = Quaternion.Euler(0, yRotation + currentRotation.y, 0);
    }

    public void FireRecoil(Vector3 recoilXYZ, float snappiness)
    {
        xRotation += currentRotation.x;
        yRotation += currentRotation.y;
        currentRotation = Vector3.zero;

        targetRotation = new Vector3(recoilXYZ.x, Random.Range(-recoilXYZ.y, recoilXYZ.y), 0);
        _snappiness = snappiness;
        _fireTime = 0.2f;
    }
}
