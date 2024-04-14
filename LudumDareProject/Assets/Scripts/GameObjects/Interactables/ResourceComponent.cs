using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class ResourceComponent : MonoBehaviour, IInteractable
{
    public EResourceType type_;

    public void Interact()
    {
        if (GameManager.Instance.inventoryManager_.AddResource(type_))
        {
            GameManager.Instance.resourceManager_.PickResource(this);
        }
        else
        {
            Debug.Log("full inventory message");
        }

    }
}
