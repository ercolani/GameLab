using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class ItemGlow : MonoBehaviour
{
    /// <summary>
    /// Holds the renderer component. 
    /// </summary>
    [SerializeField]
    private Renderer _renderer; 

    /// <summary>
    /// Holds the material of the object.
    /// </summary>
    [SerializeField]
    private Material _material;

    /// <summary>
    /// <summary>
    /// Holds the emission color.
    /// </summary>
    [SerializeField]
    private Color _emissionColor;

    /// <summary>
    /// The intensity of the emissive light.
    /// </summary>
    [SerializeField]
    private float _intensity;

    /// <summary>
    /// At what intensity point the light stops becoming brighter.
    /// </summary>
    [SerializeField]
    private float _intensityThreshold = 3f;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;

        //Gets the initial emission colour of the material, as we have to store the information before turning off the light.
        _emissionColor = _material.GetColor("_EmissionColor");

        StartCoroutine(ToggleCoroutine(true));
    }

    /// <summary>
    /// Toggle the emissive light on and off.
    /// </summary>
    /// <returns></returns>
    public IEnumerator ToggleCoroutine(bool on)
    {
        yield return new WaitForSeconds(2.5f);
        if (on) 
        {
            bool turningOn = true;
            while (turningOn)
            {
                if (_intensity <= _intensityThreshold)
                {
                    yield return new WaitForSeconds(0.0005f);
                    Activate(true, _intensity);
                    _intensity += 0.005f;
                }
                else
                {
                    turningOn = false;
                    StartCoroutine(ToggleCoroutine(true));
                }
            }
        }
        else
        {
            bool turningOff = true;
            while (turningOff)
            {
                if (_intensity >= _intensityThreshold)
                {
                    yield return new WaitForSeconds(0.0005f);
                    Activate(true, _intensity);
                    _intensity -= 0.005f;
                }
                else
                {
                    turningOff = false;
                }
            }
        }
      
    }

    //Call this method to turn on or turn off emissive light.
    public void Activate(bool on, float intensity)
    {
        if (on)
        {
            //enables emission for the material, and make the material use realtime emission.
            _material.EnableKeyword("_EMISSION");
            _material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;

            //update the emission color and intensity of the material.
            _material.SetColor("_EmissionColor", _emissionColor * intensity);

            //makes the renderer update the emission and albedo maps of our material.
            RendererExtensions.UpdateGIMaterials(_renderer);

            //inform Unity's GI system to recalculate GI based on the new emission map.
            DynamicGI.SetEmissive(_renderer, _emissionColor * intensity);
            DynamicGI.UpdateEnvironment();
        }
        else
        {

            _material.DisableKeyword("_EMISSION");
            _material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;

            _material.SetColor("_EmissionColor", Color.black);
            RendererExtensions.UpdateGIMaterials(_renderer);

            DynamicGI.SetEmissive(_renderer, Color.black);
            DynamicGI.UpdateEnvironment();
        }
    }
}