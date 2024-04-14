using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class InventoryManager : MonoBehaviour
{
    public int maxSlots;
    public List<ResourceType> resources;

    public void Start()
    {
        resources = new List<ResourceType>(maxSlots);
    }

    public bool AddResource(ResourceType resource)
    {
        if(resources.Count < maxSlots)
        {
            resources.Add(resource);
            return true;
        }

        return false;
    }

    public bool UseResource(ResourceType resource) 
    {
        return resources.Remove(resource);
    }


}
