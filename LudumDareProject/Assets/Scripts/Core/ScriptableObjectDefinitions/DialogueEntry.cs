using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dialogEntry", menuName = "Scriptable Object/Dialogue Entries", order = 1)]
public class DialogueEntry : ScriptableObject
{
    [TextArea(3, 6)] public string questNotInitialized_;
    [TextArea(3, 6)] public string questInProgress_;
    [TextArea(3, 6)] public string questAfterSummoning_;
    [TextArea(3, 6)] public string questFinished_;
}
