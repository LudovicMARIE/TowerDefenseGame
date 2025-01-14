using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickTowerSlot : MonoBehaviour
{

    private Camera mainCamera;
    public GameObject towerToCreate;

    private void Start()
    {
        mainCamera = Camera.main; 
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log($"{gameObject.name} was clicked!");
                    CreateTower(); 
                    break; 
                }
            }
        }
    }

    private void CreateTower()
    {
        Instantiate(towerToCreate, gameObject.transform.position, Quaternion.identity);
    }
}
