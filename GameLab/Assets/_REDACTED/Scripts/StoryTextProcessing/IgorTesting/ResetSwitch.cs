using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Handles the behavior of the reset switch for puzzles.
/// </summary>
public class ResetSwitch : MonoBehaviour
{
    /// <summary>
    /// Caches the flame game object.
    /// </summary>
    [SerializeField]
    private GameObject _flame;

    /// <summary>
    /// A list of all the torches to reset in the puzzle.
    /// </summary>
    [SerializeField]
    private List<Torch> _torchesToReset = new List<Torch>();

    /// <summary>
    /// Whether the reset switch is active or not.
    /// </summary>
    [SerializeField]
    private bool _active;

    /// <summary>
    /// A getter for whether the reset switch is active or not.
    /// </summary>
    public bool Active => _active;

    /// <summary>
    /// Invoked when the reset switch is toggled on or off.
    /// </summary>
    private event Action<bool> ToggleActivate;

    private void Awake()
    {
        _flame = transform.Find("Flame").gameObject;
    }
    private void OnEnable()
    {
        ToggleActivate += LightFlame;
    }

    private void OnDisable()
    {
        ToggleActivate -= LightFlame;

    }

    /// <summary>
    /// Called by the candle to activate the rest switch. After activating, it deactivates with a coroutine.
    /// </summary>
    public void ActivateTorch()
    {
        if (!_active)
        {
            _active = true;
            ToggleActivate?.Invoke(true);
            StartCoroutine(DeactivateTorchCo());
        }
    }

    /// <summary>
    /// Deactivates the reset switch after a set amount of time. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeactivateTorchCo()
    {
        yield return new WaitForSeconds(0.5f);
        ToggleActivate?.Invoke(false);
    }

    /// <summary>
    /// Handles what happens after the rest switch is activated. Deactivates all torches relevant to the puzzle.
    /// </summary>
    private void LightFlame(bool value)
    {
        _flame.SetActive(value);
        foreach (Torch torch in _torchesToReset)
        {
            torch.DeactivateTorch();
        }
    }

    /// <summary>
    /// Handles what happens after the reset switch is deactivated.
    /// </summary>
    private void PutOutFlame()
    {
        _flame.SetActive(false);
    }
}
