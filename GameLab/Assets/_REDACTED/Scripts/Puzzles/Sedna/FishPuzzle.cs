using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioController;

public class FishPuzzle : PuzzleManager
{
    [SerializeField]
    private FlameController[] _torches;

    [SerializeField]
    private PuzzleFish _puzzleFish;

    [SerializeField]
    private TorchWaypoints _finalWaypoints;

    [SerializeField]
    private AudioController audioController;

    private int _torchIndex = 0;

    public override void PuzzleCompleted()
    {
        _deity.isLastEncounter = true;
        audioController.PlaySound("Gong");
    }

    public override void ResetPuzzle()
    { 

    }

    private void Start()
    {
        _puzzleFish.UpdateWaypoints(_torches[0].GetComponent<TorchWaypoints>().Waypoints, 0);
        _puzzleFish.FollowWaypoints = true;
        _deity.OnPuzzleToggled += TogglePuzzle;
        TogglePuzzle(false);
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
        if (blownOut.gameObject == _torches[_torchIndex].gameObject) {
            if (_puzzleFish.CurrentTorchIndex == _torchIndex)
            {
                Debug.LogError("Blown Out torch equal torch index");
                if (_puzzleFish.TargetIndex == _torches[_torchIndex].GetComponent<TorchWaypoints>().CorrectIndex)
                {
                    Debug.LogError("Blown Out torch equal correct index");
                    _torchIndex++;
                    if (_torchIndex >= _torches.Length)
                    {
                        PuzzleCompleted();
                        _puzzleFish.UpdateWaypoints(_finalWaypoints.Waypoints, _torchIndex);
                    }
                    else
                    {
                        _puzzleFish.UpdateWaypoints(_torches[_torchIndex].GetComponent<TorchWaypoints>().Waypoints, _torchIndex);
                        _puzzleFish.MovementSpeed = _puzzleFish.MovementSpeed + 1f;
                    }
                }
                else
                {
                    StartCoroutine(FailedAttemmpt());
                }
            }
        }
        else
        {
            blownOut.ForceToggleFlame(true);
        }
   }

    private IEnumerator FailedAttemmpt()
    {
        _puzzleFish.FollowWaypoints = false;
        yield return new WaitForSeconds(5f);
        _torches[_torchIndex].ForceToggleFlame(true);
        _puzzleFish.FollowWaypoints = true;
    }

    public override void TogglePuzzle(bool state)
    {
        _puzzleFish.gameObject.SetActive(state);
        foreach (FlameController torch in _torches)
        {
            torch.ForceToggleFlame(state);
        }
    }
}
