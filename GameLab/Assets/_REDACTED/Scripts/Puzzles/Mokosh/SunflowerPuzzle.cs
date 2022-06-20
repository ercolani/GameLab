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


    private void Start()
    {
        TogglePuzzle(true);
    }

    protected override void OnEnable()
    {
        for (int i = 0; i < _sunflowerSegments.Count; i++)
        {
            _sunflowerSegments[i].OnSegmentCompleted += SegmentComplete;
            for (int j = 0; j < _sunflowerSegments[i]._segmentTorches.Count; j++)
            {
                _sunflowerSegments[i]._segmentTorches[j].FlameToggled += OnTorchToggled;
            }
        }
        deity.OnPuzzleToggled += TogglePuzzle;
    }

    protected override void OnDisable()
    {
        for (int i = 0; i < _sunflowerSegments.Count; i++)
        {
            _sunflowerSegments[i].OnSegmentCompleted -= SegmentComplete;
            for (int j = 0; j < _sunflowerSegments[i]._segmentTorches.Count; j++)
            {
                FlameController torch = _sunflowerSegments[i]._segmentTorches[j];
                torch.FlameToggled -= OnTorchToggled;
            }
        }
    }

    public override void ResetPuzzle()
    {

    }

    public override void PuzzleCompleted()
    {
        print("meow");
    }

    private void OnTorchToggled(FlameController torch)
    {
        _sunflowerSegments[_currentSegment].RotateSunflower(torch);
    }

    public override void TogglePuzzle(bool state)
    {
        GetComponent<GrowVines>().ToggleGrowingVines();
    }

    private void SegmentComplete()
    {
        _currentSegment++;

        if (_currentSegment > 3)
        {
            PuzzleCompleted();
            return;
        }

        foreach (PuzzleTorch torch in _sunflowerSegments[_currentSegment]._segmentTorches)
        {
            torch.ToggleFlame(true);
        }
    }
}
