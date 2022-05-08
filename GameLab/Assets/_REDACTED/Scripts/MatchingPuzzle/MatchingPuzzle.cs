using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingPuzzle : PuzzleManager
{
    [SerializeField]
    private List<PuzzleTorch> blowingOutOrder = new List<PuzzleTorch>();

    [SerializeField]
    private List<PuzzleTorch> blownOutByPlayerOrder = new List<PuzzleTorch>();

    private void Start()
    {
        int index = 0;
        foreach (PuzzleTorch torch in blowingOutOrder)
        {
            torch.SetIndex(index);
            index++;
        }
    }

    protected override void OnEnable()
    {
        resetSwitch.Reset += ResetPuzzle; 
        foreach (PuzzleTorch torch in blowingOutOrder)
        {
            torch.FlameToggled += OnTorchToggled;
        }
    }

    protected override void OnDisable()
    {
        resetSwitch.Reset -= ResetPuzzle; 
    }

    protected override void StartPuzzle()
    {

    }
    
    protected override void ResetPuzzle()
    {

    }

    private void OnTorchToggled(FlameController toggleTorch)
    {
        if (toggleTorch.FlameToggleState)
        {
            foreach (PuzzleTorch torch in blowingOutOrder)
            {
                
            }
        }
        else
        {
            blownOutByPlayerOrder.Add(toggleTorch.GetComponent<PuzzleTorch>());
        }
    }

    private void CheckTorchBlowOrder()
    {

    }
}
