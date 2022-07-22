using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioController;

public class FadeVolume : MonoBehaviour
{
    public void ToggleSoundMute(string sound, bool turningOn)
    {
        StartCoroutine(ToggleSoundMuteCoroutine(sound, turningOn, false));
    }

    public IEnumerator ToggleSoundMuteCoroutine(string sound, bool turningOn, bool isLooping)
    {
        if (!isLooping)
        {
            AudioController.currentVolumeInstance = RetrieveNameFromEventInstance(sound);
            AudioController.currentVolumeInstance.getVolume(out currentVolume);
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
                currentVolume -= 0.1f;
                currentVolumeInstance.setVolume(currentVolume);
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(ToggleSoundMuteCoroutine(sound, true, true));
            }
        }
    }
}
