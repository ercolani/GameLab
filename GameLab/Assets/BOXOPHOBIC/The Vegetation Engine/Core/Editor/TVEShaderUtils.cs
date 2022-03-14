//Cristian Pop - https://boxophobic.com/

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace TheVegetationEngine
{
    public class TVEShaderUtils
    {
        public static string[] RenderEngineOptions =
        {
            "Unity Default Renderer",
            "Vegetation Studio (Instanced Indirect)",
            "Vegetation Studio 1.4.5+ (Instanced Indirect)",
            "Nature Renderer (Procedural Instancing)",
            "GPU Instancer (Instanced Indirect)",
            "Instant Renderer (Instanced Indirect)",
            "Quadro Renderer (Instanced Indirect)",
            "Disable SRP Batcher Compatibility",
        };

        public static void CopyMaterialProperties(Material oldMaterial, Material newMaterial)
        {
            var oldShader = oldMaterial.shader;
            var newShader = newMaterial.shader;

            for (int i = 0; i < ShaderUtil.GetPropertyCount(oldShader); i++)
            {
                for (int j = 0; j < ShaderUtil.GetPropertyCount(newShader); j++)
                {
                    var propertyName = ShaderUtil.GetPropertyName(oldShader, i);
                    var propertyType = ShaderUtil.GetPropertyType(oldShader, i);

                    if (propertyName == ShaderUtil.GetPropertyName(newShader, j))
                    {
                        if (propertyType == ShaderUtil.ShaderPropertyType.Color || propertyType == ShaderUtil.ShaderPropertyType.Vector)
                        {
                            newMaterial.SetVector(propertyName, oldMaterial.GetVector(propertyName));
                        }

                        if (propertyType == ShaderUtil.ShaderPropertyType.Float || propertyType == ShaderUtil.ShaderPropertyType.Range)
                        {
                            if (propertyName != "_IsVersion")
                            {
                                newMaterial.SetFloat(propertyName, oldMaterial.GetFloat(propertyName));
                            }
                        }

                        if (propertyType == ShaderUtil.ShaderPropertyType.TexEnv)
                        {
                            newMaterial.SetTexture(propertyName, oldMaterial.GetTexture(propertyName));
                        }
                    }
                }
            }
        }

        public static void DrawBatchingSupport(Material material)
        {
            if (material.HasProperty("_VertexDataMode"))
            {
                var batching = material.GetInt("_VertexDataMode");

                bool toggle = false;

                if (batching > 0.5f)
                {
                    toggle = true;

                    EditorGUILayout.HelpBox("Use the Batching Support option when the object is statically batched. All vertex calculations are done in world space and features like Baked Pivots and Size options are not supported because the object pivot data is missing with static batching.", MessageType.Info);

                    GUILayout.Space(10);
                }

                toggle = EditorGUILayout.Toggle("Enable Batching Support", toggle);

                if (toggle)
                {
                    material.SetInt("_VertexDataMode", 1);
                }
                else
                {
                    material.SetInt("_VertexDataMode", 0);
                }
            }
        }

        public static void DrawDynamicSupport(Material material)
        {
            if (material.HasProperty("_VertexDynamicMode"))
            {
                var dynamic = material.GetInt("_VertexDynamicMode");

                bool toggle = false;

                if (dynamic > 0.5f)
                {
                    toggle = true;

                    if (material.HasProperty("_VertexPivotMode"))
                    {
                        GUILayout.Space(10);
                    }

                    EditorGUILayout.HelpBox("Use the Dynamic Support option when the object is moving or rotating. Usable when cutting tree or with scrollable environments! ", MessageType.Info);

                    GUILayout.Space(10);
                }

                toggle = EditorGUILayout.Toggle("Enable Dynamic Support", toggle);

                if (toggle)
                {
                    material.SetInt("_VertexDynamicMode", 1);
                }
                else
                {
                    material.SetInt("_VertexDynamicMode", 0);
                }
            }
        }

        public static void DrawPivotsSupport(Material material)
        {
            if (material.HasProperty("_VertexPivotMode"))
            {
                var pivot = material.GetInt("_VertexPivotMode");

                bool toggle = false;

                if (pivot > 0.5f)
                {
                    toggle = true;

                    EditorGUILayout.HelpBox("The Pre Baked Pivots Support feature allows for using per mesh element interaction and elements influence. The option requires pre-baked pivots on prefab conversion! Useful for large grass meshes wind and interaction.", MessageType.Info);

                    GUILayout.Space(10);
                }

                toggle = EditorGUILayout.Toggle("Enable Pre Baked Pivots ", toggle);

                if (toggle)
                {
                    material.SetInt("_VertexPivotMode", 1);
                }
                else
                {
                    material.SetInt("_VertexPivotMode", 0);
                }
            }
        }

        public static void DrawTechnicalDetails(Material material)
        {
            var shaderName = material.shader.name;

            var styleLabel = new GUIStyle(EditorStyles.label)
            {
                richText = true,
                alignment = TextAnchor.MiddleLeft,
                wordWrap = true,
            };

            if (shaderName.Contains("Standard Lit") || shaderName.Contains("Subsurface Lit") || shaderName.Contains("Translucency Lit"))
            {
                DrawTechincalLabel("Shader Complexity: Balanced", styleLabel);
            }

            if (shaderName.Contains("Simple Lit"))
            {
                DrawTechincalLabel("Shader Complexity: Optimized", styleLabel);
            }

            if (!material.HasProperty("_IsElementShader"))
            {
                if (material.GetTag("RenderPipeline", false) == "HDRenderPipeline")
                {
                    DrawTechincalLabel("Render Pipeline: High Definition Render Pipeline", styleLabel);
                }
                else if (material.GetTag("RenderPipeline", false) == "UniversalPipeline")
                {
                    DrawTechincalLabel("Render Pipeline: Universal Render Pipeline", styleLabel);
                }
                else
                {
                    DrawTechincalLabel("Render Pipeline: Standard Render Pipeline", styleLabel);
                }
            }
            else
            {
                DrawTechincalLabel("Render Pipeline: Any Render Pipeline", styleLabel);
            }

            if (material.GetTag("RenderPipeline", false) == "HDRenderPipeline")
            {
                DrawTechincalLabel("Render Target: Shader Model 4.5 or higher", styleLabel);
            }
            else
            {
                DrawTechincalLabel("Render Target: Shader Model 4.0 or higher", styleLabel);
            }

            DrawTechincalLabel("Render Queue: " + material.renderQueue.ToString(), styleLabel);

            if (shaderName.Contains("Standard Lit") || shaderName.Contains("Simple Lit"))
            {
                DrawTechincalLabel("Render Path: Rendered in both Forward and Deferred path", styleLabel);
            }

            if (shaderName.Contains("Subsurface Lit") || shaderName.Contains("Translucency Lit"))
            {
                DrawTechincalLabel("Render Path: Always rendered in Forward path", styleLabel);
            }

            if (shaderName.Contains("Standard Lit") || shaderName.Contains("Subsurface Lit") || shaderName.Contains("Translucency Lit"))
            {
                DrawTechincalLabel("Lighting Model: Physicaly Based Shading", styleLabel);
            }

            if (shaderName.Contains("Simple Lit"))
            {
                DrawTechincalLabel("Lighting Model: Blinn Phong Shading", styleLabel);
            }

            if (shaderName.Contains("Standard Lit") && (shaderName.Contains("Cross") || shaderName.Contains("Grass") || shaderName.Contains("Leaf")))
            {
                DrawTechincalLabel("Subsurface Model: Transmission Approximation + View Based Light Scattering", styleLabel);
            }

            if (shaderName.Contains("Subsurface Lit") && (shaderName.Contains("Cross") || shaderName.Contains("Grass") || shaderName.Contains("Leaf")))
            {
                DrawTechincalLabel("Subsurface Model: Transmission + View Based Light Scattering", styleLabel);
            }

            if (shaderName.Contains("Simple Lit") && (shaderName.Contains("Cross") || shaderName.Contains("Grass") || shaderName.Contains("Leaf")))
            {
                DrawTechincalLabel("Subsurface Model: Transmission Approximation + View Based Light Scattering", styleLabel);
            }

            if (material.HasProperty("_IsColorsElement"))
            {
                DrawTechincalLabel("Render Buffer: Rendered by the Volume Colors data", styleLabel);
            }

            if (material.HasProperty("_IsExtrasElement"))
            {
                DrawTechincalLabel("Render Buffer: Rendered by the Volume Extras data", styleLabel);
            }

            if (material.HasProperty("_IsMotionElement"))
            {
                DrawTechincalLabel("Render Buffer: Rendered by the Volume Motion data", styleLabel);
            }

            if (material.HasProperty("_IsVertexElement"))
            {
                DrawTechincalLabel("Render Buffer: Rendered by the Volume Vertex data", styleLabel);
            }

            if (material.HasProperty("_IsPropShader"))
            {
                DrawTechincalLabel("Batching Support: Yes", styleLabel);
            }
            else if (material.HasProperty("_IsTVEAIShader") || material.HasProperty("_IsElementShader"))
            {
                DrawTechincalLabel("Batching Support: No", styleLabel);
            }
            else
            {
                DrawTechincalLabel("Batching Support: Yes, with limited features", styleLabel);
            }
        }

        public static void DrawTechincalLabel(string label, GUIStyle style)
        {
            GUI.enabled = false;
            GUILayout.Label("<size=10>" + label + "</size>", style);
            GUI.enabled = true;
        }

        public static void DrawCopySettingsFromGameObject(Material material)
        {
            GameObject go = null;
            go = (GameObject)EditorGUILayout.ObjectField("Copy From GameObject", go, typeof(GameObject), true);

            if (go != null)
            {
                var oldMaterials = go.GetComponent<MeshRenderer>().sharedMaterials;

                for (int i = 0; i < oldMaterials.Length; i++)
                {
                    var oldMaterial = oldMaterials[i];

                    if (oldMaterial != null)
                    {
                        CopyMaterialProperties(oldMaterial, material);
                        material.SetFloat("_IsInitialized", 1);
                        go = null;
                    }
                }
            }
        }

        public static void DrawRenderQueue(Material material, MaterialEditor materialEditor)
        {
            if (material.HasProperty("_RenderQueue") && material.HasProperty("_RenderPriority"))
            {
                var queue = material.GetInt("_RenderQueue");
                var priority = material.GetInt("_RenderPriority");

                queue = EditorGUILayout.Popup("Render Queue Mode", queue, new string[] { "Auto", "Priority", "User Defined" });

                if (queue == 0)
                {
                    priority = 0;
                }
                else if (queue == 1)
                {
                    priority = EditorGUILayout.IntSlider("Render Priority", priority, -100, 100);
                }
                else
                {
                    priority = 0;
                    materialEditor.RenderQueueField();
                }

                material.SetInt("_RenderQueue", queue);
                material.SetInt("_RenderPriority", priority);
            }
        }

        public static void DrawPoweredByTheVegetationEngine()
        {
            var styleLabelCentered = new GUIStyle(EditorStyles.label)
            {
                richText = true,
                alignment = TextAnchor.MiddleCenter,
            };

            Rect lastRect = GUILayoutUtility.GetLastRect();
            EditorGUI.DrawRect(new Rect(0, lastRect.yMax, 1000, 1), new Color(0, 0, 0, 0.4f));

            GUILayout.Space(15);

            DrawTechincalLabel("Powered by The Vegetation Engine", styleLabelCentered);

            Rect labelRect = GUILayoutUtility.GetLastRect();

            if (GUI.Button(labelRect, "", new GUIStyle()))
            {
                Application.OpenURL("http://u3d.as/1H9u");
            }

            GUILayout.Space(10);
        }

        public static List<string> GetCoreShaderPaths()
        {
            var coreShaderPaths = new List<string>();

            var allShaderPaths = Directory.GetFiles("Assets/", "*.shader", SearchOption.AllDirectories);

            for (int i = 0; i < allShaderPaths.Length; i++)
            {
                if (IsValidTVEShader(allShaderPaths[i]))
                {
                    coreShaderPaths.Add(allShaderPaths[i]);
                }
            }

            return coreShaderPaths;
        }

        public static bool IsValidTVEShader(string shaderPath)
        {
            bool valid = false;

            if (!shaderPath.Contains("GPUI") && !shaderPath.Contains("Debug"))
            {
                var material = new Material(AssetDatabase.LoadAssetAtPath<Shader>(shaderPath));

                if (material.HasProperty("_IsTVEShader") || material.HasProperty("_IsTVEAIShader"))
                {
                    valid = true;
                }
            }

            return valid;
        }

        public static int GetRenderEngineIndexFromShader(string shaderPath)
        {
            int index = 0;

            StreamReader reader = new StreamReader(shaderPath);

            string lines = reader.ReadToEnd();

            for (int e = 0; e < TVEShaderUtils.RenderEngineOptions.Length; e++)
            {
                if (lines.Contains(TVEShaderUtils.RenderEngineOptions[e]))
                {
                    index = e;
                    break;
                }
            }

            reader.Close();

            return index;
        }

        public static void InjectShaderFeatures(string shaderAssetPath, string renderEngine)
        {
            string[] engineVegetationStudio = new string[]
            {
            "           //Vegetation Studio (Instanced Indirect)",
            "           #include \"XXX/Core/Includes/VS_Indirect.cginc\"",
            "           #pragma instancing_options procedural:setup forwardadd",
            "           #pragma multi_compile GPU_FRUSTUM_ON __",
            };

            string[] engineVegetationStudioHD = new string[]
            {
            "           //Vegetation Studio (Instanced Indirect)",
            "           #include \"XXX/Core/Includes/VS_IndirectHD.cginc\"",
            "           #pragma instancing_options procedural:setupVSPro forwardadd",
            "           #pragma multi_compile GPU_FRUSTUM_ON __",
            };

            string[] engineVegetationStudio145 = new string[]
            {
            "           //Vegetation Studio 1.4.5+ (Instanced Indirect)",
            "           #include \"XXX/Core/Includes/VS_Indirect145.cginc\"",
            "           #pragma instancing_options procedural:setupVSPro forwardadd",
            "           #pragma multi_compile GPU_FRUSTUM_ON __",
            };

            string[] engineNatureRenderer = new string[]
            {
            "           //Nature Renderer (Procedural Instancing)",
            "           #include \"XXX\"",
            "           #pragma instancing_options procedural:SetupNatureRenderer",
            };

            string[] engineGPUInstancer = new string[]
            {
            "           //GPU Instancer (Instanced Indirect)",
            "           #include \"XXX\"",
            "           #pragma instancing_options procedural:setupGPUI",
            "           #pragma multi_compile_instancing",
            };

            string[] engineInstantRenderer = new string[]
            {
            "           //Instant Renderer (Instanced Indirect)",
            "           #include \"XXX\"",
            "           #pragma instancing_options procedural:setupInstantRenderer",
            "           #pragma multi_compile_instancing",
            };

            string[] engineQuadroRenderer = new string[]
            {
            "           //Quadro Renderer (Instanced Indirect)",
            "           #include \"XXX\"",
            "           #pragma instancing_options procedural:setupQuadroRenderer",
            "           #pragma multi_compile_instancing",
            };

            string assetFolder = "Assets/BOXOPHOBIC/The Vegetation Engine";

            //Safer search, there might be many user folders
            string[] searchFolders;

            searchFolders = AssetDatabase.FindAssets("The Vegetation Engine");

            for (int i = 0; i < searchFolders.Length; i++)
            {
                if (AssetDatabase.GUIDToAssetPath(searchFolders[i]).EndsWith("The Vegetation Engine.pdf"))
                {
                    assetFolder = AssetDatabase.GUIDToAssetPath(searchFolders[i]);
                    assetFolder = assetFolder.Replace("/The Vegetation Engine.pdf", "");
                }
            }

            var cgincNR = "Assets/Visual Design Cafe/Nature Shaders/Common/Nodes/Integrations/Nature Renderer.cginc";
            searchFolders = AssetDatabase.FindAssets("Nature Renderer");

            for (int i = 0; i < searchFolders.Length; i++)
            {
                if (AssetDatabase.GUIDToAssetPath(searchFolders[i]).EndsWith("Nature Renderer.cginc"))
                {
                    cgincNR = AssetDatabase.GUIDToAssetPath(searchFolders[i]);
                }
            }

            var cgincGPUI = "Assets/GPUInstancer/Shaders/Include/GPUInstancerInclude.cginc";
            searchFolders = AssetDatabase.FindAssets("GPUInstancerInclude");

            for (int i = 0; i < searchFolders.Length; i++)
            {
                if (AssetDatabase.GUIDToAssetPath(searchFolders[i]).EndsWith("GPUInstancerInclude.cginc"))
                {
                    cgincGPUI = AssetDatabase.GUIDToAssetPath(searchFolders[i]);
                }
            }

            var cgincIR = "Assets/Vladislav Tsurikov/Instant Renderer/Shaders/Include/InstantRendererInclude.cginc";
            searchFolders = AssetDatabase.FindAssets("InstantRendererInclude");

            for (int i = 0; i < searchFolders.Length; i++)
            {
                if (AssetDatabase.GUIDToAssetPath(searchFolders[i]).EndsWith("QuadroRendererInclude.cginc"))
                {
                    cgincIR = AssetDatabase.GUIDToAssetPath(searchFolders[i]);
                }
            }

            var cgincQR = "Assets/Mega World/Quadro Renderer/Shaders/Include/QuadroRendererInclude.cginc";
            searchFolders = AssetDatabase.FindAssets("QuadroRendererInclude");

            for (int i = 0; i < searchFolders.Length; i++)
            {
                if (AssetDatabase.GUIDToAssetPath(searchFolders[i]).EndsWith("QuadroRendererInclude.cginc"))
                {
                    cgincIR = AssetDatabase.GUIDToAssetPath(searchFolders[i]);
                }
            }

            // Add correct paths for VSP and GPUI
            engineVegetationStudio[1] = engineVegetationStudio[1].Replace("XXX", assetFolder);
            engineVegetationStudioHD[1] = engineVegetationStudioHD[1].Replace("XXX", assetFolder);
            engineVegetationStudio145[1] = engineVegetationStudio145[1].Replace("XXX", assetFolder);
            engineNatureRenderer[1] = engineNatureRenderer[1].Replace("XXX", cgincNR);
            engineGPUInstancer[1] = engineGPUInstancer[1].Replace("XXX", cgincGPUI);
            engineInstantRenderer[1] = engineInstantRenderer[1].Replace("XXX", cgincIR);
            engineQuadroRenderer[1] = engineQuadroRenderer[1].Replace("XXX", cgincQR);

            var isHDPipeline = false;

            StreamReader reader = new StreamReader(shaderAssetPath);

            List<string> lines = new List<string>();

            while (!reader.EndOfStream)
            {
                lines.Add(reader.ReadLine());
            }

            reader.Close();

            int count = lines.Count;

            for (int i = 0; i < count; i++)
            {
                if (lines[i].Contains("SHADER INJECTION POINT BEGIN"))
                {
                    int c = 0;
                    int j = i + 1;

                    while (lines[j].Contains("SHADER INJECTION POINT END") == false)
                    {
                        j++;
                        c++;
                    }

                    lines.RemoveRange(i + 1, c);
                    count = count - c;
                }
            }

            count = lines.Count;

            for (int i = 0; i < count; i++)
            {
                if (lines[i].Contains("HDRenderPipeline"))
                {
                    isHDPipeline = true;
                }

                if (lines[i].Contains("[HideInInspector] _DisableSRPBatcher"))
                {
                    lines.RemoveAt(i);
                    count--;
                }
            }

            //Inject 3rd Party Support
            if (renderEngine.Contains("Vegetation Studio (Instanced Indirect)"))
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Contains("SHADER INJECTION POINT BEGIN"))
                    {
                        if (isHDPipeline)
                        {
                            lines.InsertRange(i + 1, engineVegetationStudioHD);
                        }
                        else
                        {
                            lines.InsertRange(i + 1, engineVegetationStudio);
                        }
                    }
                }
            }

            if (renderEngine.Contains("Vegetation Studio 1.4.5+ (Instanced Indirect)"))
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Contains("SHADER INJECTION POINT BEGIN"))
                    {
                        lines.InsertRange(i + 1, engineVegetationStudio145);
                    }
                }
            }

            if (renderEngine.Contains("Nature Renderer (Procedural Instancing)"))
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Contains("SHADER INJECTION POINT BEGIN"))
                    {
                        lines.InsertRange(i + 1, engineNatureRenderer);
                    }
                }
            }

            if (renderEngine.Contains("GPU Instancer (Instanced Indirect)"))
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Contains("SHADER INJECTION POINT BEGIN"))
                    {
                        lines.InsertRange(i + 1, engineGPUInstancer);
                    }
                }
            }

            if (renderEngine.Contains("Instant Renderer (Instanced Indirect)"))
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Contains("SHADER INJECTION POINT BEGIN"))
                    {
                        lines.InsertRange(i + 1, engineInstantRenderer);
                    }
                }
            }

            if (renderEngine.Contains("Quadro Renderer (Instanced Indirect)"))
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Contains("SHADER INJECTION POINT BEGIN"))
                    {
                        lines.InsertRange(i + 1, engineQuadroRenderer);
                    }
                }
            }

            if (renderEngine.Contains("Disable SRP Batcher Compatibility"))
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Contains("Properties"))
                    {
                        lines.Insert(i + 2, "		[HideInInspector] _DisableSRPBatcher(\"_DisableSRPBatcher\", Float) = 0 //Disable SRP Batcher Compatibility");
                    }
                }
            }

            for (int i = 0; i < lines.Count; i++)
            {
                // Disable ASE Drawers
                if (lines[i].Contains("[ASEBegin]"))
                {
                    lines[i] = lines[i].Replace("[ASEBegin]", "");
                }

                if (lines[i].Contains("[ASEnd]"))
                {
                    lines[i] = lines[i].Replace("[ASEnd]", "");
                }
            }

