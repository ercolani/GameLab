// Cristian Pop - https://boxophobic.com/

using UnityEngine;
using Boxophobic.StyledGUI;

namespace TheVegetationEngine
{
    [ExecuteInEditMode]
    [AddComponentMenu("BOXOPHOBIC/The Vegetation Engine/TVE Global Motion")]
    public class TVEGlobalMotion : StyledMonoBehaviour
    {
        [StyledBanner(0.890f, 0.745f, 0.309f, "Global Motion", "", "https://docs.google.com/document/d/145JOVlJ1tE-WODW45YoJ6Ixg23mFc56EnB_8Tbwloz8/edit#heading=h.czf8ud5bmaq2")]
        public bool styledBanner;

        [StyledCategory("Wind Settings", 5, 10)]
        public bool windCat;

        [Tooltip("Controls the global wind power.")]
        [StyledRangeOptions("Wind Power", 0, 1,  new string[] { "Min", "Medium", "Max" })]
        public float windPower = 0.5f;

        [StyledCategory("Motion Settings")]
        public bool motionCat;

        [Tooltip("Controls the minimum Primary (X), Second (Y) and Details (Z) motion amplitude and the Noise Power (W) when the wind power is set to Off. The Wind Power slider interpolates between the settings.")]
        public Vector4 minimumMotionSettings = new Vector4(0.2f, 0.4f, 0.6f, 1.4f);

        [Tooltip("Controls the maximum Primary (X), Second (Y) and Details (Z) motion amplitude and the Noise Power (W) when the wind power is set to High. The Wind Power slider interpolates between the settings.")]
        public Vector4 maximumMotionSettings = new Vector4(1.0f, 1.0f, 1.0f, 0.2f);

        [StyledCategory("Noise Settings")]
        public bool noiseCat;

        [Tooltip("Sets the texture used for wind gust and motion highlight.")]
        public Texture2D noiseTexture;
        [Tooltip("Controls the scale of the noise texture.")]
        [Range(0, 5)]
        public float noiseScale = 1;
        [Tooltip("Controls the speed of the noise texture.")]
        [Range(0, 5)]
        public float noiseSpeed = 1;

        [StyledMessage("Info", "When the Noise is linked with the Motion Direction, smooth direction animation is not supported!", 10, 0)]
        public bool styledLinkMessage = false;

        [Space(10)]
        [Tooltip("Moves the noise texture in the wind direction.")]
        public bool syncNoiseWithMotionDirection = true;

        [StyledSpace(10)]
        public bool styledSpace0;

        void Start()
        {

#if UNITY_EDITOR
            gameObject.GetComponent<MeshRenderer>().hideFlags = HideFlags.HideInInspector;
            gameObject.GetComponent<MeshFilter>().hideFlags = HideFlags.HideInInspector;
#endif

            // Disable Arrow in play mode
            if (Application.isPlaying == true)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().enabled = true;
            }

            gameObject.name = "Global Motion";
            gameObject.transform.SetSiblingIndex(0);

            if (noiseTexture == null)
            {
                noiseTexture = Resources.Load<Texture2D>("Internal NoiseTex");
            }

            SetGlobalShaderProperties();
        }

        void Update()
        {
            gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y, 0);

            SetGlobalShaderProperties();
        }

        void SetGlobalShaderProperties()
        {
            var windDirection = transform.forward;

            //float windPacked = (Mathf.Atan2(windDirection.x, windDirection.z) / Mathf.PI) * 0.5f + 0.5f;            
            //Vector3 decode = new Vector3( Mathf.Sin( (windPacked * 2 - 1) * Mathf.PI) , 0, Mathf.Cos((windPacked * 2 - 1) * Mathf.PI));
            //Debug.Log(windPacked + "   " + decode);

            // X Bending Motion // Y Second Motion // Z Flutter Motion
            Shader.SetGlobalVector("TVE_MotionParamsMin", minimumMotionSettings);
            Shader.SetGlobalVector("TVE_MotionParamsMax", maximumMotionSettings);
            Shader.SetGlobalVector("TVE_MotionParams", new Vector4(windDirection.x * 0.5f + 0.5f, windDirection.z * 0.5f + 0.5f, windPower, 0.0f));

            int sync;
            Vector2 speed;

            if (syncNoiseWithMotionDirection)
            {
                sync = 1;
                speed = new Vector2(-noiseSpeed * 0.1f * windDirection.x, -noiseSpeed * 0.1f * windDirection.z);

                styledLinkMessage = true;
            }
            else
            {
                sync = 0;
                speed = new Vector2(-noiseSpeed * 0.1f, -noiseSpeed * 0.1f);

                styledLinkMessage = false;
            }


            var scale = noiseScale * 0.05f;

            Shader.SetGlobalTexture("TVE_NoiseTex", noiseTexture);
            Shader.SetGlobalVector("TVE_NoiseParams", new Vector4(speed.x, speed.y, scale, sync));
        }
    }
}
