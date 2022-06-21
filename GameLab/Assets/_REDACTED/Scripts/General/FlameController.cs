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
    /// Caches the flame game object.
    /// </summary>
    [SerializeField]
    private AudioSource _flameAudio;

    /// <summary>
    /// Whether the flame is active or not.
    /// </summary>
    [SerializeField]
    private bool _flameActive;

    /// <summary>
    /// Whether the flame is active or not.
    /// </summary>
    [SerializeField]
    private bool _canBeBlownOut = true;

    /// <summary>
    /// A getter for whether the flame can be blown out or not.
    /// </summary>
    public bool CanBeBlownOut => _canBeBlownOut;

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
    }

    /// <summary>
    /// Called by an external class to toggle the state of the flame.
    /// </summary>
    public void ToggleFlame(bool state)
    {
        if (_canBeBlownOut)
        {
            _flameActive = state;
            _flame.SetActive(state);
            _flameAudio.enabled = state;
            FlameToggled?.Invoke(this);

            if (this.gameObject.TryGetComponent(out InteractiveObject interactiveComponent))
            {
                interactiveComponent.SetInteractivity(state);
            }
        }
    }
    
    public void ToggleFlame(bool state, bool invokeEvent)
    {
        if (_canBeBlownOut)
        {
            _flameActive = state;
            _flame.SetActive(state);
            _flameAudio.enabled = state;

            if (invokeEvent)
            {
                FlameToggled?.Invoke(this);
            }
            if (this.gameObject.TryGetComponent(out InteractiveObject interactiveComponent))
            {
                interactiveComponent.SetInteractivity(state);
            }
        }
    }

    /// <summary>
    /// Called by an external class to toggle the state of the flame.
    /// </summary>
    public void ForceToggleFlame(bool state)
    {
        _flameActive = state;
        _flame.SetActive(state);
        _flameAudio.enabled = state;
        if (this.gameObject.TryGetComponent(out InteractiveObject interactiveComponent))
        {
            interactiveComponent.SetInteractivity(state);
        }
    }

    public void ToggleCanBeBlownOut(bool state)
    {
        _canBeBlownOut = state;
    }
}
