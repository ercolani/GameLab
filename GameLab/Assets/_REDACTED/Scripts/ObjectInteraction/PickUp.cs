using UnityEngine;

/// <summary>
/// Class that handles picking up objects.
/// </summary>
public class PickUp : MonoBehaviour
{
    /// <summary>
    /// The target position of the object that holds the picked up item.
    /// </summary>
    [SerializeField]
    private Transform objectHolder;

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
        // Makes the object follow the position of this object when being held.
        if (beingHeld)
        {
            this.transform.position = objectHolder.position;
        }
        // Checks if the player has pressed the E key to pick up an object, will work depending on the PickRange.
        if (Input.GetKeyDown(KeyCode.E) && !beingHeld && Vector3.Distance(this.transform.position, objectHolder.position) < pickRange)
        {
            //GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().useGravity = false;
            beingHeld = true;
            this.transform.parent = objectHolder;
            this.transform.position = objectHolder.position;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
        // Checks if the player pressed the E key and lets go of an object.
        else if(Input.GetKeyDown(KeyCode.E) && beingHeld)
        {
            //GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
            this.transform.parent = null;
            beingHeld = false;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
