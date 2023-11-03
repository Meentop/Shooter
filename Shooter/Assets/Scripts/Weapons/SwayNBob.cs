using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwayNBob : MonoBehaviour
{
    private PlayerController player;

    [Header("Sway")]
    [SerializeField] private float step = 0.01f;
    [SerializeField] private float maxStepDistance = 0.06f;
    private Vector3 swayPos;

    [Header("Sway rotation")]
    [SerializeField] private float rotationStep = 4f;
    [SerializeField] private float maxRotationStep = 5f;
    private Vector3 swayEulerRot;

    private float smooth = 10f;
    private float smoothRot = 12f;

    [Header("Bobbing")]
    private float speedCurve;
    private float curveSin { get => Mathf.Sin(speedCurve); }
    private float curveCos { get => Mathf.Cos(speedCurve); }
    [Header("Bobbing")]
    [SerializeField] private Vector3 travelLimit = Vector3.one * 0.025f;
    [SerializeField] private Vector3 bobLimit = Vector3.one * 0.01f;
    [SerializeField] private Vector3 dashLimit = Vector3.one * 0.05f;
    private Vector3 bobPosition;

    [SerializeField] private float bobExaggeration;

    [Header("Bob rotation")]
    [SerializeField] private Vector3 multiplier;
    private Vector3 bobEulerRotation;

    public void Init(PlayerController playerController)
    {
        player = playerController;
        
    }

    private void Update()
    {
        if (!PauseManager.Pause)
        {
            GetInput();

            Sway();
            SwayRotation();
            if (!player.Dashing)
            {
                BobOffset();
                BobRotation();
            }
            else
            {
                BobDashingOffset();
                BobDashingRotation();
            }

            CompositePositionRotation();
        }
    }

    private Vector2 walkInput;
    private Vector2 lookInput;

    private void GetInput()
    {
        walkInput.x = !player.Dashing ? Input.GetAxis("Horizontal") : player.DashDirection.x;
        walkInput.y = !player.Dashing ? Input.GetAxis("Vertical") : player.DashDirection.y;
        walkInput = walkInput.normalized;

        lookInput.x = Input.GetAxis("Mouse X");
        lookInput.y = Input.GetAxis("Mouse Y");
    }

    private void Sway()
    {
        Vector3 invertLook = lookInput * -step;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance);

        swayPos = invertLook;
    }

    private void SwayRotation()
    {
        Vector2 invertLook = lookInput * -rotationStep;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxRotationStep, maxRotationStep);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxRotationStep, maxRotationStep);
        swayEulerRot = new Vector3(-invertLook.y, invertLook.x, invertLook.x);
    }

    private void CompositePositionRotation()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPos + bobPosition, Time.deltaTime * smooth);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(swayEulerRot) * Quaternion.Euler(bobEulerRotation), Time.deltaTime * smoothRot);
    }

    private void BobOffset()
    {
        float speed = Mathf.Clamp(Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical")), -1, 1);
        speedCurve += Time.deltaTime * (player.Grounded ? (speed * bobExaggeration) : 1f) + 0.01f;

        bobPosition.x = (curveCos * bobLimit.x * (player.Grounded ? 1 : 0)) - (walkInput.x * travelLimit.x);
        bobPosition.y = (curveSin * bobLimit.y) - (walkInput.y * travelLimit.y);
        bobPosition.z = -(walkInput.y * travelLimit.z);
    }

    private void BobDashingOffset()
    {
        bobPosition.x = -(walkInput.x * dashLimit.x);
        bobPosition.y = 0;
        bobPosition.z = -(walkInput.y * dashLimit.z);
    }

    private void BobRotation()
    {
        bobEulerRotation.x = walkInput != Vector2.zero ? multiplier.x * (Mathf.Sin(2 * speedCurve)) : multiplier.x * (Mathf.Sin(2 * speedCurve) / 2);
        bobEulerRotation.y = walkInput != Vector2.zero ? multiplier.y * curveCos : 0;
        bobEulerRotation.z = walkInput != Vector2.zero ? multiplier.z * curveCos * walkInput.x : 0; 
    }

    private void BobDashingRotation()
    {
        bobEulerRotation = Vector3.zero;
    }
}
