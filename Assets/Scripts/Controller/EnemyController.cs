using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    private PathFollower pathFollower;

    public int hp;

    void Start()
    {
        pathFollower = GetComponent<PathFollower>();
    }

    void Update()
    {
        if (pathFollower.HasReachedEnd())
        {
            Destroy(gameObject);
        }
        else
        {
            pathFollower.MoveToNextPoint(speed);
        }
    }



    #region Enemy stats

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage, remaining HP: {hp}");

        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    #endregion

}