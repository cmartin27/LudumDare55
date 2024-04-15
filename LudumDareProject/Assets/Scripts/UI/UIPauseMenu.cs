using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPauseMenu : MonoBehaviour
{
    public GameObject pauseMenu_;

    public UIInventoryMenu inventoryMenu_;
    public UICookbookMenu cookbookMenu_;

    public Button cookbookButton_;
    public Button inventoryButton_;


    public void Start()
    {
        // Create cookbook menu
        cookbookMenu_.CreateMenu(cookbookButton_);

    }

    public void EnablePauseMenu()
    {
        // Change player input
        GameManager.Instance.SetInputMode(EInputMode.UI);

        // Create inventory menu
        inventoryMenu_.CreateMenu(inventoryButton_);

        // Select cookbook
        cookbookButton_.Select();

        // Enable Pause Menu
        pauseMenu_.SetActive(true);

    }

    public void OpenCookbookMenu()
    {

        // Focus cookbook canvas
        cookbookMenu_.gameObject.SetActive(true);
        inventoryMenu_.gameObject.SetActive(false);

        // Initialize cookbook menu
        cookbookMenu_.OpenMenu();
    }


    public void OpenInventoryMenu()
    {
        // Focus inventory canvas
        cookbookMenu_.gameObject.SetActive(false);
        inventoryMenu_.gameObject.SetActive(true);

        // Initialize inventory menu
        inventoryMenu_.OpenMenu();
    }



    public void DisablePauseMenu()
    {
        // Change player input
        GameManager.Instance.SetInputMode(EInputMode.InGame);

        // Disable Pause Menu
        pauseMenu_.SetActive(false);

    }
}
