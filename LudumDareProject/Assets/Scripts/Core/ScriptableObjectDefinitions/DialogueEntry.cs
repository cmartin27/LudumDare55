using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dialogEntry", menuName = "Scriptable Object/Dialog Entries", order = 1)]
public class DialogueEntry : ScriptableObject
{
    public string questNotInitialized;
    public string questInProgress;
    public string questAfterSummoning;
    public string questFinished;
}
