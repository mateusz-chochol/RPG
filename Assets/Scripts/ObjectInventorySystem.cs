using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectInventorySystem : MonoBehaviour {

    public static ObjectInventorySystem objectInventorySystem;
    public GameObject inventoryGraphics;
    [HideInInspector]
    public Inventory objectInventory = null;
    public bool isVisible = false;

    private GameObject itemStatisticsPanel;
    private Vector3 itemStatisticsPanelOffset = new Vector3(-80f, 90f, 0f);
    private int numberOfInventorySlots = 24;
    private int itemsCount = 0;
    private bool areStatisticsShowing = false;

    private void Awake() {
        if(objectInventorySystem != null && objectInventorySystem != this) {
            Destroy(gameObject);
        }
        else {
            objectInventorySystem = this;
        }
    }

    private void Start() {
        itemStatisticsPanel = inventoryGraphics.transform.Find("ItemStatisticsPanel").gameObject;
    }

    public void SetObjectInventory(Inventory objectInventory) {
        itemsCount = 0;
        this.objectInventory = objectInventory;
        ShowObjectInventory();
        LoadAllItems();
    }

    private void LoadAllItems() {
        for(int i = 0; i < objectInventory.items.Count; i++) {
            inventoryGraphics.transform.GetChild(0).GetChild(i).Find("ItemButton").GetChild(0).GetComponent<Image>().enabled = true;
            inventoryGraphics.transform.GetChild(0).GetChild(i).Find("ItemButton").GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + NameOfTheIcon(objectInventory.items[i]));
            itemsCount++;
        }
    }

    public void TakeThisItem(Button takeItemButton) {
        if (PlayerInventorySystem.playerInventorySystem.playerInventory.items.Count < PlayerInventorySystem.playerInventorySystem.numberOfInventorySlots) {
            int index = takeItemButton.transform.parent.GetSiblingIndex();
            PlayerInventorySystem.playerInventorySystem.AddItemToInventory(objectInventory.items[index]);
            itemsCount--;

            if (itemsCount == index) {
                inventoryGraphics.transform.GetChild(0).GetChild(index).Find("ItemButton").GetChild(0).GetComponent<Image>().enabled = false;
                inventoryGraphics.transform.GetChild(0).GetChild(index).Find("ItemButton").GetChild(0).GetComponent<Image>().sprite = null;
            }
            else {
                UpdateInventory(index);
            }
            objectInventory.RemoveItem(index);
            HideItemStatistics();
        }
        else {
            Debug.Log("Not enough space in inventory!");
        }
    }

    public void TakeAllItems() {
        if(PlayerInventorySystem.playerInventorySystem.playerInventory.items.Count + objectInventory.items.Count <= PlayerInventorySystem.playerInventorySystem.numberOfInventorySlots) {
            while(itemsCount > 0) {
                PlayerInventorySystem.playerInventorySystem.AddItemToInventory(objectInventory.items[itemsCount-1]);
                itemsCount--;
                inventoryGraphics.transform.GetChild(0).GetChild(itemsCount).Find("ItemButton").GetChild(0).GetComponent<Image>().enabled = false;
                inventoryGraphics.transform.GetChild(0).GetChild(itemsCount).Find("ItemButton").GetChild(0).GetComponent<Image>().sprite = null;
                objectInventory.RemoveItem(itemsCount);
            }
        }
        else {
            Debug.Log("Not enough space in inventory!");
        }
    }

    public void ShowObjectInventory() {
        inventoryGraphics.SetActive(true);
        isVisible = true;
        inventoryGraphics.transform.Find("Title").GetComponentInChildren<TextMeshProUGUI>().text = objectInventory.gameObject.name;
    }

    public void HideObjectInventory() {
        inventoryGraphics.SetActive(false);
        isVisible = false;
        objectInventory = null;
    }

    public void ShowItemStatistics(Button highlightButton) {
        int index = highlightButton.transform.parent.GetSiblingIndex();
        if (objectInventory.items.Count - 1 >= index) {
            itemStatisticsPanel.SetActive(true);
            itemStatisticsPanel.transform.position = highlightButton.transform.position - itemStatisticsPanelOffset;
            TextMeshProUGUI statisticsToShow = inventoryGraphics.transform.Find("ItemStatisticsPanel").GetComponentInChildren<TextMeshProUGUI>();
            statisticsToShow.text = "";
            statisticsToShow.text += objectInventory.items[index].name;
            for (int i = 0; i < objectInventory.items[index].GetComponentInChildren<ObjectStatistics>().statistic.Length; i++) {
                statisticsToShow.text += "\n\n" + objectInventory.items[index].GetComponentInChildren<ObjectStatistics>().statistic[i].name + ": " + objectInventory.items[index].GetComponentInChildren<ObjectStatistics>().statistic[i].currentValue;
            }
            itemStatisticsPanel.GetComponent<Animator>().SetBool("IsVisible", true);
            areStatisticsShowing = true;
        }
    }

    public void HideItemStatistics() {
        if (areStatisticsShowing) {
            itemStatisticsPanel.GetComponent<Animator>().SetBool("IsVisible", false);
            itemStatisticsPanel.SetActive(false);
            areStatisticsShowing = false;
        }
    }

    private void UpdateInventory(int index) {
        for (int i = index; i < itemsCount; i++) {
            //Moving every item that is to the right of the removed one, to the left (updating sprites, disabling previous slots, enabling new ones)
            inventoryGraphics.transform.GetChild(0).GetChild(i).Find("ItemButton").GetChild(0).GetComponent<Image>().sprite = inventoryGraphics.transform.GetChild(0).GetChild(i + 1).Find("ItemButton").GetChild(0).GetComponent<Image>().sprite;

            if (i + 1 == itemsCount) {
                inventoryGraphics.transform.GetChild(0).GetChild(i + 1).Find("ItemButton").GetChild(0).GetComponent<Image>().enabled = false;
            }
        }
    }

    private string NameOfTheIcon(GameObject item) {
        //One icon for all swords, axes and so on requires this kind of "if statements" (at least I think so [at the moment xD])
        if (item.name == "Sword" /*|| item.name == any other sword name*/) {
            return "sword";
        }
        else if (item.name == "Axe") {
            return "axe";
        }
        else {
            return "null";
        }
    }
}
