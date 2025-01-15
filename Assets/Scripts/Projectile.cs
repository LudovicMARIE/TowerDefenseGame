using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform target; // The target the projectile is traveling toward
    public float speed = 10f; // Speed of the projectile

    public int damage;

    private void Update()
    {
        if (target == null)
        {
            // Destroy the projectile if the target is missing
            Destroy(gameObject);
            return;
        }

        // Move the projectile toward the target
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Check if the projectile has reached the target
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            OnHit();
        }
    }

    private void OnHit()
    {
        EnemyController enemy = target.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage); 
        }

        Destroy(gameObject);
    }
}
