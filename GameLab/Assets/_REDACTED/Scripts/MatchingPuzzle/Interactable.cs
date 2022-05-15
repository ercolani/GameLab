using UnityEngine;
using System;

public class Interactable : MonoBehaviour
{
    /// <summary>
    /// The range at which the player can pick up objects.
    /// </summary>
    public float pickUpRange = 5;
    
    /// <summary>
    /// The parent holding the object.
    /// </summary>
    public Transform holdParent;

    /// <summary>
    /// The object that is being held.
    /// </summary>
    private GameObject _heldObj;

    /// <summary>
    /// The object that is being held.
    /// </summary>
    public Action Interacted;

    /// <summary>
    /// Whether the object can be interacted with.
    /// </summary>
    private bool _interactive;

    public bool Interactive => _interactive;

    // Update is called once per frame
    void Update()
    {
        if (Interactive && Input.GetKeyDown(KeyCode.E))
        {
            if(_heldObj == null)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position, transform.InverseTransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    PickupObject(hit.transform.gameObject);
                }
            }
            //    else
            //    {
            //        DropObject();
            //    }
        }
    }

    private void MoveObject()
    {
        if(Vector3.Distance (_heldObj.transform.position, holdParent.position) > 0.1f)
        {
            Vector3 moveDirection = (holdParent.position - _heldObj.transform.position);
        }
    }

    private void PickupObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {
            Rigidbody objRig = pickObj.GetComponent<Rigidbody>();
            objRig.useGravity = false;
            //objRig.drag = 10;

            objRig.transform.parent = holdParent;
            _heldObj = pickObj;
        }
    }

    private void DropObject()
    {
        Rigidbody heldRig = _heldObj.GetComponent<Rigidbody>();
        _heldObj.GetComponent<Rigidbody>().useGravity = true;
        heldRig.drag = 1;
        _heldObj.transform.parent = null;
        _heldObj = null;
    }
}
