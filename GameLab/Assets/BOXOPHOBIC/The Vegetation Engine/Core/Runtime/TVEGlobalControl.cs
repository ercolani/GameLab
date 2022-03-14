﻿// Cristian Pop - https://boxophobic.com/

using UnityEngine;
using Boxophobic.StyledGUI;
using UnityEngine.Serialization;

namespace TheVegetationEngine
{
    [ExecuteInEditMode]
    [AddComponentMenu("BOXOPHOBIC/The Vegetation Engine/TVE Global Control")]
    public class TVEGlobalControl : StyledMonoBehaviour
    {
        [StyledBanner(0.890f, 0.745f, 0.309f, "Global Control", "", "https://docs.google.com/document/d/145JOVlJ1tE-WODW45YoJ6Ixg23mFc56EnB_8Tbwloz8/edit#heading=h.q3sme6mi00gy")]
        public bool styledBanner;

        [StyledCategory("Light Settings", 5, 10)]
        public bool lightCat;

        [Tooltip("Sets the main light used as the sun in the scene.")]
        public Light mainLight;

        [StyledCategory("Season Settings")]
        public bool seasonCat;

        [Tooltip("Use the Seasons slider to control the element properties when the element is set to Seasons mode.")]
        [StyledRangeOptions("Season Control", 0, 4, new string[] { "Winter", "Spring", "Summer", "Autumn", "Winter" })]
        public float seasonControl = 2f;

        [StyledCategory("Global Settings")]
        public bool defaultCat;

        [StyledMessage("Info", "The global settings provide a free alternative to the elements which will create render textures, using up more memory! Perfect for shaders compiled without support for colors, extras or vertex elements!", 0, 10)]
        public bool styledMessage = true;

        [Tooltip("Controls the global tinting color. Color Alpha is used to multiply the material colors (Intensity = 0) or to replace the material colors (Intensity = 1).")]
        [ColorUsage(true, true)]
        public Color globalColor = new Color(0.5f, 0.5f, 0.5f, 0);
        [Tooltip("Controls the global alpha fading.")]
        [Range(0.0f, 1.0f)]
        public float globalAlpha = 1.0f;
        [Tooltip("Controls the global overlay intensity.")]
        [Range(0.0f, 1.0f)]
        public float globalOverlay = 0.0f;
        [Tooltip("Controls the smoothness on vegetation and props for a wet look.")]
        [Range(0.0f, 1.0f)]
        public float globalWetness = 0.0f;
        [Tooltip("Controls the global emissive intensity.")]
        [Range(0.0f, 1.0f)]
        public float globalEmissive = 1;
        [Tooltip("Controls the global subsurface intensity.")]
        [Range(0.0f, 1.0f)]
        public float globalSubsurface = 1;
        [Tooltip("Controls the global size value.")]
        [Range(0.0f, 1.0f)]
        public float globalSizeFade = 1.0f;

        [StyledCategory("Overlay Settings")]
        public bool overlayCat;

        [Tooltip("Controls the global overlay color.")]
        [ColorUsage(false, true)]
        public Color overlayColor = Color.white;
        [Tooltip("Controls the global overlay smoothness.")]
        [Range(0.0f, 1.0f)]
        public float overlaySmoothness = 0.5f;

        [StyledMessage("Info", "The overlay texures are disabled for the built-in shaders. They will only be used for shaders compiled with the Overlay Quality option set to Standard on the Amplify Base function!", 10, 10)]
        public bool overlayMessage = true;

        [Tooltip("Sets the global overlay albedo texture.")]
        public Texture2D overlayAlbedo;
        [Tooltip("Sets the global overlay normal texture.")]
        public Texture2D overlayNormal;
        [Tooltip("Controls the global overlay albedo and normal texture tilling.")]
        [Range(0.0f, 10.0f)]
        public float overlayTilling = 1.0f;
        [Tooltip("Controls the global overlay normal texture intensity.")]
        [Range(-8.0f, 8.0f)]
        public float overlayNormalScale = 1.0f;

        [StyledCategory("Noise Settings")]
        public bool noiseCat;

        [Tooltip("Sets the global world space 3D noise texture used for material noise settings.")]
        public Texture3D worldNoiseTexture;

        [Tooltip("Sets the global screen space 3D noise texture used for camera distance and glancing angle fade.")]
        public Texture3D screenNoiseTexture;
        [Tooltip("Controls the global screen space 3D noise texture scale used for camera distance and glancing angle fade.")]
        [Range(0.0f, 20.0f)]
        public float screenNoiseScale = 5.0f;

        [StyledCategory("Fade Settings")]
        public bool fadeCat;

        [Tooltip("Controls the Size Fade paramters on the materials. With higher values, the fade will happen at a greater distance.")]
        [FormerlySerializedAs("distanceFadeBias")]
        public float sizeFadeDistanceBias = 1.0f;
        [Tooltip("Controls the Camera fade distance in world units.")]
        [FormerlySerializedAs("cameraFadeBias")]
        public float cameraFadeDistance = 1.0f;
        [Tooltip("Controls the Details Motion (Flutter) fade out distance in world units.")]
        [FormerlySerializedAs("motionFadeBias")]
        public float motionFadeDistance = 100.0f;

        [StyledSpace(10)]
        public bool styledSpace0;

        void Start()
        {
            gameObject.name = "Global Control";
            gameObject.transform.SetSiblingIndex(2);

            if (mainLight == null)
            {
                SetGlobalLightingMainLight();
            }

            if (worldNoiseTexture == null)
            {
                worldNoiseTexture = CreateNoiseTexture("Internal WorldTex3D");
            }

            if (screenNoiseTexture == null)
            {
                screenNoiseTexture = CreateNoiseTexture("Internal ScreenTex3D");
            }

            SetGlobalShaderProperties();
        }

