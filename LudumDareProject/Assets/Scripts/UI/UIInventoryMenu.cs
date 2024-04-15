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


    public void CreateMenu(Button inventoryButton)
    {
        // Clean previous list
        foreach (Transform child in resourcesList_.transform)
        {
            Destroy(child.gameObject);
        }

        List<UISelectableResource> spawnedResources = new List<UISelectableResource>();
        var resourcesList = GameManager.Instance.inventoryManager_.resources;
        
        if (resourcesList.Count < 1) return;


        // Creates inventory elements 

        foreach (var resource in resourcesList)
        {
            GameObject newResource = GameObject.Instantiate(resourcePrefab_);
            UISelectableResource resourceUI = newResource.GetComponent<UISelectableResource>();
            resourceUI.resourceSelected_.AddListener(SelectResource);
            resourceUI.EResourceType_ = resource;
            resourceUI.transform.SetParent(resourcesList_.transform);
            spawnedResources.Add(resourceUI);
        }


        // Set buttons navigation
        int resourcesCount = spawnedResources.Count;
        for (int i = 0; i < resourcesCount - 1; ++i)
        {
            var navigationRight = spawnedResources[i].button_.navigation;
            navigationRight.selectOnRight = spawnedResources[i + 1].button_;
            spawnedResources[i].button_.navigation = navigationRight;

            var navigationLeft = spawnedResources[i + 1].button_.navigation;
            navigationLeft.selectOnLeft = spawnedResources[i].button_;
            spawnedResources[i + 1].button_.navigation = navigationLeft;
        }

        var navigation = spawnedResources[0].button_.navigation;
        navigation.selectOnLeft = inventoryButton;
        spawnedResources[0].button_.navigation = navigation;

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
        foreach (Transform child in resourcesList_.transform)
        {
            UISelectableResource resourceUI = child.GetComponent<UISelectableResource>();
            if(resourceUI.EResourceType_ == selectedResource_)
            {
                Destroy(resourceUI.gameObject);
                break;
            }
        }

        confirmationDialog_.SetActive(false);
        OpenMenu();

    }

    public void CancelFlush()
    {
        confirmationDialog_.SetActive(false);
        OpenMenu();
    }




    public void OpenMenu()
    {
        if(resourcesList_.transform.childCount < 1) return;

        Transform firstResource = resourcesList_.transform.GetChild(0);
        if(firstResource != null ) {
            //Button button = firstResource.GetComponent<Button>();
            EventSystem.current.SetSelectedGameObject(firstResource.gameObject);
        }

    }



}
