using System.Linq;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public Transform[] pathPoints; // Points du chemin ou peut être le transformer en Vector3 à voir
    private int currentPointIndex = 0; // L'indice du point actuel que l'ennemi doit atteindre

    private LineRenderer lineRenderer;

    private void Start()
    {
        GameObject[] pathpointObjects = GameObject.FindGameObjectsWithTag("Pathpoint");
        pathPoints = pathpointObjects
            .OrderBy(obj => obj.name) // Sort alphabetically by name
            .Select(obj => obj.transform) // Convert GameObject to Transform
            .ToArray();

        // Debug: Log the found path points
        foreach (var point in pathPoints)
        {
            Debug.Log($"Pathpoint added: {point.name}");
        }

        lineRenderer = GetComponent<LineRenderer>();
        DrawPathLines();
    }

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

    private void DrawPathLines()
    {
        if (pathPoints == null || pathPoints.Length < 2)
        {
            Debug.LogWarning("Not enough points to draw a path.");
            return;
        }

        // Set the number of points in the LineRenderer
        lineRenderer.positionCount = pathPoints.Length;

        // Assign the positions of the path points to the LineRenderer
        for (int i = 0; i < pathPoints.Length; i++)
        {
            lineRenderer.SetPosition(i, pathPoints[i].position);
        }

        // Configure LineRenderer appearance
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }
}
