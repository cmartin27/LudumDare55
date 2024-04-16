using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int maxSlots;
    public List<EResourceType> resources;

    public void Start()
    {
        resources = new List<EResourceType>(maxSlots);
    }

    public bool AddResource(EResourceType resource)
    {
        if(resources.Count < maxSlots)
        {
            resources.Add(resource);
            return true;
        }

        return false;
    }

    public bool UseResource(EResourceType resource) 
    {
        return resources.Remove(resource);
    }

    public void Clear()
    { 
        resources.Clear(); 
    }

    public int GetResourceAmount(EResourceType resource) {

        int i = 0;

       foreach(EResourceType type in resources) {
            if(type == resource) i++;
       }

       return i;
    }


}
