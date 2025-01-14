using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public Transform[] pathPoints; // Points du chemin ou peut être le transformer en Vector3 à voir
    private int currentPointIndex = 0; // L'indice du point actuel que l'ennemi doit atteindre

    public bool HasReachedEnd()
    {
        return currentPointIndex >= pathPoints.Length;
    }

    public void MoveToNextPoint(float speed)
    {
        if (!HasReachedEnd())
        {
            Vector3 targetPosition = pathPoints[currentPointIndex].position;
            this.gameObject.transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            /* Si l'ennemi atteint le point cible (avec une petite marge d'erreur)
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            */
            
            if (this.gameObject.transform.position == targetPosition)
            {
                currentPointIndex++;
            }
        }
    }
}
