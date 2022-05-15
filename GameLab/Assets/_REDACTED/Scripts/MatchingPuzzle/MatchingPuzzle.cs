using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that manages the matching puzzle for Anansi's area.
/// </summary>
public class MatchingPuzzle : PuzzleManager
{
    /// <summary>
    /// The order in which the torches should be lit off.
    /// </summary>
    [SerializeField]
    private List<FlameController> _puzzleTorches = new List<FlameController>();

    /// <summary>
    /// The order in which the torches should be lit off.
    /// </summary>
    [SerializeField]
    private List<FlameController> _blownOutTorches = new List<FlameController>();

    /// <summary>
    /// The initial position for each torch.
    /// </summary>
    private List<Vector3> _initialPosition = new List<Vector3>();

    /// <summary>
    /// The current phase of the puzzle.
    /// </summary>
    private int phase = 0;

    private void Start()
    {
        int index = 0;
        foreach (FlameController torch in _puzzleTorches)
        {
            torch.GetComponent<PuzzleTorch>().Index = index;
            torch.ToggleFlame(true);
            _initialPosition.Add(torch.transform.position);
            index++;
        }
        ResetPuzzle();
    }

    protected override void OnEnable()
    {
        resetSwitch.Reset += ResetPuzzle; 
        foreach (FlameController torch in _puzzleTorches)
        {
            torch.FlameToggled += OnTorchToggled;
        }
    }

    protected override void OnDisable()
    {
        resetSwitch.Reset -= ResetPuzzle; 
    }

    /// <summary>
    /// Resets the puzzle.
    /// </summary>
    public override void ResetPuzzle()
    {
        if (phase == 0)
        {
            for (int i = 0; i < _puzzleTorches.Count; i++)
            {
                _puzzleTorches[i].transform.position = _initialPosition[i];
            }
        }
    }

    /// <summary>
    /// Triggered when a torch is toggled.
    /// </summary>
    /// <param name="toggledTorch"></param>
    private void OnTorchToggled(FlameController toggledTorch)
    {
        if (!toggledTorch.FlameActive)
        {
            _blownOutTorches.Add(toggledTorch);
        }
        if (_blownOutTorches.Count == _puzzleTorches.Count)
        {
            CheckBlownOrder();
        }
    }

    /// <summary>
    /// Checks the order of the blown candles.
    /// </summary>
    private void CheckBlownOrder()
    {
        for (int i = 0; i < _puzzleTorches.Count; i++)
        {
            if (_puzzleTorches[i] == _blownOutTorches[i])
            {
                continue;
            }
            else
            {
                return;
            }
        }

        PuzzleCompleted();
        // If reaches end of for loop without returning it means it was completed successfully.
    }

    /// <summary>
    /// Checks whether or not the torch has been places in the correct position.
    /// </summary>
    private void CheckTorchPosition()
    {

    }

    /// <summary>
    /// When the puzzle is completed.
    /// </summary>
    public override void PuzzleCompleted()
    {
        Debug.LogError("Puzzle has been completed succesfully");
    }
}
