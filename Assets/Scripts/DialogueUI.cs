using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;

    public bool IsOpen { get; private set; }
    [SerializeField] private GameObject player;
    [SerializeField] private TMP_Text textLabel;

    private TypewriterEffect typewriterEffect;

    [SerializeField] private GameObject promptQ;
    [SerializeField] private GameObject promptE;

    private void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
        CloseDialogue();
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        IsOpen = true;
        dialogueBox.SetActive(true);
        RectTransform rt = dialogueBox.GetComponent<RectTransform>();
        rt.transform.localPosition = new Vector2(player.transform.position.x + 200, player.transform.position.y + 200);

        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        Time.timeScale = 0;
        foreach (string dialogue in dialogueObject.dialogue)
        {
            promptQ.SetActive(true);
            yield return typewriterEffect.Run(dialogue, textLabel);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Q));
        }
        if (dialogueObject.IsTutorial)
        {
            promptQ.SetActive(false);
            promptE.SetActive(true);

            PlayerInfo.CanTurnOnLight = true;
            PlayerInfo.IsTutorialDone = false;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            
            promptE.SetActive(false);

        }
        else if (dialogueObject.IsTutorialDone)
        {
            promptQ.SetActive(false);
            promptE.SetActive(true);

            PlayerInfo.CanTurnOnLight = true;
            PlayerInfo.IsTutorialDone = true;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            
            promptE.SetActive(false);
        }
        else if (dialogueObject.FirstDialogue)
        {
            yield return null;
        }
        CloseDialogue();
    }

    public void CloseDialogue()
    {
        IsOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
        Time.timeScale = 1;
    }

}
