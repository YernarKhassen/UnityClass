using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Damage")]
    public float damage = 20f;
    public float radius = 4f;

    [Header("Push")]
    public float pushForce = 20f;
    public ForceMode pushMode = ForceMode.Impulse;

    [Header("Lifetime")]
    public float lifeTime = 2f;

    private void Start()
    {
        ApplyDamage();
        ApplyPush();

        Destroy(gameObject, lifeTime);
    }

    private void ApplyDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

        foreach (var hit in hits)
        {
            Damageable dmg = hit.GetComponent<Damageable>();
            if (dmg != null)
            {
                dmg.ApplyDamage(damage);
            }
        }
    }

    private void ApplyPush()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

        foreach (var hit in hits)
        {
            Rigidbody rb = hit.attachedRigidbody;
            if (rb != null)
            {
                Vector3 dir = (rb.transform.position - transform.position).normalized;
                rb.AddForce(dir * pushForce, pushMode);
            }
        }
    }
}