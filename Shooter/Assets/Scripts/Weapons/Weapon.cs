using UnityEngine;

public abstract class Weapon : MonoBehaviour, ICollectableItem
{
    [SerializeField] protected ParticleSystem shootEffect;
    [SerializeField] protected ParticleSystem decalPrefab;
    [Space]
    [SerializeField] protected Vector2Int damageRange;
    [SerializeField] protected Vector3 f;
    [SerializeField] protected float range;

    public CollectableItems ItemType => CollectableItems.Weapon;

    private Player _player;
    private Collider _collider;

    private Transform _targetLook;
    private Transform _weaponHolder;

    private bool _isInited;


    public void Init(Player player, Transform weaponHolder, Transform targetLook)
    {
        _player = player;
        _weaponHolder = weaponHolder;
        _targetLook = targetLook;
        _collider = GetComponent<Collider>();
        gameObject.layer = LayerMask.NameToLayer("Weapon");



        _isInited = true;
        OnInit();
    }

    public abstract void OnInit();

    public abstract void Shoot();

    public abstract void StopShooting();

    public void DisconnectWeaponFromPlayer(Weapon savedWeapon)
    {
        GetComponent<Weapon>().enabled = false;
        transform.parent.parent = savedWeapon.transform.parent.parent;
        transform.parent.localPosition = savedWeapon.transform.parent.localPosition;
        transform.parent.localRotation = Quaternion.identity;
        GetComponent<Collider>().enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void ConectWeaponToPlayer()
    {
        GetComponent<Weapon>().enabled = true;
        GetComponent<Collider>().enabled = false;
        transform.parent.transform.parent = _weaponHolder.transform;
        transform.parent.localPosition = new Vector3(0, 0, 0);
        transform.parent.localRotation = Quaternion.identity;
    }
    


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
    }

    public void OnCollect()
    {
        
    }
}
