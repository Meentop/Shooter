using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected Transform shotPoint;
    [Space]
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected ParticleSystem shootEffect;

    private Player _player;
    private Rigidbody _rigidbody;
    private Collider _collider;

    private Transform _targetLook;
    private Transform _weaponPos;

    private bool _isInited;


    public void Init(Player player, Transform weaponHolder, Transform targetLook)
    {
        _player = player;
        _weaponPos = weaponHolder;
        _targetLook = targetLook;


        _isInited = true;
        OnInit();
    }

    public abstract void OnInit();

    public abstract void Shoot();

    public abstract void StopShooting();


    protected virtual void Update()
    {
        if (!_isInited)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            StopShooting();
        }

        
        Vector3 origin = shotPoint.position;
        Vector3 dir = _targetLook.position;

        Debug.DrawLine(origin, dir, Color.red);
        shotPoint.transform.LookAt(_targetLook);
    }
}
