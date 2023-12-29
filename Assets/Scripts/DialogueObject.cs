using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueObject", menuName = "ScriptableObjects/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField][TextArea] public List<string> dialogue;

}