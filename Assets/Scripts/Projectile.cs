using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform target;
    public float speed = 10f; 

    public int damage;

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

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
