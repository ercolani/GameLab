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
    /// The order in which the symbols glow up.
    /// </summary>
    [SerializeField]
    private List<ItemGlow> _symbolGlowOrder = new List<ItemGlow>();

    /// <summary>
    /// The list of symbols on the stools.
    /// </summary>
    [SerializeField]
    private List<ItemGlow> _stoolSymbols = new List<ItemGlow>();

    /// <summary>
    /// The current symbol index that is glowing.
    /// </summary>
    private int _glowIndex = 0;

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
    /// How often the symbols begin to light up (are invoked).
    /// </summary>
    private float _symbolInvokeTime = 2.5f;

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
            torch.ToggleCanBeBlownOut(false);
        }

        foreach (ObjectHolder stool in _puzzleStools)
        {
            stool.ObjectPlaced += CheckTorchPlacements;
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
        StartCoroutine(ResetPuzzleCoroutine());
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
        int correctStools = 0;

        for (int i = 0; i < _puzzleStools.Count; i++)
        {
            if (_puzzleStools[i].HasObject())
            {
                if (_puzzleStools[i].HeldObject.GetComponent<PuzzleTorch>().Index == i)
                {
                    correctStools++;
                    StartCoroutine(_stoolSymbols[i].ToggleCoroutine(true, true, 0f));
                }
            }
        }

        if (correctStools == _puzzleStools.Count)
        {
            phase++;
            InvokeRepeating("StartSymbolGlows", 0f, _symbolInvokeTime);
            foreach (FlameController torch in _puzzleTorches)
            {
                torch.ToggleCanBeBlownOut(true);
            }
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
                if (_blownOutTorches.Count == _puzzleTorches.Count)
                {
                    ResetPuzzle();
                }
                return;
            }
        }

        PuzzleCompleted();
    }

    /// <summary>
    /// When the puzzle is completed.
    /// </summary>
    public override void PuzzleCompleted()
    {
        CancelInvoke("StartSymbolGlows");
        foreach (ItemGlow symbol in _stoolSymbols)
        {
            StartCoroutine(symbol.ToggleCoroutine(false, false, 0f));
        }
    }

    private void StartSymbolGlows()
    {
       
        if (_glowIndex == 0)
        {
            _symbolGlowOrder[_glowIndex].SetAlternateEmissionColor();
        }

        StartCoroutine(_symbolGlowOrder[_glowIndex].ToggleCoroutine(true, false, 0f));

        _glowIndex++;

        if (_glowIndex == _symbolGlowOrder.Count)
        {
            _glowIndex = 0;
        }
    }

    private IEnumerator ResetPuzzleCoroutine()
    {
        if (phase == 0)
        {
            for (int i = 0; i < _puzzleTorches.Count; i++)
            {
                _puzzleTorches[i].transform.position = _initialPosition[i];
            }
        }
        else if (phase == 1)
        {
            yield return new WaitForSeconds(1.5f);

            for (int i = 0; i < _puzzleTorches.Count; i++)
            {
                _puzzleTorches[i].ToggleFlame(true, false);
            }
            _blownOutTorches.Clear();
        }
    }
}
