public class SignPost : Interactable {

    public string[] dialogue;

    public override void Interact() {
        DialogueSystem.dialogueSystemInstance.AddNewDialogue(dialogue, "Sign post");
    }
}
