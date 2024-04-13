using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Type0 = 0, 
    Type1, 
    Type2, 
    Type3, 
    Type4
}


public class ResourceComponent : MonoBehaviour, IInteractable
{
    public ResourceType type_;


    public void Interact()
    {
        if (GameManager.Instance.inventoryManager_.AddResource(type_))
        {   
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("full inventory message");
        }
        // else, show full inventory message?

    }
}
