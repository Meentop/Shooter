using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPauseble
{
    [SerializeField] private Transform checkOnGroundPoint;
    [SerializeField] private AudioClip[] stepSounds;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip fallSound;
    [SerializeField] private AudioClip[] deshSounds;
    [SerializeField] private float stepTimer;
    private KeyCode jumpKey = KeyCode.Space;

    private Rigidbody _rb;
    private bool _exitingSlope;
    public bool Grounded { get; private set; }
    public bool Dashing { get; private set; }
    public Vector2 DashDirection { get; private set; }
    private float _dashTimer;
    private float _horizontalInput, _verticalInput;
    private Vector3 _moveDirection;
    private RaycastHit _slopeHit;
    private Player _player;
    private int _curDashCharges = 0;
    private Vector3 prepauseVelocity;
    private AudioSource _audioSource;
    private float _curStepTimer = 0;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        _player = GetComponent<Player>();
        PauseManager.OnSetPause += OnSetPause;
        _audioSource = Camera.main.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!PauseManager.Pause)
        {
            if (Grounded && !Physics.Raycast(transform.position, Vector3.down, 1.3f, LayerMask.GetMask("Solid"))) 
            {
                Grounded = false;
            }
            else if(!Grounded && Physics.Raycast(transform.position, Vector3.down, 1.3f, LayerMask.GetMask("Solid")))
            {
                Grounded = true;
                _audioSource.PlayOneShot(fallSound);
            }

            MyInput();
            SpeedControl();

            if (Grounded && !Dashing)
                _rb.drag = _player.Characteristics.groundDrag;
            else
                _rb.drag = 0;

            if (_dashTimer < _player.Characteristics.dashReloadTime && _curDashCharges < _player.Characteristics.dashCharges)
            {
                _dashTimer += Time.deltaTime;
            }
            if( _dashTimer >= _player.Characteristics.dashReloadTime && _curDashCharges < _player.Characteristics.dashCharges)
            {
                _dashTimer = 0;
                _curDashCharges++;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && _curDashCharges > 0 && !Dashing)
            {
                Dash();
                _audioSource.PlayOneShot(deshSounds[Random.Range(0, deshSounds.Length)]);
            }
            _player.SetDashInfo(_dashTimer / _player.Characteristics.dashReloadTime, _curDashCharges);
        }
    }

    private void FixedUpdate()
    {
        if (!PauseManager.Pause && !Dashing)
        {
            Movement();
        }    
    }

    private void MyInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(jumpKey) && Grounded && !Dashing)
        {
            Jump();

            Invoke(nameof(ResetJump), 1.0f);
        }
    }

    private void Movement()
    {
        _moveDirection = transform.forward * _verticalInput + transform.right * _horizontalInput;

        if (OnSlope() && !_exitingSlope)
        {
            _rb.AddForce(20f * _player.Characteristics.movementSpeed * GetSlopeMoveDirection(), ForceMode.Force);

            if (_moveDirection != Vector3.zero)
            {
                _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else if (Grounded)
        {
            _rb.AddForce(20f * _player.Characteristics.movementSpeed * _moveDirection, ForceMode.Force);
        }
        else if(!Grounded)
            _rb.AddForce(20f * _player.Characteristics.airMultiplayer * _player.Characteristics.movementSpeed * _moveDirection, ForceMode.Force);

        if (_curStepTimer <= 0 && _moveDirection != Vector3.zero && Grounded)
        {
            FootStep();
            _curStepTimer = stepTimer;
        }

        if (_curStepTimer > 0)
            _curStepTimer -= Time.fixedDeltaTime;
        _rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if (!Dashing)
        {
            if (OnSlope() && !_exitingSlope)
            {
                if (_rb.velocity.magnitude > _player.Characteristics.movementSpeed)
                    _rb.velocity = _rb.velocity.normalized * _player.Characteristics.movementSpeed;
            }
            else
            {
                Vector3 flatVel = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
                if (flatVel.magnitude > _player.Characteristics.movementSpeed)
                {
                    Vector3 limitedVel = flatVel.normalized * _player.Characteristics.movementSpeed;
                    _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
                }
            }
        }
    }

    private void Jump()
    {
        _exitingSlope = true;
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        _audioSource.PlayOneShot(jumpSound);

        _rb.AddForce(transform.up * _player.Characteristics.jumpStrength, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _exitingSlope = false;
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out _slopeHit, 1.4f))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < _player.Characteristics.maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal).normalized;
    }

    private void Dash()
    {
        StartCoroutine(DashCor());
    }

    private IEnumerator DashCor()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            direction = Vector3.forward;
        DashDirection = new Vector2(direction.x, direction.z);
        _curDashCharges--;
        Dashing = true;
        _dashTimer = 0;
        float dashTimer = _player.Characteristics.dashDuration;
        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        _rb.useGravity = false;
        while (dashTimer > 0)
        {
            if (!PauseManager.Pause)
            {
                Vector3 dashDirection = transform.TransformDirection(direction) * _player.Characteristics.dashStrength;
                _rb.velocity = new Vector3(dashDirection.x, _rb.velocity.y, dashDirection.z);
                dashTimer -= Time.fixedDeltaTime;
            }
            yield return new WaitForFixedUpdate();
        }
        _rb.useGravity = true;
        Dashing = false;
    }

    public void OnSetPause(bool value)
    {
        if (value)
        {
            prepauseVelocity = _rb.velocity;
            _rb.isKinematic = true;
        }
        else
        {
            _rb.isKinematic = false;
            _rb.velocity = prepauseVelocity;
        }
    }

    private void OnDestroy()
    {
        PauseManager.OnSetPause -= OnSetPause;
    }

    private void FootStep()
    {
        int randIndex = Random.Range(0, stepSounds.Length);
        _audioSource.PlayOneShot(stepSounds[randIndex]);
        _curStepTimer = stepTimer;
    }
}
