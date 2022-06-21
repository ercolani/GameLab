using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles the movement of stalking the player.
/// </summary>
public class StalkPlayer : MonoBehaviour
{
    /// <summary>
    /// The target player.
    /// </summary>
    [SerializeField]
    private Transform _player;

    /// <summary>
    /// The speed of the movement.
    /// </summary>
    [SerializeField]
    private float _originalSpeed;

    /// <summary>
    /// The speed of the movement.
    /// </summary>
    private float _speed;

    /// <summary>
    /// The offset for where this objects movement. (Ex if this object should fly over the player then the Y should be offsetted).
    /// </summary>
    [SerializeField]
    private Vector3 _offset;

    /// <summary>
    /// The range at which the stalker stops before a candle.
    /// </summary>
    [SerializeField]
    private float _candleRange;

    /// <summary>
    /// The particle systems of the stalker.
    /// </summary>
    [SerializeField]
    private ParticleSystem[] _particleSystems;

    [SerializeField]
    private GameObject _candles;

    /// <summary>
    /// The new destination for the stalking
    /// </summary>
    private Vector3 _newDestination;

    /// <summary>
    /// The timer for the changing of the destination.
    /// </summary>
    private float _newDestinationTimer;

    /// <summary>
    /// The time range for stalking.
    /// </summary>
    [SerializeField]
    private Vector2 _stalkingFrequency;

    /// <summary>
    /// The time range for stalking.
    /// </summary>
    [SerializeField]
    private Vector2 _stalkingTimeRange;

    private float _currentStalkingTime;

    private float _currentStalkingFrequency;

    /// <summary>
    /// The timer for the changing of the destination.
    /// </summary>
    private float _stalkingTimer;

    /// <summary>
    /// Whether the stalker should follow the player.
    /// </summary>
    private bool _stalkingPlayer = true;

    private void Start()
    {
        _speed = _originalSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        _newDestinationTimer += Time.deltaTime;

        if (_stalkingPlayer == true)
        {
            _stalkingTimer += Time.deltaTime;

            if (_newDestinationTimer >= 5)
            {
                if (_stalkingPlayer)
                {
                    _newDestination = new Vector3(_player.position.x + Random.Range(-5f, 5f), _player.position.y, _player.position.z + Random.Range(-5f, 5f));
                }
                _newDestinationTimer = 0;
            }

            float distance = Vector3.Distance(this.transform.position, _player.position);
            if (distance >= 35)
            {
                _speed = 0.0f;
            }
            else
            {
                _speed = _originalSpeed;
            }
        }

        MoveToDestination();
        RotateTowardsTarget();
        CheckCandleRange();

        if(_stalkingTimer >= _currentStalkingTime)
        {
            _stalkingPlayer = false;
            _speed = 0.2f;
            StartCoroutine(StopStalking(_currentStalkingFrequency));
            _currentStalkingTime = Random.Range(_stalkingTimeRange.x, _stalkingTimeRange.y);
            _currentStalkingFrequency = Random.Range(_stalkingFrequency.x, _stalkingFrequency.y);
            _stalkingTimer = 0;
        }
    }

    /// <summary>
    /// Moves the stalker to the new destination.
    /// </summary>
    private void MoveToDestination()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 targetPos = new Vector3(_newDestination.x + _offset.x, _newDestination.y + _offset.y, _newDestination.z + _offset.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, _speed);
    }

    /// <summary>
    /// Rotates this object towards the player.
    /// </summary>
    private void RotateTowardsTarget()
    {
        Vector3 _direction = (_player.position - transform.position).normalized;

        //create the rotation we need to be in to look at the target
        Quaternion _lookRotation = Quaternion.LookRotation(_direction);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 1);
    }

    private void CheckCandleRange()
    {
        //float distance = Vector3.Distance(transform.position, _candles.transform.position);
        //Debug.LogError(distance);
        //if(distance <= _candleRange)
        //{
        //    _newDestination = this.transform.position;
        //    foreach (ParticleSystem particles in _particleSystems)
        //    {
        //        particles.Stop();
        //    }
        //}
    }

    private IEnumerator StopStalking(float time)
    {
        this.gameObject.GetComponent<Animator>().Play("HideShadow");
        _newDestination = new Vector3(this.transform.position.x, this.transform.position.y - 18, this.transform.position.z);
        yield return new WaitForSeconds(time);
        _stalkingPlayer = true;
        this.gameObject.GetComponent<Animator>().Play("ShadowPerson");
    }

    private IEnumerator Test()
    {
        yield return new WaitForSeconds(4f);
        this.GetComponent<ParticleSystem>().Stop();
    }
}
