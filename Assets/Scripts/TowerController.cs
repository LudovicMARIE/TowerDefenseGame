using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    private List<GameObject> enemiesInRange = new List<GameObject>();
    private Coroutine printMessageCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Enemy")
        {
            if (!enemiesInRange.Contains(collider.gameObject))
            {
                enemiesInRange.Add(collider.gameObject);
                Debug.Log("Enemy entered the range!");

                // Start printing messages if this is the first enemy
                if (printMessageCoroutine == null)
                {
                    printMessageCoroutine = StartCoroutine(PrintMessage());
                }
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        // Check if the object has the "Enemy" tag
        if (collider.CompareTag("Enemy"))
        {
            // Remove the enemy from the list
            if (enemiesInRange.Contains(collider.gameObject))
            {
                enemiesInRange.Remove(collider.gameObject);
                Debug.Log("Enemy exited the range!");

                // Stop the coroutine if no enemies remain
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
            yield return new WaitForSeconds(1f); // Wait for 1 second
        }
    }
}
