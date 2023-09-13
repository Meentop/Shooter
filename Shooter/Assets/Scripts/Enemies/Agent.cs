using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Agent : MonoBehaviour, IPauseble
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] private string enemyName;
    [SerializeField] private float normalMaxSpeed;
    [SerializeField] private float normalAccel;
    [SerializeField] private float normalRotationSpeed;

    private float maxSpeed;
    private float accel;
    private float rotationSpeed;
    public float orientation { get; private set; }
    private Vector3 velocity;
    private Quaternion rotation;
    private Steering _steering;
    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        foreach (var state in GetComponentsInChildren<AgentState>())
        {
            state.Init(player);
        }
        foreach (var condition in GetComponentsInChildren<Condition>())
        {
            condition.Init(player);
        }
    }

    private void Start()
    {
        maxSpeed = normalMaxSpeed;
        accel = normalAccel;
        rotationSpeed = normalRotationSpeed;
        velocity = Vector3.zero;  
        _steering = new Steering();
        PauseManager.OnSetPause += OnSetPause;
    }

    private void Update()
    {
        if(!PauseManager.Pause)
            transform.parent.rotation = Quaternion.RotateTowards(transform.parent.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (!PauseManager.Pause)
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }

    private void LateUpdate()
    {
        if (!PauseManager.Pause)
        {
            velocity += accel * Time.deltaTime * _steering.linear;
            if (velocity.magnitude > maxSpeed)
            {
                velocity.Normalize();
                velocity *= maxSpeed;
            }
            if (_steering.linear.sqrMagnitude == 0f)
            {
                velocity = Vector3.zero;
            }

            orientation = _steering.orientation;
            rotation = Quaternion.Euler(0, orientation, 0);
            _steering = new Steering();
        }
    }

    public void SetSteering(Steering steering)
    {
        _steering = steering;
    }

    public string GetName() => enemyName;

    public void SetMaxSpeed(float maxSpeed)
    {
        this.maxSpeed = maxSpeed;
    }

    public void SetNormalMaxSpeed()
    {
        maxSpeed = normalMaxSpeed;
    }

    public void SetAcceleration(float acceleration)
    {
        accel = acceleration;
    }

    public void SetNormalAcceleration()
    {
        accel = normalAccel;
    }

    public void SetRotationSpeed(float rotationSpeed)
    {
        this.rotationSpeed = rotationSpeed;
    }

    public void SetNormalRotationSpeed()
    {
        rotationSpeed = normalRotationSpeed;
    }

    public Vector3 GetVelocity() => velocity;

    public float MapToRange(float rotation)
    {
        rotation %= 360f;
        if (Mathf.Abs(rotation) > 180f)
        {
            if (rotation < 0f)
                rotation += 360f;
            else
                rotation -= 360f;
        }
        return rotation;
    }

    public void OnSetPause(bool value)
    {
        if (value)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            animator.enabled = false;
        }
        else
        {
            rb.isKinematic = false;
            animator.enabled = true;
        }
    }

    private void OnDestroy()
    {
        PauseManager.OnSetPause -= OnSetPause;
    }
}
