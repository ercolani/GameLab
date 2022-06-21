using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowCollider : MonoBehaviour
{
    public FlameController _currentTorch;

    public void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.GetComponent<FlameController>() != null)
        {
            _currentTorch = collisionInfo.gameObject.GetComponent<FlameController>();
        }
        else
        {
            _currentTorch = null;
        }
    }
}
