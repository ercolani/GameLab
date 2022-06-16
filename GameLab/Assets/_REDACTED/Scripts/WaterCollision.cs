using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private CharacterController _playerController;

    [SerializeField]
    private float _updateTime = 1;

    private GameObject _player;

    private Vector3 _savedPosition;

    private float _timer = 0;

    private bool _onGround = true;

    private void Update()
    {
        _timer += Time.deltaTime;
        if(_timer > _updateTime && _onGround)
        {
            _savedPosition = _player.transform.localPosition;
            _timer = 0;
        }
        else if (!_onGround)
        {
            _playerController.enabled = false;
            _player.transform.localPosition = _savedPosition;
            _playerController.enabled = true;
            _onGround = true;
        }
    }

    private void Awake()
    {
        _player = _playerController.gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            _onGround = false;
        }
    }
}
