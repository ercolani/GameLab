using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTree : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            child.transform.rotation = new Quaternion(0, 0, 100, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
