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
    /// Holds the emission color.
    /// </summary>
    [SerializeField]
    private Color _emissionColor;


    /// <summary>
    /// <summary>
    /// Holds the alternate emission color.
    /// </summary>
    [SerializeField]
    private Color _alternateEmissionColor;
    /// <summary>
    /// The intensity of the emissive light.
    /// </summary>
    [SerializeField]
    private float _intensity;

    /// <summary>
    /// At what intensity point the light stops becoming brighter.
    /// </summary>
    [SerializeField]
    private float _intensityHighThreshold = 3f;

    /// <summary>
    /// At what intensity point the light stops becoming darker.
    /// </summary>
    [SerializeField]
    private float _intensityLowThreshold = 0.5f;

    /// <summary>
    /// How quickly the light becomes brighter and darker.
    /// </summary>
    [SerializeField]
    private float _intensitySpeed = 0.02f;

    /// <summary>
    /// How quickly the light becomes brighter and darker.
    /// </summary>
    [SerializeField]
    private bool _alreadyGlowing = false;

    /// <summary>
    /// A getter for _alreadyGlowing.
    /// </summary>
    public bool AlreadyGlowing => _alreadyGlowing;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;

        //Gets the initial emission colour of the material, as we have to store the information before turning off the light.
        _emissionColor = _material.GetColor("_EmissionColor");
    }

    /// <summary>
    /// Toggle the emissive light on and off.
    /// </summary>
    /// <returns></returns>
    public IEnumerator ToggleCoroutine(bool on, bool stayOn, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (on) 
        {
            if (!_alreadyGlowing)
            {
                bool turningOn = true;
                while (turningOn)
                {
                    if (_intensity <= _intensityHighThreshold)
                    {
                        yield return new WaitForSeconds(0.0005f);
                        Activate(_intensity);
                        _intensity += _intensitySpeed;
                    }
                    else
                    {
                        turningOn = false;
                        if (!stayOn)
                        {
                            StartCoroutine(ToggleCoroutine(false, false, 0f));
                        }
                        else
                        {
                            _alreadyGlowing = true;
                        }
                    }
                }
            }
        }
        else
        {
            bool turningOff = true;
            while (turningOff)
            {
                if (_intensity >= _intensityLowThreshold)
                {
                    yield return new WaitForSeconds(0.0005f);
                    Activate(_intensity);
                    _intensity -= _intensitySpeed;
                }
                else
                {
                    turningOff = false;
                }
            }
        }
    }

    //Call this method to turn on or turn off emissive light.
    private void Activate(float intensity)
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

    public void SetAlternateEmissionColor()
    {
        _emissionColor = _alternateEmissionColor;
    }
}