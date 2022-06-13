using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerBlowOut : MonoBehaviour
{
    /// <summary>
    /// The input key that has to be checked to interact with the object.
    /// </summary>
    [SerializeField]
    private KeyCode _interactInput;

    /// <summary>
    /// The range at which the player can pick up objects.
    /// </summary>
    public float _blowOutRange = 2f;

    private void Update()
    {
        if (Input.GetKeyDown(_interactInput))
        {
            Blow();
        }
    }

    private void Blow()
    {
        //makes the overlap sphere in front of the player so that items that are not in the field of view are not picked up
        Collider[] objects = Physics.OverlapSphere(transform.position + (transform.forward), _blowOutRange / 2);
        objects.OrderBy(obj => (transform.position - obj.transform.position).sqrMagnitude).ToArray(); //sort by proximity
        foreach (Collider obj in objects)
        {
            //if the object is a torch
            FlameController flameComponent = obj.GetComponent<FlameController>();
            if (flameComponent != null)
            {
                if (flameComponent.FlameActive)
                {
                    flameComponent.ToggleFlame(false);
                    break;
                }
            }
        }
    }

}
