using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPuzzle : PuzzleManager
{
    [SerializeField]
    private FlameController[] _torches;

    [SerializeField]
    private PuzzleFish _puzzleFish;

    [SerializeField]
    private TorchWaypoints _finalWaypoints;

    private int _torchIndex = 0;

    public override void PuzzleCompleted()
    {
        deity.isLastEncounter = true;
        deity.ToggleDeityReadyForDialogue();
    }

    public override void ResetPuzzle()
    {
    }

    private void Start()
    {
        _puzzleFish.UpdateWaypoints(_torches[0].GetComponent<TorchWaypoints>().Waypoints, 0);
        _puzzleFish.FollowWaypoints = true;
        //TogglePuzzle(false);
    }

    protected override void OnDisable()
    {
    }

    protected override void OnEnable()
    {
        foreach (FlameController torch in _torches)
        {
            torch.FlameToggled += OnBlownOutTorch;
        }
    }

   private void OnBlownOutTorch(FlameController blownOut)
   {
        if (_puzzleFish.CurrentTorchIndex == _torchIndex)
        {
            if (_puzzleFish.TargetIndex == _torches[_torchIndex].GetComponent<TorchWaypoints>().CorrectIndex)
            {
                _torchIndex++;
                if (_torchIndex >= _torches.Length)
                {
                    PuzzleCompleted();
                    _puzzleFish.UpdateWaypoints(_finalWaypoints.Waypoints, _torchIndex);
                }
                else
                {
                    _puzzleFish.MovementSpeed = _puzzleFish.MovementSpeed + 1f;
                    _puzzleFish.UpdateWaypoints(_torches[_torchIndex].GetComponent<TorchWaypoints>().Waypoints, _torchIndex);
                }
            }
            else
            {
                StartCoroutine(FailedAttemmpt());
            }
        }
   }

    private IEnumerator FailedAttemmpt()
    {
        _puzzleFish.FollowWaypoints = false;
        yield return new WaitForSeconds(5f);
        _torches[_torchIndex].ToggleFlame(true);
        _puzzleFish.FollowWaypoints = true;
    }

    public override void TogglePuzzle(bool state)
    {
        _puzzleFish.gameObject.SetActive(state);
        foreach (FlameController torch in _torches)
        {
            torch.ToggleFlame(state);
        }
    }
}
