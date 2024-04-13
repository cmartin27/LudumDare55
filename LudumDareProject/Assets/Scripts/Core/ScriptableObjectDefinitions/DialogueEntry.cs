using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dialogEntry", menuName = "Scriptable Object/Dialog Entries", order = 1)]
public class DialogueEntry : ScriptableObject
{
    [TextArea(3, 6)] public string questNotInitialized;
    [TextArea(3, 6)] public string questInProgress;
    [TextArea(3, 6)] public string questAfterSummoning;
    [TextArea(3, 6)] public string questFinished;
}
