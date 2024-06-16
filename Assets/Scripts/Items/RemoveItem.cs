using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RemoveItem : MonoBehaviour
{

public Item item;

public Button removeButton;


public void RemoveItems()
{

InventoryManager.instance.Remove(item);

Destroy(gameObject);

}

public void AddItem(Item newItem)
{
    item = newItem;
}




}
