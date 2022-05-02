using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Handles torch behaviour.
/// </summary>
public class Torch : MonoBehaviour
{
    /// <summary>
    /// Caches the flame game object.
    /// </summary>
    [SerializeField]
    private GameObject _flame;

    /// <summary>
    /// Whether the torch is active or not.
    /// </summary>
    [SerializeField]
    private bool _active;

    /// <summary>
    /// A getter for whether the torch is active or not.
    /// </summary>
    public bool Active => _active;

    /// <summary>
    /// Invoked when torch is activated.
    /// </summary>
    private event Action Activate;

    /// <summary>
    /// Invoked when torch is deactivated.
    /// </summary>
    private event Action Deactivate;

    private void Awake()
    {
        _flame = transform.Find("Flame").gameObject;
    }

    private void OnEnable()
    {
        Activate += LightFlame;
        Deactivate += PutOutFlame;
    }

    private void OnDisable()
    {
        Activate -= LightFlame;
        Deactivate -= PutOutFlame;
    }

    /// <summary>
    /// Called by the candle to activate torch.
    /// </summary>
    public void ActivateTorch()
    {
        if (!_active)
        {
            _active = true;
            Activate?.Invoke();
        }
    } 
    
    /// <summary>
    /// Called by the player to deactivate torch.
    /// </summary>
    public void DeactivateTorch()
    {
        if (_active)
        {
            _active = false;
            Deactivate?.Invoke();
        }
    }

    /// <summary>
    /// Handles what happens after the torch is activated.
    /// </summary>
    private void LightFlame()
    {
        _flame.SetActive(true);
    }

    /// <summary>
    /// Handles what happens after the torch is deactivated.
    /// </summary>
    private void PutOutFlame()
    {
        _flame.SetActive(false);
    }

}
