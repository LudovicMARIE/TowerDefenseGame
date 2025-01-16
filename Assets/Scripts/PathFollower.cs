using System.Linq;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public Transform[] pathPoints; 
    private int currentPointIndex = 0; 

    private LineRenderer lineRenderer;

    private void Start()
    {
        GameObject[] pathpointObjects = GameObject.FindGameObjectsWithTag("Pathpoint");
        pathPoints = pathpointObjects
            .OrderBy(obj => obj.name) 
            .Select(obj => obj.transform) 
            .ToArray();

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

            Vector3 directionToTarget = targetPosition - transform.position;

            // Rotate towards the target
            if (directionToTarget != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speed * Time.deltaTime * 100); // Adjust the multiplier for smoother rotation
            }
            this.gameObject.transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            /* Mirror if to add error margin
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
        //if (pathPoints == null || pathPoints.Length < 2)
        //{
        //    Debug.LogWarning("Not enough points to draw a path.");
        //    return;
        //}

        //// Set the number of points in the LineRenderer
        //lineRenderer.positionCount = pathPoints.Length;

        //// Assign the positions of the path points to the LineRenderer
        //for (int i = 0; i < pathPoints.Length; i++)
        //{
        //    lineRenderer.SetPosition(i, pathPoints[i].position);
        //}

        //// Configure LineRenderer appearance
        //lineRenderer.startWidth = 0.2f;
        //lineRenderer.endWidth = 0.2f;
        //lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        //lineRenderer.startColor = Color.green;
        //lineRenderer.endColor = Color.green;
    }
}
