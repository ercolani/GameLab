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
    /// The stools for the puzzle where the torches have to be placed.
    /// </summary>
    [SerializeField]
    private List<ObjectHolder> _puzzleStools = new List<ObjectHolder>();

    /// <summary>
    /// The initial holders for the torches.
    /// </summary>
    [SerializeField]
    private List<ObjectHolder> _torchHolders = new List<ObjectHolder>();

    /// <summary>
    /// The order in which the torches should be lit off.
    /// </summary>
    private List<FlameController> _blownOutTorches = new List<FlameController>();

    /// <summary>
    /// The initial position for each torch.
    /// </summary>
    private List<Vector3> _initialPosition = new List<Vector3>();

    /// <summary>
    /// The current phase of the puzzle.
    /// </summary>
    private int phase = 0;

    /// <summary>
    /// Sets the index for each puzzle torch and saves their initial position.
    /// </summary>
    private void Start()
    {
        int index = 0;
        foreach (FlameController torch in _puzzleTorches)
        {
            torch.GetComponent<PuzzleTorch>().SetIndex(index);
            torch.ToggleFlame(true);
            _initialPosition.Add(torch.transform.position);
            index++;
        }
        ResetPuzzle();
    }

    /// <summary>
    /// Subscribes to the reset switch and torches events.
    /// </summary>
    protected override void OnEnable()
    {
        resetSwitch.Reset += ResetPuzzle; 
        foreach (FlameController torch in _puzzleTorches)
        {
            torch.FlameToggled += OnTorchToggled;
        }
    }


    /// <summary>
    /// Subscribes to switch.
    /// </summary>
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
            // RESET TO THE STARTING POS OR HOLDER
            for (int i = 0; i < _puzzleTorches.Count; i++)
            {
                _puzzleTorches[i].transform.position = _initialPosition[i];
            }
        }
        else if(phase == 1)
        {
            for (int i = 0; i < _puzzleTorches.Count; i++)
            {
                _puzzleTorches[i].ToggleFlame(true);
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
    /// Checks whether or not the torch has been places in the correct position.
    /// </summary>
    private void CheckTorchPlacements()
    {
        for (int i = 0; i < _puzzleStools.Count; i++)
        {
            if (_puzzleStools[i].HeldObject == null)
            {
                return;
            }
            if (_puzzleStools[i].HeldObject.GetComponent<PuzzleTorch>().Index != i)
            {
                return;
            }
            phase++;
            Debug.Log("placing puzzle succesfully completed");
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
        // If reaches end of for loop without returning it means it was completed successfully.
        PuzzleCompleted();
    }

    /// <summary>
    /// When the puzzle is completed.
    /// </summary>
    public override void PuzzleCompleted()
    {
        Debug.Log("Anansi's puzzle has been completed");
    }
}
