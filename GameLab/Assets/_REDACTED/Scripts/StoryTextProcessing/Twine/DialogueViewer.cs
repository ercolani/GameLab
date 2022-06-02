using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static DialogueObject;
using static AudioController;
using UnityEngine.Events;
using System;

public class DialogueViewer : MonoBehaviour
{
    [SerializeField] private Transform dialogueParent;
    [SerializeField] private GameObject dialogueBoxPrefab;
    [SerializeField] private TMP_Text nodeText;
    [SerializeField] private DialogueController dialogueController;

    private void OnEnable()
    {
        dialogueController.onEnteredNode += OnNodeEntered;
        dialogueController.ToggleDialogue += ToggleDialogue;
    }
    private void OnDisable()
    {
        dialogueController.onEnteredNode -= OnNodeEntered;
        dialogueController.ToggleDialogue -= ToggleDialogue;
    }

    private void ToggleDialogue(bool state)
    {
        dialogueBoxPrefab.SetActive(state);
    }

    private void OnNodeEntered(Node newNode)
    {
        nodeText.text = newNode.text;
        AudioController.ParseVoiceLineCode(newNode.title);
        if (newNode.tags.Contains("TorchThought") || newNode.tags.Contains("PuzzleComment"))
        {
            StartCoroutine(dialogueController.PlayAndRemoveTemporaryDialogueCoroutine(newNode));
        }
    }
}

