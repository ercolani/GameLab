using UnityEngine;
using System.Collections.Generic;

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
    /// The lights that turn off when Sedna's candle is placed.
    /// </summary>
    [SerializeField]
    private List<GameObject> sednaLights = new List<GameObject>();

    /// <summary>
    /// The lights that turn off when Mokosh's candle is placed.
    /// </summary>
    [SerializeField]
    private List<GameObject> mokoshLights = new List<GameObject>();

    /// <summary>
    /// The lights that turn off when Ekeko's candle is placed.
    /// </summary>
    [SerializeField]
    private List<GameObject> ekekoLights = new List<GameObject>();

    /// <summary>
    /// The lights that turn off when Anansi's candle is placed.
    /// </summary>
    [SerializeField]
    private List<GameObject> anansiLights = new List<GameObject>();

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
        foreach (ObjectHolder objectHolder in _objectHolders)
        {
            if (objectHolder.HeldObject != null && objectHolder.HeldObject.tag == "DeityCandle")
            {
                switch (objectHolder.HeldObject.name)
                {
                    case "AnansiCandle":
                        TurnOffLights(anansiLights);
                        break;

                    case "EkekoCandle":
                        TurnOffLights(ekekoLights);
                        break;

                    case "SednaCandle":
                        TurnOffLights(sednaLights);
                        break;

                    case "MokoshCandle":
                        TurnOffLights(mokoshLights);
                        break;
                }
            }
            else
            {
                return;
            }

            _playerCandleAllowed = true;
            Debug.LogError("The Player Has Won The Game");
        }
    }

    private void TurnOffLights(List<GameObject> lightsToTurnOff)
    {
        foreach (GameObject light in lightsToTurnOff)
        {
            light.SetActive(false);
        }
    }
}
