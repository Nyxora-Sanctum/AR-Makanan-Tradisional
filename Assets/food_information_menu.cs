using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FoodSelectionUI : MonoBehaviour
{
    [Header("Navigation")]
    [SerializeField] private GameObject backButtonTarget;
    [SerializeField] private Button backButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button startGameButton;
    private int currentFoodIndex = 0;
    [Header("Food Display")]
    [SerializeField] private TMP_Text foodTitleText;
    [SerializeField] private Image foodImage;
    [SerializeField] private TMP_Text foodInfoText;

    [Header("Menu Dropdown")]
    [SerializeField] private GameObject dropdownPanel;
    [SerializeField] private List<Button> foodButtons; // Assign all your existing food buttons here

    [Header("Food Data")]
    [SerializeField] private List<FoodData> foodItems = new List<FoodData>();

    private void Awake()
    {
        // Initialize navigation buttons
        backButton.onClick.AddListener(OnBackButtonPressed);
        menuButton.onClick.AddListener(ToggleDropdown);
        startGameButton.onClick.AddListener(OnStartGamePressed);

        // Initialize food buttons
        SetupFoodButtons();

        // Close dropdown initially
        dropdownPanel.SetActive(false);

        // Select first food by default if there are any
        if (foodItems.Count > 0 && foodButtons.Count > 0)
        {
            SelectFood(foodItems[0]);
        }
    }

    private void SetupFoodButtons()
    {
        // Make sure we have enough buttons for all food items
        if (foodButtons.Count < foodItems.Count)
        {
            Debug.LogWarning("Not enough buttons for all food items!");
        }

        // Setup each button with corresponding food data
        for (int i = 0; i < Mathf.Min(foodButtons.Count, foodItems.Count); i++)
        {
            int index = i; // Important for closure
            foodButtons[i].onClick.AddListener(() => SelectFood(foodItems[index]));

            // You can also set button text or icon here if needed
            if (foodButtons[i].GetComponentInChildren<TMP_Text>() != null)
            {
                foodButtons[i].GetComponentInChildren<TMP_Text>().text = foodItems[i].foodName;
            }
        }
    }

    public void SelectNextFood()
    {
        if (foodItems.Count == 0) return;

        currentFoodIndex = (currentFoodIndex + 1) % foodItems.Count;
        SelectFood(foodItems[currentFoodIndex]);
    }

    public void SelectPreviousFood()
    {
        if (foodItems.Count == 0) return;

        currentFoodIndex--;
        if (currentFoodIndex < 0) currentFoodIndex = foodItems.Count - 1;
        SelectFood(foodItems[currentFoodIndex]);
    }

    public void SelectFood(FoodData food)
    {
        // Update current index
        currentFoodIndex = foodItems.IndexOf(food);

        // Update UI with selected food data
        foodTitleText.text = food.foodName;
        foodImage.sprite = food.foodSprite;
        foodInfoText.text = food.foodDescription;

        // Close dropdown
        dropdownPanel.SetActive(false);
    }

    private void OnBackButtonPressed()
    {
        if (backButtonTarget != null)
        {
            backButtonTarget.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    private void ToggleDropdown()
    {
        dropdownPanel.SetActive(!dropdownPanel.activeSelf);
    }

    private void OnStartGamePressed()
    {
        Debug.Log("Starting game with selected food: " + foodTitleText.text);
        // Add your game start logic here
    }
}

[System.Serializable]
public class FoodData
{
    public string foodName;
    [TextArea] public string foodDescription;
    public Sprite foodSprite;
}