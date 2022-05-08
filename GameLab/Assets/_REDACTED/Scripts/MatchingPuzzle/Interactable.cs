using UnityEngine;
using System;

public class Interactable : MonoBehaviour
{
    /// <summary>
    /// The range at which the player can pick up objects.
    /// </summary>
    public float  = 5;
    
    /// <summary>
    /// The parent holding the object.
    /// </summary>
    public Transform holdParent;

    /// <summary>
    /// The object that is being held.
    /// </summary>
    private GameObject heldObj;

    /// <summary>
    /// The object that is being held.
    /// </summary>
    public Action Interacted;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(heldObj == null)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position, transform.InverseTransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    PickupObject(hit.transform.gameObject);
                    Debug.Log("tets");
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
        if(Vector3.Distance (heldObj.transform.position, holdParent.position) > 0.1f)
        {
            Vector3 moveDirection = (holdParent.position - heldObj.transform.position);
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
            heldObj = pickObj;
        }
    }

    private void DropObject()
    {
        Rigidbody heldRig = heldObj.GetComponent<Rigidbody>();
        heldObj.GetComponent<Rigidbody>().useGravity = true;
        heldRig.drag = 1;
        heldObj.transform.parent = null;
        heldObj = null;
    }
}
