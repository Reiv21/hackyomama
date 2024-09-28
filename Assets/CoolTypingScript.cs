using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoolTypingScript : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text messageText;
    [SerializeField] GameObject pressAnythingText;
    [SerializeField] Dialogue[] dialogues;
    int letterIndex = 0;
    int dialogueIndex = 0;
    
    [Serializable]
    public class Dialogue
    {
        public string name;
        public string message;
    }

    int typeRatePerSec = 400;

    void Start()
    {
        StartTyping();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            
            if (pressAnythingText.activeSelf)
            {
                dialogueIndex++;
                StartTyping();
            }
            else
            {
                CancelInvoke("AddLetter");
                messageText.text = dialogues[dialogueIndex].message;
                pressAnythingText.SetActive(true);
            }
        }
}

    public void StartTyping()
    {
        if (dialogueIndex >= dialogues.Length)
        {
            Debug.LogWarning("End of dialogues");
            gameObject.SetActive(false);
            return;
        }
        pressAnythingText.SetActive(false);
        messageText.text = "";
        nameText.text = dialogues[dialogueIndex].name;
        letterIndex = 0;
        InvokeRepeating("AddLetter",0,60f/typeRatePerSec);
    }

    void AddLetter()
    {
        messageText.text += dialogues[dialogueIndex].message[letterIndex];
        letterIndex++;
        if (letterIndex >= dialogues[dialogueIndex].message.Length)
        {
            pressAnythingText.SetActive(true);
            CancelInvoke("AddLetter");
        }
    }
}
