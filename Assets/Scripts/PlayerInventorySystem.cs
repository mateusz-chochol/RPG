using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInventorySystem : MonoBehaviour {

    public static PlayerInventorySystem playerInventorySystem;
    public Inventory playerInventory;
    public GameObject inventoryGraphics;
    public int numberOfInventorySlots = 24;
    public bool isVisible = false;

    private GameObject itemStatisticsPanel;
    private Vector3 itemStatisticsPanelOffset = new Vector3(-80f, 90f, 0f);
    private int itemsCount = 0;
    private bool areStatisticsShowing = false;
    private GameObject equippedObject = null;

    private void Awake() {
        if(playerInventorySystem != null && playerInventorySystem != this) {
            Destroy(gameObject);
        }
        else {
            playerInventorySystem = this;
        }
    }

    private void Start() {
        itemStatisticsPanel = inventoryGraphics.transform.Find("ItemStatisticsPanel").gameObject;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            ToggleInventorySystem();
            if (ObjectInventorySystem.objectInventorySystem.isVisible) {
                ObjectInventorySystem.objectInventorySystem.HideObjectInventory();
            }
        }
    }

    public void ToggleInventorySystem() {
        if (!isVisible) {
            MakeInventoryVisible();
        }
        else {
            MakeInventoryHidden();
        }
    }

    public void MakeInventoryVisible() {
        Camera.main.transform.parent.GetComponent<CameraEnabler>().ShowCursorAndStopCameraRotation();
        inventoryGraphics.SetActive(true);
        isVisible = true;
    }

    public void MakeInventoryHidden() {
        inventoryGraphics.SetActive(false);
        Camera.main.transform.parent.GetComponent<CameraEnabler>().HideCursorAndStartCameraRotation();
        isVisible = false;
    }

    public void EquippOrInteractWithItem(Button interactButton) {
        int index = interactButton.transform.parent.GetSiblingIndex();
        if(index < itemsCount) {
            if(playerInventory.transform.Find("Right Hand").childCount == 0 && playerInventory.items[index].GetComponentInChildren<ObjectStatistics>().objectType == ObjectStatistics.ObjectTypes.Weapon) {
                equippedObject = Instantiate(playerInventory.items[index], playerInventory.transform.Find("Right Hand"));
                equippedObject.name = playerInventory.items[index].name;
                equippedObject.transform.position = playerInventory.transform.Find("Right Hand").position;

                Debug.Log("Equipping " + equippedObject.name);
                Debug.Log("Turning slot to green");
            }
        }
    }

    public void DeequippItem(Button interactButton) {
        int index = interactButton.transform.parent.GetSiblingIndex();

        if(playerInventory.transform.Find("Right Hand").childCount == 1) {

        }
    }

    public void ShowItemStatistics(Button highlightButton) {
        int index = highlightButton.transform.parent.GetSiblingIndex();
        if (playerInventory.items.Count - 1  >= index) {
            itemStatisticsPanel.SetActive(true);
            itemStatisticsPanel.transform.position = highlightButton.transform.position - itemStatisticsPanelOffset;
            TextMeshProUGUI statisticsToShow = inventoryGraphics.transform.Find("ItemStatisticsPanel").GetComponentInChildren<TextMeshProUGUI>();
            statisticsToShow.text = "";
            statisticsToShow.text += playerInventory.items[index].name;
            for(int i = 0; i < playerInventory.items[index].GetComponentInChildren<ObjectStatistics>().statistic.Length; i++) {
                statisticsToShow.text += "\n\n" + playerInventory.items[index].GetComponentInChildren<ObjectStatistics>().statistic[i].name + ": " + playerInventory.items[index].GetComponentInChildren<ObjectStatistics>().statistic[i].currentValue;
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

    public void AddItemToInventory(GameObject newItem) {
        if (playerInventory.items.Count < numberOfInventorySlots) {
            playerInventory.AddNewItem(newItem);
            inventoryGraphics.transform.GetChild(0).GetChild(playerInventory.items.Count - 1).Find("ItemButton").GetChild(0).GetComponent<Image>().enabled = true;
            inventoryGraphics.transform.GetChild(0).GetChild(playerInventory.items.Count - 1).Find("RemoveButton").GetComponent<Button>().interactable = true;
            inventoryGraphics.transform.GetChild(0).GetChild(playerInventory.items.Count - 1).Find("ItemButton").GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + NameOfTheIcon(newItem));
            itemsCount++;
        }
    }

    public void AddItemsToInventory(List<GameObject> newItems) {
        for (int i = 0; i < newItems.Count; i++) {
            AddItemToInventory(newItems[i]);
        }
    }

    public void RemoveItemFromInventory(Button removeButton) {
        int index = removeButton.transform.parent.GetSiblingIndex();
        itemsCount--;

        if(itemsCount == index) {
            inventoryGraphics.transform.GetChild(0).GetChild(index).Find("ItemButton").GetChild(0).GetComponent<Image>().enabled = false;
            inventoryGraphics.transform.GetChild(0).GetChild(index).Find("RemoveButton").GetComponent<Button>().interactable = false;
            inventoryGraphics.transform.GetChild(0).GetChild(index).Find("ItemButton").GetChild(0).GetComponent<Image>().sprite = null;
        }
        else {
            UpdateInventory(index);
        }
        playerInventory.RemoveItem(index);
    }

    private void UpdateInventory(int index) {
        for (int i = index; i < itemsCount; i++) {
            //Moving every item that is to the right of the removed one, to the left (updating sprites, disabling previous slots, enabling new ones)
            inventoryGraphics.transform.GetChild(0).GetChild(i).Find("ItemButton").GetChild(0).GetComponent<Image>().sprite = inventoryGraphics.transform.GetChild(0).GetChild(i + 1).Find("ItemButton").GetChild(0).GetComponent<Image>().sprite;

            if(i + 1 == itemsCount) {
                inventoryGraphics.transform.GetChild(0).GetChild(i + 1).Find("ItemButton").GetChild(0).GetComponent<Image>().enabled = false;
                inventoryGraphics.transform.GetChild(0).GetChild(i + 1).Find("RemoveButton").GetComponent<Button>().interactable = false;
            }
        }
    }

    private string NameOfTheIcon(GameObject item) {
        //One icon for all swords, axes and so on requires this kind of "if statements" (at least I think so [at the moment xD])
        if(item.name == "Sword" /*|| item.name == any other sword name*/) {
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
