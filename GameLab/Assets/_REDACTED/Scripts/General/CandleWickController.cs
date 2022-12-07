using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CandleWickController : MonoBehaviour
{
    [Header("Material and Light")]
    [SerializeField]
    private Material material;

    [SerializeField]
    private Light lightSource;

    [SerializeField]
    private LensFlareComponentSRP lensFlare;

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

    [SerializeField]
    private float candleYOffset;

    [SerializeField]
    private float minCandleY;

    [SerializeField]
    private float defaultLightIntensity;

    [SerializeField]
    private float defaultLensFlareIntensity;

    [SerializeField]
    private float defaultCandleBrightness;

    [SerializeField]
    private float candleBrightnessIncrement;

    [Header("Random Ranges")]
    [SerializeField]
    private Vector2 randomLerpRange;

    [SerializeField]
    private Vector2 randomNoiseScaleRange;

    private Vector3 previousPosition;

    private Vector3 previousRotation;

    private Vector3 currentMove; 
    
    private Vector3 currentRotation;

    private float outVelocity = 0.0f;

    private bool candleOff = false;

    void FixedUpdate()
    {
        candleYOffset -= 0.01f;

        //generating random changes based on a range
        float randomLerpValue = lerpValue + Random.Range(randomLerpRange.x, randomLerpRange.y);
        float randomNoiseScale = noiseScale + Random.Range(randomNoiseScaleRange.x, randomNoiseScaleRange.y);

        //setting the y value 
        float randomCandleYOffset = candleYOffset;

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
        
        SetNewLightAndLensFlareIntensity();
        CheckCandleY();
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

    private void SetNewLightAndLensFlareIntensity()
    {
        float candleYandMinDifference = candleYOffset / minCandleY;

        lightSource.intensity = defaultLightIntensity * (1f - candleYandMinDifference);
        lensFlare.intensity = defaultLensFlareIntensity * (1f - candleYandMinDifference);
    }

    private void CheckCandleY()
    {
        if (candleYOffset < minCandleY)
        {
            if (!candleOff)
            {
                StartCoroutine(RemoveCandleBrightnessCoroutine());
            }
        } 
        else
        {
            material.SetFloat("_Brightness", defaultCandleBrightness);
            candleOff = false;
        }
    }

    private IEnumerator RemoveCandleBrightnessCoroutine()
    {
        float currentCandleBrightness = defaultCandleBrightness;

        material.SetFloat("_Brightness", currentCandleBrightness);
        currentCandleBrightness -= 0.1f;
        yield return new WaitForSeconds(0.1f);

        if (currentCandleBrightness < 0f)
        {
            StartCoroutine(RemoveCandleBrightnessCoroutine());
        }

        candleOff = true;
    }
}