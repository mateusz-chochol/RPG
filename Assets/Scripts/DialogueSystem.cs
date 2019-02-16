using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour {

    public static DialogueSystem dialogueSystemInstance;
    public GameObject dialoguePanel;

    private List<string> newDialogue = new List<string>();
    private int dialogueIndex = 0;
    private Animator animator;
    private string npcName;

    private void Awake() {
        if(dialogueSystemInstance != null && dialogueSystemInstance != this) {
            Destroy(gameObject);
        }
        else {
            dialogueSystemInstance = this;
            animator = dialoguePanel.GetComponent<Animator>();
            dialoguePanel.SetActive(false);
        }
    }

    public void AddNewDialogue(string[] dialogueLines, string npcName) {
        newDialogue.Clear();
        newDialogue.AddRange(dialogueLines);
        this.npcName = npcName;
        ShowDialogue();
    }

    public void ShowDialogue() {
        Camera.main.transform.parent.GetComponent<CameraEnabler>().ShowCursorAndStopCameraRotation();
        dialoguePanel.SetActive(true);
        animator.SetBool("IsVisible", true);
        dialoguePanel.transform.Find("NPC Name Panel").GetComponentInChildren<Text>().text = npcName;
        ChangeLines();
    }

    public void ChangeLines() {
        if(dialogueIndex < newDialogue.Count) {
            StopAllCoroutines();
            StartCoroutine(TypeSentence(newDialogue[dialogueIndex]));
            dialogueIndex++;
        }
        else {
            StartCoroutine(HideTheDialogue());
        }
    }

    IEnumerator TypeSentence(string sentence) {
        dialoguePanel.transform.Find("Dialogue Text").GetComponent<Text>().text = "";

        foreach (char letter in sentence.ToCharArray()) {
            dialoguePanel.transform.Find("Dialogue Text").GetComponent<Text>().text += letter;
            yield return null;
        }
    }

    IEnumerator HideTheDialogue() {
        Camera.main.transform.parent.GetComponent<CameraEnabler>().HideCursorAndStartCameraRotation();
        animator.SetBool("IsVisible", false);
        yield return new WaitForSeconds(1f);
        dialoguePanel.SetActive(false);
        dialogueIndex = 0;
    }

    public void HideTheDialogueFromOutside() {
        if (dialoguePanel.activeSelf) {
            StartCoroutine(HideTheDialogue());
        }
    }
}
