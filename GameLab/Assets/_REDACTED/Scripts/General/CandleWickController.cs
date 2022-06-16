using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleWickController : MonoBehaviour
{
    [Header("Material")]
    [SerializeField]
    private Material material;

    [Header("Vector Values")]
    [SerializeField]
    private Vector2 defaultCandleOffset;

    [SerializeField]
    private Vector2 candleOffset;

    [SerializeField]
    private Vector2 maxCandleOffset;

    [SerializeField]
    private Vector2 minCandleOffset;

    [Header("Floats")]
    [SerializeField]
    private float noiseScale;

    [SerializeField]
    private float lerpValue;

    [SerializeField]
    private float rotationLerp;

    [Header("Random Ranges")]
    [SerializeField]
    private Vector2 randomLerpRange;

    [SerializeField]
    private Vector2 randomNoiseScaleRange;

    [SerializeField]
    private Vector2 randomCandleYOffsetRange;

    private Vector3 previousPosition;

    private Vector3 previousRotation;

    private Vector3 currentMove; 
    
    private Vector3 currentRotation;

    private float outVelocity = 0.0f;
   
    void FixedUpdate()
    {
        //generating random changes based on a range
        float randomLerpValue = lerpValue + Random.Range(randomLerpRange.x, randomLerpRange.y);
        float randomNoiseScale = noiseScale + Random.Range(randomNoiseScaleRange.x, randomNoiseScaleRange.y);
        float randomCandleYOffset = Random.Range(randomCandleYOffsetRange.x, randomCandleYOffsetRange.y);
        float randomMaxCandleXOffset = maxCandleOffset.x + Random.Range(-0.2f, 0.2f);
        float randomMinCandleXOffset = minCandleOffset.x + Random.Range(-0.2f, 0.2f);

        //change in velocity
        currentMove = transform.localPosition - previousPosition; 
        previousPosition = transform.localPosition;
        Vector3 localVelocity = transform.InverseTransformDirection(currentMove);

        //change in rotation
        currentRotation = transform.forward - previousRotation; 
        previousRotation = transform.forward;
        Quaternion localRotation = Quaternion.FromToRotation(previousRotation, currentRotation);

        CalculateVelocity(localVelocity.z, randomLerpValue);
        CalculateRotation(localRotation.y);

        material.SetVector("_Offset", new Vector2(candleOffset.x + defaultCandleOffset.x, randomCandleYOffset));
        material.SetFloat("_NoiseScale", noiseScale);
    }

    private void CalculateRotation(float rotation)
    {
        if (rotation < 0)
        {
            candleOffset.x = Mathf.SmoothDamp(candleOffset.x, rotation / Time.deltaTime, ref outVelocity, rotationLerp);
        }

        else if (rotation > 0)
        {
            candleOffset.x = Mathf.SmoothDamp(candleOffset.x, rotation / Time.deltaTime, ref outVelocity, rotationLerp);
        }
        else
        {
            candleOffset.x = Mathf.SmoothDamp(candleOffset.x, rotation / Time.deltaTime, ref outVelocity, 0.3f);
        }
    }

    private void CalculateVelocity(float velocity, float randomLerpValue)
    {
        candleOffset.x = Mathf.SmoothDamp(candleOffset.x, velocity / Time.deltaTime, ref outVelocity, randomLerpValue);

        if (candleOffset.x > maxCandleOffset.x)
        {
            candleOffset.x = maxCandleOffset.x;
        }

        if (candleOffset.x < minCandleOffset.x)
        {
            candleOffset.x = minCandleOffset.x;
        }
    }
}
