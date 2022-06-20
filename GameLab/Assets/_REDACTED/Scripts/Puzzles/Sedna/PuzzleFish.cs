using UnityEngine;

/// <summary>
/// The manager class for the fish that moves around the torches in the fish puzzle.
/// </summary>
public class PuzzleFish : MonoBehaviour
{
    /// <summary>
    /// The set of waypoints that the fish should follow.
    /// </summary>
    [SerializeField]
    private Transform[] _currentWaypoints;

    /// <summary>
    /// The speed at which the fish should move.
    /// </summary>
    [SerializeField]
    private float _movementSpeed;

    /// <summary>
    /// The speed at which the fish should rotate.
    /// </summary>
    [SerializeField]
    private float _rotationSpeed = 0.1f;

    /// <summary>
    /// Wether or not the fish should follow the waypoint system.
    /// </summary>
    public bool FollowWaypoints
    {
        get => _followWaypoints;
        set => _followWaypoints = value;
    }

    /// <summary>
    /// The current
    /// </summary>
    public int TargetIndex => _targetIndex;

    /// <summary>
    /// The current torch that the fish follows.
    /// </summary>
    private int _currentTorchIndex;

    /// <summary>
    /// The current
    /// </summary>
    private int _targetIndex = 1;

    /// <summary>
    /// Wether or not the fish should follow the waypoint system.
    /// </summary>
    private bool _followWaypoints;

    /// <summary>
    /// Wether or not the fish should follow the waypoint system.
    /// </summary>
    public float MovementSpeed
    {
        get => _movementSpeed;
        set => _movementSpeed = value;
    }

    /// <summary>
    /// The current torch that the fish follows.
    /// </summary>
    public int CurrentTorchIndex => _currentTorchIndex;

    private void Update()
    {
        if(FollowWaypoints == true)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, _currentWaypoints[_targetIndex].position, _movementSpeed * Time.deltaTime);

            //transform.LookAt(_currentWaypoints[_targetIndex].position);

            Vector3 direction = _currentWaypoints[_targetIndex].position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);

            if (Vector3.Distance(this.transform.position, _currentWaypoints[_targetIndex].position) < 3)
            {
                if (_targetIndex + 1 >= _currentWaypoints.Length)
                {
                    _targetIndex = 0;
                }
                else
                {
                    _targetIndex++;
                }
            }
        }
        else
        {
            transform.position += transform.forward * _movementSpeed * Time.deltaTime;
        }
    }

    /// <summary>
    /// Sets the references to the new waypoints for the fish.
    /// </summary>
    /// <param name="_waypoints">The new set of waypoints</param>
    /// <param name="startWaypoint">The starting index</param>
    public void UpdateWaypoints(Transform[] _waypoints, int torchIndex)
    {
        _currentWaypoints = _waypoints;
        _targetIndex = 0;
        _currentTorchIndex = torchIndex;
    }
}
