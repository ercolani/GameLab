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
    public int currentStartNode;

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
        currentNodeTitle = allStartNodes[currentStartNode]; //currentStartNode must be incremented to start a new dialogue passage
        currentNode = dialogueObject.GetNode(currentNodeTitle);
        onEnteredNode?.Invoke(currentNode);
            
    }

    public void ChooseResponse(int responseIndex)
    {
        string nextNodeID = currentNode.responses[responseIndex].destinationNode;
        Node nextNode = dialogueObject.GetNode(nextNodeID);
        currentNode = nextNode;
        onEnteredNode?.Invoke(nextNode);
    }
}
