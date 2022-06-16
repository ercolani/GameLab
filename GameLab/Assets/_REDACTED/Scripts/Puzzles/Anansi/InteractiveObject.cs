using UnityEngine;
using System;

public class InteractiveObject : MonoBehaviour
{
    /// <summary>
    /// The particles that emit from interactible objects.
    /// </summary>
    [SerializeField]
    private ParticleSystem _particleEffect;

    /// <summary>
    /// The object that is being held.
    /// </summary>
    public Action Interacted;

    /// <summary>
    /// The object that is being held with the object as a parameter.
    /// </summary>
    public Action<InteractiveObject> InteractedWithReturn;

    /// <summary>
    /// Whether the object can be interacted with.
    /// </summary>
    public bool IsInteractive => _IsInteractive;

    /// <summary>
    /// Whether the object is in range to be interacted with.
    /// </summary>
    private bool _inRange;

    /// <summary>
    /// Whether the object can be interacted with.
    /// </summary>
    [SerializeField]
    private bool _IsInteractive = true;

    /// <summary>
    /// Whether the object can be picked up.
    /// </summary>
    [SerializeField]
    private bool _canBePickedUp = true;


    /// <summary>
    /// Whether the object can be picked up.
    /// </summary>
    public bool CanBePickedUp => _canBePickedUp;

    public void Interact()
    {
        Interacted?.Invoke();
        InteractedWithReturn?.Invoke(this);
    }

    public void SetInteractivity(bool state)
    {
        _IsInteractive = state;
        if (_particleEffect != null)
        {
            _particleEffect.gameObject.SetActive(state);
        }
    }
}
