using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorshipController : MonoBehaviour
{
    [SerializeField]
    private CandleWickController wickController;

    [SerializeField]
    private AudioController audioController;

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

    private void Start()
    {
        audioController.PlaySoundLooping("Candle Worship");
    }

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
            CheckWorshipSound();
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
            audioController.ToggleSoundMute("Candle Worship", true);
        }
        else
        {
            audioController.ToggleSoundMute("Candle Worship", false);
        }
    }

    private void CheckWorshipSound()
    {
        float timeAfterLastWorship = Time.time - timeOfWorship;
        if (timeAfterLastWorship > worshipCheckDelay)
        {
            isWorshipping = false;
            ToggleWorshipSound(false);
        }
    }

}
