using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the flames of the torches in the torchline.
/// </summary>
public class Torchline : MonoBehaviour
{
    /// <summary>
    /// Keeps a list of all the torches in the torchline.
    /// </summary>
    [SerializeField]
    private List<GameObject> torches = new List<GameObject>();

    /// <summary>
    /// Activates the flame of every torch in the torchline.
    /// </summary>
    public void ActivateFlames()
    {
        foreach (GameObject torch in torches)
        {
            torch.transform.Find("Flame").gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Deactivates the flame of every torch in the torchline.
    /// </summary>
    public void DeactivateFlames()
    {
        foreach (GameObject torch in torches)
        {
            torch.transform.Find("Flame").gameObject.SetActive(true);
        }
    }
}
