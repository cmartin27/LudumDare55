using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// TODO: Create menu interface
public class UIInventoryMenu : MonoBehaviour
{
    public GameObject resourcePrefab_;
    public GameObject resourcesList_;
    public GameObject confirmationDialog_;
    public GameObject flushButton_;

    EResourceType selectedResource_;
    Button inventoryButton_;


    public void CreateMenu(Button inventoryButton)
    {
        inventoryButton_ = inventoryButton;

        // Clean previous list
        foreach (Transform child in resourcesList_.transform)
        {
            Destroy(child.gameObject);
        }

        List<Button> spawnedResources = new List<Button>();
        var resourcesList = GameManager.Instance.inventoryManager_.resources;

        // Creates inventory elements 
        foreach (var resource in resourcesList)
        {
            GameObject newResource = GameObject.Instantiate(resourcePrefab_);
            UISelectableResource resourceUI = newResource.GetComponent<UISelectableResource>();
            resourceUI.resourceSelected_.AddListener(SelectResource);
            resourceUI.sprite_.sprite = GameManager.Instance.resourceManager_.GetResourceSprite(resource);
            resourceUI.EResourceType_ = resource;
            resourceUI.transform.SetParent(resourcesList_.transform);
            spawnedResources.Add(newResource.GetComponent<Button>());
        }
        SetButtonNavigation(spawnedResources);
    }

    public void SetButtonNavigation(List<Button> buttonsList)
    {

        int resourcesCount = buttonsList.Count;
        if (resourcesCount < 1) return;

        // Set buttons navigation
        for (int i = 0; i < resourcesCount - 1; ++i)
        {
            Button currentResource = buttonsList[i];
            Button nextResource = buttonsList[i + 1];


            var navigationRight = currentResource.navigation;
            navigationRight.selectOnRight = nextResource;
            currentResource.navigation = navigationRight;

            var navigationLeft = nextResource.navigation;
            navigationLeft.selectOnLeft = currentResource;
            nextResource.navigation = navigationLeft;
        }

        var firstButton = buttonsList[0];
        var firstButtonNavigation = firstButton.navigation;
        firstButtonNavigation.selectOnLeft = inventoryButton_;
        firstButton.navigation = firstButtonNavigation;

    }


    public void SelectResource(EResourceType EResourceType)
    {
        confirmationDialog_.SetActive(true);
        EventSystem.current.SetSelectedGameObject(flushButton_.gameObject);
        selectedResource_ = EResourceType;
    }

    public void RemoveResource()    
    {
        GameManager.Instance.inventoryManager_.UseResource(selectedResource_);
        List<Button> newResourcesList = new List<Button>();
        bool resourceRemoved = false;
        for(int i = 0; i < resourcesList_.transform.childCount; ++i) 
        {
            Transform child = resourcesList_.transform.GetChild(i);
            UISelectableResource resourceUI = child.GetComponent<UISelectableResource>();
            if(resourceUI.EResourceType_ == selectedResource_ && !resourceRemoved)
            {
                resourceRemoved = true;
                Destroy(resourceUI.gameObject);
            }else 
            {
                Button resourceButton = child.GetComponent<Button>();
                newResourcesList.Add(resourceButton);
            }
        }

        SetButtonNavigation(newResourcesList);
        confirmationDialog_.SetActive(false);
        newResourcesList[0].Select();

    }

    public void CancelFlush()
    {
        confirmationDialog_.SetActive(false);
        OpenMenu();
    }




    public void OpenMenu()
    {
        if(resourcesList_.transform.childCount < 1) return;

        resourcesList_.transform.GetChild(0).GetComponent<Button>().Select();
        //EventSystem.current.SetSelectedGameObject(firstResource);
    }



}
