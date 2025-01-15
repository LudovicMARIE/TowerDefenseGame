using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    private Coroutine printMessageCoroutine;
    public GameObject rangeVisual; // range visualizer 3D object
    private CapsuleCollider capsuleCollider;

    

    public List<GameObject> enemiesInRange = new List<GameObject>();
    private List<LineRenderer> lineRenderers = new List<LineRenderer>();

    private GameObject newRange;
    public int towerDamage;
    public int towerLevel;

    public GameObject projectilePrefab;
    public float firingRate = 1000f; // Firing rate in milliseconds
    public Vector3 firePoint; // Point where the projectile will be instantiated
    public float fireCooldown; // Time tracking for firing cooldown

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();

        if (capsuleCollider == null)
        {
            Debug.LogError("No CapsuleCollider found on this GameObject!");
            return;
        }
        InitializeLineRenderers();
        CreateRange(gameObject);


        if (rangeVisual != null)
        {
            UpdateVisualSize();
        }
        else
        {
            Debug.LogError("Range visual object is not assigned!");
        }
        firePoint = firePoint + gameObject.transform.position;



    }

    // Update is called once per frame
    void Update()
    {
        UpdateLines();
        enemiesInRange = enemiesInRange
        .OrderByDescending(enemy => enemy.GetComponent<EnemyController>().timeAlive)
        .ToList();
        fireCooldown -= Time.deltaTime * 1000f;
        if (fireCooldown <= 0 && enemiesInRange.Count > 0)
        {
            FireAtEnemy(enemiesInRange[0]); // Fire at the first enemy in the list
            fireCooldown = firingRate; // Reset cooldown
        }
    }

#region Range visualizer

    private void CreateRange(GameObject cylinder)
    {
        // Ensure the cylinder has a CapsuleCollider
        CapsuleCollider collider = cylinder.GetComponent<CapsuleCollider>();
        if (collider == null)
        {
            Debug.LogError("Cylinder does not have a CapsuleCollider!");
            return;
        }

        // Spawn the range sphere at the same position as the cylinder
        GameObject rangeSphere = Instantiate(rangeVisual, cylinder.transform.position, Quaternion.identity);

        // Calculate the scale based on the formula: 3 * X scale * CapsuleCollider radius
        float calculatedScale = capsuleCollider.radius * 2 * 3;

        // Apply the calculated scale to the range sphere
        rangeSphere.transform.localScale = new Vector3(calculatedScale, calculatedScale, calculatedScale);

        // Optionally, parent the range sphere to the cylinder
        rangeSphere.transform.SetParent(cylinder.transform);

        Debug.Log($"Range sphere created with scale: {calculatedScale}");
    }


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

                EnemyController enemyController = collider.gameObject.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.AddTowerController(this); // Add this tower to the enemy's list
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

                EnemyController enemyController = collider.gameObject.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.RemoveTowerController(this); // Remove this tower from the enemy's list
                }
            }
        }
    }

    public void RemoveEnemyFromRange(GameObject enemy)
    {
        if (enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Remove(enemy);
            Debug.Log($"Enemy {enemy.name} removed from range.");
        }
    }
    #endregion

    #region Enemy attack
    private void InitializeLineRenderers()
    {
        for (int i = 0; i < 10; i++) // Create a pool of 10 LineRenderers
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
                if (enemiesInRange[i] != null) { 
                    lineRenderers[i].SetPosition(1, enemiesInRange[i].transform.position); // Enemy position
                }
            }
            else
            {
                lineRenderers[i].enabled = false;
            }
        }
    }


    private void FireAtEnemy(GameObject enemy)
    {
        if (enemy == null)
        {
            // Remove the enemy from the list if it no longer exists
            enemiesInRange.Remove(enemy);
            return;
        }

        // Create the projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint, Quaternion.identity);

        // Set the target for the projectile
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.target = enemy.transform;
            projectileScript.damage = towerDamage;
        }

        Debug.Log($"Fired at {enemy.name}");
    }
    #endregion
}
