using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleWickController : MonoBehaviour
{
    [SerializeField]
    private Material material;
    
    [SerializeField]
    private Vector2 candleOffset;

    [SerializeField]
    private float noiseScale;

    [SerializeField]
    private Vector3 previousPosition;

    [SerializeField]
    private Vector3 currentMove;

    [SerializeField]
    private float lerpValue;

    private void Awake()
    {
        //material = GetComponent<MeshRenderer>().material;
    }

    void FixedUpdate()
    {
        currentMove = transform.localPosition - previousPosition; //velocity
        previousPosition = transform.localPosition;

        Vector3 localVelocity = transform.InverseTransformDirection(currentMove);

        candleOffset.x = Mathf.Lerp(candleOffset.x, localVelocity.x / Time.deltaTime, lerpValue);
        candleOffset.y = Mathf.Lerp(candleOffset.y, localVelocity.y / Time.deltaTime, lerpValue);

        material.SetVector("_Offset", candleOffset);
    }
}
