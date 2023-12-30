using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueActivator : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private List<string> dialogueContent;
    [SerializeField] private DialogueUI dialogueUI;
    private bool dialogueActivated = false;
    [SerializeField] private bool activateTimeline = false;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private GameObject melon;
    [SerializeField] public bool IsTutorial;
    [SerializeField] public bool IsTutorialDone;
    [SerializeField] public bool FirstDialogue;

    public void Interact()
    {
        dialogueObject.dialogue = dialogueContent;
        dialogueUI.ShowDialogue(dialogueObject);
        dialogueObject.IsTutorial = IsTutorial;
        dialogueObject.IsTutorialDone = IsTutorialDone;
        dialogueObject.FirstDialogue = FirstDialogue;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        if (activateTimeline == true)
        {
            melon.SetActive(true);
            director?.Play();
            activateTimeline = false;
        }

        if (!dialogueActivated)
        {
            Interact();
            dialogueActivated = true;
        }
    }
    public void HideMelon()
    {
        melon?.SetActive(false);
    }
}
