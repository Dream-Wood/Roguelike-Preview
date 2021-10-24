using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "ScriptableObjects/ItemsGroups", order = 1)]
public class ItemsGroups : ScriptableObject
{
    public List<ItemDrop> boxDrops;
    public List<ItemDrop> slimeDrops;
}