using UnityEngine;

/// <summary>
/// Class that manages the behaviour of the final area of the game.
/// </summary>
public class FinalArea : MonoBehaviour
{
    /// <summary>
    /// The holders for the candle deities.
    /// </summary>
    [SerializeField]
    private ObjectHolder[] _objectHolders;

    /// <summary>
    /// Whether the player candle is allowed in the altar or not.
    /// </summary>
    private bool _playerCandleAllowed;

    private void OnEnable()
    {
        foreach (ObjectHolder objectHolder in _objectHolders)
        {
            objectHolder.ObjectPlaced += CheckPuzzleCompletion;
        }
    }

    private void CheckPuzzleCompletion()
    {
        foreach(ObjectHolder objectHolder in _objectHolders)
        {
            if(objectHolder.HeldObject == null)
            {
                return;
            }
            else if (objectHolder.HeldObject.tag != "DeityCandle" || objectHolder.HeldObject.tag != "DeityCandle")
            {
                return;
            }
        }
        _playerCandleAllowed = true;
        Debug.LogError("The Player Has Won The Game");
    }
}
