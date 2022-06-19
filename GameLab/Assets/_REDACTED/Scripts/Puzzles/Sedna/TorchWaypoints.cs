using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchWaypoints : MonoBehaviour
{
    /// <summary>
    /// The list of waypoints around the torch used to guide the fish.
    /// </summary>
    [SerializeField]
    private Transform[] _waypoints;

    /// <summary>
    /// The correct waypoint that was that the fish has to move towards when the torch is turned off for the fish to proceed.
    /// </summary>
    [SerializeField]
    private int _correctIndex;

    /// <summary>
    /// The list of waypoints around the torch used to guide the fish.
    /// </summary>
    public Transform[] Waypoints
    {
        get => _waypoints;
        set => _waypoints = value;
    }

    /// <summary>
    /// The correct waypoint that was that the fish has to move towards when the torch is turned off for the fish to proceed.
    /// </summary>
    public int CorrectIndex => _correctIndex;
}
