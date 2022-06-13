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
    private float rotateSpeed = 5f;

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
                _sunflower.transform.rotation = Quaternion.RotateTowards(_sunflower.transform.rotation, Quaternion.Euler(_sunflower.transform.rotation.x, _torchRotationAngles[torchIndex], _sunflower.transform.rotation.z), Time.deltaTime * rotateSpeed);
            }
        }
    }
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
