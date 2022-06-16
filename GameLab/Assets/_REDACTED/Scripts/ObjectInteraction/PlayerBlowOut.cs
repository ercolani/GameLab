using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static AudioController;

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

    /// <summary>
    /// The particles of the wind puff.
    /// </summary>
    [SerializeField]
    private GameObject _particles;

    [SerializeField]
    private float _blowDelay;

    private bool _onCooldown = false;

    [SerializeField]
    private float _cooldown = 2f;


    private void Update()
    {
        if (Input.GetKeyDown(_interactInput) && !_onCooldown)
        {
            StartCoroutine(Blow());
        }
    }

    private IEnumerator Blow()
    {
        AudioController.PlaySound("Blow");
        StartCoroutine(BlowOnCooldown());
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
                    yield return new WaitForSeconds(_blowDelay);
                    flameComponent.ToggleFlame(false);
                    break;
                }
            }
        }
    }

    private IEnumerator BlowOnCooldown()
    {
        _particles.SetActive(true);
        _onCooldown = true;
        yield return new WaitForSeconds(_cooldown);
        _particles.SetActive(false);
        _onCooldown = false;
    }



}
