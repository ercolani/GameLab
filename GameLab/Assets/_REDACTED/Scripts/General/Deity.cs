using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deity : MonoBehaviour
{
    /// <summary>
    /// The index for the first encounter start node in the text file for a specific deity.
    /// </summary>
    [SerializeField]
    private int firstEncounterIndex;

    /// <summary>
    /// The index for the candle acquired start node in the text file for a specific deity.
    /// </summary>
    [SerializeField]
    private int candleAcquiredIndex;

    /// <summary>
    /// The index for the last encounter start node in the text file for a specific deity.
    /// </summary>
    [SerializeField]
    private int lastEnconterIndex;

    /// <summary>
    /// The index for the start node of the torch thoughts for a specific deity (dependent on how they are put in the file, goes from 0 to 3). 
    /// </summary>
    [SerializeField]
    private int torchThoughtOrderIndex;

    /// <summary>
    /// The count of how many torch thoughts have been seen by the player.
    /// </summary>
    [SerializeField]
    private int currentTorchThoughtIndex;

    /// <summary>
    /// The area code used by the deity for dialogue.
    /// </summary>
    [SerializeField]
    private string deityAreaCode;

    /// <summary>
    /// The current puzzle comment type exclaimed by the deity.
    /// </summary>
    [SerializeField]
    private string currentDeityPuzzleCommentType;

    /// <summary>
    /// Caches the PlayerDialogueInteraction component.
    /// </summary>
    [SerializeField]
    private PlayerDialogueInteraction playerDialogueInteraction;

    /// <summary>
    /// Caches the DialogueController component.
    /// </summary>
    [SerializeField]
    private DialogueController dialogueController;

    /// <summary>
    /// Whether the player has interacted with the deity before or not.
    /// </summary>
    [SerializeField]
    private bool isFirstEncounter;

    /// <summary>
    /// Whether the player has acquired the deity's candle.
    /// </summary>
    [SerializeField]
    private bool hasAcquiredCandle;

    /// <summary>
    /// Whether the player is awaiting their last encounter with the deity.
    /// </summary>
    [SerializeField]
    private bool isLastEncounter;

    /// <summary>
    /// Whether the deity is available for dialogue.
    /// </summary>
    [SerializeField]
    private bool isReadyForDialogue;

    /// <summary>
    /// A getter isReadyForDialogue.
    /// </summary>
    public bool IsReadyForDialogue => isReadyForDialogue;

    private void OnEnable()
    {
        playerDialogueInteraction.DialogueFired += FireDialogue;
    }

    private void OnDisable()
    {
        playerDialogueInteraction.DialogueFired -= FireDialogue;
    }

    /// <summary>
    /// Called when the player interacts to speak with the deity. The passage changes depending on when the player interacts with the deity (first encounter, then candle acquired, then last encounter).
    /// </summary>
    /// <param name="deity"></param>
    private void FireDialogue(Deity deity)
    {
        if (deity == this)
        {
            if (isFirstEncounter)
            {
                dialogueController.InitializeDialogue(firstEncounterIndex, new string[0]);
                isFirstEncounter = false;
                hasAcquiredCandle = true;
                //ToggleDeityReadyForDialogue();
            }
            else if (hasAcquiredCandle && !isLastEncounter)
            {
                dialogueController.InitializeDialogue(candleAcquiredIndex, new string[0]);
                isLastEncounter = true;
            }
            else if (isLastEncounter)
            {
                dialogueController.InitializeDialogue(lastEnconterIndex, new string[0]);
                isLastEncounter = false;
                //ToggleDeityReadyForDialogue();
            }
        }
    }

    /// <summary>
    /// Fires a torch thought. Called after a torch has been blown out by the player.
    /// </summary>
    public void FireTorchThought()
    {
        dialogueController.InitializeDialogue(deityAreaCode, torchThoughtOrderIndex, currentTorchThoughtIndex);
        currentTorchThoughtIndex++;
    }

    /// <summary>
    /// Fire a puzzle comment. The codes are as follows: "dw" is doing well, "dn" is doing neutral, and "mm" is made a mistake.
    /// </summary>
    public void FirePuzzleComment(string puzzleThoughtType)
    {
        string[] puzzleCommentInfo = new string[] { deityAreaCode, currentDeityPuzzleCommentType };
        dialogueController.InitializeDialogue(-1, puzzleCommentInfo);
    }

    public void ToggleDeityReadyForDialogue() 
    {
        isReadyForDialogue = !isReadyForDialogue;
    }
}
