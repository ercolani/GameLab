using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioController;

[System.Serializable]
public class FigurineSegment
{
    public InteractiveObject _figurine;
    public AnimationController box;
}

public class MatrixPuzzle : PuzzleManager
{
    [SerializeField]
    private List<Matrix> _matrices;

    [SerializeField]
    private GameObject _mainFigurine;

    [SerializeField]
    private int _obtainedFigurines;

    private int _totalFigurines;

    protected override void OnEnable()
    {
        _deity.OnPuzzleToggled += TogglePuzzle;
        TogglePuzzle(false);
    }

    protected override void OnDisable()
    {
        _deity.OnPuzzleToggled -= TogglePuzzle;
    }

    private void Start()
    {
        _totalFigurines = _matrices.Count;

        foreach (Matrix matrix in _matrices)
        {
            matrix.OnObtainedFigurine += ObtainedFigurine;
        }
    }

    public override void PuzzleCompleted()
    {
        _mainFigurine.SetActive(true);
        _deity.isLastEncounter = true;
        PlaySound("Gong");
    }

    public override void ResetPuzzle()
    {

    }

    private void ObtainedFigurine()
    {
        _obtainedFigurines++;

        if (_obtainedFigurines == _totalFigurines)
        {
            PuzzleCompleted();
        }
    }

    public override void TogglePuzzle(bool state)
    {
        foreach (Matrix matrix in _matrices)
        {
            foreach (FlameController torch in matrix._matrixTorches) 
            {
                torch.ForceToggleFlame(state);
            }
        }
    }
}

