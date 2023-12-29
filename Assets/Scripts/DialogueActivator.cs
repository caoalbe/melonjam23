using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivator : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private List<string> dialogueContent;
    [SerializeField] private DialogueUI dialogueUI;
    private bool dialogueActivated = false;

    public void Interact()
    {
        dialogueObject.dialogue = dialogueContent;
        dialogueUI.ShowDialogue(dialogueObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        if (!dialogueActivated)
        {
            Interact();
            dialogueActivated = true;
        }
    }
}
