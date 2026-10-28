using UnityEngine; // Importa as funções da Unity.

public class Projectile : MonoBehaviour
{
    public float speed = 10f; 
    public float damage = 25f;
    public float knockbackForce = 8f;
    private Vector3 direction; 
    private ShootPool Shootpool; 

    void OnEnable()
    {
        Invoke("SpawnTime", 5f); 
    }

    public void StartProjectile(Vector3 direction, ShootPool shooter)
    {
        this.direction = direction; 
        this.Shootpool = shooter;        
    }
    void Update()
    {       
        transform.position += direction * speed * Time.deltaTime;
    }
    void OnCollisionEnter(Collision collision)
    {
        GameObject attacker = Shootpool != null ? Shootpool.transform.root.gameObject : null;

        HealthSystem health = collision.collider.GetComponentInParent<HealthSystem>();
        if (health != null) {
            health.TakeDamage(damage, attacker);
            KnockbackReceiver kb = health.GetComponent<KnockbackReceiver>();
            if (kb != null) kb.ApplyKnockback(direction, knockbackForce);
        } else {
            // Compatibilidade: inimigos que ainda usam o EnemyHealth antigo.
            EnemyHealth legacy = collision.gameObject.GetComponent<EnemyHealth>();
            if (legacy != null) legacy.TakeDamage(damage);
        }
        CancelInvoke("SpawnTime");
        Shootpool.ReturnProjectile(gameObject);
    }
    void SpawnTime()
    {
         Shootpool.ReturnProjectile(gameObject);
    }
}