using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SunflowerSegment : MonoBehaviour
{
    [SerializeField]
    private List<int> _torchRotationAngles;

    public List<FlameController> _segmentTorches;

    [SerializeField]
    private GameObject _sunflower;

    [SerializeField]
    private int _sunflowerRotationOrigin;

    [SerializeField]
    private int _sunflowerRotationDestination;

    [SerializeField]
    private float timeToRotate = 2f;

    public event Action OnSegmentCompleted;

    private bool rotating;

    private bool _finalCorrectTorch;
    
    [SerializeField]
    private int _blownOutCount;

    public void ResetSegment()
    {
        for (int i = 0; i < _segmentTorches.Count; i++)
        {
            _segmentTorches[i].ToggleFlame(true);
        }
    }

    public void RotateSunflower(FlameController torch)
    {
        _blownOutCount++;

        int torchIndex = 0;

        foreach (FlameController segmentTorch in _segmentTorches)
        {
            torchIndex = torch == segmentTorch ? _segmentTorches.IndexOf(torch) : -1;

            if (torchIndex != -1)
            {
                Vector3 endRotation = new Vector3(0, _torchRotationAngles[torchIndex], 0);
                _sunflower.transform.Rotate(endRotation * Time.deltaTime);
                StartCoroutine(RotateSunflowerCoroutine(endRotation));
            }
        }
    }

    private IEnumerator RotateSunflowerCoroutine(Vector3 eulerAngles)
    {
        if (_finalCorrectTorch)
        {
            OnSegmentCompleted?.Invoke();
            yield break;
        }

        if (rotating)
        {
            yield break;
        }
        rotating = true;

        Vector3 newRotation = _sunflower.transform.eulerAngles + eulerAngles;
        Vector3 currentRotation = _sunflower.transform.eulerAngles;
        float counter = 0;

        while (counter < timeToRotate)
        {
            counter += Time.deltaTime;
            _sunflower.transform.eulerAngles = Vector3.Lerp(currentRotation, newRotation, counter / timeToRotate);
            yield return null;
        }
        rotating = false;
        CheckSunflowerState();
    }

    private void CheckSunflowerState()
    {
        if (_blownOutCount == _segmentTorches.Count)
        {
            ResetSegment();
            return;
        }

        int torchesLeft = 0;

        foreach (FlameController segmentTorch in _segmentTorches)
        {
            if (!segmentTorch.FlameActive)
            {
                torchesLeft++;
            }
        }

        if (torchesLeft == 1)
        {
            Vector3 targetRotation = new Vector3(_sunflower.transform.eulerAngles.x, _sunflowerRotationDestination, _sunflower.transform.eulerAngles.z);
            if (_sunflower.transform.eulerAngles == targetRotation)
            {
                _finalCorrectTorch = true;
            }
        }
    }
}