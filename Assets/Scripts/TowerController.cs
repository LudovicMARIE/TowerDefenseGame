using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    private Coroutine printMessageCoroutine;
    public GameObject rangeVisual; // range visualizer 3D object
    private CapsuleCollider capsuleCollider;

    private List<GameObject> enemiesInRange = new List<GameObject>();
    private List<LineRenderer> lineRenderers = new List<LineRenderer>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();

        if (capsuleCollider == null)
        {
            Debug.LogError("No CapsuleCollider found on this GameObject!");
            return;
        }

        if (rangeVisual != null)
        {
            UpdateVisualSize();
        }
        else
        {
            Debug.LogError("Range visual object is not assigned!");
        }

        InitializeLineRenderers();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateLines();
    }

#region Range visualizer
    private void UpdateVisualSize()
    {
        float radius = capsuleCollider.radius;
        float height = capsuleCollider.height;

        rangeVisual.transform.localScale = new Vector3(radius * 2, height * 2, radius * 2);

        rangeVisual.transform.localPosition = capsuleCollider.center;
    }

    public void RefreshVisualSize()
    {
        if (capsuleCollider != null && rangeVisual != null)
        {
            UpdateVisualSize();
        }
    }
    #endregion

#region Range detection
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Enemy")
        {
            if (!enemiesInRange.Contains(collider.gameObject))
            {
                enemiesInRange.Add(collider.gameObject);
                Debug.Log("Enemy entered the range!");

                if (printMessageCoroutine == null)
                {
                    printMessageCoroutine = StartCoroutine(PrintMessage());
                }
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            if (enemiesInRange.Contains(collider.gameObject))
            {
                enemiesInRange.Remove(collider.gameObject);
                Debug.Log("Enemy exited the range!");

                if (enemiesInRange.Count == 0 && printMessageCoroutine != null)
                {
                    StopCoroutine(printMessageCoroutine);
                    printMessageCoroutine = null;
                }
            }
        }
    }

    private IEnumerator PrintMessage()
    {
        while (enemiesInRange.Count > 0)
        {
            Debug.Log($"Enemies in range: {enemiesInRange.Count}");
            yield return new WaitForSeconds(1f);
        }
    }
    #endregion

#region Enemy attacked visualizer
    private void InitializeLineRenderers()
    {
        for (int i = 0; i < 1; i++) // Create a pool of 10 LineRenderers
        {
            GameObject lineObject = new GameObject("LineRenderer_" + i);
            lineObject.transform.SetParent(transform);

            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false; 

            lineRenderers.Add(lineRenderer);
        }
    }

    private void UpdateLines()
    {
        for (int i = 0; i < lineRenderers.Count; i++)
        {
            if (i < enemiesInRange.Count)
            {
                lineRenderers[i].enabled = true;
                lineRenderers[i].SetPosition(0, transform.position); // Tower position
                lineRenderers[i].SetPosition(1, enemiesInRange[i].transform.position); // Enemy position
            }
            else
            {
                lineRenderers[i].enabled = false;
            }
        }
    }
#endregion
}
