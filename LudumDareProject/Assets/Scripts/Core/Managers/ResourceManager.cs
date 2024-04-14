using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ResourceType
{
    Mushroom = 0,
    WaterBucket,
    RatTail,
    Bone,
    Wood,
    Stone,
    Carrot,
    Size
}

public class ResourceManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> resourceParents_;
    [SerializeField]
    private int minAvailableResources_;

    private Dictionary<ResourceType, List<ResourceComponent>> resources_;

    public void PickResource(ResourceComponent resource)
    {
        resource.gameObject.SetActive(false);
    }

    public void RestoreResource(ResourceComponent resource)
    {
        resource.gameObject.SetActive(true);
    }

    private void Start()
    {
        resources_ = new Dictionary<ResourceType, List<ResourceComponent>>();
        foreach(GameObject parent in resourceParents_)
        {
            List<ResourceComponent> children = parent.GetComponentsInChildren<ResourceComponent>(includeInactive: true).ToList();
            resources_[children[0].type_] = children;
        }

        ReplenishResources();
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.R)) {
            ReplenishResources();
        }
    }

    private void ReplenishResources()
    {
        foreach(List<ResourceComponent> resources in resources_.Values) 
        {
            List<ResourceComponent> hiddenResources = resources.FindAll(x => x.isActiveAndEnabled == false);
            int availableResources = resources.Count - hiddenResources.Count;
            while(availableResources < minAvailableResources_)
            {
                int idx = Random.Range(0, hiddenResources.Count);
                RestoreResource(hiddenResources[idx]);
                hiddenResources.RemoveAt(idx);
                ++availableResources;
            }
        }
    }
}
