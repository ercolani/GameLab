using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogueObject;

public class DialogueController : MonoBehaviour
{

    [SerializeField] private TextAsset twineText;
    private Dialogue dialogueObject;
    private Node currentNode;
    private string currentNodeTitle;
    private List<string> allStartNodes;

    public delegate void NodeEnteredHandler(Node node);
    public event NodeEnteredHandler onEnteredNode;

    private void Awake()
    {
        dialogueObject = new Dialogue(twineText);
    }

    private void Start()
    {
        allStartNodes = dialogueObject.FindAllStartNodes();
        InitializeDialogue();
    }

    public void SetCurrentNodeTitle(string title)
    {
        if (allStartNodes.Contains(title))
        {
            currentNodeTitle = title;
        }
    }

    public void InitializeDialogue()
    {
        currentNode = dialogueObject.GetNode(currentNodeTitle);
        onEnteredNode(currentNode);
    }

    public void ChooseResponse(int responseIndex)
    {
        string nextNodeID = currentNode.responses[responseIndex].destinationNode;
        Node nextNode = dialogueObject.GetNode(nextNodeID);
        currentNode = nextNode;
        onEnteredNode(nextNode);
    }
}
