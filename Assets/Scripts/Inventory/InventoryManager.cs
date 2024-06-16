using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager instance;
    public List<Item> Items = new List<Item>();

    public Transform itemContent;
    public GameObject inventoryItem;

    private void Awake() {
       instance = this; 
    }


    public void Add(Item item)
    {
        Items.Add(item);
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
    }

    public void ListItems()
    {
        foreach (Transform item in itemContent)
    {
        Destroy(item.gameObject);
    }

        foreach (var item in Items)
        {
            GameObject obj = Instantiate(inventoryItem,itemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
            var itemIcon = obj.transform.Find("Image").GetComponent<Image> ();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
       
        }

    }


}
