using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the perspective puzzle.
/// </summary>
public class PerspectivePuzzle : MonoBehaviour
{
    /// <summary>
    /// Caches the anansi area manager object.
    /// </summary>
    [SerializeField]
    private PuzzleManager puzzleManager;

    /// <summary>
    /// A list of the correct torch states.
    /// </summary>
    [SerializeField]
    private List<bool> _correctTorchStates = new List<bool>();

    /// <summary>
    /// A list of all the torches used for the perspective puzzle.
    /// </summary>
    [SerializeField]
    private List<Torch> _torches = new List<Torch>();

    private void Update()
    {
        //checks the states of all the torches, and if they are all correct, marks the puzzle as complete
        for (int i = 0; i < _correctTorchStates.Count; i++) 
        {
            if (_torches[i].Active != _correctTorchStates[i])
            {
                return;
            }

            puzzleManager.CompletePuzzle();
        }
    }
}
