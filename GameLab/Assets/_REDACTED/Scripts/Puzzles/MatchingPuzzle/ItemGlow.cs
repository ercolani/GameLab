using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcess;

public class ItemGlow : MonoBehaviour
{
    /// <summary>
    /// The intensity of the glow on the object.
    /// </summary>
    [SerializeField]
    private float bloomIntensity;

    /// <summary>
    /// The bloom value of an object not glowing.
    /// </summary>
    [SerializeField]
    private float lowIntensity;

    /// <summary>
    /// The bloom value of an object glowing.
    /// </summary>
    [SerializeField]
    private float highIntensity;

    //private PostProcessVolume postProcessing;

    /// <summary>
    /// Called by an external class to change the intensity of the object glow.
    /// </summary>
    public void ToggleGlow(bool state)
    {
        switch (state)
        {
            case true:
                bloomIntensity = highIntensity;
                break;
            case false:
                bloomIntensity = lowIntensity;
                break;
        }
    }

}
