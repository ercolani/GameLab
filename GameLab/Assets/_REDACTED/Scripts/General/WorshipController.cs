using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioController;

public class WorshipController : MonoBehaviour
{
    [SerializeField]
    private CandleWickController wickController;

    [SerializeField]
    private StalkPlayer stalkPlayer;
    
    [SerializeField]
    private KeyCode worshipInput;

    [SerializeField]
    private bool canWorship;

    [SerializeField]
    private float worshipCheckDelay;

    [SerializeField]
    private bool isWorshipping;

    private float timeOfWorship = 0f;

    private void OnEnable()
    {
        stalkPlayer.OnShadowPersonStalk += ToggleWorship;
    }

    private void OnDisable()
    {
        stalkPlayer.OnShadowPersonStalk -= ToggleWorship;
    }


    private void Update()
    {
        if (canWorship)
        {
            if (Input.GetKeyDown(worshipInput))
            {
                ToggleWorshipSound(true);
                isWorshipping = true;
                timeOfWorship = Time.time;
            }
        }
    }

    public void EnableWorship()
    {
        canWorship = true;
    }

    private void ToggleWorship(bool state)
    {
        canWorship = state;
    }

    private void ToggleWorshipSound(bool state)
    {
        if (state)
        {
            //if (AudioController.RetrieveNameFromEventInstance("Candle Worship").getVolume )
            StartCoroutine(AudioController.ToggleSoundMute("Candle Worship", true, false));
        }
        float timeAfterLastWorship = Time.time - timeOfWorship;
        if (timeAfterLastWorship > worshipCheckDelay)
        {
            //stop sound
        }
       
    }

}
