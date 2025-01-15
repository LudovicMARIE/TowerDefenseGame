using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickTowerSlot : MonoBehaviour
{

    private Camera mainCamera;
    public GameObject towerToCreate;

    public GameObject towerSelectionMenu;
    public GameObject currentSlot; // Le slot sur lequel l'utilisateur a cliqué

    public Button buyButton;
    // public Text moneyText; // Affichage de l'argent disponible (optionnel, selon ton système de ressources)
    private int playerMoney = 100; // Exemple d'argent du joueur

    private void Start()
    {
        mainCamera = Camera.main;
        towerSelectionMenu.SetActive(false);

        if (buyButton != null)
        {
            buyButton.onClick.AddListener(OnBuyButtonClick);
        }
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
                    ShowTowerSelectionMenu();
                    break; 
                }
            }
        }
    }

    public void ShowTowerSelectionMenu()
    {
        towerSelectionMenu.SetActive(true);
        
        // Tu peux ici définir si tu veux placer la tour directement ou afficher une prévisualisation
        currentSlot = this.gameObject; // On garde une référence au slot sur lequel l'utilisateur a cliqué

        // // Met à jour l'affichage de l'argent (si nécessaire)
        // if (moneyText != null)
        // {
        //     moneyText.text = "Money: " + playerMoney.ToString(); // Mise à jour du texte d'argent
        // }
    }

    public void OnBuyButtonClick()
    {
        // Vérifier si le joueur a assez d'argent pour acheter la tour
        // int towerCost = 50; // Exemple de coût de la tour

        // if (playerMoney >= towerCost)
        // {
        //     // Déduire le coût de la tour
        //     playerMoney -= towerCost;
        Debug.Log($"{towerToCreate.name} yesss.");
        Debug.Log($"{this.gameObject.name} yesss again.");



        // Créer la tour sur le slot sélectionné
        CreateTower();

        // Fermer le menu de sélection après l'achat
        towerSelectionMenu.SetActive(false);
        Debug.Log($"{towerToCreate.name} has been placed.");
        // }
        // else
        // {
        //     Debug.Log("Not enough money!");
        //     // Ajoute une alerte si le joueur n'a pas assez d'argent
        // }

        // // Met à jour l'affichage de l'argent
        // if (moneyText != null)
        // {
        //     moneyText.text = "Money: " + playerMoney.ToString();
        // }
    }

    public void CreateTower()
    {
        if (towerToCreate != null && currentSlot != null)
        {
            Instantiate(towerToCreate, currentSlot.transform.position, Quaternion.identity);
            Debug.Log($"{towerToCreate.name} placed at {currentSlot.transform.position}");
        }
        // Instantiate(towerToCreate, gameObject.transform.position, Quaternion.identity);
    }
}
