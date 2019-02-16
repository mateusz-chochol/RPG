using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour {

    private NavMeshAgent playerAgent;
    private CharacterController movementController;
    private Vector3 moveDirection = Vector3.zero;
    private Quaternion playerRotation;

    public float rotationSpeed;
    public float movementSpeed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private void Start() {
        playerAgent = GetComponent<NavMeshAgent>();
        movementController = GetComponent<CharacterController>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && movementController.isGrounded) {
            GetInteraction();
        }
        ArrowKeysMovement();
        if (!playerAgent.enabled) {
            RotateToCameraRotation();
        }
    }
    
    private void ArrowKeysMovement() {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
            StopAnyInteractions();
        }
        if (movementController.isGrounded) {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= movementSpeed;
            if (Input.GetButton("Jump")) {
                moveDirection.y = jumpSpeed;
            }
        }
        moveDirection.y -= gravity * Time.deltaTime;
        movementController.Move(moveDirection * Time.deltaTime);
    }

    private void GetInteraction() {
        Ray interactionRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit interactionInfo;

        if (Physics.Raycast(interactionRay, out interactionInfo)) {
            GameObject interactedObject = interactionInfo.collider.gameObject;

            if (interactedObject.tag == "InteractableObject") {
                playerAgent.enabled = true;
                interactedObject.GetComponent<Interactable>().MoveToInteractable(playerAgent);
            }
        }
    }

    private void StopAnyInteractions() {
        playerAgent.enabled = false;
        DialogueSystem.dialogueSystemInstance.HideTheDialogueFromOutside();
        if (PlayerInventorySystem.playerInventorySystem.isVisible) {
            PlayerInventorySystem.playerInventorySystem.MakeInventoryHidden();
        }
        if (ObjectInventorySystem.objectInventorySystem.isVisible) {
            ObjectInventorySystem.objectInventorySystem.HideObjectInventory();
        }
        Camera.main.transform.parent.GetComponent<CameraEnabler>().HideCursorAndStartCameraRotation();
    }

    private void RotateToCameraRotation() {
        transform.rotation = Quaternion.Slerp(transform.rotation, Camera.main.transform.rotation, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, 0f));
    }
}
