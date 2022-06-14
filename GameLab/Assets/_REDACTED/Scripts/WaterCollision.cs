using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 _savedPosition;

    private bool _colliding;

    // Update is called once per frame
    private void LateUpdate()
    {
        _savedPosition = this.transform.position;
    }

    private void OnCollisionEnter(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            Debug.LogError("Collided");
            _colliding = true;
            this.transform.position = _savedPosition;
        }
    }
}
