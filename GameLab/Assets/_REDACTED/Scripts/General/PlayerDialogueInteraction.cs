using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Controls the player's interaction with dialogue.
/// </summary>
public class PlayerDialogueInteraction : MonoBehaviour
{
    /// <summary>
    /// The range at which the player can interact to speak with deities.
    /// </summary>
    [SerializeField]
    private float dialogueRange = 3f;

    /// <summary>
    /// Whether the player is in the dialogue range.
    /// </summary>
    [SerializeField]
    private bool inRange;

    /// <summary>
    /// The dialogue button that shows when the player is in the dialogue range.
    /// </summary>
    [SerializeField]
    private GameObject dialogueButton;

    /// <summary>
    /// The key the player presses to interact with dialogue.
    /// </summary>
    [SerializeField]
    private KeyCode dialogueInteraction;

    /// <summary>
    /// The deity that currently is in range to be in dialogue with.
    /// </summary>
    [SerializeField]
    private Deity currentDeityInDialogue;

    /// <summary>
    /// Invoked when the player is in the dialogue range and presses the dialogue interaction key.
    /// </summary>
    public event Action<Deity> DialogueFired;

    private void Update()
    {
        bool deityFound = false;
        //makes the overlap sphere in front of the player so that items that are not in the field of view are not picked up
        Collider[] deities = Physics.OverlapSphere(transform.position + (transform.forward), dialogueRange / 2);
        deities.OrderBy(obj => (transform.position - obj.transform.position).sqrMagnitude).ToArray(); //sort by proximity
        foreach (Collider deity in deities)
        {
            Deity deityComponent = deity.GetComponent<Deity>();
            if (deityComponent != null)
            {
                deityFound = true;
                currentDeityInDialogue = deityComponent;
            }
        }

        inRange = deityFound ? true : false;
        dialogueButton.SetActive(inRange);

        CheckPlayerInput();
    }

    private void CheckPlayerInput()
    {
        if (Input.GetKeyDown(dialogueInteraction) && inRange && currentDeityInDialogue.IsReadyForDialogue)
        {
            DialogueFired?.Invoke(currentDeityInDialogue);
        }
    }
}

