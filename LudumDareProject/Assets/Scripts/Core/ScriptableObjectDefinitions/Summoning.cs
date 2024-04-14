using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "summoningInfo", menuName = "Scriptable Object/Summoning Info", order = 2)]
public class SummoningInfo : ScriptableObject
{
    public string name_;
    [TextArea(3, 6)] public string description_;
    public List<ResourceType> resources_;
    public List<uint> amount_;
    public GameObject summoning_;
}
