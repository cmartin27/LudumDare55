using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummoningManager : MonoBehaviour
{

    [Header("Summoning Menu")]
    public GameObject summoningMenu_;
    public Button selectIngredientsButton_;
    public GameObject confirmSummoningMenu_;

    [Header("Resources Selection")]
    public GameObject selectResourcesMenu_;
    public GameObject resourcesList_;
    public GameObject selectableResourcePrefab_;
    public GameObject selectedResourcesList_;
    public GameObject selectedResourcePrefab_;


    [Header("Confirmation Menu")]
    public GameObject confirmResourcesList_;


    List<ResourceType> selectedResources_;


    private void Start()
    {
        selectedResources_ = new List<ResourceType>();
    }


    public void EnableSummoningMenu()
    {
        summoningMenu_.SetActive(true);
        ShowInventory();
    }

    public void DisableSummoningMenu()
    { 
        summoningMenu_.SetActive(false);
        GameManager.Instance.SetInputMode(EInputMode.InGame);
    }

    public void EnableResourceSelection()
    {
        selectResourcesMenu_.SetActive(true);
        confirmSummoningMenu_.SetActive(false);
        UpdateResourcesSelected(confirmResourcesList_);
    }

    public void EnableConfirmSummoning()
    {
        selectResourcesMenu_.SetActive(false);
        confirmSummoningMenu_.SetActive(true);
        UpdateResourcesSelected(confirmResourcesList_);
    }

    void ShowInventory()
    {
        var resourcesList = GameManager.Instance.inventoryManager_.resources;
        List<UISelectableResource> resourcesUIList = new List<UISelectableResource>();

        for (int i = 0; i < resourcesList_.transform.childCount; ++i)
        {
            Destroy(resourcesList_.transform.GetChild(i).gameObject);
        }

        foreach (var resource in resourcesList) 
        {
            GameObject newResource = GameObject.Instantiate(selectableResourcePrefab_);
            UISelectableResource resourceUI = newResource.GetComponent<UISelectableResource>();
            resourceUI.resourceSelected_.AddListener(SelectResource);
            resourceUI.resourceUnselected_.AddListener(UnselectResource);
            resourceUI.text_.text = "Resource " + resource.ToString();
            resourceUI.resourceType_ = resource;
            resourceUI.transform.SetParent(resourcesList_.transform);
            resourcesUIList.Add(resourceUI); 
        }

        // Set buttons navigation
        int resourcesCount = resourcesUIList.Count;
        for (int i = 0; i < resourcesCount; ++i)
        {
            var navigation = resourcesUIList[i].button_.navigation;

            navigation.selectOnLeft = selectIngredientsButton_;

            int upButtonIdx = (i + resourcesCount - 1) % resourcesCount;
            navigation.selectOnUp = resourcesUIList[upButtonIdx].button_;

            int downButtonIdx = (i + 1) % resourcesCount;
            navigation.selectOnDown = resourcesUIList[downButtonIdx].button_;

            resourcesUIList[i].button_.navigation = navigation;
        }

    }

    void SelectResource(ResourceType resourceType)
    {
        selectedResources_.Add(resourceType);
        UpdateResourcesSelected(selectedResourcesList_);
        UpdateResourcesSelected(selectedResourcesList_);

    }

    void UnselectResource(ResourceType resourceType)
    {
        selectedResources_.Remove(resourceType);
        UpdateResourcesSelected(selectedResourcesList_);
    }

    void UpdateResourcesSelected(GameObject selectedResourcesList)
    {
        int currentChildren = selectedResourcesList.transform.childCount;
        if(currentChildren < selectedResources_.Count)
        {
            int neededResources = selectedResources_.Count - currentChildren;
            for (int i = 0; i < neededResources; ++i)
            {
                var newResource = GameObject.Instantiate(selectedResourcePrefab_);
                newResource.transform.SetParent(selectedResourcesList.transform, false);
            }

        }else if(currentChildren > selectedResources_.Count)
        {
            int removeResources = currentChildren - selectedResources_.Count;
            for (int i = 0; i < removeResources; ++i)
            {
                Destroy(selectedResourcesList.transform.GetChild(i).gameObject);
            }
        }

        for(int i = 0; i < selectedResources_.Count; ++i)
        {
            selectedResourcesList.transform.GetChild(i).GetComponent<TMP_Text>().text = "Resource " + selectedResources_[i].ToString();
        }
    }


    public void MakeSummoning()
    {
        Debug.Log("Start Summoning Animation");
        GameManager.Instance.SetInputMode(EInputMode.Summoning);

    }

}
