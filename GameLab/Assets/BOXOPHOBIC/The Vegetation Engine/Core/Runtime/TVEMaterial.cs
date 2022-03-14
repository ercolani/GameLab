//Cristian Pop - https://boxophobic.com/

using UnityEditor;
using UnityEngine;

namespace TheVegetationEngine
{
    public class TVEMaterial
    {
        public static void SetMaterialSettings(Material material)
        {
            var shaderName = material.shader.name;

            if (!material.HasProperty("_IsTVEShader"))
            {
                return;
            }

            if (material.HasProperty("_IsVersion"))
            {
                if (material.GetInt("_IsVersion") < 500)
                {
                    if (material.HasProperty("_RenderPriority"))
                    {
                        if (material.GetInt("_RenderPriority") != 0)
                        {
                            material.SetInt("_RenderQueue", 1);
                        }
                    }

                    material.SetInt("_IsVersion", 500);
                }

                if (material.GetInt("_IsVersion") < 600)
                {
                    if (material.HasProperty("_LayerReactValue"))
                    {
                        material.SetInt("_LayerVertexValue", material.GetInt("_LayerReactValue"));
                    }

                    material.SetInt("_IsVersion", 600);
                }

                if (material.GetInt("_IsVersion") < 620)
                {
                    if (material.HasProperty("_VertexRollingMode"))
                    {
                        material.SetInt("_MotionValue_20", material.GetInt("_VertexRollingMode"));
                    }

                    material.SetInt("_IsVersion", 620);
                }
            }

            // Set Internal Render Values
            if (material.HasProperty("_RenderMode"))
            {
                material.SetInt("_render_mode", material.GetInt("_RenderMode"));
            }

            if (material.HasProperty("_RenderCull"))
            {
                material.SetInt("_render_cull", material.GetInt("_RenderCull"));
            }

            if (material.HasProperty("_RenderNormals"))
            {
                material.SetInt("_render_normals", material.GetInt("_RenderNormals"));
            }

            if (material.HasProperty("_RenderZWrite"))
            {
                material.SetInt("_render_zw", material.GetInt("_RenderZWrite"));
            }

            if (material.HasProperty("_RenderClip"))
            {
                material.SetInt("_render_clip", material.GetInt("_RenderClip"));
            }

            // Set Render Mode
            if (material.HasProperty("_RenderMode"))
            {
                int mode = material.GetInt("_RenderMode");
                int zwrite = material.GetInt("_RenderZWrite");
                int queue = 0;
                int priority = 0;

                if (material.HasProperty("_RenderQueue") && material.HasProperty("_RenderPriority"))
                {
                    queue = material.GetInt("_RenderQueue");
                    priority = material.GetInt("_RenderPriority");
                }

                // Opaque
                if (mode == 0)
                {
                    if (queue != 2)
                    {
                        material.SetOverrideTag("RenderType", "AlphaTest");
                        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest + priority;
                    }

                    // Standard and Universal Render Pipeline
                    material.SetInt("_render_src", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_render_dst", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_render_zw", 1);
                    material.SetInt("_render_premul", 0);

                    // Set Main Color alpha to 1
                    if (material.HasProperty("_MainColor"))
                    {
                        var mainColor = material.GetColor("_MainColor");
                        material.SetColor("_MainColor", new Color(mainColor.r, mainColor.g, mainColor.b, 1.0f));
                    }

                    // HD Render Pipeline
                    material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
                    material.DisableKeyword("_ENABLE_FOG_ON_TRANSPARENT");

                    material.DisableKeyword("_BLENDMODE_ALPHA");
                    material.DisableKeyword("_BLENDMODE_ADD");
                    material.DisableKeyword("_BLENDMODE_PRE_MULTIPLY");

                    material.SetInt("_RenderQueueType", 1);
                    material.SetInt("_SurfaceType", 0);
                    material.SetInt("_BlendMode", 0);
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_AlphaSrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_AlphaDstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.SetInt("_TransparentZWrite", 1);
                    material.SetInt("_ZTestDepthEqualForOpaque", 3);
                    material.SetInt("_ZTestGBuffer", 4);
                    material.SetInt("_ZTestTransparent", 4);
                }
                // Transparent
                else
                {
                    if (queue != 2)
                    {
                        material.SetOverrideTag("RenderType", "Transparent");
                        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent + priority;
                    }

                    // Standard and Universal Render Pipeline
                    material.SetInt("_render_src", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_render_dst", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_render_premul", 0);

                    // HD Render Pipeline
                    material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                    material.EnableKeyword("_ENABLE_FOG_ON_TRANSPARENT");

                    material.EnableKeyword("_BLENDMODE_ALPHA");
                    material.DisableKeyword("_BLENDMODE_ADD");
                    material.DisableKeyword("_BLENDMODE_PRE_MULTIPLY");

                    material.SetInt("_RenderQueueType", 5);
                    material.SetInt("_SurfaceType", 1);
                    material.SetInt("_BlendMode", 0);
                    material.SetInt("_SrcBlend", 1);
                    material.SetInt("_DstBlend", 10);
                    material.SetInt("_AlphaSrcBlend", 1);
                    material.SetInt("_AlphaDstBlend", 10);
                    material.SetInt("_ZWrite", zwrite);
                    material.SetInt("_TransparentZWrite", zwrite);
                    material.SetInt("_ZTestDepthEqualForOpaque", 4);
                    material.SetInt("_ZTestGBuffer", 4);
                    material.SetInt("_ZTestTransparent", 4);
                }
            }

            // Set Receive Mode in HDRP
            if (material.GetTag("RenderPipeline", false) == "HDRenderPipeline")
            {
                if (material.HasProperty("_RenderDecals"))
                {
                    int decals = material.GetInt("_RenderDecals");

                    if (decals == 0)
                    {
                        material.EnableKeyword("_DISABLE_DECALS");
                    }
                    else
                    {
                        material.DisableKeyword("_DISABLE_DECALS");
                    }
                }

                if (material.HasProperty("_RenderSSR"))
                {
                    int ssr = material.GetInt("_RenderSSR");

                    if (ssr == 0)
                    {
                        material.EnableKeyword("_DISABLE_SSR");

                        material.SetInt("_StencilRef", 0);
                        material.SetInt("_StencilRefDepth", 0);
                        material.SetInt("_StencilRefDistortionVec", 4);
                        material.SetInt("_StencilRefGBuffer", 2);
                        material.SetInt("_StencilRefMV", 32);
                        material.SetInt("_StencilWriteMask", 6);
                        material.SetInt("_StencilWriteMaskDepth", 8);
                        material.SetInt("_StencilWriteMaskDistortionVec", 4);
                        material.SetInt("_StencilWriteMaskGBuffer", 14);
                        material.SetInt("_StencilWriteMaskMV", 40);
                    }
                    else
                    {
                        material.DisableKeyword("_DISABLE_SSR");

                        material.SetInt("_StencilRef", 0);
                        material.SetInt("_StencilRefDepth", 8);
                        material.SetInt("_StencilRefDistortionVec", 4);
                        material.SetInt("_StencilRefGBuffer", 10);
                        material.SetInt("_StencilRefMV", 40);
                        material.SetInt("_StencilWriteMask", 6);
                        material.SetInt("_StencilWriteMaskDepth", 8);
                        material.SetInt("_StencilWriteMaskDistortionVec", 4);
                        material.SetInt("_StencilWriteMaskGBuffer", 14);
                        material.SetInt("_StencilWriteMaskMV", 40);
                    }
                }
            }

            // Set Cull Mode
            if (material.HasProperty("_RenderCull"))
            {
                int cull = material.GetInt("_RenderCull");

                material.SetInt("_CullMode", cull);
                material.SetInt("_TransparentCullMode", cull);
                material.SetInt("_CullModeForward", cull);

                // Needed for HD Render Pipeline
                material.DisableKeyword("_DOUBLESIDED_ON");
            }

            // Set Clip Mode
            if (material.HasProperty("_RenderClip"))
            {
                int clip = material.GetInt("_RenderClip");
                float cutoff = material.GetFloat("_Cutoff");

                if (clip == 0)
                {
                    material.DisableKeyword("TVE_ALPHA_CLIP");

                    // HD Render Pipeline
                    material.SetInt("_AlphaCutoffEnable", 0);
                }
                else
                {
                    material.EnableKeyword("TVE_ALPHA_CLIP");

                    // HD Render Pipeline
                    material.SetInt("_AlphaCutoffEnable", 1);
                }

                material.SetFloat("_render_cutoff", cutoff);

                // HD Render Pipeline
                material.SetFloat("_AlphaCutoff", cutoff);
                material.SetFloat("_AlphaCutoffPostpass", cutoff);
                material.SetFloat("_AlphaCutoffPrepass", cutoff);
                material.SetFloat("_AlphaCutoffShadow", cutoff);
            }

            // Set Normals Mode
            if (material.HasProperty("_RenderNormals"))
            {
                int normals = material.GetInt("_RenderNormals");

                // Standard, Universal, HD Render Pipeline
                // Flip 0
                if (normals == 0)
                {
                    material.SetVector("_render_normals_options", new Vector4(-1, -1, -1, 0));
                    material.SetVector("_DoubleSidedConstants", new Vector4(-1, -1, -1, 0));
                }
                // Mirror 1
                else if (normals == 1)
                {
                    material.SetVector("_render_normals_options", new Vector4(1, 1, -1, 0));
                    material.SetVector("_DoubleSidedConstants", new Vector4(1, 1, -1, 0));
                }
                // None 2
                else if (normals == 2)
                {
                    material.SetVector("_render_normals_options", new Vector4(1, 1, 1, 0));
                    material.SetVector("_DoubleSidedConstants", new Vector4(1, 1, 1, 0));
                }
            }

            // Assign Default HD Foliage profile
            if (material.HasProperty("_SubsurfaceDiffusion"))
            {
                if (material.GetFloat("_SubsurfaceDiffusion") == 0)
                {
                    material.SetFloat("_SubsurfaceDiffusion", 3.5648174285888672f);
                    material.SetVector("_SubsurfaceDiffusion_asset", new Vector4(228889264007084710000000000000000000000f, 0.000000000000000000000000012389357880079404f, 0.00000000000000000000000000000000000076932702684439582f, 0.00018220426863990724f));
                    material.SetVector("_SubsurfaceDiffusion_Asset", new Vector4(228889264007084710000000000000000000000f, 0.000000000000000000000000012389357880079404f, 0.00000000000000000000000000000000000076932702684439582f, 0.00018220426863990724f));
                }

#if UNITY_EDITOR
                // Fix when the HDRP project is a new empty template
                if (AssetDatabase.GUIDToAssetPath("78322c7f82657514ebe48203160e3f39") == "" && AssetDatabase.GUIDToAssetPath("879ffae44eefa4412bb327928f1a96dd") != "")
                {
                    material.SetFloat("_SubsurfaceDiffusion", 2.6486763954162598f);
                    material.SetVector("_SubsurfaceDiffusion_asset", new Vector4(-36985449400010195000000f, 20.616847991943359f, -0.00000000000000000000000000052916750040661612f, -1352014335655804900f));
                    material.SetVector("_SubsurfaceDiffusion_Asset", new Vector4(-36985449400010195000000f, 20.616847991943359f, -0.00000000000000000000000000052916750040661612f, -1352014335655804900f));
                }
#endif
            }


            // Set Detail Mode
            if (material.HasProperty("_DetailMode") && material.HasProperty("_SecondColor"))
            {
                if (material.GetInt("_DetailMode") == 0)
                {
                    material.EnableKeyword("TVE_DETAIL_MODE_OFF");
                    material.DisableKeyword("TVE_DETAIL_MODE_ON");
                }
                else
                {
                    material.DisableKeyword("TVE_DETAIL_MODE_OFF");
                    material.EnableKeyword("TVE_DETAIL_MODE_ON");
                }
            }

            if (material.HasProperty("_DetailBlendMode") && material.HasProperty("_SecondColor"))
            {
                if (material.GetInt("_DetailBlendMode") == 0)
                {
                    material.EnableKeyword("TVE_DETAIL_BLEND_OVERLAY");
                    material.DisableKeyword("TVE_DETAIL_BLEND_REPLACE");
                }
                else
                {
                    material.DisableKeyword("TVE_DETAIL_BLEND_OVERLAY");
                    material.EnableKeyword("TVE_DETAIL_BLEND_REPLACE");
                }
            }

            // Set Detail Type
            if (material.HasProperty("_DetailTypeMode") && material.HasProperty("_SecondColor"))
            {
                if (material.GetInt("_DetailTypeMode") == 0)
                {
                    material.EnableKeyword("TVE_DETAIL_TYPE_VERTEX_BLUE");
                    material.DisableKeyword("TVE_DETAIL_TYPE_PROJECTION");
                }
                else
                {
                    material.DisableKeyword("TVE_DETAIL_TYPE_VERTEX_BLUE");
                    material.EnableKeyword("TVE_DETAIL_TYPE_PROJECTION");
                }
            }

            // Set GI Mode
            if (material.HasProperty("_EmissiveFlagMode"))
            {
                int flag = material.GetInt("_EmissiveFlagMode");

                if (flag == 0)
                {
                    material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
                }
                else if (flag == 10)
                {
                    material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.AnyEmissive;
                }
                else if (flag == 20)
                {
                    material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.BakedEmissive;
                }
                else if (flag == 30)
                {
                    material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                }
            }

            // Set Batching Mode
            if (material.HasProperty("_VertexDataMode"))
            {
                int batching = material.GetInt("_VertexDataMode");

                if (batching == 0)
                {
                    material.DisableKeyword("TVE_VERTEX_DATA_BATCHED");
                }
                else
                {
                    material.EnableKeyword("TVE_VERTEX_DATA_BATCHED");
                }
            }

            //Set Pivots Mode
            if (material.HasProperty("_VertexPivotMode"))
            {
                material.SetInt("_vertex_pivot_mode", material.GetInt("_VertexPivotMode"));
            }

            // Enable Nature Rendered support
            material.SetOverrideTag("NatureRendererInstancing", "True");

            // Set Legacy props for external bakers
            if (material.HasProperty("_MainColor"))
            {
                material.SetColor("_Color", material.GetColor("_MainColor"));
            }

            // Set BlinnPhong Spec Color
            if (material.HasProperty("_SpecColor"))
            {
                material.SetColor("_SpecColor", Color.white);
            }

            if (material.HasProperty("_MainAlbedoTex"))
            {
                material.SetTexture("_MainTex", material.GetTexture("_MainAlbedoTex"));
            }

            if (material.HasProperty("_MainNormalTex"))
            {
                material.SetTexture("_BumpMap", material.GetTexture("_MainNormalTex"));
            }

            if (material.HasProperty("_MainUVs"))
            {
                material.SetTextureScale("_MainTex", new Vector2(material.GetVector("_MainUVs").x, material.GetVector("_MainUVs").y));
                material.SetTextureOffset("_MainTex", new Vector2(material.GetVector("_MainUVs").z, material.GetVector("_MainUVs").w));
            }

            // Set internals for impostor baking 
            if (material.HasProperty("_VertexOcclusionColor"))
            {
                material.SetInt("_HasOcclusion", 1);
            }
            else
            {
                material.SetInt("_HasOcclusion", 0);
            }

            if (material.HasProperty("_GradientColorOne"))
            {
                material.SetInt("_HasGradient", 1);
            }
            else
            {
                material.SetInt("_HasGradient", 0);
            }

            if (material.HasProperty("_EmissiveColor"))
            {
                material.SetInt("_HasEmissive", 1);
            }
            else
            {
                material.SetInt("_HasEmissive", 0);
            }
        }

