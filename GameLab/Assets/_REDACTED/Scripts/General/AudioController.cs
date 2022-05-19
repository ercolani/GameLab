using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    private string[] _areaCodes;
    
    [SerializeField]
    private string[] _encounterTypes;

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

    private void PlayVoiceLine()
    {
        if (_isPuzzleThought)
        {
            FMODUnity.RuntimeManager.PlayOneShot($"event:/{_currentAreaCode} + {_currentEncounterType} + { _currentLineNumber}");
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot($"event:/{_currentAreaCode} + {_currentEncounterType} + {_currentPuzzleThoughtType} + { _currentLineNumber}");
        }
    }

    public void SetVoiceLineParameters(string areaCode, string encounterType)
    {
        
    }
}
