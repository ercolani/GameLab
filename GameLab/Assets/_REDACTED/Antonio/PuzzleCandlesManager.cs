using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gaia;

public class PuzzleCandlesManager : MonoBehaviour
{
    /// <summary>
    /// The candles that form the puzzle.
    /// </summary>
    [SerializeField]
    private List<LightUp> _candles;

    [SerializeField]
    private PWS_WaterSystem _waterSystem;

    private void OnEnable()
    {
        foreach (LightUp candle in _candles)
        {
            candle.Activated += CheckCompleted;
        }
    }

    private void CheckCompleted()
    {
        foreach (LightUp candle in _candles)
        {
            Debug.LogError(candle.Active);
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

    private IEnumerator OpenSeaAnimation()
    {
        for(int i= 0; i<60; i++)
        {
            _waterSystem.SeaLevel = _waterSystem.SeaLevel - 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
