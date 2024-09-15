using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable Object/Item")]
public class Item : ScriptableObject
{
    [Header("Only Gameplay")] public ItemType type;

    [Header("Only UI")] public bool stackableItem = false;

    [Header("Both")] public Sprite sprite;
    
    
    public enum ItemType
    {
        Default,
        Helmet,
        Chestplate,
        Leggings,
        Boots
    }
}
