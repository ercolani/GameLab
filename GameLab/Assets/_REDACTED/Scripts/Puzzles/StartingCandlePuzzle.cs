using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gaia;

public class StartingCandlePuzzle : MonoBehaviour
{
    /// <summary>
    /// The candles that form the puzzle.
    /// </summary>
    [SerializeField]
    private List<LightUp> _candles;

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
        foreach (LightUp candle in _candles)
        {
            candle.Activated += CheckCompleted;
        }
    }

    /// <summary>
    /// Checks if every candle has been lit up.
    /// </summary>
    private void CheckCompleted()
    {
        foreach (LightUp candle in _candles)
        {
            if (!candle.Active)
            {
                return;
            }
        }
        foreach (LightUp candle in _candles)
        {
            candle.KeepActive = true;
        }
        StartCoroutine(OpenSeaAnimation());
    }

    /// <summary>
    /// The animation for lowering the sea level.
    /// </summary>
    private IEnumerator OpenSeaAnimation()
    {
        for(int i= 0; i<60; i++)
        {
            _waterSystem.SeaLevel = _waterSystem.SeaLevel - 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
