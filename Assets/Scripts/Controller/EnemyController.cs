using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    private PathFollower pathFollower;

    public float hp;
    public float timeAlive = 0f;
    public int goldValue = 1;
    public int scoreValue = 10;

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
            FindAnyObjectByType<PlayerController>()?.LoseHP(1);
            Destroy(gameObject);
        }
        else
        {
            pathFollower.MoveToNextPoint(speed);
        }
    }

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

        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindAnyObjectByType<AudioManager>().PlaySound("Button2");
        foreach (var tower in towerControllers)
        {
            tower.RemoveEnemyFromRange(gameObject);
        }
        FindAnyObjectByType<PlayerController>()?.AddGold(goldValue); 
        FindAnyObjectByType<PlayerController>()?.AddScore(scoreValue); 
        Destroy(gameObject);
    }


}