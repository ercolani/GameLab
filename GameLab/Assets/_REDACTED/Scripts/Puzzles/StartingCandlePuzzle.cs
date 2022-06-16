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
    private PWS_WaterSystem _waterSystem;

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
        for (int i = 0; i < 50; i++)
        {
            _waterSystem.SeaLevel = _waterSystem.SeaLevel - 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(20);

        for (int i = 0; i < 50; i++)
        {
            _waterSystem.SeaLevel = _waterSystem.SeaLevel + 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
