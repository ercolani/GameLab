using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Handles puzzle completion tracking during the game.
/// </summary>
public class PuzzleManager : MonoBehaviour
{
    /// <summary>
    /// The total number of puzzles in the game.
    /// </summary>
    [SerializeField]
    private int _numberOfPuzzles;
    
    /// <summary>
    /// The total number of puzzles the player has completed.
    /// </summary>
    [SerializeField]
    private int _numberOfCompletedPuzzles;

    /// <summary>
    /// Caches the torchline manager to be able to control which ones light up.
    /// </summary>
    [SerializeField]
    private TorchlineManager torchlineManager;

    /// <summary>
    /// A list of all the puzzle areas in the game.
    /// </summary>
    [SerializeField]
    private List<GameObject> _puzzleAreas = new List<GameObject>();

    /// <summary>
    /// Invoked when the player completes a puzzle.
    /// </summary>
    private event Action CompletedPuzzle;

    private void OnEnable()
    {
        CompletedPuzzle += UnlockNextPuzzle;
    } 
    
    private void OnDisable()
    {
        CompletedPuzzle -= UnlockNextPuzzle;
    }
    private void Start()
    {
        torchlineManager.ActivateTorchline();
    }

    /// <summary>
    /// Called when the player completes a puzzle. For now, turns of the puzzle they completed and turns on the next one (if that is not the last one).
    /// </summary>
    private void UnlockNextPuzzle()
    {
        _puzzleAreas[_numberOfCompletedPuzzles].SetActive(false);
        _numberOfCompletedPuzzles++;
        if (_numberOfCompletedPuzzles == _numberOfPuzzles)
        {
            //end game
        } 
        else
        {
            _puzzleAreas[_numberOfCompletedPuzzles].SetActive(true);
        }
    }

    /// <summary>
    /// Called by the classes of each puzzle to notify the puzzle manager that a puzzle was completed.
    /// </summary>
    public void CompletePuzzle()
    {
        CompletedPuzzle?.Invoke();
    }
}
