using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static DialogueObject;
using static AudioController;

public class DialogueController : MonoBehaviour
{
    [SerializeField]
    private TextAsset twineText;

    private Dialogue dialogueObject;

    private Node currentNode;

    [SerializeField]
    private string currentNodeTitle;

    [SerializeField]
    private List<string> allStartNodes;

    [SerializeField]
    private List<string> allTorchThoughtStartNodes;

    [SerializeField]
    private List<string> allPuzzleCommentNodes;

    [SerializeField]
    private float _deityAutoBeginDelay = 7f;

    [SerializeField]
    private float _subtitleDelay = 1f;

    [SerializeField]
    private float _deityAutoDelay = 15f;

    public delegate void NodeEnteredHandler(Node node);

    public event NodeEnteredHandler onEnteredNode;

    public event Action<bool> ToggleDialogue;

    public event Action<bool> OnToggleDeityInteraction;

    public event Action OnActivatePuzzle;

    public event Action OnFinalDialogueEnded;

    private bool dialogueActive;

    private bool _autoMode;

    [SerializeField]
    private PlayerDialogueInteraction playerDialogueInteraction;

    private void Awake()
    {
        dialogueObject = new Dialogue(twineText);
    }

    private void Start()
    {
        allStartNodes = dialogueObject.FindAllStartNodes();
        allTorchThoughtStartNodes = dialogueObject.FindAllTorchThoughtNodes();
        allPuzzleCommentNodes = dialogueObject.FindAllPuzzleCommentNodes();
    }

    private IEnumerator NextDialogue()
    {

        if (!_autoMode)
        {
            yield return new WaitForSeconds(AudioController.InstanceLength - _subtitleDelay);
            ToggleDialogueState();

            if (currentNode.tags.Contains("Auto"))
            {
                yield return new WaitForSeconds(_subtitleDelay);
                StartCoroutine(BeginAutomaticMode());
            }
            else
            {
                if (currentNode.tags.Contains("Pillar"))
                {
                    playerDialogueInteraction.PlayPillarAnimation();
                }

                if (!currentNode.tags.Contains("END"))
                {
                    yield return new WaitForSeconds(_subtitleDelay);
                    NextNode(0);
                }
            }
        }
        else
        {
            yield return new WaitForSeconds(AudioController.InstanceLength - _subtitleDelay);

            if (currentNode.tags.Contains("END"))
            {
                _autoMode = false;
                ToggleDialogueState();
            }
            else
            {
                ToggleDialogueState();
                yield return new WaitForSeconds(_subtitleDelay);
                StartAutoVoiceLines();
            }
        }
    }

    public void SetCurrentNodeTitle(string title)
    {
        if (allStartNodes.Contains(title))
        {
            currentNodeTitle = title;
        }
    }

    /// <summary>
    /// Deities initialize dialogue using an index for the correct passage. If their exclamation is a puzzle comment rather than dialogue, a random puzzle comment is played based on their criteria. NOT for torch thoughts - the overload method should be used for this.
    /// </summary>
    /// <param name="passageIndex">The index for the deity's current passage. Pass null for a puzzle comment.</param>
    /// <param name="puzzleCommentInfo">Pass -1 if not a puzzle comment. Index 0 represents the area code and Index 1 represents the type of puzzle comment.</param>
    public void InitializeDialogue(int passageIndex, string[] puzzleCommentInfo)
    {
        if (puzzleCommentInfo.Length == 0)
        {
            ToggleDialogueState();

            currentNodeTitle = allStartNodes[passageIndex];
            currentNode = dialogueObject.GetNode(currentNodeTitle);
            onEnteredNode?.Invoke(currentNode);
            StartCoroutine(NextDialogue());
        }
        else
        {
            string areaCode = puzzleCommentInfo[0];
            string puzzleCommentType = puzzleCommentInfo[1];

            ToggleDialogueState();

            List<string> matchingAreaPuzzleCommentNodes = allPuzzleCommentNodes.FindAll(node => node.Contains($"{areaCode}") && node.Contains($"{puzzleCommentType}"));
            currentNodeTitle = matchingAreaPuzzleCommentNodes[UnityEngine.Random.Range(0, matchingAreaPuzzleCommentNodes.Count)];
            currentNode = dialogueObject.GetNode(currentNodeTitle);
            onEnteredNode?.Invoke(currentNode);
            StartCoroutine(NextDialogue());
            allPuzzleCommentNodes.Remove(currentNodeTitle);
        }
    }

    /// <summary>
    /// An overload of initializing dialogue specifically for torch thoughts. The arguments correspond to what is necessary to process a torch thought.
    /// </summary>
    public void InitializeDialogue(string areaCode, int passageIndex, int torchThoughtIndex)
    {
        List<string> areaTorchThoughtNodes = allTorchThoughtStartNodes.FindAll(node => node.Contains($"{areaCode}"));

        if (torchThoughtIndex < areaTorchThoughtNodes.Count)
        {
            ToggleDialogueState();

            currentNodeTitle = areaTorchThoughtNodes[torchThoughtIndex];
            currentNode = dialogueObject.GetNode(currentNodeTitle);
            onEnteredNode?.Invoke(currentNode);
            StartCoroutine(NextDialogue());
            allPuzzleCommentNodes.Remove(currentNodeTitle);
        }
        else
        {
            Debug.LogError("Torch thought index out of range.");
        }
    }

    public void NextNode(int responseIndex)
    {
        ToggleDialogueState();
        string nextNodeID = currentNode.responses[responseIndex].destinationNode;
        Node nextNode = dialogueObject.GetNode(nextNodeID);
        currentNode = nextNode;
        onEnteredNode?.Invoke(nextNode);
        StartCoroutine(NextDialogue());
    }

    public IEnumerator PlayAndRemoveTemporaryDialogueCoroutine(Node node)
    {
        float voiceLineLength = AudioController.InstanceLength;
        yield return new WaitForSeconds(voiceLineLength);
        ToggleDialogueState();
    }

    private void ToggleDialogueState()
    {
        dialogueActive = !dialogueActive;
        ToggleDialogue?.Invoke(dialogueActive);
    } 
    
    private IEnumerator BeginAutomaticMode()
    {
        yield return new WaitForSeconds(_deityAutoBeginDelay);
        StartAutoVoiceLines();
    }

    private void StartAutoVoiceLines()
    {
        _autoMode = true;
        NextNode(0);
    }
}


