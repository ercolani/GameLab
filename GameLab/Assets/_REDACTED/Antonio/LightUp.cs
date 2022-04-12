using System;
using System.Collections;
using UnityEngine;

public class LightUp : MonoBehaviour
{
    /// <summary>
    /// The flame of the candle.
    /// </summary>
    [SerializeField]
    private GameObject _flame;

    /// <summary>
    /// The timer for the candle.
    /// </summary>
    [SerializeField]
    public float _timer = 7f;

    /// <summary>
    /// Invoked when the flame is activated.
    /// </summary>
    public event Action Activated;

    /// <summary>
    /// Wether the flame is active or not.
    /// </summary>
    [SerializeField]
    public bool Active => _active;

    /// <summary>
    /// Wether the flame is active or not.
    /// </summary>
    private bool _active;

    public bool KeepActive;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Candle")
        {
            if (!this._flame.activeSelf)
            {
                this._flame.SetActive(true);
                _active = true;
                Activated?.Invoke();
                StartCoroutine(CountDown());
            }
        }
    }

    private IEnumerator CountDown()
    {
        yield return new WaitForSeconds(_timer);
        if (!KeepActive)
        {
            _active = false;
            this._flame.SetActive(false);
        }
    }
}
