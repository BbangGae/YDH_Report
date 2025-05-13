using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public Button continueButton;

    private Queue<DialogueSentence> sentences = new();
    private Coroutine typingCoroutine;
    private DialogueSentence currentSentence;

    private PlayerBaseController playerController; // ✅ 이동 제어용

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dialoguePanel.SetActive(false);
    }

    private void Start()
{
    // 🔄 수정: PlayerController를 가져오되 내부는 BaseController 기준
    GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
    if (playerObj != null)
    {
        playerController = playerObj.GetComponent<PlayerController>();
    }
}


    public void StartDialogue(DialogueData data)
    {
        if (playerController != null)
            playerController.canMove = false;

        sentences.Clear();
        foreach (var sentence in data.sentences)
        {
            sentences.Enqueue(sentence);
        }

        dialoguePanel.SetActive(true);
        DisplayNextSentence();
    }

    public void OnContinue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentSentence.text;
            typingCoroutine = null;
            continueButton.interactable = true;
            return;
        }

        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();
        speakerNameText.text = currentSentence.speakerName;
        continueButton.interactable = false;
        typingCoroutine = StartCoroutine(TypeSentence(currentSentence.text));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char c in sentence.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.03f); // 타자기 속도
        }

        typingCoroutine = null;
        continueButton.interactable = true;
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        if (playerController != null)
            playerController.canMove = true;
    }
}
