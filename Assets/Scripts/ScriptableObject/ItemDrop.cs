using UnityEngine;

[System.Serializable]
public class ItemDrop
{
    public GameObject item;
    [Range(0,100)]
    public float chance;
}