using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static DialogueObject;
using UnityEngine.Events;
using System;

public class DialogueViewer : MonoBehaviour
{
    [SerializeField] private Transform responseParent;
    [SerializeField] private GameObject responsePrefab;
    [SerializeField] private TMP_Text nodeText;
    [SerializeField] private DialogueController dialogueController;

    private void Start()
    {
        dialogueController.onEnteredNode += OnNodeEntered;
    }

    public static void KillAllChildren(UnityEngine.Transform parent)
    {
        UnityEngine.Assertions.Assert.IsNotNull(parent);
        for (int childIndex = parent.childCount - 1; childIndex >= 0; childIndex--)
        {
            UnityEngine.Object.Destroy(parent.GetChild(childIndex).gameObject);
        }
    }

    private void OnNodeSelected(int indexChosen)
    {
        Debug.Log("Chose: " + indexChosen);
        dialogueController.ChooseResponse(indexChosen);
    }

    private void OnNodeEntered(Node newNode)
    {
        Debug.Log("Entering node: " + newNode.title);
        nodeText.text = newNode.text;

        KillAllChildren(responseParent);
        for (int i = newNode.responses.Count - 1; i >= 0; i--)
        {
            int currentChoiceIndex = i;
            var response = newNode.responses[i];
            var responseButton = Instantiate(responsePrefab, responseParent);
            responseButton.GetComponentInChildren<TMP_Text>().text = response.displayText;
            responseButton.GetComponent<Button>().onClick.AddListener(delegate { OnNodeSelected(currentChoiceIndex); });
        }

        if (newNode.tags.Contains("END"))
        {
            Debug.Log("End!");
        }
    }
}