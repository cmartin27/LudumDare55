using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueManager))]
[RequireComponent(typeof(AudioManager))]
[RequireComponent(typeof(InventoryManager))]
[RequireComponent(typeof(UIManager))]
public class GameManager : MonoBehaviour
{
    private static GameManager instance_ = null;
    public static GameManager Instance
    {
        get { return instance_; }
    }

    private void Awake()
    {
        instance_ = this;
    }
}
