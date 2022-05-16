using UnityEngine;
using System;

public class InteractiveObject : MonoBehaviour
{
    /// <summary>
    /// The range at which the player can pick up objects.
    /// </summary>
    private ParticleSystem _particleEffect;

    /// <summary>
    /// The object that is being held.
    /// </summary>
    public Action Interacted;

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
    private bool _IsInteractive;
}
