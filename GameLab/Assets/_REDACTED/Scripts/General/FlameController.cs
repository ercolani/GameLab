using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(InteractiveObject))]
public class FlameController : MonoBehaviour
{
    /// <summary>
    /// Caches the flame game object.
    /// </summary>
    [SerializeField]
    private GameObject _flame;

    /// <summary>
    /// Whether the flame is active or not.
    /// </summary>
    [SerializeField]
    private bool _flameActive;

    /// <summary>
    /// Whether the flame is active or not.
    /// </summary>
    public event Action<FlameController> FlameToggled;

    /// <summary>
    /// The interactable component of this script.
    /// </summary>
    private InteractiveObject _interactiveObject;

    /// <summary>
    /// Keeps track if the flame is burning or not.
    /// </summary>
    public bool FlameActive => _flameActive;

    /// <summary>
    /// Sets reference to the interactive object component.
    /// </summary>
    private void Awake()
    {
        _interactiveObject = this.GetComponent<InteractiveObject>();
    }

    private void OnEnable()
    {
        _flame.SetActive(_flameActive);
        _interactiveObject.Interacted += ()=> ToggleFlame(!_flameActive);
    }

    /// <summary>
    /// Called by an external class to toggle the state of the flame.
    /// </summary>
    public void ToggleFlame(bool state)
    {
        _flameActive = state;
        _flame.SetActive(state);
        FlameToggled?.Invoke(this);
    }
}
