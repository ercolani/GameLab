using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PuzzleManager : MonoBehaviour
{
    [SerializeField]
    protected ResetSwitch resetSwitch;

    [SerializeField]
    protected Deity deity;

    protected abstract void OnEnable();

    protected abstract void OnDisable();

    protected abstract void StartPuzzle();
    
    protected abstract void ResetPuzzle();
    

}
