using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that manages the behaviour of the final area of the game.
/// </summary>
public class FinalArea : MonoBehaviour
{
    /// <summary>
    /// The holders for the candle deities.
    /// </summary>
    private ObjectHolder[] _objectHolders;

    private void OnEnable()
    {
        foreach (ObjectHolder objectHolder in _objectHolders)
        {
            objectHolder.ObjectPlaced += CheckPuzzleCompletion;
        }
    }

    private void OnDisable()
    {
        
    }

    private void CheckPuzzleCompletion()
    {
        foreach(ObjectHolder objectHolder in _objectHolders)
        {
        }
    }

    /// <summary>
    /// Allows the player to drop the candleand place it on the altar.
    /// </summary>
    private void AllowCandleDrop()
    {

    }
}
