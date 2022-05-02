using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles communication to light up a torchline.
/// </summary>
public class TorchlineManager : MonoBehaviour
{
    /// <summary>
    /// A list of all the flames of each torch.
    /// </summary>
    [SerializeField]
    private List<GameObject> _allTorchlines = new List<GameObject>();

    /// <summary>
    /// A number to keep count of how many torchlines were lit up so far.
    /// </summary>
    [SerializeField]
    private int _torchlineCount;
    
    /// <summary>
    /// Called by the puzzle class to activate a certain torchline.
    /// </summary>
    public void ActivateTorchline()
    {
        _allTorchlines[_torchlineCount].GetComponent<Torchline>().ActivateFlames();
    }

}
