using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SunflowerSegment
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

    private bool rotating;

    public void ResetSegment()
    {
        for (int i = 0; i < _segmentTorches.Count; i++)
        {
            _segmentTorches[i].ToggleFlame(true);
        }
    }

    public void RotateSunflower(FlameController torch)
    {
        int torchIndex = 0;

        foreach (FlameController segmentTorch in _segmentTorches)
        {
            torchIndex = torch == segmentTorch ? _segmentTorches.IndexOf(torch) : -1;

            if (torchIndex != -1)
            {
                Vector3 endRotation = new Vector3(_torchRotationAngles[torchIndex], 0, 0);
                _sunflower.transform.Rotate(endRotation * Time.deltaTime);
                //StartCoroutine(RotateSunflowerCoroutine(endRotation));
            }
        }
    }

    //private IEnumerator RotateSunflowerCoroutine(Vector3 eulerAngles)
    //{
    //    if (rotating)
    //        yield break;
    //    }
    //    {
    //    rotating = true;

    //    Vector3 newRotation = _sunflower.transform.eulerAngles + eulerAngles;

    //    Vector3 currentRotation = _sunflower.transform.eulerAngles;

    //    float counter = 0;

    //    while (counter < timeToRotate)
    //    {
    //        counter += Time.deltaTime;
    //        _sunflower.transform.eulerAngles = Vector3.Lerp(currentRotation, newRotation, counter / timeToRotate);
    //        yield return null;
    //    }

    //    rotating = false;
    //}
}

public class SunflowerPuzzle : PuzzleManager
{
    [SerializeField]
    private List<SunflowerSegment> _sunflowerSegments;
    
    [SerializeField]
    private int _currentSegment;

    protected override void OnEnable()
    {
        for (int i = 0; i < _sunflowerSegments.Count; i++)
        {
            for (int j = 0; j < _sunflowerSegments[i]._segmentTorches.Count; j++)
            {
                _sunflowerSegments[i]._segmentTorches[j].FlameToggled += OnTorchToggled;
            }
        }
    }

    protected override void OnDisable()
    {
        for (int i = 0; i < _sunflowerSegments.Count; i++)
        {
            for (int j = 0; j < _sunflowerSegments[i]._segmentTorches.Count; j++)
            {
                FlameController torch = _sunflowerSegments[i]._segmentTorches[j];
                torch.FlameToggled -= OnTorchToggled;
            }
        }
    }

    public override void ResetPuzzle()
    {
        _sunflowerSegments[_currentSegment].ResetSegment();
    }

    public override void PuzzleCompleted()
    {
        throw new System.NotImplementedException();
    }

    private void OnTorchToggled(FlameController torch)
    {
        _sunflowerSegments[_currentSegment].RotateSunflower(torch);
    }
}
