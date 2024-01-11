using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;

    public void StartDialogue()
    {
        FindObjectOfType<DialogueManager>().OpenDialogue(messages, actors);
    }


    // Start is called before the first frame update
    void Start()
    {
        StartDialogue();
    }
}


[System.Serializable] // classe particulière donc nécessaire pour afficher les éléments dans l'inspector
public class Message
{
    public int actorId;
    [TextArea(3, 10)]
    public string message;
}

[System.Serializable]
public class Actor
{
    public string name;
    public Sprite sprite;
}
