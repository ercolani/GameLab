using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A singleton class controlling all in-game audio using FMOD.
/// </summary>
public class AudioController : MonoBehaviour
{
    private FadeVolume fadeVolume;

    //private static AudioController _instance;

    //public static AudioController Instance => _instance;

    [SerializeField]
    private string _currentAreaCode;

    [SerializeField]
    private string _currentEncounterType;

    [SerializeField]
    private string _currentPuzzleThoughtType;

    [SerializeField]
    private int _currentLineNumber;

    [SerializeField]
    private bool _isPuzzleThought;

    [SerializeField]
    private List<FMOD.Studio.EventInstance> instances = new List<FMOD.Studio.EventInstance>();

    [SerializeField]
    private int _instanceLength;

    public int InstanceLength => _instanceLength;

    public float currentVolume;

    public FMOD.Studio.EventInstance currentVolumeInstance;
    
    private void Awake()
    {
        ////singleton destroy pattern
        //if (_instance != null && _instance != this)
        //{
        //    Destroy(this.gameObject);
        //}
        //else
        //{
        //    _instance = this;
        //}
    }

    /// <summary>
    /// Plays a voice line using a code from FMOD depending on if it is a puzzle thought or not.
    /// </summary>
    private void PlayVoiceLine()
    {
        string currentLineNumberFormatted = "";

        if (_currentLineNumber < 10)
        {
            currentLineNumberFormatted = "0" + $"{_currentLineNumber}";
        }
        else
        {
            currentLineNumberFormatted = _currentLineNumber.ToString();
        }

        if (!_isPuzzleThought)
        {
            FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance($"event:/{_currentAreaCode}" + "_" + $"{_currentEncounterType}" + "_" + currentLineNumberFormatted);
            instances.Add(instance);
            instance.start();
            GetInstanceLength(instance);
            instance.release();
        }
        else
        {
            FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance($"event:/{_currentAreaCode}" + "_" + $"{_currentEncounterType}" + "_" + $"{_currentPuzzleThoughtType}" + "" + currentLineNumberFormatted);
            instances.Add(instance);
            instance.start();
            GetInstanceLength(instance);
            instance.release();
        }
    }

    /// <summary>
    /// Sets the parameters for playing the correct voice line if it is not a puzzle thought.
    /// </summary>
    private void SetVoiceLineParameters(string areaCode, string encounterType, int currentLineNumber)
    {
        _currentAreaCode = areaCode;
        _currentEncounterType = encounterType;
        _currentLineNumber = currentLineNumber;
        PlayVoiceLine();
    }

    /// <summary>
    /// Sets the parameters for playing the correct voice line if it is a puzzle thought.
    /// </summary>
    private void SetVoiceLineParameters(string areaCode, string encounterType, string currentPuzzleThoughtType, int currentLineNumber)
    {
        _currentAreaCode = areaCode;
        _currentEncounterType = encounterType;
        _currentLineNumber = currentLineNumber;
        _currentPuzzleThoughtType = currentPuzzleThoughtType;
        PlayVoiceLine();
    }

    /// <summary>
    /// Parses the input voice line code from the dialogue node's title and sets the parameters for the correct voice line.
    /// </summary>
    public void ParseVoiceLineCode(string input)
    {
        string[] parameterCodes = input.Split("_"); 
        if (parameterCodes.Length == 3)
        {
            SetVoiceLineParameters(parameterCodes[0], parameterCodes[1], int.Parse(parameterCodes[2]));
        }
        else if (parameterCodes.Length == 4)    
        {
            SetVoiceLineParameters(parameterCodes[0], parameterCodes[1], parameterCodes[2], int.Parse(parameterCodes[3]));
        }
        else
        {
            Debug.LogError("Voice line input string invalid.");
        }
    }

    public void GetInstanceLength(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.EventDescription eventDescription;
        instance.getDescription(out eventDescription);
        eventDescription.getLength(out _instanceLength);
        float ceil = (float)_instanceLength / 1000;
        _instanceLength = Mathf.CeilToInt(ceil);
    }

    public void PlaySound(string sound)
    {
        FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance($"event:/{sound}");
        instances.Add(instance);
        instance.start();
        instance.release();
    }

    public void PlaySoundLooping(string sound)
    {
        FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance($"event:/{sound}");
        instances.Add(instance);
        instance.start();
    }

    public FMOD.Studio.EventInstance RetrieveNameFromEventInstance(string sound)
    {
        FMOD.Studio.EventInstance eventInstance = new FMOD.Studio.EventInstance();
        string result = "";
        FMOD.Studio.EventDescription eventDescription;
        foreach (FMOD.Studio.EventInstance instance in instances)
        {
            instance.getDescription(out eventDescription);
            eventDescription.getPath(out result);
            if (result != null && result.Contains(sound))
            {
                eventInstance = instance;
                return eventInstance;
            }
        }
        Debug.LogError("No event instance of such name found.");
        return eventInstance;
    }

    private IEnumerator ToggleSoundMuteCoroutine(string sound, bool turningOn, bool isLooping)
    {
        if (!isLooping)
        {
            currentVolumeInstance = RetrieveNameFromEventInstance(sound);
            currentVolumeInstance.getVolume(out currentVolume);
        }

        if (turningOn)
        {
            if (currentVolume < 1f)
            {
                currentVolume += 0.1f;
                currentVolumeInstance.setVolume(currentVolume);
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(ToggleSoundMuteCoroutine(sound, true, true));
            }
        }
        else
        {
            if (currentVolume > 0f)
            {
                currentVolume -= 0.01f;
                currentVolumeInstance.setVolume(currentVolume);
                yield return new WaitForSeconds(0.05f);
                StartCoroutine(ToggleSoundMuteCoroutine(sound, false, true));
            }
        }
    }

    public void ToggleSoundMute(string sound, bool turningOn)
    {
        StartCoroutine(ToggleSoundMuteCoroutine(sound, turningOn, false));
    }
}




