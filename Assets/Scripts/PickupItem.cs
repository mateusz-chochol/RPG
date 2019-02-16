public class PickupItem : Interactable {

    public override void Interact() {
        if (!PlayerInventorySystem.playerInventorySystem.inventoryGraphics.activeSelf) {
            PlayerInventorySystem.playerInventorySystem.AddItemToInventory(gameObject.transform.parent.gameObject);
        }
    }
}
