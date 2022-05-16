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
    private GameObject _heldObj;

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
        if (pickObj.GetComponent<Rigidbody>())
        {
            Rigidbody objRig = pickObj.GetComponent<Rigidbody>();
            objRig.useGravity = false;

            objRig.transform.parent = holdParent;
            _heldObj = pickObj;
        }
    }

    /// <summary>
    /// Drops the held object.
    /// </summary>
    private void DropObject()
    {
        Rigidbody heldRig = _heldObj.GetComponent<Rigidbody>();
        _heldObj.GetComponent<Rigidbody>().useGravity = true;
         heldRig.drag = 1;
        _heldObj.transform.parent = null;
        _heldObj = null;
    }

    /// <summary>
    /// Makes the overlap sphere near the player to interact with objects in front of you.
    /// </summary>
    private void CheckForInteractibles()
    {
        //makes the overlap sphere in front of the player so that items that are not in the field of view are not picked up
        Collider[] _objects = Physics.OverlapSphere(transform.position + (transform.forward * pickUpRange), pickUpRange / 2);

        _objects.OrderBy(obj => (transform.position - obj.transform.position).sqrMagnitude).ToArray(); //sort by proximity
        foreach (Collider obj in _objects)
        {
            //if the player is holding an object
            if (_heldObj != null)
            {
                if (obj.gameObject.CompareTag("Holder"))
                {
                    //set the transform of the held object to what the holder's transform parent is to put the object in the right place
                    return;
                }
                DropObject();
            }
            else
            {
                if (obj.gameObject.CompareTag("Yoinkable"))
                {
                    PickUpObject(obj.gameObject);
                }
            }
        }
    }
}
