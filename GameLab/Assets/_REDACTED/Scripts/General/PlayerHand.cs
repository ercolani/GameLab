using UnityEngine;
using System.Linq;

/// <summary>
/// Class that handle the hand of the player, it is responsible for picking up stuff and holding items.
/// </summary>
public class PlayerHand : MonoBehaviour
{
    /// <summary>
    /// The input key that has to be checked to interact with the object.
    /// </summary>
    [SerializeField]
    private KeyCode _interactInput;

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
    private GameObject _objectInHand = null;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_interactInput))
        {
            CheckForInteractibles();
        }
    }
 
    /// <summary>
    /// Picks up an object.
    /// </summary>
    /// <param name="pickObj"></param>
    private void PickUpObject(GameObject pickObj)
    {
        _objectInHand = pickObj;
        pickObj.transform.rotation = Quaternion.identity;
        pickObj.transform.position = holdParent.transform.position;
        if (pickObj.TryGetComponent(out Rigidbody body))
        {
            body.useGravity = false;
            body.transform.parent = holdParent;
            body.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    /// <summary>
    /// Drops the held object.
    /// </summary>
    private void DropObject()
    {
        if (_objectInHand.TryGetComponent(out Rigidbody body))
        {
            body.useGravity = true;
            body.constraints = RigidbodyConstraints.None;
        }
        _objectInHand.transform.parent = null;
        _objectInHand = null;
    }

    /// <summary>
    /// Makes the overlap sphere near the player to interact with objects in front of you.
    /// </summary>
    private void CheckForInteractibles()
    {
        //makes the overlap sphere in front of the player so that items that are not in the field of view are not picked up
        Collider[] objects = Physics.OverlapSphere(transform.position + (transform.forward), pickUpRange/2);
        objects.OrderBy(obj => (transform.position - obj.transform.position).sqrMagnitude).ToArray(); //sort by proximity
        foreach (Collider obj in objects)
        {
            //if the player is holding an object
            if (_objectInHand == null)
            {
                if (obj.gameObject.CompareTag("PickUp"))
                {
                    InteractiveObject interactiveComponent = obj.GetComponent<InteractiveObject>();
                    if (interactiveComponent.IsInteractive)
                    {
                        obj.GetComponent<InteractiveObject>().Interact();
                        PickUpObject(obj.gameObject);
                        return;
                    }
                }
            }
            else
            {
                if (obj.gameObject.CompareTag("Holder"))
                {
                    if (obj.TryGetComponent(out ObjectHolder objectHolder))
                    {
                        if (!objectHolder.HasObject())
                        {
                            objectHolder.SetHeldObject(_objectInHand);
                            _objectInHand = null;
                            return;
                        }
                    }
                }
            }
        }
        if(_objectInHand != null)
        {
            DropObject();
        }
    }

    /// <summary>
    /// Draws the range for the pickup.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position + (transform.forward), pickUpRange/2);
    }
}
