using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTorch : FlameController
{
    /// <summary>
    /// The unique ID index of the torch.
    /// </summary>
    public int Index;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    public void SetIndex(int index)
    {
        Index = index;
    }
}


