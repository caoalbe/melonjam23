using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;

    public bool IsOpen { get; private set; }
    [SerializeField] private GameObject player;
    [SerializeField] private TMP_Text textLabel;

    private TypewriterEffect typewriterEffect;
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
            yield return typewriterEffect.Run(dialogue, textLabel);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Q));
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
