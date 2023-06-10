using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackout : MonoBehaviour
{
    public static Blackout Instance;
    private Camera blackoutCamera;
    [SerializeField] private Color blackoutColor;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        blackoutCamera = GetComponent<Camera>();
    }

    public void OnBlackout()
    {
        blackoutCamera.clearFlags = CameraClearFlags.SolidColor;
        blackoutCamera.backgroundColor = blackoutColor;
    }

    public void OffBlackout()
    {
        blackoutCamera.clearFlags = CameraClearFlags.Depth;
    }
}
