using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MatrixSolution 
{
    public List<bool> _solution;
}

[System.Serializable]
public class FigurineSegment
{
    public InteractiveObject _figurine;
    public GameObject box;
}

public class MatrixPuzzle : PuzzleManager
{
    [SerializeField]
    private List<FlameController> _matrixTorches;

    [SerializeField]
    private List<MatrixSolution> _solutions;

    [SerializeField]
    private List<FigurineSegment> _figurineSegments;

    [SerializeField]
    private int _solvedFigurines;

    private int _totalFigurines;

    protected override void OnEnable()
    {
        foreach (FlameController torch in _matrixTorches)
        {
            torch.GetComponent<FlameController>().FlameToggled += CheckMatrix;
        }

        //foreach (FigurineSegment segment in _figurineSegments)
        //{
        //    segment._figurine.Interacted += ObtainedFigurine;
        //}
    }

    protected override void OnDisable()
    {
        foreach(FlameController torch in _matrixTorches)
        {
            torch.GetComponent<FlameController>().FlameToggled -= CheckMatrix;
        }

        foreach (FigurineSegment segment in _figurineSegments)
        {
            segment._figurine.Interacted += ObtainedFigurine;
        }
    }

    private void Start()
    {
        _totalFigurines = _solutions.Count;
    }

    public override void PuzzleCompleted()
    {
        foreach (FlameController torch in _matrixTorches)
        {
            torch.GetComponent<FlameController>().ToggleFlame(false, false);
        }
    }

    public override void ResetPuzzle()
    {
        StartCoroutine(RelightTorchesCoroutine());
    }

    private void CheckMatrix(FlameController torch)
    {
        int correctTorches = 0;
        int turnedOffTorches = 0;
        int solutionID = -1;
        
        for (int i = 0; i < _solutions.Count; i++)
        {
            //reset values for each solution
            solutionID = i;
            correctTorches = 0;
            turnedOffTorches = 0;

            for (int j = 0; j < _matrixTorches.Count; j++)
            {
                //if the current torch is turned off
                if (_matrixTorches[j].FlameActive == false)
                {
                    turnedOffTorches++;

                    //if all of the torches are turned off
                    if (turnedOffTorches == _matrixTorches.Count)
                    {
                        turnedOffTorches = 0;
                        ResetPuzzle();
                        break;
                    }
                }

                //if the current torch has the same state as the current solution
                if (_matrixTorches[j].FlameActive == _solutions[i]._solution[j])
                {
                    correctTorches++;

                    //if all of the torches have the same state as the current solution
                    if (correctTorches == _matrixTorches.Count)
                    {
                        ResetPuzzle();
                        OpenFigurineBox();
                        _solutions.RemoveAt(i);
                        break;
                    }
                }
            } 
        }
    }

    private void OpenFigurineBox()
    {
        _solvedFigurines++;
        print(_solvedFigurines);

        //open figurine box

        if (_solvedFigurines == _totalFigurines)
        {
            PuzzleCompleted();
        }
    }

    private void ObtainedFigurine()
    {

    }

    private IEnumerator RelightTorchesCoroutine()
    {
        FlameController flame;

        foreach (FlameController torch in _matrixTorches)
        {
            flame = torch.GetComponent<FlameController>();
            if (flame.FlameActive)
            {
                flame.ToggleFlame(false, false);
            }
        }

        yield return new WaitForSeconds(1f);

        if (!(_solvedFigurines == _totalFigurines))
        {
            foreach (FlameController torch in _matrixTorches)
            {
                flame = torch.GetComponent<FlameController>();
                flame.ToggleFlame(true, false);
            }
        }
    }

}

