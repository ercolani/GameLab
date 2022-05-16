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
    private Transform _handle;

    /// <summary>
    /// Event for when an object is places inside the stool.
    /// </summary>
    public event Action ObjectPlaced;

    /// <summary>
    /// Reference to the object being held by the holder.
    /// </summary>
    public GameObject HeldObject;

    /// <summary>
    /// Reference to the object being held by the holder.
    /// </summary>
    private GameObject _heldObject;

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
            if (_heldObject.TryGetComponent(out InteractiveObject interactiveObject))
            {
                interactiveObject.Interacted += RemoveHeldObject;
            }
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
}
