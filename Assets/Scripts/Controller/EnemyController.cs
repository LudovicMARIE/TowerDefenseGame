using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    private PathFollower pathFollower;

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

    // Ajouter d'autres fonctionnalités liées à l'ennemi, comme la gestion de la santé
}