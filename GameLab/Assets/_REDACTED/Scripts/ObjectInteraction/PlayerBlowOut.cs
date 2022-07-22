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

    [SerializeField]
    private BlowCollider _blowCollider;

    [SerializeField]
    private AudioController audioController;

    private void Update()
    {
        if (Input.GetKeyDown(_interactInput) && !_onCooldown)
        {
            StartCoroutine(Blow());
        }
    }

    private IEnumerator Blow()
    {
        audioController.PlaySound("Blow");
        StartCoroutine(BlowOnCooldown());
        if (_blowCollider._currentTorch == null)
        {
            yield break;
        }
        if (_blowCollider._currentTorch.FlameActive)
        {
            yield return new WaitForSeconds(_blowDelay);
            _blowCollider._currentTorch.ToggleFlame(false);
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
