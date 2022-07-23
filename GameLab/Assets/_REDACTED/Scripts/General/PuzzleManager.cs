using System.Collections.Generic;
using UnityEngine;

public abstract class PuzzleManager : MonoBehaviour
{
    [SerializeField]
    protected Deity _deity;

    [SerializeField]
    protected List<GameObject> lightsToTurnOff = new List<GameObject>();

    protected abstract void OnEnable();

    protected abstract void OnDisable();

    public abstract void ResetPuzzle();

    public abstract void PuzzleCompleted();

    public abstract void TogglePuzzle(bool state);
}