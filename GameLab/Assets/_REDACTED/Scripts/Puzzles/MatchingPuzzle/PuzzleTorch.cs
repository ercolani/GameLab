using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTorch : FlameController
{
    /// <summary>
    /// The unique ID index of the torch.
    /// </summary>
    private int index;

    /// <summary>
    /// The unique ID index of the torch.
    /// </summary>
    public int Index => index;

    /// <summary>
    /// Sets the index of the torch.
    /// </summary>
    /// <param name="index"></param>
    public void SetIndex(int n)
    {
        index = n;
    }
}


