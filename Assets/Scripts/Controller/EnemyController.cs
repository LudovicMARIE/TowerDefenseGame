using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    private PathFollower pathFollower;

    public int hp;
    public float timeAlive = 0f;

    private HashSet<TowerController> towerControllers = new HashSet<TowerController>();

    void Start()
    {
        pathFollower = GetComponent<PathFollower>();

    }

    void Update()
    {
        timeAlive += Time.deltaTime * 1000f;
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

    public void AddTowerController(TowerController controller)
    {
        towerControllers.Add(controller); // Add tower to the set
    }

    public void RemoveTowerController(TowerController controller)
    {
        towerControllers.Remove(controller); // Remove tower from the set
    }
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
        foreach (var tower in towerControllers)
        {
            tower.RemoveEnemyFromRange(gameObject);
        }
        Destroy(gameObject);
    }

    #endregion

}