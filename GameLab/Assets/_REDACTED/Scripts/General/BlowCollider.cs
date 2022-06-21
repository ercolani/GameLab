using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowCollider : MonoBehaviour
{
    public FlameController _currentTorch;

    public void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Torch" || collision.gameObject.tag == "PickUp")
        {
            if (collision.gameObject.GetComponent<FlameController>() != null)
            {
                Debug.LogError("Flame");
                _currentTorch = collision.gameObject.GetComponent<FlameController>();
            }
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Torch" || collision.gameObject.tag == "PickUp")
        {
            if (collision.gameObject.GetComponent<FlameController>() != null)
            {
                _currentTorch = null;
            }
        }
    }
}
