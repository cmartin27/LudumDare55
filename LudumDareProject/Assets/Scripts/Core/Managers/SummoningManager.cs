using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ESummoningResult
{
    Succesful,  // the summoning was expected for the quest
    Failed,     // the summoning was not expecte for the quest
    Invalid     // there exist no summoning
}

public class SummoningManager : MonoBehaviour
{
    [Header("Summoning Info")]
    public List<SummoningInfo> summonings_;

    [Header("Summoning Menu")]
    public GameObject summoningMenu_;
    public Button selectResourcesButton_;
    public GameObject confirmSummoningMenu_;

    [Header("Resources Selection")]
    public GameObject selectResourcesMenu_;
    public GameObject resourcesList_;
    public GameObject selectableResourcePrefab_;
    public GameObject selectedResourcesList_;
    public GameObject selectedResourcePrefab_;

    [Header("Confirmation Menu")]
    public GameObject confirmResourcesList_;

    [Header("Animation")]
    public SummoningAnimationComponent summoningAnimationComp_;


    List<EResourceType> selectedResources_;
    QuestInfo currentQuest_;
    GameObject summonedObject_;
    ESummoningResult currentQuestResult_;
    int currentResources_;

    private void Start()
    {
        selectedResources_ = new List<EResourceType>();
        summoningAnimationComp_.animationEnded_.AddListener(OnSummoningAnimationEnded);
    }


    public void EnableSummoningMenu(QuestInfo quest)
    {
        currentQuest_ = quest;
        summoningMenu_.SetActive(true);
        selectResourcesButton_.Select();
        ShowInventory();
    }

    public void DissableSummoningMenu()
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
            resourceUI.EResourceType_ = resource;
            resourceUI.transform.SetParent(resourcesList_.transform);
            resourcesUIList.Add(resourceUI); 
        }

        // Set buttons navigation
        int resourcesCount = resourcesUIList.Count;
        for (int i = 0; i < resourcesCount; ++i)
        {
            var navigation = resourcesUIList[i].button_.navigation;

            navigation.selectOnLeft = selectResourcesButton_;

            int upButtonIdx = (i + resourcesCount - 1) % resourcesCount;
            navigation.selectOnUp = resourcesUIList[upButtonIdx].button_;

            int downButtonIdx = (i + 1) % resourcesCount;
            navigation.selectOnDown = resourcesUIList[downButtonIdx].button_;

            resourcesUIList[i].button_.navigation = navigation;
        }

    }

    void SelectResource(EResourceType EResourceType)
    {
        selectedResources_.Add(EResourceType);
        UpdateResourcesSelected(selectedResourcesList_);
        UpdateResourcesSelected(confirmResourcesList_);

    }

    void UnselectResource(EResourceType EResourceType)
    {
        selectedResources_.Remove(EResourceType);
        UpdateResourcesSelected(selectedResourcesList_);
        UpdateResourcesSelected(confirmResourcesList_);
    }

    void UpdateResourcesSelected(GameObject selectedResourcesList)
    {
        int currentChildren = selectedResourcesList.transform.childCount;
        int currentSelectedResources = selectedResources_.Count;

        if (currentChildren < currentSelectedResources)
        {
            int neededResources = selectedResources_.Count - currentChildren;
            for (int i = 0; i < neededResources; ++i)
            {
                var newResource = GameObject.Instantiate(selectedResourcePrefab_);
                newResource.transform.SetParent(selectedResourcesList.transform, false);
            }

        }

        for(int i = 0; i < currentSelectedResources; ++i)
        {
            string resourceName = "Resource " + selectedResources_[i].ToString();
            selectedResourcesList.transform.GetChild(i).GetComponent<TMP_Text>().text = resourceName;
        }


        if (currentChildren > currentSelectedResources)
        {
            for (int i = currentSelectedResources; i < currentChildren; ++i)
            {
                Destroy(selectedResourcesList.transform.GetChild(i).gameObject);
            }
        }

    }


    public void MakeSummoning()
    {
        // check correct summoning ingredients
        currentQuestResult_ = CheckSummoningInfo();

        if (currentQuestResult_ != ESummoningResult.Invalid)
        {
            summoningMenu_.SetActive(false);
            summoningAnimationComp_.StartSummoningAnimation(currentQuest_.summoningPosition_);
            GameManager.Instance.inventoryManager_.Clear();
        }
        else {
            // TODO: Print message saying that there is no summoning with those ingredients
        }
    }

    void OnSummoningAnimationEnded() 
    {
        Clear();

        if (currentQuestResult_ == ESummoningResult.Succesful)
        {
            GameManager.Instance.questManager_.QuestSuccessful();
        }
        else
        {
            GameManager.Instance.questManager_.QuestFailed();
        }
    }

    public void AddResource()
    {

        if (currentResources_ < selectedResources_.Count)
        {
            summoningAnimationComp_.AddResourceAnimation(currentResources_);
            currentResources_++;
        }
        else
        {
            summoningAnimationComp_.GoBackAnimation(GetCurrentResourceId());
        }
    }

    private ESummoningResult CheckSummoningInfo()
    {
        // Create a dictionary with the resources gathered
        Dictionary<EResourceType, uint> resourceCount = new Dictionary<EResourceType, uint>();
        foreach(EResourceType resource in selectedResources_) 
        {
            if(resourceCount.ContainsKey(resource))
            {
                resourceCount[resource] += 1;
            }
            else
            {
                resourceCount[resource] = 1;
            }
        }

        SummoningInfo matchingSummonInfo = null;
        // for each summoning recipe
        // TODO: SUMMONING RECIPES SHOULD NOT BE OBTAINED FROM THE QUESTS, THEY SHOULD BE STORED SOMEWHERE ELSE
        for(int i = 0; i < summonings_.Count; ++i) 
        {
            SummoningInfo summoningInfo = summonings_[i];
            List<EResourceType> resourcesInSummoning = summoningInfo.resources_;
            List<uint> amountsInSummoning = summoningInfo.resourceAmounts_;
            bool matchFound = true;
            for (int j = 0; j < resourcesInSummoning.Count && matchFound; ++j)
            {
                EResourceType resource = resourcesInSummoning[j];
                uint amountInSummoning = amountsInSummoning[j];
                resourceCount.TryGetValue(resource, out uint currentAmount);

                if (amountInSummoning != currentAmount)
                {
                    matchFound = false;
                }
            }

            if(matchFound)
            {
                matchingSummonInfo = summoningInfo;
                break;
            }
        }
        summonedObject_ = matchingSummonInfo.summoning_;

        return matchingSummonInfo == null ? ESummoningResult.Invalid :
            matchingSummonInfo.summoning_ == currentQuest_.summoningInfo_.summoning_ ? ESummoningResult.Succesful : ESummoningResult.Failed;
    }

    public int GetCurrentResourceId()
    {
        return currentResources_ - 1;

    }


    public Sprite GetCurrentResourceSprite()
    {
        int resourceId = GetCurrentResourceId();
        return GameManager.Instance.resourceManager_.GetResourceSprite(selectedResources_[resourceId]);
    }


    private void Clear()
    {
        currentResources_ = 0;
        selectedResources_.Clear();
        currentQuest_ = null;
    }

    public GameObject GetSummonedObject()
    {
        return summonedObject_;
    }
}