        void Update()
        {
            SetGlobalShaderProperties();
        }

        void SetGlobalShaderProperties()
        {
            if (mainLight != null)
            {
                //var intensity = Mathf.Clamp01(mainLight.intensity * mainLightSubsurface);
                var mainLightParams = new Vector4(mainLight.color.r, mainLight.color.g, mainLight.color.b, globalSubsurface);

                Shader.SetGlobalVector("TVE_MainLightParams", mainLightParams);
                Shader.SetGlobalVector("TVE_MainLightDirection", Vector4.Normalize(-mainLight.transform.forward));
            }
            else
            {
                var mainLightParams = new Vector4(1, 1, 1, globalSubsurface);

                Shader.SetGlobalVector("TVE_MainLightParams", mainLightParams);
                Shader.SetGlobalVector("TVE_MainLightDirection", new Vector4(0, 1, 0, 0));
            }

            float seasonLerp = 0;

            if (seasonControl >= 0 && seasonControl < 1)
            {
                seasonLerp = seasonControl;
                Shader.SetGlobalVector("TVE_SeasonOptions", new Vector4(1, 0, 0, 0));
            }
            else if (seasonControl >= 1 && seasonControl < 2)
            {
                seasonLerp = seasonControl - 1.0f;
                Shader.SetGlobalVector("TVE_SeasonOptions", new Vector4(0, 1, 0, 0));
            }
            else if (seasonControl >= 2 && seasonControl < 3)
            {
                seasonLerp = seasonControl - 2.0f;
                Shader.SetGlobalVector("TVE_SeasonOptions", new Vector4(0, 0, 1, 0));
            }
            else if (seasonControl >= 3 && seasonControl <= 4)
            {
                seasonLerp = seasonControl - 3.0f;
                Shader.SetGlobalVector("TVE_SeasonOptions", new Vector4(0, 0, 0, 1));
            }

            var smoothLerp = Mathf.SmoothStep(0, 1, seasonLerp);
            Shader.SetGlobalFloat("TVE_SeasonLerp", smoothLerp);

            if (QualitySettings.activeColorSpace == ColorSpace.Linear)
            {
                Shader.SetGlobalVector("TVE_ColorsParams", globalColor.linear);
            }
            else
            {
                Shader.SetGlobalVector("TVE_ColorsParams", globalColor);
            }

            Shader.SetGlobalFloat("TVE_OverlayValue", globalOverlay);
            Shader.SetGlobalFloat("TVE_WetnessValue", globalWetness);
            Shader.SetGlobalFloat("TVE_EmissiveValue", globalEmissive);
            Shader.SetGlobalFloat("TVE_AlphaValue", globalAlpha);
            Shader.SetGlobalFloat("TVE_SizeValue", globalSizeFade);

            Shader.SetGlobalColor("TVE_OverlayColor", overlayColor);
            Shader.SetGlobalTexture("TVE_OverlayAlbedoTex", overlayAlbedo);
            Shader.SetGlobalTexture("TVE_OverlayNormalTex", overlayNormal);
            Shader.SetGlobalFloat("TVE_OverlayUVTilling", overlayTilling);
            Shader.SetGlobalFloat("TVE_OverlayNormalValue", overlayNormalScale);
            Shader.SetGlobalFloat("TVE_OverlaySmoothness", overlaySmoothness);

            var extras = new Vector4(globalEmissive, globalWetness, globalOverlay, globalAlpha);
            Shader.SetGlobalVector("TVE_ExtrasParams", extras);

            var vertex = new Vector4(0.0f, 0.0f, 0.0f, globalSizeFade);
            Shader.SetGlobalVector("TVE_VertexParams", vertex);

            Shader.SetGlobalTexture("TVE_WorldTex3D", worldNoiseTexture);
            Shader.SetGlobalTexture("TVE_ScreenTex3D", screenNoiseTexture);
            Shader.SetGlobalFloat("TVE_ScreenTexCoord", screenNoiseScale);

            Shader.SetGlobalFloat("TVE_DistanceFadeBias", sizeFadeDistanceBias);
            Shader.SetGlobalFloat("TVE_CameraFadeStart", cameraFadeDistance * 0.5f);
            Shader.SetGlobalFloat("TVE_CameraFadeEnd", cameraFadeDistance);
            Shader.SetGlobalFloat("TVE_MotionFadeStart", motionFadeDistance * 0.5f);
            Shader.SetGlobalFloat("TVE_MotionFadeEnd", motionFadeDistance);
        }

        void SetGlobalLightingMainLight()
        {
            var allLights = FindObjectsOfType<Light>();
            var intensity = 0.0f;

            for (int i = 0; i < allLights.Length; i++)
            {
                if (allLights[i].type == LightType.Directional)
                {
                    if (allLights[i].intensity > intensity)
                    {
                        mainLight = allLights[i];
                    }
                }
            }
        }

        Texture3D CreateNoiseTexture(string name)
        {
            int size = 16;

            Texture3D texture = new Texture3D(size, size, size, TextureFormat.R8, false);
            texture.wrapMode = TextureWrapMode.Repeat;
            texture.name = name;

            Color32[] colors = new Color32[size * size * size];

            for (int z = 0; z < size; z++)
            {
                int zOffset = z * size * size;
                for (int y = 0; y < size; y++)
                {
                    int yOffset = y * size;
                    for (int x = 0; x < size; x++)
                    {
                        colors[x + yOffset + zOffset] = new Color(Random.Range(0f, 1f), 0, 0);
                    }
                }
            }

            texture.SetPixels32(colors);
            texture.Apply();

            return texture;
        }
    }
}
