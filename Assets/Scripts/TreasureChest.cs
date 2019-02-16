public class TreasureChest : Interactable {

    public override void Interact() {
        ObjectInventorySystem.objectInventorySystem.SetObjectInventory(GetComponent<Inventory>());
        PlayerInventorySystem.playerInventorySystem.MakeInventoryVisible();
    }
}
