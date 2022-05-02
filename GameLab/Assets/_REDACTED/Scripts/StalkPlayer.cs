using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform _player;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private Vector3 _offset;

    [SerializeField]
    private float _candleRange;

    [SerializeField]
    private ParticleSystem[] _particleSystems;

    [SerializeField]
    private GameObject _candles;

    private Vector3 _newDestination;

    private float _newDestinationTimer;

    private bool _followPlayer = true;

    // Update is called once per frame
    void Update()
    {
        _newDestinationTimer += Time.deltaTime;
        if(_newDestinationTimer >= 5 && _followPlayer)
        {
            _newDestination = new Vector3(_player.position.x + Random.Range(-5f, 5f), _player.position.y , _player.position.z + Random.Range(-5f, 5f));
            _newDestinationTimer = 0;
        }
        MoveToDestination();
        RotateTowardsTarget();
        CheckCandleRange();
    }

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
        float distance = Vector3.Distance(transform.position, _candles.transform.position);
        Debug.LogError(distance);
        if(distance <= _candleRange)
        {
            _newDestination = this.transform.position;
            //foreach (ParticleSystem particles in _particleSystems)
            //{
            //    particles.Stop();
            //}
        }
    }

    private IEnumerator Test()
    {
        yield return new WaitForSeconds(4f);
        this.GetComponent<ParticleSystem>().Stop();
    }
}
