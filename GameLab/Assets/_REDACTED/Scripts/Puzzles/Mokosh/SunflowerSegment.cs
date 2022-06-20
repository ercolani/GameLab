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
    private float _sunflowerRotationOrigin;

    [SerializeField]
    private float _sunflowerRotationDestination;

    [SerializeField]
    private float timeToRotate = 2f;

    public event Action OnSegmentCompleted;

    private bool rotating;

    private bool _finalCorrectTorch;

    private bool _lastTorch;

    [SerializeField]
    private int _blownOutCount;

    [SerializeField]
    private float _sunflowerPreviousRotationY;

    private void Start()
    {
        _sunflowerPreviousRotationY = _sunflowerRotationOrigin;
        Vector3 originalRotation = new Vector3(_sunflower.transform.eulerAngles.x, _sunflowerRotationOrigin, _sunflower.transform.eulerAngles.z);
        _sunflower.transform.eulerAngles = originalRotation;
    }

    private IEnumerator ResetSegment()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < _segmentTorches.Count; i++)
        {
            _segmentTorches[i].ToggleFlame(true);
        }
        Vector3 originalRotation = new Vector3(_sunflower.transform.eulerAngles.x, _sunflowerRotationOrigin, _sunflower.transform.eulerAngles.z);
        _sunflower.transform.eulerAngles = originalRotation;
        _blownOutCount = 0;
        _lastTorch = false;
        _sunflowerPreviousRotationY = _sunflowerRotationOrigin;
    }

    public void RotateSunflower(FlameController torch)
    {
        if (_finalCorrectTorch)
        {
            _blownOutCount = 0;
            OnSegmentCompleted?.Invoke();
            return;
        }

        if (_lastTorch)
        {
            StartCoroutine(ResetSegment());
            return;
        }

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

        FixSunflowerRotation(eulerAngles.y);
        rotating = false;
    }

    private void CheckSunflowerState()
    {
        int torchesLeft = 0;

        foreach (FlameController segmentTorch in _segmentTorches)
        {
            if (segmentTorch.FlameActive)
            {
                torchesLeft++;
            }
        }

        if (torchesLeft == 1)
        {
            _lastTorch = true;
            Vector3 targetRotation = new Vector3(_sunflower.transform.eulerAngles.x, _sunflowerRotationDestination, _sunflower.transform.eulerAngles.z);
            if (_sunflower.transform.eulerAngles == targetRotation)
            {
                _finalCorrectTorch = true;
            }
        }
    }

    private void FixSunflowerRotation(float value)
    {
        float realTargetRotation = value + _sunflowerPreviousRotationY;
        _sunflowerPreviousRotationY = realTargetRotation;
        _sunflower.transform.eulerAngles = new Vector3(_sunflower.transform.eulerAngles.x, realTargetRotation, _sunflower.transform.eulerAngles.x);
        CheckSunflowerState();
    }
}