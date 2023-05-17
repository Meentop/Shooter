using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float dmgRangeMin;
    [SerializeField] private float dmgRangeMax;
    [SerializeField] private GameObject decalPrefab;
    private Vector3 lastPos;

    void Start()
    {
        lastPos = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        Debug.DrawLine(lastPos, transform.position);

        if (Physics.Linecast(lastPos, transform.position, out var hit))
        {
            if (hit.transform.TryGetComponent<IDamageReceiver>(out var damageReceiver))
            {
                damageReceiver.OnGetDamage(new DamageData(Random.Range(dmgRangeMin, dmgRangeMax), hit));
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
