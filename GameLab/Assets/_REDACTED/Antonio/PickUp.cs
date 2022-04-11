using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    /// <summary>
    /// The target position when held.
    /// </summary>
    [SerializeField]
    private Transform target;

    /// <summary>
    /// The range at which the object can be picked up;
    /// </summary>
    [SerializeField]
    private float pickRange = 2;

    /// <summary>
    /// Whether the object is being held or not.
    /// </summary>
    private bool beingHeld = false;

    // Update is called once per frame
    void Update()
    {
        if (beingHeld)
        {
            this.transform.position = target.position;
        }
        if (Input.GetKeyDown(KeyCode.E) && !beingHeld && Vector3.Distance(this.transform.position, target.position) < pickRange)
        {
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().useGravity = false;
            beingHeld = true;
            this.transform.parent = target;
            this.transform.position = target.position;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
        else if(Input.GetKeyDown(KeyCode.E) && beingHeld)
        {
            GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
            this.transform.parent = null;
            beingHeld = false;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
