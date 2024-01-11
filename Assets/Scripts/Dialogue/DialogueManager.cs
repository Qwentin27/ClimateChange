using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public Image actorImage; // le reconnaît directement donc pas besoin de mettre "serializable"
    public TMP_Text actorName;
    public TMP_Text messageText;
    public RectTransform backgroundBox;

    public GameObject panelContinuer;
    public GameObject panelNextLevel;

    Message[] currentMessages;
    Actor[] currentActors;
    int activeMessage = 0;
    public static bool isActive = false;

    [Header("Dialogue")]
    public GameObject StartDialogue1;
    public GameObject StartDialogue2;
    public GameObject StartDialogue3;
    public GameObject StartDialogue4;
    public GameObject StartDialogue5;

    [Header("Gestion Boutons")]
    //public Button Continuer;
    private string scene;

    public Button _passer;

    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        currentMessages = messages;
        currentActors = actors;
        //activeMessage = 0;
        isActive = true;
        //Debug.Log("Started conversation : Loaded messages " + messages.Length);
        DisplayMessage();
    }

    void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessage];
        messageText.text = messageToDisplay.message;

        Actor actorToDisplay = currentActors[messageToDisplay.actorId];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;

        StopAllCoroutines();
        StartCoroutine(TypeMessage(messageToDisplay));
    }

    IEnumerator TypeMessage(Message messages)
    {
        messageText.text = "";
        foreach (char letter in messages.message.ToCharArray())
        {
            messageText.text += letter;
            yield return null;
        }
        if (activeMessage == 0)
        {
            panelContinuer.SetActive(true);
            //Continuer.interactable = true;
            //scene = "Scenes/Niveau " + PlayerPrefs.GetInt("Level").ToString();
            //PlayerPrefs.SetString("NomScene", scene);
        }
        else
        {
            panelContinuer.SetActive(false);
        }

    }

    public void NextMessage()
    {
        activeMessage++;
        if (activeMessage < currentMessages.Length)
        {
            DisplayMessage();
        }
        else
        {
            //Debug.Log("Conversation ended!");
            isActive = false;
            panelNextLevel.SetActive(true);
        }
    }

    public void FinDialogue()
    {
        isActive = false;
        panelNextLevel.SetActive(true);
        panelContinuer.SetActive(false);

    }

    public void FContinuer()
    {
        SceneManager.LoadScene("Niveau 1");
    }

    // Start is called before the first frame update
    void Start()
    {
        panelContinuer.SetActive(false);
        panelNextLevel.SetActive(false);

        //Continuer.interactable = true;
        _passer.interactable = false;

        StartDialogue1.SetActive(false);
        StartDialogue2.SetActive(true);
        StartDialogue3.SetActive(false);
        StartDialogue4.SetActive(false);
        StartDialogue5.SetActive(false);


        /*if (PlayerPrefs.GetInt("Level") == 1) //à voir si on remplace par setpref init player
        {
            StartDialogue1.SetActive(true);
            StartDialogue2.SetActive(false);
            StartDialogue3.SetActive(false);
            StartDialogue4.SetActive(false);
            StartDialogue5.SetActive(false);
        }

        if (PlayerPrefs.GetInt("Level") == 2)
        {
            StartDialogue2.SetActive(true);
            StartDialogue1.SetActive(false);
            StartDialogue3.SetActive(false);
            StartDialogue4.SetActive(false);
            StartDialogue5.SetActive(false);
        }

        if (PlayerPrefs.GetInt("Level") == 3)
        {
            StartDialogue3.SetActive(true);
            StartDialogue1.SetActive(false);
            StartDialogue4.SetActive(false);
            StartDialogue2.SetActive(false);
            StartDialogue5.SetActive(false);
        }

        if (PlayerPrefs.GetInt("Level") == 4)
        {
            StartDialogue4.SetActive(true);
            StartDialogue1.SetActive(false);
            StartDialogue3.SetActive(false);
            StartDialogue2.SetActive(false);
            StartDialogue5.SetActive(false);
        }

        if (PlayerPrefs.GetInt("Level") == 5)
        {
            StartDialogue4.SetActive(false);
            StartDialogue1.SetActive(false);
            StartDialogue3.SetActive(false);
            StartDialogue2.SetActive(false);
            StartDialogue5.SetActive(true);
        }

        //Debug.Log(PlayerPrefs.GetInt("LevelMax"));
        //Debug.Log(PlayerPrefs.GetInt("Level"));
        if (PlayerPrefs.GetInt("LevelMax") >= (PlayerPrefs.GetInt("Level")))
        {
            _passer.interactable = true;
        }*/

    }

    // Update is called once per frame
    void Update() 
    {
        /*if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) 
        {
            NextMessage();
        }*/

        if (Input.GetKeyDown(KeyCode.Space) && isActive == true) // on appui sur barre espace pour voir le message suivant
        {
             NextMessage();
        }


    }
}
