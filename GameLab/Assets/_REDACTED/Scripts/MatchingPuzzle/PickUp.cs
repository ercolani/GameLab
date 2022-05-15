using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    /// <summary>
    /// The place where the object being held will be placed in the hand.
    /// </summary>
    [SerializeField]
    private Transform _objectHolder;

    /// <summary>
    /// From how far away the player can pick up objects.
    /// </summary>
    [SerializeField]
    private float _pickRange;

    /// <summary>
    /// The key the player pressed to pick up items.
    /// </summary>
    [SerializeField]
    private KeyCode _playerInput;

    private void Update()
    {
        CheckPlayerInput();
    }

    /// <summary>
    /// Checks to see if the player is pressing the input key.
    /// </summary>
    private void CheckPlayerInput()
    {
        if (Input.GetKey(_playerInput))
        {
            this.transform.SetParent(_objectHolder);
        }
    }
}