#if !AMPLIFY_SHADER_EDITOR && !UNITY_2020_2_OR_NEWER

            // Add diffusion profile support for HDRP 7
            if (isHDPipeline)
            {
                if (shaderAssetPath.Contains("Subsurface Lit"))
                {
                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (lines[i].Contains("[DiffusionProfile]"))
                        {
                            lines[i] = lines[i].Replace("[DiffusionProfile]", "[StyledDiffusionMaterial(_SubsurfaceDiffusion)]");
                        }
                    }
                }
            }

#elif AMPLIFY_SHADER_EDITOR && !UNITY_2020_2_OR_NEWER

            // Add diffusion profile support
            if (isHDPipeline)
            {
                if (shaderAssetPath.Contains("Subsurface Lit"))
                {
                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (lines[i].Contains("[HideInInspector][Space(10)][ASEDiffusionProfile(_SubsurfaceDiffusion)]"))
                        {
                            lines[i] = lines[i].Replace("[HideInInspector][Space(10)][ASEDiffusionProfile(_SubsurfaceDiffusion)]", "[Space(10)][ASEDiffusionProfile(_SubsurfaceDiffusion)]");
                        }

                        if (lines[i].Contains("[DiffusionProfile]") && !lines[i].Contains("[HideInInspector]"))
                        {
                            lines[i] = lines[i].Replace("[DiffusionProfile]", "[HideInInspector][DiffusionProfile]");
                        }

                        if (lines[i].Contains("[StyledDiffusionMaterial(_SubsurfaceDiffusion)]"))
                        {
                            lines[i] = lines[i].Replace("[StyledDiffusionMaterial(_SubsurfaceDiffusion)]", "[HideInInspector][StyledDiffusionMaterial(_SubsurfaceDiffusion)]");
                        }
                    }
                }
            }
#endif

            StreamWriter writer = new StreamWriter(shaderAssetPath);

            for (int i = 0; i < lines.Count; i++)
            {
                writer.WriteLine(lines[i]);
            }

            writer.Close();

            lines = new List<string>();

            //AssetDatabase.ImportAsset(shaderAssetPath);
        }
    }
}
