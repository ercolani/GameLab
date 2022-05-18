using UnityEngine;
using System.Linq;

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
    private GameObject _heldObj = null;

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
        _heldObj = pickObj;
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
        if (_heldObj.TryGetComponent(out Rigidbody body))
        {
            body.useGravity = true;
            body.constraints = RigidbodyConstraints.None;
        }
        _heldObj.transform.parent = null;
        _heldObj = null;
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
            if (_heldObj == null)
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
                    Debug.LogError(_heldObj == null);
                    ObjectHolder objHolder = obj.GetComponent<ObjectHolder>();
                    if (!objHolder.HasObject())
                    {
                        Debug.LogError(_heldObj == null);
                        objHolder.SetHeldObject(_heldObj);
                        Debug.LogError(_heldObj == null);
                        _heldObj = null;
                        return;
                    }
                }
            }
        }
        if(_heldObj != null)
        {
            Debug.LogError("drop");
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
