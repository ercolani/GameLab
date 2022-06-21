using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static AudioController;

public class Matrix : MonoBehaviour
{
    [SerializeField]
    public List<FlameController> _matrixTorches;

    [SerializeField]
    private List<bool> _solution;

    [SerializeField]
    private FigurineSegment _figurineSegment;

    [SerializeField]
    private MatrixPuzzle matrixPuzzle;

    [SerializeField]
    private bool _solved;

    public event Action OnObtainedFigurine;

    private void OnEnable()
    {
        foreach (FlameController torch in _matrixTorches)
        {
            torch.GetComponent<FlameController>().FlameToggled += CheckMatrix;
        }

        _figurineSegment._figurine.Interacted += ObtainedFigurine;
    }

    private void OnDisable()
    {
        foreach (FlameController torch in _matrixTorches)
        {
            torch.GetComponent<FlameController>().FlameToggled -= CheckMatrix;
        }

        _figurineSegment._figurine.Interacted -= ObtainedFigurine;
    }

    private void MatrixCompleted()
    {
        foreach (FlameController torch in _matrixTorches)
        {
            torch.GetComponent<FlameController>().ToggleFlame(false, false);
        }
        OpenFigurineBox();
    }

    public void ResetMatrix()
    {
        StartCoroutine(RelightTorchesCoroutine());
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

        if (!_solved)
        {
            foreach (FlameController torch in _matrixTorches)
            {
                flame = torch.GetComponent<FlameController>();
                flame.ToggleFlame(true, false);
            }
        }
    }

    private void OpenFigurineBox()
    {
        _figurineSegment.box.PlayAnimation("WoodenBox");
        PlaySound("Chest");
        _figurineSegment._figurine.SetInteractivity(true);
    }

    private void ObtainedFigurine()
    {
        // _figurineSegments[_solvedFigurines - 1]._figurine.GetComponent<MeshRenderer>().materials[1].SetFloat("")
        _figurineSegment._figurine.gameObject.SetActive(false);
        _solved = true;
        OnObtainedFigurine?.Invoke();
    }

    private void CheckMatrix(FlameController torch)
    {
        int correctTorches = 0;
        int turnedOffTorches = 0;

        //reset values for each solution
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
                    ResetMatrix();
                    break;
                }
            }

            //if the current torch has the same state as the current solution
            if (_matrixTorches[j].FlameActive == _solution[j])
            {
                correctTorches++;

                //if all of the torches have the same state as the current solution
                if (correctTorches == _matrixTorches.Count)
                {
                    MatrixCompleted();
                    break;
                }
            }
        }
    }
}

