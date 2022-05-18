using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    /// <summary>
    /// The transform to use as teleport reference.
    /// </summary>
    [SerializeField]
    private Transform _targetLocation;

    private void Awake()
    {
        this.transform.position = _targetLocation.position;
    }
}
