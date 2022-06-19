using UnityEngine;

public abstract class PuzzleManager : MonoBehaviour
{
    [SerializeField]
    protected ResetSwitch resetSwitch;

    [SerializeField]
    protected Deity deity;

    protected abstract void OnEnable();

    protected abstract void OnDisable();

    public abstract void ResetPuzzle();

    public abstract void PuzzleCompleted();

    public abstract void TogglePuzzle(bool state);
}