        public static void SetElementSettings(Material material)
        {
            if (material.HasProperty("_IsVersion"))
            {
                if (material.GetInt("_IsVersion") < 600)
                {
                    if (material.HasProperty("_ElementLayerValue"))
                    {
                        var oldLayer = material.GetInt("_ElementLayerValue");

                        if (material.GetInt("_ElementLayerValue") > 0)
                        {
                            material.SetInt("_ElementLayerMask", (int)Mathf.Pow(2, oldLayer));
                            material.SetInt("_ElementLayerValue", -1);
                        }
                    }

                    if (material.HasProperty("_InvertX"))
                    {
                        material.SetInt("_ElementInvertMode", material.GetInt("_InvertX"));
                    }

                    if (material.HasProperty("_ElementFadeSupport"))
                    {
                        material.SetInt("_ElementVolumeFadeMode", material.GetInt("_ElementFadeSupport"));
                    }

                    material.SetInt("_IsVersion", 600);
                }
            }

            if (material.HasProperty("_ElementLayerMask"))
            {
                var layers = material.GetInt("_ElementLayerMask");

                if (layers > 1)
                {
                    material.SetInt("_ElementLayerMessage", 1);
                }
                else
                {
                    material.SetInt("_ElementLayerMessage", 0);
                }

                if (layers == -1)
                {
                    material.SetInt("_ElementLayerWarning", 1);
                }
                else
                {
                    material.SetInt("_ElementLayerWarning", 0);
                }
            }

            if (material.HasProperty("_ElementColorsMode"))
            {
                var effect = material.GetInt("_ElementColorsMode");

                material.SetInt("_render_colormask", effect);
            }

            if (material.HasProperty("_ElementInteractionMode"))
            {
                var effect = material.GetInt("_ElementInteractionMode");

                material.SetInt("_render_colormask", effect);
            }

            if (material.HasProperty("_ElementBlendA"))
            {
                var blend = material.GetInt("_ElementBlendA");

                if (blend == 0)
                {
                    material.SetInt("_render_src", (int)UnityEngine.Rendering.BlendMode.DstColor);
                    material.SetInt("_render_dst", (int)UnityEngine.Rendering.BlendMode.Zero);
                }
                else
                {
                    material.SetInt("_render_src", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_render_dst", (int)UnityEngine.Rendering.BlendMode.One);
                }
            }

            if (material.HasProperty("_ElementRaycastMode"))
            {
                var raycast = material.GetInt("_ElementRaycastMode");

                if (raycast == 1)
                {
                    material.enableInstancing = false;
                }
            }
        }

        public static void SetImpostorSettings(Material material)
        {
            var shaderName = material.shader.name;

            int queue = 0;
            int priority = 0;

            if (material.HasProperty("_RenderQueue") && material.HasProperty("_RenderPriority"))
            {
                queue = material.GetInt("_RenderQueue");
                priority = material.GetInt("_RenderPriority");
            }

            if (queue != 2)
            {
                material.SetOverrideTag("RenderType", "AlphaTest");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest + priority;
            }

            // Assign Default HD Foliage profile
            if (material.HasProperty("_SubsurfaceDiffusion"))
            {
                if (material.GetFloat("_SubsurfaceDiffusion") == 0)
                {
                    material.SetFloat("_SubsurfaceDiffusion", 3.5648174285888672f);
                    material.SetVector("_SubsurfaceDiffusion_asset", new Vector4(228889264007084710000000000000000000000f, 0.000000000000000000000000012389357880079404f, 0.00000000000000000000000000000000000076932702684439582f, 0.00018220426863990724f));
                    material.SetVector("_SubsurfaceDiffusion_Asset", new Vector4(228889264007084710000000000000000000000f, 0.000000000000000000000000012389357880079404f, 0.00000000000000000000000000000000000076932702684439582f, 0.00018220426863990724f));
                }

#if UNITY_EDITOR
                // Fix when the HDRP project is a new empty template
                if (AssetDatabase.GUIDToAssetPath("78322c7f82657514ebe48203160e3f39") == "" && AssetDatabase.GUIDToAssetPath("879ffae44eefa4412bb327928f1a96dd") != "")
                {
                    material.SetFloat("_SubsurfaceDiffusion", 2.6486763954162598f);
                    material.SetVector("_SubsurfaceDiffusion_asset", new Vector4(-36985449400010195000000f, 20.616847991943359f, -0.00000000000000000000000000052916750040661612f, -1352014335655804900f));
                    material.SetVector("_SubsurfaceDiffusion_Asset", new Vector4(-36985449400010195000000f, 20.616847991943359f, -0.00000000000000000000000000052916750040661612f, -1352014335655804900f));
                }
#endif
            }

            // Set Material Type
            if (material.HasProperty("_MaterialType"))
            {
                if (material.GetInt("_MaterialType") == 10)
                {
                    material.EnableKeyword("TVE_IS_VEGETATION_SHADER");
                    material.DisableKeyword("TVE_IS_GRASS_SHADER");
                }
                else if (material.GetInt("_MaterialType") == 20)
                {
                    material.DisableKeyword("TVE_IS_VEGETATION_SHADER");
                    material.EnableKeyword("TVE_IS_GRASS_SHADER");
                }
            }

            // Set Detail Mode
            if (material.HasProperty("_DetailMode") && material.HasProperty("_SecondColor"))
            {
                if (material.GetInt("_DetailMode") == 0)
                {
                    material.EnableKeyword("TVE_DETAIL_MODE_OFF");
                    material.DisableKeyword("TVE_DETAIL_MODE_ON");
                }
                else
                {
                    material.DisableKeyword("TVE_DETAIL_MODE_OFF");
                    material.EnableKeyword("TVE_DETAIL_MODE_ON");
                }
            }

            if (material.HasProperty("_DetailBlendMode") && material.HasProperty("_SecondColor"))
            {
                if (material.GetInt("_DetailBlendMode") == 0)
                {
                    material.EnableKeyword("TVE_DETAIL_BLEND_OVERLAY");
                    material.DisableKeyword("TVE_DETAIL_BLEND_REPLACE");
                }
                else
                {
                    material.DisableKeyword("TVE_DETAIL_BLEND_OVERLAY");
                    material.EnableKeyword("TVE_DETAIL_BLEND_REPLACE");
                }
            }

            // Set GI Mode
            if (material.HasProperty("_EmissiveFlagMode"))
            {
                int flag = material.GetInt("_EmissiveFlagMode");

                if (flag == 0)
                {
                    material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
                }
                else if (flag == 10)
                {
                    material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.AnyEmissive;
                }
                else if (flag == 20)
                {
                    material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.BakedEmissive;
                }
                else if (flag == 30)
                {
                    material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                }
            }

            // Enable Nature Rendered support
            material.SetOverrideTag("NatureRendererInstancing", "True");

            if (shaderName.Contains("Translucency"))
            {
                material.SetFloat("_Translucency", material.GetFloat("_TranslucencyIntensityValue"));
                material.SetFloat("_TransNormalDistortion", material.GetFloat("_TranslucencyNormalValue"));
                material.SetFloat("_TransScattering", material.GetFloat("_TranslucencyScatteringValue"));
                material.SetFloat("_TransDirect", material.GetFloat("_TranslucencyDirectValue"));
                material.SetFloat("_TransAmbient", material.GetFloat("_TranslucencyAmbientValue"));
                material.SetFloat("_TransShadow", material.GetFloat("_TranslucencyShadowValue"));
            }

            // Set Internal shader type
            if (shaderName.Contains("Simple Lit"))
            {
                material.SetInt("_IsSimpleShader", 1);
                material.SetInt("_IsStandardShader", 0);
                material.SetInt("_IsSubsurfaceShader", 0);
            }

            if (shaderName.Contains("Standard Lit"))
            {
                material.SetInt("_IsSimpleShader", 0);
                material.SetInt("_IsStandardShader", 1);
                material.SetInt("_IsSubsurfaceShader", 0);
            }

            if (shaderName.Contains("Subsurface Lit"))
            {
                material.SetInt("_IsSimpleShader", 0);
                material.SetInt("_IsStandardShader", 0);
                material.SetInt("_IsSubsurfaceShader", 1);
            }
        }
    }
}
