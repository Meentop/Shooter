using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected Transform shotPoint;
    [Space]
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected ParticleSystem shootEffect;
    [Space]
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected int startAmmo;
    [SerializeField] protected int ammoPerShoot;


    public int CurrentAmmo
    {
        get => _currentAmmo;
        set
        {
            _currentAmmo = value;
            _UIManager.SetText(UIManager.TextFieldKeys.AmmoCurrentText, _currentAmmo.ToString());
        }
    }
    public int FreeAmmo
    {
        get => _freeAmmo;
        set
        {
            _freeAmmo = value;
            _UIManager.SetText(UIManager.TextFieldKeys.AmmoLeftText, _freeAmmo.ToString());
        }
    }


    private UIManager _UIManager;
    private Player _player;
    private Rigidbody _rigidbody;
    private Collider _collider;

    private Transform _targetLook;
    private Transform _weaponPos;

    private int _currentAmmo;
    private int _freeAmmo;
    private bool _isInited;


    public void Init(UIManager uIManager, Player player, Transform weaponHolder)
    {
        _UIManager = uIManager;
        _player = player;
        _weaponPos = weaponHolder;
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _collider.enabled = false;

        FreeAmmo = startAmmo;
        Reload();

        _isInited = true;
        OnInit();
    }

    public abstract void OnInit();

    public abstract void Shoot();


    void Update()
    {
        if (!_isInited)
            return;

        Vector3 origin = shotPoint.position;
        Vector3 dir = _targetLook.position;

        Debug.DrawLine(origin, dir, Color.red);
        shotPoint.transform.LookAt(_targetLook);
    }

    protected void SpendAmmo(int amount)
    {
        if (CurrentAmmo > amount)
        {
            CurrentAmmo -= amount;
            return;
        }

        CurrentAmmo = 0;
    }

    public void AddAmmo(int amount)
    {
        FreeAmmo += amount;
    }

    public virtual void Reload()
    {
        if (FreeAmmo > maxAmmo)
        {
            CurrentAmmo = maxAmmo;
            FreeAmmo -= maxAmmo;
            return;
        }

        if (FreeAmmo > 0)
        {
            CurrentAmmo = FreeAmmo;
            FreeAmmo = 0;
        }
    }
}
