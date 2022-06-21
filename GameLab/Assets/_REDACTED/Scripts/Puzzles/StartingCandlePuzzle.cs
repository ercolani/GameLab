using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gaia;
using static AudioController;

public class StartingCandlePuzzle : MonoBehaviour
{
    /// <summary>
    /// The candles that form the puzzle.
    /// </summary>
    [SerializeField]
    private List<FlameController> _torches;

    /// <summary>
    /// The water system for Gaia.
    /// </summary>
    [SerializeField]
    private Animator _waterObject;

    /// <summary>
    /// Subscribing to event
    /// </summary>
    private void OnEnable()
    {
        foreach (FlameController torch in _torches)
        {
            torch.FlameToggled += CheckCompleted;
        }
    }

    /// <summary>
    /// Checks if every candle has been lit up.
    /// </summary>
    private void CheckCompleted(FlameController torchParameter)
    {
        foreach (FlameController torch in _torches)
        {
            if (torch.FlameActive)
            {
                return;
            }
        }

        StartCoroutine(OpenSeaAnimation());
    }

    /// <summary>
    /// The animation for lowering the sea level.
    /// </summary>
    private IEnumerator OpenSeaAnimation()
    {
        _waterObject.Play("LowerWater");
        yield return new WaitForSeconds(60);
        _waterObject.Play("RiseWater");
    }
}
