using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickTowerSlot : MonoBehaviour
{

    private Camera mainCamera;
    public GameObject towerToCreate;

    public GameObject towerMenuPrefab;
    private GameObject instantiatedMenu;

    public GameObject currentSlot;
    private Button buyButton;
    private PlayerController playerController;

    private void Start()
    {
        mainCamera = Camera.main;

        playerController = FindAnyObjectByType<PlayerController>();

        
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
                    ShowTowerSelectionMenu();
                    break; 
                }
            }
        }
    }

    public void ShowTowerSelectionMenu()
    {
        GameObject[] menusToDelete = GameObject.FindGameObjectsWithTag("TowerMenu");
        foreach (GameObject menu in menusToDelete)
        {
            if (menu != null)
            {
                Destroy(menu);
            }
        }
        instantiatedMenu = Instantiate(towerMenuPrefab);

        currentSlot = this.gameObject;

        buyButton = instantiatedMenu.GetComponentInChildren<Button>();

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();

            buyButton.onClick.AddListener(() => OnMenuButtonClicked());
        }

    }

    private void OnMenuButtonClicked()
    {
        Debug.Log($"Menu button clicked for TowerSlot: {gameObject.name}");
        CreateTower(); 
        Destroy(instantiatedMenu);
    }


    public void CreateTower()
    {
        if (towerToCreate != null && currentSlot != null && playerController.gold >= 10)
        {
            playerController.RemoveGold(10);
            Instantiate(towerToCreate, currentSlot.transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
            
    }
}
