using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static DialogueObject;
using static DialogueQueries;
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

    public static void KillAllChildren(UnityEngine.Transform parent)
    {
        UnityEngine.Assertions.Assert.IsNotNull(parent);
        for (int childIndex = parent.childCount - 1; childIndex >= 0; childIndex--)
        {
            UnityEngine.Object.Destroy(parent.GetChild(childIndex).gameObject);
        }
    }

    private void ToggleDialogue(bool state)
    {
        dialogueBoxPrefab.SetActive(state);
    }

    private void OnNodeSelected(int indexChosen)
    {
        dialogueController.NextNode(indexChosen);
    }

    private void OnNodeEntered(Node newNode)
    {
        nodeText.text = newNode.text;

        //KillAllChildren(responseParent);
        //for (int i = newNode.responses.Count - 1; i >= 0; i--)
        //{
        //    int currentChoiceIndex = i;
        //    var response = newNode.responses[i];
        //    var responseButton = Instantiate(responsePrefab, responseParent);
        //    responseButton.GetComponentInChildren<TMP_Text>().text = response.displayText;
        //    responseButton.GetComponent<Button>().onClick.AddListener(delegate { OnNodeSelected(currentChoiceIndex); });
        //}

    }
}

