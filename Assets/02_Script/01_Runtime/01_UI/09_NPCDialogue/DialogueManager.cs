using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TheLastHeir.Runtime.Shop;   // ShopData 사용
using TheLastHeir.Runtime.Entity; // PlayerInputHandler 사용

namespace TheLastHeir.Runtime.UI
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }

        [Header("UI References")]
        public GameObject dialoguePanel;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI dialogueText;
        public Transform choiceContainer;
        public GameObject choicePrefab;

        [Header("Settings")]
        public float typingSpeed = 0.05f;

        private bool _isTyping = false;
        private bool _isWaitingForClose = false; 
        private DialogueData _currentData;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            dialoguePanel.SetActive(false);
        }

        private void Update()
        {
            if (_isWaitingForClose)
            {
                if (PlayerInputHandler.Instance != null && PlayerInputHandler.Instance.DialogueInput)
                {
                    EndDialogue();
                }
            }
        }

        public void StartDialogue(DialogueData data)
        {
            _currentData = data;
            dialoguePanel.SetActive(true);
            _isWaitingForClose = false;
            
            ClearChoices();
            nameText.text = data.npcName;
            
            StopAllCoroutines();
            StartCoroutine(TypeSentence(data.sentence));
        }
        
        private IEnumerator TypeSentence(string sentence)
        {
            _isTyping = true;
            dialogueText.text = "";

            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            _isTyping = false;
            DisplayChoices();
        }

        private void DisplayChoices()
        {
            if (_currentData.choices == null || _currentData.choices.Count == 0)
            {
                _isWaitingForClose = true;
                return;
            }

            foreach (var choice in _currentData.choices)
            {
                GameObject choiceObj = Instantiate(choicePrefab, choiceContainer);
                var textComp = choiceObj.GetComponentInChildren<TextMeshProUGUI>();
                if (textComp != null) textComp.text = choice.choiceText;
                
                Button btn = choiceObj.GetComponent<Button>();
                btn.onClick.AddListener(() => OnChoiceSelected(choice));
            }
        }
        
        private void OnChoiceSelected(DialogueChoice choice)
        {
            ClearChoices();
            
            if (choice.shopData != null)
            {
                EndDialogue();
                
                if (ShopUIHandler.Instance != null)
                {
                    ShopUIHandler.Instance.OpenShop(choice.shopData);
                }
                else
                {
                    Debug.LogError("ShopUIHandler가 씬에 X");
                }
                return;
            }
            
            if (choice.nextDialogue != null)
            {
                StartDialogue(choice.nextDialogue);
            }
            else
            {
                EndDialogue();
            }
        }

        private void ClearChoices()
        {
            foreach (Transform child in choiceContainer)
            {
                Destroy(child.gameObject);
            }
        }

        public void EndDialogue()
        {
            _isWaitingForClose = false;
            dialoguePanel.SetActive(false);
            ClearChoices();
        }
    }
}