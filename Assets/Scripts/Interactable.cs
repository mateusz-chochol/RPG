using UnityEngine;
using UnityEngine.AI;

public class Interactable : MonoBehaviour {

    private NavMeshAgent playerAgent;
    private bool hasInteracted;

    public virtual void MoveToInteractable(NavMeshAgent playerAgent) {
        hasInteracted = false;
        this.playerAgent = playerAgent;
        playerAgent.destination = transform.position;
        playerAgent.stoppingDistance = 4f;
    }

    private void Update() {
        if (playerAgent != null && playerAgent.enabled && !playerAgent.pathPending && !hasInteracted && playerAgent.stoppingDistance != 0f) {
            if (playerAgent.remainingDistance <= playerAgent.stoppingDistance) {
                Interact();
                hasInteracted = true;
            }
        }
    }

    public virtual void Interact() {
        Debug.Log("Interacting with an interactable object");
    }
}

