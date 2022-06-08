using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TintEditor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Volume volume;

    [SerializeField]
    private int offset = 0;

    private WhiteBalance whiteBalance;

    private float tintTimer = 0;

    private float startTint = 0;

    private float tintValue = 0;

    private float valueChange = 1f;

    void Start()
    {

        VolumeProfile profile = volume.sharedProfile;
        volume.profile.TryGet(out whiteBalance);
        startTint = whiteBalance.tint.value;
        tintValue = startTint;
    }

    // Update is called once per frame
    void Update()
    {
        tintTimer += Time.deltaTime;
        if (tintTimer > 0.1f)
        {
            if(tintValue > startTint + offset)
            {
                valueChange = -valueChange;
            }
            else if (tintValue < startTint - offset)
            {
                valueChange = -valueChange;
            }
            tintValue += valueChange;
            Debug.LogError(whiteBalance.tint.value);
            tintTimer = 0;
            whiteBalance.tint.value = tintValue;
            volume.profile.Reset();
        }
    }
}
