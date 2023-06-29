using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private GameObject decalPrefab;
    private Vector3 lastPos;

    void Start()
    {
        lastPos = transform.position;
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);

        Debug.DrawLine(lastPos, transform.position);

        if (Physics.Linecast(lastPos, transform.position, out var hit))
        {
            if (hit.transform.TryGetComponent<IDamageReceiver>(out var damageReceiver))
            {
                DamageData damageData = new DamageData() { Damage = damage };
                damageReceiver.GetDamage(damageData); ;
            }
            else
            {
                var decal = Instantiate(decalPrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                decal.transform.SetParent(hit.transform);
                Destroy(decal, 5);
            }
            Destroy(gameObject);
        }
        lastPos = transform.position;
    }
}
