using UnityEngine;
using UnityEngine.UI;

public class TowerMenuController : MonoBehaviour
{
    public Button BuyButton;
    public PlayerController PlayerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BuyButton = gameObject.GetComponentInChildren<Button>();
        PlayerController = FindAnyObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (BuyButton != null && PlayerController != null)
        {
            if (PlayerController.gold < 10)
            {
                BuyButton.gameObject.SetActive(false);
            }
            else
            {
                BuyButton.gameObject.SetActive(true);
            }
        }
    }
}
