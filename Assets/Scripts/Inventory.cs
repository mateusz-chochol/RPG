using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public List<GameObject> startingItems;
    [HideInInspector]
    public List<GameObject> items = new List<GameObject>();

    private void Start() {
        items = startingItems;
    }    

    public void AddNewItem(GameObject item) {
        items.Add(item);
    }

    public void AddNewItems(List<GameObject> items) {
        this.items.AddRange(items);
    }

    public void RemoveItem(int itemIndex) {
        items.RemoveAt(itemIndex);
    }
}
