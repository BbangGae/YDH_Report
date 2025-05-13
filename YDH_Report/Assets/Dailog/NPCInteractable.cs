using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    public string npcId;
    public DialogueData firstDialogue;
    public DialogueData defaultDialogue;

    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            var state = NPCManager.Instance.GetState(npcId);

            if (!state.hasTalkedBefore && firstDialogue != null)
            {
                DialogueManager.Instance.StartDialogue(firstDialogue);
                state.hasTalkedBefore = true;
            }
            else if (defaultDialogue != null)
            {
                DialogueManager.Instance.StartDialogue(defaultDialogue);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
