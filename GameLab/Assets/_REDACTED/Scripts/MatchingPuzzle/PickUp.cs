using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PuzzleTorch))]
public class PickUp : MonoBehaviour
{
    /// <summary>
    /// The place where the object being held will be placed in the hand.
    /// </summary>
    [SerializeField]
    private Transform objectHolder;

    /// <summary>
    /// From how far away the player can pick up objects.
    /// </summary>
    [SerializeField]
    private float pickRange;

    /// <summary>
    /// The key the player pressed to pick up items.
    /// </summary>
    [SerializeField]
    private KeyCode playerInput;

    /// <summary>
    /// Checks to see if the player is pressing the input key.
    /// </summary>
    private void CheckPlayerInput()
    {
        if (Input.GetKey(playerInput))
        {

        }
    }
}
