using System;
using UnityEngine;

/// <summary>
/// Class that handles a puzzle stool.
/// </summary>
[RequireComponent(typeof(InteractiveObject))]
public class ObjectHolder : MonoBehaviour
{
    /// <summary>
    /// The transform for where the held object should be placed.
    /// </summary>
    [SerializeField]
    private Transform _handle;

    /// <summary>
    /// Reference to the object being held by the holder.
    /// </summary>
    [SerializeField]
    private GameObject _heldObject;

    /// <summary>
    /// Event for when an object is places inside the stool.
    /// </summary>
    public event Action ObjectPlaced;

    /// <summary>
    /// Reference to the object being held by the holder.
    /// </summary>
    public GameObject HeldObject => _heldObject;

    private void Awake()
    {
        if(_heldObject != null)
        {
            _heldObject.transform.SetParent(_handle);
            _heldObject.transform.position = _handle.transform.position;

            if (_heldObject.TryGetComponent(out Rigidbody body))
            {
                body.useGravity = false;
                body.constraints = RigidbodyConstraints.FreezeAll;
            }

            if (_heldObject.TryGetComponent(out InteractiveObject interactiveObject))
            {
                interactiveObject.Interacted += RemoveHeldObject;
            }
        }
    }

    /// <summary>
    /// Sets the object that the puzzle stool 
    /// </summary>
    public void SetHeldObject(GameObject heldObj)
    {
        if(_heldObject == null)
        {
            _heldObject = heldObj;
            _heldObject.transform.SetParent(_handle);
            _heldObject.transform.position = _handle.transform.position;

            if (heldObj.TryGetComponent(out Rigidbody body))
            {
                body.useGravity = false;
                body.constraints = RigidbodyConstraints.FreezeAll;
            }

            if (_heldObject.TryGetComponent(out InteractiveObject interactiveObject))
            {
                interactiveObject.Interacted += RemoveHeldObject;
            }

            ObjectPlaced?.Invoke();
        }
    }

    /// <summary>
    /// Removes the reference to the held object.
    /// </summary>
    public void RemoveHeldObject()
    {
        if (_heldObject.TryGetComponent(out InteractiveObject interactiveObject))
        {
            interactiveObject.Interacted -= RemoveHeldObject;
            _heldObject = null;
        }
    }

    /// <summary>
    /// Whether the holder is holding an object.
    /// </summary>
    /// <returns></returns>
    public bool HasObject()
    {
        return (_heldObject != null);
    }
}
