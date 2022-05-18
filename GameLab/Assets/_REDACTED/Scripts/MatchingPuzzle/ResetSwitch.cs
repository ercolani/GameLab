using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Handles the behavior of the reset switch for puzzles.
/// </summary>
public class ResetSwitch : FlameController
{ 
    /// <summary>
    /// Invoked when the reset switch is toggled on.
    /// </summary>
    public event Action Reset;

    private void Start()
    {
        ToggleFlame(false);
    }

    /// <summary>
    /// The coroutine for deactivating the flame after player input.
    /// </summary>
    private IEnumerator DeactivateFlameCoroutine(float time)
    {
        ToggleFlame(false);
        Reset?.Invoke();
        yield return new WaitForSeconds(time);
        ToggleFlame(true);
    }
}