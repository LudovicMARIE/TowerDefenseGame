using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickTowerSlot : MonoBehaviour
{

    private Camera mainCamera;
    public GameObject mageTower;
    public GameObject sniperTower;
    public GameObject towerToCreate;

    public GameObject towerMenuPrefab;
    private GameObject instantiatedMenu;

    public Button[] buttons;

    public GameObject currentSlot;
    private Button buyButton;
    private Button mageTowerButton;
    private Button sniperTowerButton;
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

        buttons = instantiatedMenu.GetComponentsInChildren<Button>();

        buyButton = buttons[0];
        mageTowerButton = buttons[1];
        sniperTowerButton = buttons[2];

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() => OnBuyButtonClicked());
        }


        if (mageTowerButton != null)
        {
            mageTowerButton.onClick.RemoveAllListeners();
            mageTowerButton.onClick.AddListener(() => chooseMageTower());
        }

        if (sniperTowerButton != null)
        {
            sniperTowerButton.onClick.RemoveAllListeners();
            sniperTowerButton.onClick.AddListener(() => chooseSniperTower());
        }

    }

    private void OnBuyButtonClicked()
    {
        Debug.Log($"Menu button clicked for TowerSlot: {gameObject.name}");
        FindAnyObjectByType<AudioManager>().PlaySound("CoinUsed");
        CreateTower(); 
        Destroy(instantiatedMenu);
    }

    private void chooseSniperTower()
    {
        towerToCreate = sniperTower;
        FindAnyObjectByType<AudioManager>().PlaySound("Button1");
    }

    private void chooseMageTower()
    {
        towerToCreate = mageTower;
        FindAnyObjectByType<AudioManager>().PlaySound("Button1");
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
