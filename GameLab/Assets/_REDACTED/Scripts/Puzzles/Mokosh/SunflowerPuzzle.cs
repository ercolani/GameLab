using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
