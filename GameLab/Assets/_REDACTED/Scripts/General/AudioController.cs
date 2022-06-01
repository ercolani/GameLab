using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A singleton class controlling all in-game audio using FMOD.
/// </summary>
public class AudioController : MonoBehaviour
{
    private static AudioController _instance;

    public static AudioController Instance => _instance;

    [SerializeField]
    private static string _currentAreaCode;

    [SerializeField]
    private static string _currentEncounterType;

    [SerializeField]
    private static string _currentPuzzleThoughtType;

    [SerializeField]
    private static int _currentLineNumber;

    [SerializeField]
    private static bool _isPuzzleThought;

    private void Awake()
    {
        //singleton destroy pattern
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    /// <summary>
    /// Plays a voice line using a code from FMOD depending on if it is a puzzle thought or not.
    /// </summary>
    private static void PlayVoiceLine()
    {
        string currentLineNumberFormatted = "";

        if (_currentLineNumber < 10)
        {
            currentLineNumberFormatted = "0" + $"{_currentLineNumber}";
        }

        if (_isPuzzleThought)
        {
            //print($"event:/{_currentAreaCode}" + "_" + $"{_currentEncounterType}" + "_" + $"{_currentLineNumber}");
            FMODUnity.RuntimeManager.PlayOneShot($"event:/{_currentAreaCode}"+ "_" + $"{_currentEncounterType}" + "_" + currentLineNumberFormatted);
        }
        else
        {
            //print($"event:/{_currentAreaCode}" + "_" + $"{_currentEncounterType}" + "_" + $"{_currentPuzzleThoughtType}" + "_" + $"{_currentLineNumber}");
            FMODUnity.RuntimeManager.PlayOneShot($"event:/{_currentAreaCode}" + "_" + $"{_currentEncounterType}" + "_" + $"{_currentPuzzleThoughtType}" + "_" + currentLineNumberFormatted);
        }
    }

    /// <summary>
    /// Sets the parameters for playing the correct voice line if it is not a puzzle thought.
    /// </summary>
    private static void SetVoiceLineParameters(string areaCode, string encounterType, int currentLineNumber)
    {
        _currentAreaCode = areaCode;
        _currentEncounterType = encounterType;
        _currentLineNumber = currentLineNumber;
        PlayVoiceLine();
    }

    /// <summary>
    /// Sets the parameters for playing the correct voice line if it is a puzzle thought.
    /// </summary>
    private static void SetVoiceLineParameters(string areaCode, string encounterType, string currentPuzzleThoughtType, int currentLineNumber)
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
    public static void ParseVoiceLineCode(string input)
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
}
