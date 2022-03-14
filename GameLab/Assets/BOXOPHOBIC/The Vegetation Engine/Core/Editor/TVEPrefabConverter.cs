﻿// Cristian Pop - https://boxophobic.com/

using UnityEngine;
using UnityEditor;
using Boxophobic.StyledGUI;
using Boxophobic.Utils;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System;

namespace TheVegetationEngine
{
    public enum PrefabStatus
    {
        Undefined = -1,
        Converted = 10,
        Supported = 20,
        Unsupported = 30,
    }

    [System.Serializable]
    public class TVEPrefabData
    {
        public GameObject prefabObject;
        public PrefabStatus prefabStatus;

        public TVEPrefabData(GameObject prefabObject, PrefabStatus prefabStatus)
        {
            this.prefabObject = prefabObject;
            this.prefabStatus = prefabStatus;
        }
    }

    public class TVEPrefabConverter : EditorWindow
    {
        const int SPACE_SMALL = 5;
        const int GUI_MESH = 24;

        const int NONE = 0;
        const bool SRGB = true;
        const bool ALPHA_DEFAULT = true;

        const int SQUARE_BUTTON_WIDTH = 20;
        const int SQUARE_BUTTON_HEIGHT = 18;
        float GUI_HALF_EDITOR_WIDTH = 200;

        const string BACKUP_DATA_PATH = "/Assets Data/Backup Data";
        const string ORIGINAL_DATA_PATH = "/Assets Data/Original Data";
        const string PREFABS_DATA_PATH = "/Assets Data/Prefabs Data";
        const string SHARED_DATA_PATH = "/Assets Data/Shared Data";

        readonly int[] MaxTextureSizes = new int[]
        {
        0,
        32,
        64,
        128,
        256,
        512,
        1024,
        2048,
        4096,
        8192,
        };

        string[] SourceMaskEnum = new string[]
        {
        "None", "Channel", "Procedural", "Texture", "3rd Party",
        };

        string[] SourceMaskMeshEnum = new string[]
        {
        "[0]  Vertex R", "[1]  Vertex G", "[2]  Vertex B", "[3]  Vertex A",
        "[4]  UV 0 X", "[5]  UV 0 Y", "[6]  UV 0 Z", "[7]  UV 0 W",
        "[8]  UV 2 X", "[9]  UV 2 Y", "[10]  UV 2 Z", "[11]  UV 2 W",
        "[12]  UV 3 X", "[13]  UV 3 Y", "[14]  UV 3 Z", "[16]  UV 3 W",
        "[16]  UV 4 X", "[17]  UV 4 Y", "[18]  UV 4 Z", "[19]  UV 4 W",
        };

        string[] SourceMaskProceduralEnum = new string[]
        {
        "[0]  Constant Black", "[1]  Constant White", "[2]  Random Element Variation", "[3]  Predictive Element Variation", "[4]  Height", "[5]  Sphere", "[6]  Cylinder", "[7]  Capsule",
        "[8]  Bottom To Top", "[9]  Top To Bottom", "[10]  Bottom Projection", "[11]  Top Projection", "[12]  Height Exp", "[13]  Hemi Sphere", "[14]  Hemi Cylinder", "[15]  Hemi Capsule",
        "[16]  Height Offset (Low)", "[17]  Height Offset (Medium)", "[18]  Height Offset (High)"
        };

        string[] SourceMask3rdPartyEnum = new string[]
        {
        "[0]  CTI Leaves Mask", "[1]  CTI Leaves Variation", "[2]  ST8 Leaves Mask", "[3]  NM Leaves Mask"
        };

        string[] SourceFromTextureEnum = new string[]
        {
        "[0]  Channel R", "[1]  Channel G", "[2]  Channel B", "[3]  Channel A"
        };

        string[] SourceCoordEnum = new string[]
        {
        "None", "Channel", "Procedural", "3rd Party",
        };

        string[] SourceCoordMeshEnum = new string[]
        {
        "[0]  UV 0", "[1]  UV 2", "[2]  UV 3", "[3]  UV 4",
        };

        string[] SourceCoordProceduralEnum = new string[]
        {
        "[0]  Planar XZ", "[1]  Planar XY", "[2]  Planar ZY", "[3]  Procedural Pivots",
        };

        string[] SourceCoord3rdPartyEnum = new string[]
        {
        "[0]  NM Trunk Blend"
        };

        string[] SourceNormalsEnum = new string[]
        {
        "From Mesh", "Procedural",
        };

        string[] SourceNormalsProceduralEnum = new string[]
        {
        "[0]  Recalculate Normals",
        "[1]  Flat Shading (Low)", "[2]  Flat Shading (Medium)", "[3]  Flat Shading (Full)",
        "[4]  Spherical Shading (Low)", "[5]  Spherical Shading (Medium)", "[6]  Spherical Shading (Full)",
        };

        string[] SourceActionEnum = new string[]
        {
        "None", "Invert", "Negate", "Remap 0-1", "Multiply by Height"
        };

        enum OutputMesh
        {
            Off = 0,
            Default = 10,
            Custom = 20,
            Polygonal = 30,
            DEEnvironment = 100,
        }

        enum OutputReadWrite
        {
            MarkMeshesAsNonReadable = 0,
            MarkMeshesAsReadable = 10,
        }

        enum OutputMaterial
        {
            Off = 0,
            Default = 10,
        }

        enum OutputTexture
        {
            UseCurrentResolution = 0,
            UseHighestResolution = 10,
        }

        enum OutputTransform
        {
            KeepOriginalTransforms = 0,
            TransformToWorldSpace = 10,
        }

        OutputMesh outputMeshIndex = OutputMesh.Default;
        OutputReadWrite outputReadWrite = OutputReadWrite.MarkMeshesAsNonReadable;
        OutputTexture outputTextureIndex = OutputTexture.UseHighestResolution;
        OutputMaterial outputMaterialIndex = OutputMaterial.Default;
        OutputTransform outputTransformIndex = OutputTransform.TransformToWorldSpace;
        string outputSuffix = "TVE";
        bool outputValid = true;

        string infoTitle = "";
        string infoPreset = "";
        string infoStatus = "";
        string infoOnline = "";
        string infoWarning = "";
        string infoError = "";

        int sourceVariation = 0;
        int optionVariation = 0;
        int actionVariation = 0;
        Texture2D textureVariation;

        int sourceOcclusion = 0;
        int optionOcclusion = 0;
        int actionOcclusion = 0;
        Texture2D textureOcclusion;

        int sourceDetail = 0;
        int optionDetail = 0;
        int actionDetail = 0;
        Texture2D textureDetail;

        int sourceMulti = 0;
        int optionMulti = 0;
        int actionMulti = 0;
        Texture2D textureMulti;

        int sourceDetailCoord = 0;
        int optionDetailCoord = 0;

        int sourceMotion1 = 0;
        int optionMotion1 = 0;
        int actionMotion1 = 0;
        Texture2D textureMotion1;

        int sourceMotion2 = 0;
        int optionMotion2 = 0;
        int actionMotion2 = 0;
        Texture2D textureMotion2;

        int sourceMotion3 = 0;
        int optionMotion3 = 0;
        int actionMotion3 = 0;
        Texture2D textureMotion3;

        int sourceNormals = 0;
        int optionNormals = 0;

        string projectDataFolder;
        string prefabDataFolder;
        string userFolder = "Assets/BOXOPHOBIC/User";

        List<TVEPrefabData> prefabObjects;
        int convertedPrefabCount;
        int supportedPrefabCount;
        int validPrefabCount;

        GameObject prefabObject;
        GameObject prefabInstance;
        GameObject prefabBackup;
        string prefabName;

        List<GameObject> gameObjectsInPrefab;
        List<MeshRenderer> meshRenderersInPrefab;
        List<Material[]> materialArraysInPrefab;
        List<Material[]> materialArraysInstances;
        List<MeshFilter> meshFiltersInPrefab;
        List<Mesh> meshesInPrefab;
        List<Mesh> meshInstances;
        List<MeshCollider> meshCollidersInPrefab;
        List<Mesh> collidersInPrefab;
        List<Mesh> colliderInstances;
        Vector4 maxBoundsInfo;

        Material blitMaterial;
        Texture blitTexture;
        TextureImporter[] sourceTexImporters;
        TextureImporterSettings[] sourceTexSettings;
        TextureImporterCompression[] sourceTexCompressions;
        int[] sourceimportSizes;

        int[] maskChannels;
        int[] maskActions0;
        int[] maskActions1;
        int[] maskActions2;
        Texture[] maskTextures;
        List<string> packedTextureNames;
        List<Texture> packedTextureObjcts;

        Mesh convertedMesh;
        Material convertedMaterial;

        int presetIndex = 0;
        bool presetAutoDetected = false;
        bool presetMixedValues = false;
        List<int> overrideIndices;
        List<bool> overrideGlobals;

        bool showAdvancedSettings = false;
        bool collectConvertedData = false;
        bool collectOriginalTextures = true;
        bool collectPrefabsAsNew = true;
        bool shareCommonMaterials = false;
        bool showMaterialSharingDialogue = true;
        bool showFolderSelectionDialogue = true;
        bool keepConvertedMaterials = false;
        bool hasOutputModifications = false;
        bool hasMeshModifications = false;
        int collectConvertedDataIndex;

        string[] allPresetPaths;
        List<string> presetPaths;
        string[] PresetOptions;
        List<string> presetLines;
        List<string> overridePaths;
        string[] OverrideOptions;
        List<string> detectLines;

        Shader shaderCross;
        Shader shaderLeaf;
        Shader shaderBark;
        Shader shaderGrass;
        Shader shaderProp;

        bool useLine;
        List<bool> useLines;
        bool isValid = true;
        bool showSelectedPrefabs = true;
        float seed = 1;

        GUIStyle stylePopup;
        GUIStyle styleCenteredHelpBox;
        GUIStyle styleMiniToggleButton;
        Color bannerColor;
        string bannerText;
        string helpURL;
        static TVEPrefabConverter window;
        Vector2 scrollPosition = Vector2.zero;

        [MenuItem("Window/BOXOPHOBIC/The Vegetation Engine/Prefab Converter", false, 2000)]
        public static void ShowWindow()
        {
            window = GetWindow<TVEPrefabConverter>(false, "Prefab Converter", true);
            window.minSize = new Vector2(400, 280);
        }

        void OnEnable()
        {
            bannerColor = new Color(0.890f, 0.745f, 0.309f);
            bannerText = "Prefab Converter";
            helpURL = "https://docs.google.com/document/d/145JOVlJ1tE-WODW45YoJ6Ixg23mFc56EnB_8Tbwloz8/edit#heading=h.46l51yqt2zky";

            if (GameObject.Find("The Vegetation Engine") == null)
            {
                isValid = false;
            }

            string[] searchFolders = AssetDatabase.FindAssets("User");

            for (int i = 0; i < searchFolders.Length; i++)
            {
                if (AssetDatabase.GUIDToAssetPath(searchFolders[i]).EndsWith("User.pdf"))
                {
                    userFolder = AssetDatabase.GUIDToAssetPath(searchFolders[i]);
                    userFolder = userFolder.Replace("/User.pdf", "");
                    userFolder += "/The Vegetation Engine/";
                }
            }

            collectConvertedData = Convert.ToBoolean(SettingsUtils.LoadSettingsData(userFolder + "Converter Collect.asset", "False"));
            collectOriginalTextures = Convert.ToBoolean(SettingsUtils.LoadSettingsData(userFolder + "Converter Textures.asset", "True"));
            collectPrefabsAsNew = Convert.ToBoolean(SettingsUtils.LoadSettingsData(userFolder + "Converter Prefabs.asset", "True"));
            showMaterialSharingDialogue = Convert.ToBoolean(SettingsUtils.LoadSettingsData(userFolder + "Converter Share.asset", "True"));
            showFolderSelectionDialogue = Convert.ToBoolean(SettingsUtils.LoadSettingsData(userFolder + "Converter Folder.asset", "True"));

            int intSeed = UnityEngine.Random.Range(1, 99);
            float floatSeed = UnityEngine.Random.Range(0.1f, 0.9f);
            seed = intSeed + floatSeed;

            InitTexturePacker();
            GetDefaultShaders();

            GetPresets();
            Initialize();
        }

        void OnSelectionChange()
        {
            GetPrefabObjects();
            GetPrefabPresets();
            GetAllPresetInfo();

            Repaint();
        }

        void OnFocus()
        {
            GetPrefabObjects();
            GetPrefabPresets();
            GetAllPresetInfo();

            Repaint();
        }

        void Initialize()
        {
            overrideIndices = new List<int>();
            overrideGlobals = new List<bool>();

            GetPrefabObjects();
            GetGlobalOverrides();
            GetPrefabPresets();

            if (overrideIndices.Count == 0)
            {
                overrideIndices.Add(0);
                overrideGlobals.Add(false);
            }

            GetAllPresetInfo();
        }

        void OnGUI()
        {
            GUI_HALF_EDITOR_WIDTH = this.position.width / 2.0f - 24;

            SetGUIStyles();

            StyledGUI.DrawWindowBanner(bannerColor, bannerText, helpURL);

            GUILayout.BeginHorizontal();
            GUILayout.Space(15);

            GUILayout.BeginVertical();

            DrawMessage();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(this.position.width - 28), GUILayout.Height(this.position.height - 230));

            DrawPrefabObjects();

            if (isValid == false || validPrefabCount == 0 || outputValid == false)
            {
                GUI.enabled = false;
            }

            DrawConversionSettings();
            DrawConvert();

            GUILayout.EndScrollView();

            GUILayout.EndVertical();

            GUILayout.Space(13);
            GUILayout.EndHorizontal();
        }

        void SetGUIStyles()
        {
            stylePopup = new GUIStyle(EditorStyles.popup)
            {
                alignment = TextAnchor.MiddleCenter
            };

            styleCenteredHelpBox = new GUIStyle(GUI.skin.GetStyle("HelpBox"))
            {
                richText = true,
                alignment = TextAnchor.MiddleCenter,
            };

            styleMiniToggleButton = new GUIStyle(GUI.skin.GetStyle("Button"))
            {
                richText = true,
                alignment = TextAnchor.MiddleCenter,
            };
        }

        void DrawMessage()
        {
            GUILayout.Space(-2);

            if (isValid && validPrefabCount > 0)
            {
                if (presetIndex != 0)
                {
                    var preset = "";
                    var status = "";
                    //var warning = "";
                    //var error = "";

                    if (infoPreset != "")
                    {
                        preset = "\n<size=10>" + infoPreset + "</size>";
                    }

                    if (infoStatus != "")
                    {
                        status = "\n\n" + infoStatus + "\n";
                    }

                    //if (infoWarning != "")
                    //{
                    //    if (EditorGUIUtility.isProSkin)
                    //    {
                    //        warning = "\n<b><color=#ddbc59>Warning! " + infoWarning + "</color></b>";
                    //    }
                    //    else
                    //    {
                    //        warning = "\n<b><color=#e16f00>Warning! " + infoWarning + "</color></b>";
                    //    }
                    //}

                    //if (infoError != "")
                    //{
                    //    if (EditorGUIUtility.isProSkin)
                    //    {
                    //        warning = "\n<b><color=#ff8260>Error! " + infoError + "</color></b>";
                    //    }
                    //    else
                    //    {
                    //        warning = "\n<b><color=#be1600>Error! " + infoError + "</color></b>";
                    //    }
                    //}

                    if (GUILayout.Button("\n<size=14>" + infoTitle + "</size>\n"
                                        /*+ "\n\n" + infoPreset + " Click here for more details!"*/
                                        + preset
                                        + status
                                        , styleCenteredHelpBox))
                    {
                        Application.OpenURL(infoOnline);
                    }

                    if (infoWarning != "")
                    {
                        GUILayout.Space(10);
                        EditorGUILayout.HelpBox(infoWarning, MessageType.Warning, true);
                    }

                    if (infoError != "")
                    {
                        GUILayout.Space(10);
                        EditorGUILayout.HelpBox(infoError, MessageType.Error, true);
                    }
                }
                else
                {
                    if (presetMixedValues)
                    {
                        GUILayout.Button("\n<size=14>Multiple conversion presets detected!</size>\n", styleCenteredHelpBox);
                    }
                    else
                    {
                        GUILayout.Button("\n<size=14>Choose a preset to convert the selected prefabs!</size>\n", styleCenteredHelpBox);
                    }
                }
            }
            else
            {
                if (isValid == false)
                {
                    EditorGUILayout.HelpBox("The Vegetation Engine manager is missing from your scene. Make sure setup it up first and the reopen the Prefab Converter!", MessageType.Warning, true);
                }
                else if (validPrefabCount == 0)
                {
                    GUILayout.Button("\n<size=14>Select one or multiple prefabs to get started!</size>\n", styleCenteredHelpBox);
                }
            }
        }

        void DrawPrefabObjects()
        {
            if (prefabObjects.Count > 0)
            {
                GUILayout.Space(10);

                if (showSelectedPrefabs)
                {
                    if (StyledButton("Hide Prefab Selection"))
                        showSelectedPrefabs = !showSelectedPrefabs;
                }
                else
                {
                    if (StyledButton("Show Prefab Selection"))
                        showSelectedPrefabs = !showSelectedPrefabs;
                }

                if (showSelectedPrefabs)
                {
                    for (int i = 0; i < prefabObjects.Count; i++)
                    {
                        StyledPrefab(prefabObjects[i]);
                    }
                }
            }
        }

        void DrawConversionSettings()
        {
            GUILayout.Space(10);

            EditorGUI.BeginChangeCheck();

            GUILayout.BeginHorizontal();

            presetIndex = StyledPresetPopup("Conversion Preset", "The preset used to convert the selected prefab or prefabs.", presetIndex, PresetOptions);

            if (presetIndex != 0)
            {
                if (StyledMiniToggleButton("S", "Select the preset file.", 12, false))
                {
                    EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(presetPaths[presetIndex]));
                }
            }

            GUILayout.EndHorizontal();

            if (presetIndex != 0)
            {
                for (int i = 0; i < overrideIndices.Count; i++)
                {
                    GUILayout.BeginHorizontal();

                    overrideIndices[i] = StyledPresetPopup("Conversion Override", "Adds extra functionality over the currently used preset.", overrideIndices[i], OverrideOptions);
                    overrideGlobals[i] = StyledMiniToggleButton("G", "Set Override as global for future conversions.", 11, overrideGlobals[i]);

                    if (overrideIndices[i] == 0)
                    {
                        overrideGlobals[i] = false;
                    }

                    GUILayout.EndHorizontal();
                }

                var overridesCount = overrideIndices.Count;

                if (overrideIndices[0] != 0 || overridesCount > 1)
                {
                    GUILayout.BeginHorizontal();

                    GUILayout.Label("");

                    if (overridesCount > 1)
                    {
                        if (GUILayout.Button(new GUIContent("-", "Remove the last override."), GUILayout.MaxWidth(SQUARE_BUTTON_WIDTH), GUILayout.MaxHeight(SQUARE_BUTTON_HEIGHT)))
                        {
                            overrideIndices.RemoveAt(overridesCount - 1);
                            overrideGlobals.RemoveAt(overridesCount - 1);
                        }
                    }

                    if (GUILayout.Button(new GUIContent("+", "Add a new override."), GUILayout.MaxWidth(SQUARE_BUTTON_WIDTH), GUILayout.MaxHeight(SQUARE_BUTTON_HEIGHT)))
                    {
                        overrideIndices.Add(0);
                        overrideGlobals.Add(false);
                    }

                    GUILayout.EndHorizontal();
                }

                if (EditorGUI.EndChangeCheck())
                {
                    GetAllPresetInfo();
                    SaveGlobalOverrides();
                }

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                GUILayout.Label("Show Advanced Settings", GUILayout.Width(GUI_HALF_EDITOR_WIDTH - 5));
                showAdvancedSettings = EditorGUILayout.Toggle(showAdvancedSettings);
                GUILayout.EndHorizontal();

                if (showAdvancedSettings)
                {
                    GUILayout.Space(10);
                    StyledGUI.DrawWindowCategory("Saving Settings");
                    GUILayout.Space(10);

                    if (collectConvertedData)
                    {
                        EditorGUILayout.HelpBox("When Collect Converted Data is enabled, all prefabs and the associated assets are copied to a new folder of your choice for better project organization! The settings from below are saved for futhur conversions!", MessageType.Info, true);
                    }

                    if (collectConvertedData && collectOriginalTextures && collectPrefabsAsNew)
                    {
                        EditorGUILayout.HelpBox("When Copy Original Texture and Save Prefabs As New are enabled, the converted prefabs, meshes, materials and texture are guaranteed to be unique without any connection with the original asset, but the convererter will not check for additional dependencies such as scripts!", MessageType.Info, true);
                    }

                    if (collectConvertedData)
                    {
                        GUILayout.Space(10);
                    }

                    EditorGUI.BeginChangeCheck();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Collect Converted Data", "Collect all prefabs and the associated assets in a new folder called Assets Data for better project organization."), GUILayout.Width(GUI_HALF_EDITOR_WIDTH - 5));
                    collectConvertedData = EditorGUILayout.Toggle(collectConvertedData);
                    GUILayout.EndHorizontal();

                    if (collectConvertedData)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("Copy Original Textures", "Copy all original textures as new assets to the Assets Data/Original Data folder."), GUILayout.Width(GUI_HALF_EDITOR_WIDTH - 5));
                        collectOriginalTextures = EditorGUILayout.Toggle(collectOriginalTextures);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("Save Prefabs As New", "Save a new prefab instead of replacing the old one."), GUILayout.Width(GUI_HALF_EDITOR_WIDTH - 5));
                        collectPrefabsAsNew = EditorGUILayout.Toggle(collectPrefabsAsNew);
                        GUILayout.EndHorizontal();

                        //GUILayout.BeginHorizontal();
                        //GUILayout.Label(new GUIContent("Share Common Materials", "Save the converted materials by sharing the common assets."), GUILayout.Width(GUI_HALF_EDITOR_WIDTH - 5));
                        //shareCommonMaterials = EditorGUILayout.Toggle(shareCommonMaterials);
                        //GUILayout.EndHorizontal();

                        GUILayout.Space(10);

                        GUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("Show Material Sharing Dialogue", "Shows a dialogue box for choosing if the common materials should be shared across multiple assets. If disabled, each material will be saved as a unique asset."), GUILayout.Width(GUI_HALF_EDITOR_WIDTH - 5));
                        showMaterialSharingDialogue = EditorGUILayout.Toggle(showMaterialSharingDialogue);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("Show Folder Selection Dialogue", "Shows a new folder saving dialogue to choose where the converted assets are saved. If disabled, the latest folder path will be used if available."), GUILayout.Width(GUI_HALF_EDITOR_WIDTH - 5));
                        showFolderSelectionDialogue = EditorGUILayout.Toggle(showFolderSelectionDialogue);
                        GUILayout.EndHorizontal();
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        SaveCollectSettings();
                    }

                    GUILayout.Space(10);
                    StyledGUI.DrawWindowCategory("Output Settings");
                    GUILayout.Space(10);

                    if (hasOutputModifications)
                    {
                        EditorGUILayout.HelpBox("The output settings have been overriden and they will not update when changing the preset or adding overrides!", MessageType.Info, true);
                        GUILayout.Space(10);
                    }

                    EditorGUI.BeginChangeCheck();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Output Meshes", "Mesh packing for the current preset."), GUILayout.Width(GUI_HALF_EDITOR_WIDTH - 5));
                    outputMeshIndex = (OutputMesh)EditorGUILayout.EnumPopup(outputMeshIndex, stylePopup);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Output ReadWrite", "Read/Write enabled on the saved meshes."), GUILayout.Width(GUI_HALF_EDITOR_WIDTH - 5));
                    outputReadWrite = (OutputReadWrite)EditorGUILayout.EnumPopup(outputReadWrite, stylePopup);
                    GUILayout.EndHorizontal();

                    if (outputMeshIndex != OutputMesh.Off)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("Output Transforms", "Transform meshes to world space."), GUILayout.Width(GUI_HALF_EDITOR_WIDTH - 5));
                        outputTransformIndex = (OutputTransform)EditorGUILayout.EnumPopup(outputTransformIndex, stylePopup);
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Output Materials", "Material conversion for the current preset."), GUILayout.Width(GUI_HALF_EDITOR_WIDTH - 5));
                    outputMaterialIndex = (OutputMaterial)EditorGUILayout.EnumPopup(outputMaterialIndex, stylePopup);
                    GUILayout.EndHorizontal();

                    if (outputMaterialIndex != OutputMaterial.Off)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("Output Textures", "The Use Current Resolution option packs the textures at the currently used texture resolutions and it is faster. The Use Highest Resolution option packs the textures at the highest available resolution and it can be slower."), GUILayout.Width(GUI_HALF_EDITOR_WIDTH - 5));
                        outputTextureIndex = (OutputTexture)EditorGUILayout.EnumPopup(outputTextureIndex, stylePopup);
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Output Suffix", "Suffix used when saving assets to disk."), GUILayout.Width(GUI_HALF_EDITOR_WIDTH - 5));
                    outputSuffix = EditorGUILayout.TextField(outputSuffix);
                    GUILayout.EndHorizontal();

                    if (EditorGUI.EndChangeCheck())
                    {
                        hasOutputModifications = true;
                    }

                    if (outputMeshIndex == OutputMesh.Default)
                    {
                        GUILayout.Space(10);
                        StyledGUI.DrawWindowCategory("Mesh Settings");
                        GUILayout.Space(10);

                        if (hasMeshModifications)
                        {
                            EditorGUILayout.HelpBox("The mesh settings have been overriden and they will not update when changing the preset or adding overrides!", MessageType.Info, true);
                            GUILayout.Space(10);
                        }

                        EditorGUI.BeginChangeCheck();

                        GUILayout.Space(10);

                        sourceNormals = StyledSourcePopup("Normals", "Mesh vertex normals override.", sourceNormals, SourceNormalsEnum);
                        optionNormals = StyledNormalsOptionEnum("Normals", "", sourceNormals, optionNormals, true);

                        GUILayout.Space(10);

                        sourceVariation = StyledSourcePopup("Variation", "Variation mask used for wind animation and global effects. Stored in Vertex Red.", sourceVariation, SourceMaskEnum);
                        optionVariation = StyledMaskOptionEnum("Variation", "", sourceVariation, optionVariation, false);
                        textureVariation = StyledTexture("Variation", "", sourceVariation, textureVariation);
                        actionVariation = StyledActionOptionEnum("Variation", "", sourceVariation, actionVariation, true);

                        sourceOcclusion = StyledSourcePopup("Occlusion", "Vertex Occlusion mask used to add depth and light scattering mask. Stored in Vertex Green.", sourceOcclusion, SourceMaskEnum);
                        optionOcclusion = StyledMaskOptionEnum("Occlusion", "", sourceOcclusion, optionOcclusion, false);
                        textureOcclusion = StyledTexture("Occlusion", "", sourceOcclusion, textureOcclusion);
                        actionOcclusion = StyledActionOptionEnum("Occlusion", "", sourceOcclusion, actionOcclusion, true);

                        sourceDetail = StyledSourcePopup("Detail Mask", "Detail mask used for layer blending for bark. Stored in Vertex Blue.", sourceDetail, SourceMaskEnum);
                        optionDetail = StyledMaskOptionEnum("Detail Mask", "", sourceDetail, optionDetail, false);
                        textureDetail = StyledTexture("Detail Mask", "", sourceDetail, textureDetail);
                        actionDetail = StyledActionOptionEnum("Detail Mask", "", sourceDetail, actionDetail, true);

                        sourceMulti = StyledSourcePopup("Multi Mask", "Multi mask used for gradinet colors for leaves and subsurface/overlay mask for grass. The default value is set to height. Stored in Vertex Alpha.", sourceMulti, SourceMaskEnum);
                        optionMulti = StyledMaskOptionEnum("Multi Mask", "", sourceMulti, optionMulti, false);
                        textureMulti = StyledTexture("Multi Mask", "", sourceMulti, textureMulti);
                        actionMulti = StyledActionOptionEnum("Multi Mask", "", sourceMulti, actionMulti, true);

                        GUILayout.Space(10);

                        sourceDetailCoord = StyledSourcePopup("Detail Coord or Pivots", "Detail UVs used for layer blending for bark. Pivots storing for grass when multiple grass blades are combined into a single mesh. Stored in UV0.ZW.", sourceDetailCoord, SourceCoordEnum);
                        optionDetailCoord = StyledCoordOptionEnum("Detail Coord or Pivots", "", sourceDetailCoord, optionDetailCoord, true);

                        GUILayout.Space(10);

                        sourceMotion1 = StyledSourcePopup("Motion Primary", "Motion mask used for bending animation. Stored in UV3.X.", sourceMotion1, SourceMaskEnum);
                        optionMotion1 = StyledMaskOptionEnum("Motion Primary", "", sourceMotion1, optionMotion1, false);
                        textureMotion1 = StyledTexture("Motion Primary", "", sourceMotion1, textureMotion1);
                        actionMotion1 = StyledActionOptionEnum("Motion Primary", "", sourceMotion1, actionMotion1, true);

                        sourceMotion2 = StyledSourcePopup("Motion Second", "Motion mask used for rolling, vertical and squash animations. Stored in UV3.Y.", sourceMotion2, SourceMaskEnum);
                        optionMotion2 = StyledMaskOptionEnum("Motion Second", "", sourceMotion2, optionMotion2, false);
                        textureMotion2 = StyledTexture("Motion Second", "", sourceMotion2, textureMotion2);
                        actionMotion2 = StyledActionOptionEnum("Motion Second", "", sourceMotion2, actionMotion2, true);

                        sourceMotion3 = StyledSourcePopup("Motion Details", "Motion mask used for flutter animation. Stored in UV3.Z.", sourceMotion3, SourceMaskEnum);
                        optionMotion3 = StyledMaskOptionEnum("Motion Details", "", sourceMotion3, optionMotion3, false);
                        textureMotion3 = StyledTexture("Motion Details", "", sourceMotion3, textureMotion3);
                        actionMotion3 = StyledActionOptionEnum("Motion Details", "", sourceMotion3, actionMotion3, true);

                        if (EditorGUI.EndChangeCheck())
                        {
                            hasMeshModifications = true;
                        }
                    }

                    if (outputMeshIndex == OutputMesh.Custom)
                    {
                        GUILayout.Space(10);
                        StyledGUI.DrawWindowCategory("Mesh Settings");
                        GUILayout.Space(10);

                        if (hasMeshModifications)
                        {
                            EditorGUILayout.HelpBox("The mesh settings have been overriden and they will not update when changing the preset or adding overrides!", MessageType.Info, true);
                            GUILayout.Space(10);
                        }

                        EditorGUI.BeginChangeCheck();

                        sourceNormals = StyledSourcePopup("Normals", "", sourceNormals, SourceNormalsEnum);
                        optionNormals = StyledNormalsOptionEnum("Normals", "", sourceNormals, optionNormals, true);

                        GUILayout.Space(10);

                        sourceVariation = StyledSourcePopup("Vertex Red", "", sourceVariation, SourceMaskEnum);
                        optionVariation = StyledMaskOptionEnum("Vertex Red", "", sourceVariation, optionVariation, false);
                        textureVariation = StyledTexture("Vertex Red", "", sourceVariation, textureVariation);
                        actionVariation = StyledActionOptionEnum("Vertex Red", "", sourceVariation, actionVariation, true);

                        sourceOcclusion = StyledSourcePopup("Vertex Green", "", sourceOcclusion, SourceMaskEnum);
                        optionOcclusion = StyledMaskOptionEnum("Vertex Green", "", sourceOcclusion, optionOcclusion, false);
                        textureOcclusion = StyledTexture("Vertex Green", "", sourceOcclusion, textureOcclusion);
                        actionOcclusion = StyledActionOptionEnum("Vertex Green", "", sourceOcclusion, actionOcclusion, true);

                        sourceDetail = StyledSourcePopup("Vertex Blue", "", sourceDetail, SourceMaskEnum);
                        optionDetail = StyledMaskOptionEnum("Vertex Blue", "", sourceDetail, optionDetail, false);
                        textureDetail = StyledTexture("Vertex Blue", "", sourceDetail, textureDetail);
                        actionDetail = StyledActionOptionEnum("Vertex Blue", "", sourceDetail, actionDetail, true);

                        sourceMulti = StyledSourcePopup("Vertex Alpha", "", sourceMulti, SourceMaskEnum);
                        optionMulti = StyledMaskOptionEnum("Vertex Alpha", "", sourceMulti, optionMulti, false);
                        textureMulti = StyledTexture("Vertex Blue", "", sourceMulti, textureMulti);
                        actionMulti = StyledActionOptionEnum("Vertex Alpha", "", sourceMulti, actionMulti, true);

                        if (EditorGUI.EndChangeCheck())
                        {
                            hasMeshModifications = true;
                        }
                    }

                    if (outputMeshIndex == OutputMesh.DEEnvironment)
                    {
                        GUILayout.Space(10);
                        StyledGUI.DrawWindowCategory("Mesh Settings");
                        GUILayout.Space(10);

                        if (hasMeshModifications)
                        {
                            EditorGUILayout.HelpBox("The mesh settings have been overriden and they will not update when changing the preset or adding overrides!", MessageType.Info, true);
                            GUILayout.Space(10);
                        }

                        EditorGUI.BeginChangeCheck();

                        sourceNormals = StyledSourcePopup("Normals", "", sourceNormals, SourceNormalsEnum);
                        optionNormals = StyledNormalsOptionEnum("Normals", "", sourceNormals, optionNormals, true);

                        GUILayout.Space(10);

                        sourceVariation = StyledSourcePopup("Variation", "", sourceVariation, SourceMaskEnum);
                        optionVariation = StyledMaskOptionEnum("Variation", "", sourceVariation, optionVariation, false);
                        textureVariation = StyledTexture("Variation", "", sourceVariation, textureVariation);
                        actionVariation = StyledActionOptionEnum("Variation", "", sourceVariation, actionVariation, true);

                        sourceOcclusion = StyledSourcePopup("Occlusion", "", sourceOcclusion, SourceMaskEnum);
                        optionOcclusion = StyledMaskOptionEnum("Occlusion", "", sourceOcclusion, optionOcclusion, false);
                        textureOcclusion = StyledTexture("Occlusion", "", sourceOcclusion, textureOcclusion);
                        actionOcclusion = StyledActionOptionEnum("Occlusion", "", sourceOcclusion, actionOcclusion, true);

                        sourceMotion1 = StyledSourcePopup("Motion Primary", "", sourceMotion1, SourceMaskEnum);
                        optionMotion1 = StyledMaskOptionEnum("Motion Primary", "", sourceMotion1, optionMotion1, false);
                        textureMotion1 = StyledTexture("Motion Primary", "", sourceMotion1, textureMotion1);
                        actionMotion1 = StyledActionOptionEnum("Motion Primary", "", sourceMotion1, actionMotion1, true);

                        sourceMotion2 = StyledSourcePopup("Motion Detail", "", sourceMotion2, SourceMaskEnum);
                        optionMotion2 = StyledMaskOptionEnum("Motion Detail", "", sourceMotion2, optionMotion2, false);
                        textureMotion2 = StyledTexture("Motion Detail", "", sourceMotion2, textureMotion2);
                        actionMotion2 = StyledActionOptionEnum("Motion Detail", "", sourceMotion2, actionMotion2, true);

                        if (EditorGUI.EndChangeCheck())
                        {
                            hasMeshModifications = true;
                        }
                    }
                }
            }
        }

        void DrawConvert()
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();

            if (convertedPrefabCount == 0)
            {
                GUI.enabled = false;
            }
            else
            {
                GUI.enabled = true;
            }

            if (GUILayout.Button("Revert", GUILayout.Width(GUI_HALF_EDITOR_WIDTH - 2)))
            {
                for (int i = 0; i < prefabObjects.Count; i++)
                {
                    var data = prefabObjects[i];

                    if (data.prefabStatus == PrefabStatus.Converted)
                    {
                        prefabObject = prefabObjects[i].prefabObject;

                        RevertPrefab(true);
                    }
                }

                GetPrefabObjects();
            }

            if (supportedPrefabCount + convertedPrefabCount == 0)
            {
                GUI.enabled = false;
            }
            else
            {
                if (presetIndex == 0 || outputValid == false)
                {
                    GUI.enabled = false;
                }
                else
                {
                    GUI.enabled = true;
                }
            }

            var convertLabel = "Convert";

            if (collectConvertedData)
            {
                convertLabel = "Convert and Collect";
            }

            if (GUILayout.Button(convertLabel))
            {
                if (collectConvertedData)
                {
                    var latestDataFolder = SettingsUtils.LoadSettingsData(userFolder + "Converter Latest.asset", "Assets");

                    if (!Directory.Exists(latestDataFolder))
                    {
                        latestDataFolder = "Assets";
                    }

                    if (showFolderSelectionDialogue)
                    {
                        projectDataFolder = EditorUtility.OpenFolderPanel("Save Converted Assets to Folder", latestDataFolder, "");

                        if (projectDataFolder != "")
                        {
                            projectDataFolder = "Assets" + projectDataFolder.Substring(Application.dataPath.Length);
                        }
                    }
                    else
                    {
                        if (latestDataFolder == "Assets")
                        {
                            projectDataFolder = EditorUtility.OpenFolderPanel("Save Converted Assets to Folder", latestDataFolder, "");

                            if (projectDataFolder != "")
                            {
                                projectDataFolder = "Assets" + projectDataFolder.Substring(Application.dataPath.Length);
                            }
                        }
                        else
                        {
                            projectDataFolder = latestDataFolder;
                        }
                    }

                    if (projectDataFolder != "")
                    {
                        if (!Directory.Exists(projectDataFolder))
                        {
                            Directory.CreateDirectory(projectDataFolder);
                            AssetDatabase.Refresh();
                        }

                        SettingsUtils.SaveSettingsData(userFolder + "Converter Latest.asset", projectDataFolder);

                        CreateAssetsDataSubFolders();

                        if (showMaterialSharingDialogue)
                        {
                            shareCommonMaterials = EditorUtility.DisplayDialog("Share Common Materials?", "When enabled, the common materials are shared accross multiple assets and it can lead to unexpected results if different materials with the same name exists! Use it at your own risk!", "Share Common Materials", "No");
                        }
                    }
                    else
                    {
                        GUIUtility.ExitGUI();
                        return;
                    }
                }

                if (convertedPrefabCount > 0 && !shareCommonMaterials)
                {
                    keepConvertedMaterials = EditorUtility.DisplayDialog("Keep Converted Materials?", "When enabled, the prefab converter will only convert the meshes and keep the converted materials and textures from the previous conversion if available!", "Keep Converted Materials", "No");
                }

                for (int i = 0; i < prefabObjects.Count; i++)
                {
                    var data = prefabObjects[i];

                    if (data.prefabStatus == PrefabStatus.Converted || data.prefabStatus == PrefabStatus.Supported)
                    {
                        prefabObject = prefabObjects[i].prefabObject;

                        if (data.prefabStatus == PrefabStatus.Converted)
                        {
                            RevertPrefab(false);
                        }

                        ConvertPrefab();
                    }
                }

                GetPrefabObjects();

                EditorUtility.ClearProgressBar();
            }

            if (collectConvertedData)
            {
                collectConvertedDataIndex = 1;
            }
            else
            {
                collectConvertedDataIndex = 0;
            }

            EditorGUI.BeginChangeCheck();

            collectConvertedDataIndex = EditorGUILayout.Popup(new GUIContent("", "Choose if the converted assets are collected. The collect settings are found under the advanced settings."), collectConvertedDataIndex, new string[] { "Convert", "Convert and Collect" }, GUILayout.MaxWidth(SQUARE_BUTTON_WIDTH));

            if (collectConvertedDataIndex == 1)
            {
                collectConvertedData = true;
            }
            else
            {
                collectConvertedData = false;
            }

            //collectConvertedData = StyledMiniToggleButton("C", "Set Override as global for future conversions.", 11, collectConvertedData);

            if (EditorGUI.EndChangeCheck())
            {
                SaveCollectSettings();
            }

            GUILayout.EndHorizontal();
            GUI.enabled = true;
        }

        void StyledPrefab(TVEPrefabData prefabData)
        {
            if (prefabData.prefabStatus == PrefabStatus.Converted)
            {
                if (EditorGUIUtility.isProSkin)
                {
                    GUILayout.Label("<size=10><b><color=#f6d161>" + prefabData.prefabObject.name + "</color></b></size>", styleCenteredHelpBox, GUILayout.Height(GUI_MESH));
                }
                else
                {
                    GUILayout.Label("<size=10><b><color=#e16f00>" + prefabData.prefabObject.name + "</color></b></size>", styleCenteredHelpBox, GUILayout.Height(GUI_MESH));
                }
            }

            if (prefabData.prefabStatus == PrefabStatus.Supported)
            {
                if (EditorGUIUtility.isProSkin)
                {
                    GUILayout.Label("<size=10><b><color=#87b8ff>" + prefabData.prefabObject.name + "</color></b></size>", styleCenteredHelpBox, GUILayout.Height(GUI_MESH));
                }
                else
                {
                    GUILayout.Label("<size=10><b><color=#0b448b>" + prefabData.prefabObject.name + "</color></b></size>", styleCenteredHelpBox, GUILayout.Height(GUI_MESH));
                }
            }

            if (prefabData.prefabStatus == PrefabStatus.Unsupported)
            {
                if (EditorGUIUtility.isProSkin)
                {
                    GUILayout.Label("<size=10><b><color=#637a9c>" + prefabData.prefabObject.name + "</color></b></size>", styleCenteredHelpBox, GUILayout.Height(GUI_MESH));
                }
                else
                {
                    GUILayout.Label("<size=10><b><color=#6e8aac>" + prefabData.prefabObject.name + "</color></b></size>", styleCenteredHelpBox, GUILayout.Height(GUI_MESH));
                }

            }

            var lastRect = GUILayoutUtility.GetLastRect();

            if (GUI.Button(lastRect, "", GUIStyle.none))
            {
                EditorGUIUtility.PingObject(prefabData.prefabObject);
            }

            if (prefabData.prefabStatus == PrefabStatus.Unsupported)
            {
                var iconRect = new Rect(lastRect.width - 18, lastRect.y + 2, 20, 20);

                GUI.Label(iconRect, EditorGUIUtility.IconContent("console.warnicon.sml"));
                GUI.Label(iconRect, new GUIContent("", "SpeedTree, Tree Creator, Models or any other asset type prefabs cannot be converted directly, you will need to create a new regular prefab first!"));
            }
        }

        int StyledPopup(string name, string tooltip, int index, string[] options)
        {
            if (index >= options.Length)
            {
                index = 0;
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent(name, tooltip), GUILayout.Width(GUI_HALF_EDITOR_WIDTH - 5));
            index = EditorGUILayout.Popup(index, options, stylePopup);
            GUILayout.EndHorizontal();

            return index;
        }

        int StyledPresetPopup(string name, string tooltip, int index, string[] options)
        {
            if (index >= options.Length)
            {
                index = 0;
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent(name, tooltip), GUILayout.Width(GUI_HALF_EDITOR_WIDTH - 5));
            index = EditorGUILayout.Popup(index, options, stylePopup);

            var lastRect = GUILayoutUtility.GetLastRect();
            GUI.Label(lastRect, new GUIContent("", options[index]));

            GUILayout.EndHorizontal();

            return index;
        }

        int StyledSourcePopup(string name, string tooltip, int index, string[] options)
        {
            index = StyledPopup(name + " Source", tooltip, index, options);

            return index;
        }

        int StyledActionOptionEnum(string name, string tooltip, int source, int option, bool space)
        {
            if (source > 0)
            {
                option = StyledPopup(name + " Action", tooltip, option, SourceActionEnum);
            }

            if (space)
            {
                GUILayout.Space(SPACE_SMALL);
            }

            return option;
        }

        int StyledMaskOptionEnum(string name, string tooltip, int source, int option, bool space)
        {
            if (source == 1)
            {
                option = StyledPopup(name + " Channel", tooltip, option, SourceMaskMeshEnum);
            }
            if (source == 2)
            {
                option = StyledPopup(name + " Procedural", tooltip, option, SourceMaskProceduralEnum);
            }
            if (source == 3)
            {
                option = StyledPopup(name + " Channel", tooltip, option, SourceFromTextureEnum);
            }
            if (source == 4)
            {
                option = StyledPopup(name + " 3rd Party", tooltip, option, SourceMask3rdPartyEnum);
            }

            if (space)
            {
                GUILayout.Space(SPACE_SMALL);
            }

            return option;
        }

        Texture2D StyledTexture(string name, string tooltip, int source, Texture2D texture)
        {
            if (source == 3)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent(name + " Texture", ""), GUILayout.Width(GUI_HALF_EDITOR_WIDTH - 5));
                texture = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), false);
                GUILayout.EndHorizontal();
            }

            return texture;
        }

        int StyledCoordOptionEnum(string name, string tooltip, int source, int option, bool space)
        {
            if (source == 1)
            {
                option = StyledPopup(name + " Channel", tooltip, option, SourceCoordMeshEnum);
            }
            if (source == 2)
            {
                option = StyledPopup(name + " Procedural", tooltip, option, SourceCoordProceduralEnum);
            }
            if (source == 3)
            {
                option = StyledPopup(name + " 3rd Party", tooltip, option, SourceCoord3rdPartyEnum);
            }

            GUILayout.Space(SPACE_SMALL);

            return option;
        }

        int StyledNormalsOptionEnum(string name, string tooltip, int source, int option, bool space)
        {
            if (source == 1)
            {
                option = StyledPopup(name + " Procedural", tooltip, option, SourceNormalsProceduralEnum);
            }

            GUILayout.Space(SPACE_SMALL);

            return option;
        }

        bool StyledButton(string text)
        {
            bool value = GUILayout.Button("<b>" + text + "</b>", styleCenteredHelpBox, GUILayout.Height(GUI_MESH));

            return value;
        }

        bool StyledMiniToggleButton(string text, string tooltip, int size, bool value)
        {
            value = GUILayout.Toggle(value, new GUIContent("<size="+ size +">" + text + "</size>", tooltip), styleMiniToggleButton, GUILayout.MaxWidth(SQUARE_BUTTON_WIDTH), GUILayout.MaxHeight(SQUARE_BUTTON_HEIGHT));

            return value;
        }

        /// <summary>
        /// Convert and Revert Macros
        /// </summary>

        void ConvertPrefab()
        {
            prefabName = prefabObject.name;

            string dataPath;

            if (collectConvertedData)
            {
                dataPath = projectDataFolder + "/" + prefabName + ".prefab";

                //if (File.Exists(dataPath))
                //{
                //    if (AssetDatabase.LoadAssetAtPath<GameObject>(dataPath) != prefabObject)
                //    {
                //        dataPath = AssetDatabase.GenerateUniqueAssetPath(dataPath);
                //prefabName = Path.GetFileNameWithoutExtension(dataPath);
                //    }
                //}
            }
            else
            {
                dataPath = AssetDatabase.GetAssetPath(prefabObject);
                dataPath = Path.GetDirectoryName(dataPath);
                dataPath = dataPath + "/" + prefabName;
                prefabDataFolder = dataPath;
            }

            EditorUtility.DisplayProgressBar("The Vegetation Engine", prefabObject.name + ": Prepare Prefab", 0.0f);

            var prefabScale = prefabObject.transform.localScale;

            prefabInstance = Instantiate(prefabObject);
            prefabInstance.transform.localPosition = Vector3.zero;
            prefabInstance.transform.rotation = Quaternion.identity;
            prefabInstance.transform.localScale = Vector3.one;
            prefabInstance.AddComponent<TVEPrefab>();

            var prefabComponent = prefabInstance.GetComponent<TVEPrefab>();
            prefabComponent.storedPreset = PresetOptions[presetIndex];
            prefabComponent.storedOverrides = new List<string>();

            for (int i = 0; i < overrideIndices.Count; i++)
            {
                if (overrideIndices[i] != 0)
                {
                    prefabComponent.storedOverrides.Add(OverrideOptions[overrideIndices[i]]);
                }
            }

            EditorUtility.DisplayProgressBar("The Vegetation Engine", prefabObject.name + ": Create Backup", 0.1f);

            CreatePrefabDataFolder();
            CreatePrefabBackupFile();

            prefabComponent.storedPrefabBackupGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(prefabBackup));

            EditorUtility.DisplayProgressBar("The Vegetation Engine", prefabObject.name + ": Prepare Assets", 0.2f);

            GetGameObjectsInPrefab();
            FixInvalidPrefabScripts();

            DisableInvalidGameObjectsInPrefab();

            GetMeshRenderersInPrefab();
            GetMaterialArraysInPrefab();

            GetMeshFiltersInPrefab();
            GetMeshesInPrefab();
            CreateMeshInstances();

            GetMeshCollidersInPrefab();
            GetCollidersInPrefab();
            CreateColliderInstances();

            EditorUtility.DisplayProgressBar("The Vegetation Engine", prefabObject.name + ": Transform Space", 0.3f);

            if (outputMeshIndex != OutputMesh.Off)
            {
                if (outputTransformIndex == OutputTransform.TransformToWorldSpace)
                {
                    TransformMeshesToWorldSpace();
                }
            }

            EditorUtility.DisplayProgressBar("The Vegetation Engine", prefabObject.name + ": Get Bounds", 0.4f);

            GetMaxBoundsInPrefab();

            EditorUtility.DisplayProgressBar("The Vegetation Engine", prefabObject.name + ": Convert Materials", 0.6f);

            if (outputMaterialIndex != OutputMaterial.Off)
            {
                CreateMaterialArraysInstances();

                ConvertMaterials();
                AssignConvertedMaterials();
            }

            EditorUtility.DisplayProgressBar("The Vegetation Engine", prefabObject.name + ": Convert Meshes", 0.8f);

            ConvertMeshes();
            ConvertColliders();

            prefabInstance.transform.localScale = prefabScale;

            EnableInvalidGameObjectsInPrefab();

            EditorUtility.DisplayProgressBar("The Vegetation Engine", prefabObject.name + ": Save Prefab", 0.9f);

            if (collectConvertedData)
            {
                if (collectPrefabsAsNew)
                {
                    if (File.Exists(dataPath))
                    {
                        SavePrefab(prefabInstance, prefabObject, true);
                        EditorGUIUtility.PingObject(prefabObject);
                    }
                    else
                    {
                        PrefabUtility.SaveAsPrefabAsset(prefabInstance, dataPath);
                        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<GameObject>(dataPath));
                    }
                }
                else
                {
                    SavePrefab(prefabInstance, prefabObject, true);
                    AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(prefabObject), dataPath);
                    EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<GameObject>(dataPath));
                }
            }
            else
            {
                SavePrefab(prefabInstance, prefabObject, true);
                EditorGUIUtility.PingObject(prefabObject);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayProgressBar("The Vegetation Engine", prefabObject.name + ": Finish Conversion", 1.0f);

            DestroyImmediate(prefabInstance);
        }

        void RevertPrefab(bool deleteConvertedAssets)
        {
            var prefabBackup = GetPrefabBackupFile(prefabObject);

            prefabInstance = Instantiate(prefabBackup);

            GetGameObjectsInPrefab();
            FixInvalidPrefabScripts();

            SavePrefab(prefabInstance, prefabObject, false);

            if (deleteConvertedAssets)
            {
                // Cleaup converted data on revert
                var prefabPath = AssetDatabase.GetAssetPath(prefabObject);
                var standalonePath = prefabPath.Replace(Path.GetFileName(prefabPath), "") + prefabObject.name;
                var collectedPath = prefabPath.Replace(Path.GetFileName(prefabPath), "") + PREFABS_DATA_PATH + "/" + prefabObject.name;

                try
                {
                    FileUtil.DeleteFileOrDirectory(standalonePath);
                    FileUtil.DeleteFileOrDirectory(standalonePath + ".meta");
                    FileUtil.DeleteFileOrDirectory(collectedPath);
                    FileUtil.DeleteFileOrDirectory(collectedPath + ".meta");
                }
                catch
                {
                    Debug.Log("<b>[The Vegetation Engine]</b> " + "The converted prefab data cannot be deleted on revert!");
                }
            }

            //AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            DestroyImmediate(prefabInstance);
        }

        void SavePrefab(GameObject prefabInstance, GameObject prefabObject, bool setLabel)
        {
            //PrefabUtility.ReplacePrefab(prefabInstance, prefabObject, ReplacePrefabOptions.ReplaceNameBased);

            var prefabPath = AssetDatabase.GetAssetPath(prefabObject);

            PrefabUtility.SaveAsPrefabAssetAndConnect(prefabInstance, prefabPath, InteractionMode.AutomatedAction);

            if (setLabel)
            {
                AssetDatabase.SetLabels(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(prefabPath), new string[] { outputSuffix + " Prefab" });
            }
        }

        void SaveCollectSettings()
        {
            SettingsUtils.SaveSettingsData(userFolder + "Converter Collect.asset", collectConvertedData.ToString());
            SettingsUtils.SaveSettingsData(userFolder + "Converter Textures.asset", collectOriginalTextures.ToString());
            SettingsUtils.SaveSettingsData(userFolder + "Converter Prefabs.asset", collectPrefabsAsNew.ToString());
            SettingsUtils.SaveSettingsData(userFolder + "Converter Share.asset", showMaterialSharingDialogue.ToString());
            SettingsUtils.SaveSettingsData(userFolder + "Converter Folder.asset", showFolderSelectionDialogue.ToString());
        }

        /// <summary>
        /// Get GameObjects, Materials and MeshFilters in Prefab
        /// </summary>

        void CreateAssetsDataSubFolders()
        {
            if (!Directory.Exists(projectDataFolder + BACKUP_DATA_PATH))
            {
                Directory.CreateDirectory(projectDataFolder + BACKUP_DATA_PATH);
            }

            if (!Directory.Exists(projectDataFolder + PREFABS_DATA_PATH))
            {
                Directory.CreateDirectory(projectDataFolder + PREFABS_DATA_PATH);
            }

            if (!Directory.Exists(projectDataFolder + ORIGINAL_DATA_PATH))
            {
                Directory.CreateDirectory(projectDataFolder + ORIGINAL_DATA_PATH);
            }

            if (!Directory.Exists(projectDataFolder + SHARED_DATA_PATH))
            {
                Directory.CreateDirectory(projectDataFolder + SHARED_DATA_PATH);
            }

            AssetDatabase.Refresh();
        }

        void GetPrefabObjects()
        {
            prefabObjects = new List<TVEPrefabData>();
            convertedPrefabCount = 0;
            supportedPrefabCount = 0;
            validPrefabCount = 0;

            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                var selection = Selection.gameObjects[i];
                var prefabData = GetPrefabData(selection);

                bool prefabDataExists = false;

                for (int j = 0; j < prefabObjects.Count; j++)
                {
                    if (prefabObjects[j].prefabObject == prefabData.prefabObject)
                    {
                        prefabDataExists = true;
                    }
                }

                if (!prefabDataExists)
                {
                    prefabObjects.Add(prefabData);
                }

                if (prefabData.prefabStatus == PrefabStatus.Converted)
                {
                    convertedPrefabCount++;
                }

                if (prefabData.prefabStatus == PrefabStatus.Supported)
                {
                    supportedPrefabCount++;
                }

                validPrefabCount = convertedPrefabCount + supportedPrefabCount;
            }
        }

        void GetPrefabPresets()
        {
            //presetIndex = 0;
            presetMixedValues = false;
            presetAutoDetected = false;

            var presetIndices = new int[prefabObjects.Count];

            if (prefabObjects.Count > 0)
            {
                for (int o = 0; o < prefabObjects.Count; o++)
                {
                    var prefabComponent = prefabObjects[o].prefabObject.GetComponent<TVEPrefab>();

                    if (prefabComponent != null)
                    {
                        if (prefabComponent.storedPreset != null && prefabComponent.storedPreset != "")
                        {
                            for (int i = 0; i < PresetOptions.Length; i++)
                            {
                                if (prefabComponent.storedPreset == PresetOptions[i])
                                {
                                    presetIndex = i;
                                    presetIndices[o] = i;
                                }
                            }
                        }

                        if (prefabComponent.storedOverrides != null && prefabComponent.storedOverrides.Count > 0)
                        {
                            for (int i = 0; i < prefabComponent.storedOverrides.Count; i++)
                            {
                                for (int j = 0; j < OverrideOptions.Length; j++)
                                {
                                    if (prefabComponent.storedOverrides[i] == OverrideOptions[j])
                                    {
                                        if (!overrideIndices.Contains(j))
                                        {
                                            overrideIndices.Add(j);
                                            overrideGlobals.Add(false);
                                        }
                                    }
                                }
                            }
                        }

                        presetAutoDetected = false;
                    }
                    else
                    {
                        // Try to autodetect preset
                        for (int i = 0; i < detectLines.Count; i++)
                        {
                            if (detectLines[i].StartsWith("Detect"))
                            {
                                var detect = detectLines[i].Replace("Detect ", "").Split(new string[] { " && " }, System.StringSplitOptions.None);
                                var preset = detectLines[i + 1].Replace("Preset ", "").Replace(" - ", "/");

                                int detectCount = 0;

                                for (int d = 0; d < detect.Length; d++)
                                {
                                    var element = detect[d].ToUpperInvariant();

                                    if (element.StartsWith("!"))
                                    {
                                        element = element.Replace("!", "");

                                        if (!AssetDatabase.GetAssetPath(prefabObjects[o].prefabObject).ToUpperInvariant().Contains(element))
                                        {
                                            detectCount++;
                                        }
                                    }
                                    else
                                    {
                                        if (AssetDatabase.GetAssetPath(prefabObjects[o].prefabObject).ToUpperInvariant().Contains(element))
                                        {
                                            detectCount++;
                                        }
                                    }
                                }

                                if (detectCount == detect.Length)
                                {
                                    for (int j = 0; j < PresetOptions.Length; j++)
                                    {
                                        if (PresetOptions[j] == preset)
                                        {
                                            presetIndex = j;
                                            presetIndices[o] = (j);
                                            presetAutoDetected = true;

                                            //break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                for (int i = 1; i < presetIndices.Length; i++)
                {
                    if (presetIndices[i - 1] != presetIndices[i])
                    {
                        presetIndex = 0;
                        presetMixedValues = true;

                        break;
                    }
                }
            }
        }

        void CreatePrefabDataFolder()
        {
            string dataPath;
            string savePath = "/" + prefabName;

            if (collectConvertedData)
            {
                dataPath = projectDataFolder + PREFABS_DATA_PATH + savePath;
            }
            else
            {
                dataPath = AssetDatabase.GetAssetPath(prefabObject);
                dataPath = Path.GetDirectoryName(dataPath);
                dataPath = dataPath + savePath;
                prefabDataFolder = dataPath;
            }

            if (Directory.Exists(dataPath) == false)
            {
                Directory.CreateDirectory(dataPath);
                AssetDatabase.Refresh();
            }
        }

        void CreatePrefabBackupFile()
        {
            string dataPath;
            string savePath = "/" + prefabName + " (" + outputSuffix + " Backup).prefab";

            if (collectConvertedData)
            {
                dataPath = projectDataFolder + BACKUP_DATA_PATH + savePath;
            }
            else
            {
                dataPath = prefabDataFolder + savePath;
            }

            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(prefabObject), dataPath);
            AssetDatabase.Refresh();

            prefabBackup = AssetDatabase.LoadAssetAtPath<GameObject>(dataPath);
        }

        TVEPrefabData GetPrefabData(GameObject selection)
        {
            TVEPrefabData prefabData = new TVEPrefabData(selection, PrefabStatus.Undefined);

            if (PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(selection).Length > 0)
            {
                var prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(selection);
                var prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                var prefabAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(prefabPath);
                var prefabType = PrefabUtility.GetPrefabAssetType(prefabAsset);

                if (prefabAssets.Length == 0)
                {
                    if (prefabType == PrefabAssetType.Regular || prefabType == PrefabAssetType.Variant)
                    {
                        if (prefabAsset.GetComponent<TVEPrefab>() != null && prefabAsset.GetComponent<TVEPrefab>().storedPrefabBackupGUID != "")
                        {
                            prefabData.prefabStatus = PrefabStatus.Converted;
                        }
                        else
                        {
                            prefabData.prefabStatus = PrefabStatus.Supported;
                        }
                    }
                    else if (prefabType == PrefabAssetType.MissingAsset || prefabType == PrefabAssetType.NotAPrefab)
                    {
                        prefabData.prefabStatus = PrefabStatus.Undefined;
                    }
                }
                else
                {
                    if (prefabType == PrefabAssetType.Model || prefabPath.EndsWith(".st") || prefabPath.EndsWith(".prefab"))
                    {
                        prefabData.prefabStatus = PrefabStatus.Unsupported;
                    }
                }

                prefabData.prefabObject = prefabAsset;
            }
            else
            {
                prefabData.prefabObject = selection;
                prefabData.prefabStatus = PrefabStatus.Undefined;
            }

            return prefabData;
        }

        GameObject GetPrefabBackupFile(GameObject prefabInstance)
        {
            GameObject prefabBackupGO = null;

            var prefabBackupGUID = prefabInstance.GetComponent<TVEPrefab>().storedPrefabBackupGUID;

            if (prefabBackupGUID != null || prefabBackupGUID != "")
            {
                var prefabBackupPath = AssetDatabase.GUIDToAssetPath(prefabBackupGUID);
                prefabBackupGO = AssetDatabase.LoadAssetAtPath<GameObject>(prefabBackupPath);
            }

            return prefabBackupGO;
        }

        void GetGameObjectsInPrefab()
        {
            gameObjectsInPrefab = new List<GameObject>();
            gameObjectsInPrefab.Add(prefabInstance);

            GetChildRecursive(prefabInstance);
        }

        void GetChildRecursive(GameObject go)
        {
            foreach (Transform child in go.transform)
            {
                if (child == null)
                    continue;

                gameObjectsInPrefab.Add(child.gameObject);
                GetChildRecursive(child.gameObject);
            }
        }

        void DisableInvalidGameObjectsInPrefab()
        {
            for (int i = 0; i < gameObjectsInPrefab.Count; i++)
            {
                if (gameObjectsInPrefab[i].name.Contains("Impostor") == true)
                {
                    gameObjectsInPrefab[i].SetActive(false);
                    Debug.Log("<b>[The Vegetation Engine]</b> " + "Impostor Mesh are not supported! The " + gameObjectsInPrefab[i].name + " gameobject remains unchanged!");
                }

                if (gameObjectsInPrefab[i].GetComponent<BillboardRenderer>() != null)
                {
                    gameObjectsInPrefab[i].SetActive(false);
                    Debug.Log("<b>[The Vegetation Engine]</b> " + "Billboard Renderers are not supported! The " + gameObjectsInPrefab[i].name + " gameobject has been disabled. You can manually enable them after the conversion is done!");
                }

                if (gameObjectsInPrefab[i].GetComponent<MeshRenderer>() != null)
                {
                    var material = gameObjectsInPrefab[i].GetComponent<MeshRenderer>().sharedMaterial;

                    if (material != null)
                    {
                        if (material.shader.name.Contains("BK/Billboards"))
                        {
                            gameObjectsInPrefab[i].SetActive(false);
                            Debug.Log("<b>[The Vegetation Engine]</b> " + "BK Billboard Renderers are not supported! The " + gameObjectsInPrefab[i].name + " gameobject has been disabled. You can manually enable them after the conversion is done!");
                        }
                    }
                }

                if (gameObjectsInPrefab[i].GetComponent<Tree>() != null)
                {
                    DestroyImmediate(gameObjectsInPrefab[i].GetComponent<Tree>());
                }
            }
        }

        void EnableInvalidGameObjectsInPrefab()
        {
            for (int i = 0; i < gameObjectsInPrefab.Count; i++)
            {
                if (gameObjectsInPrefab[i].name.Contains("Impostor") == true)
                {
                    gameObjectsInPrefab[i].SetActive(true);
                }
            }
        }

        void FixInvalidPrefabScripts()
        {
            for (int i = 0; i < gameObjectsInPrefab.Count; i++)
            {
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObjectsInPrefab[i]);
            }
        }

        void GetMeshRenderersInPrefab()
        {
            meshRenderersInPrefab = new List<MeshRenderer>();

            for (int i = 0; i < gameObjectsInPrefab.Count; i++)
            {
                if (IsValidGameObject(gameObjectsInPrefab[i]) && gameObjectsInPrefab[i].GetComponent<MeshRenderer>() != null)
                {
                    meshRenderersInPrefab.Add(gameObjectsInPrefab[i].GetComponent<MeshRenderer>());
                }
                else
                {
                    meshRenderersInPrefab.Add(null);
                }
            }
        }

        void GetMaterialArraysInPrefab()
        {
            materialArraysInPrefab = new List<Material[]>();

            for (int i = 0; i < meshRenderersInPrefab.Count; i++)
            {
                if (meshRenderersInPrefab[i] != null)
                {
                    materialArraysInPrefab.Add(meshRenderersInPrefab[i].sharedMaterials);
                }
                else
                {
                    materialArraysInPrefab.Add(null);
                }
            }
        }

        void CreateMaterialArraysInstances()
        {
            materialArraysInstances = new List<Material[]>();

            for (int i = 0; i < materialArraysInPrefab.Count; i++)
            {
                if (materialArraysInPrefab[i] != null)
                {
                    var materials = materialArraysInPrefab[i];
                    var materialsInstances = new Material[materials.Length];

                    for (int j = 0; j < materials.Length; j++)
                    {
                        if (materials[j] != null)
                        {
                            if (materials[j].name.Contains("Impostor") == true)
                            {
                                materialsInstances[j] = materials[j];
                            }
                            else
                            {
                                materialsInstances[j] = new Material(shaderLeaf);
                                materialsInstances[j].name = materials[j].name + " (" + outputSuffix + " Material)";
                            }
                        }
                        else
                        {
                            materialsInstances[j] = null;
                        }
                    }

                    materialArraysInstances.Add(materialsInstances);
                }
                else
                {
                    materialArraysInstances.Add(null);
                }
            }
        }

        void GetMeshFiltersInPrefab()
        {
            meshFiltersInPrefab = new List<MeshFilter>();

            for (int i = 0; i < gameObjectsInPrefab.Count; i++)
            {
                var meshFilter = gameObjectsInPrefab[i].GetComponent<MeshFilter>();

                if (IsValidGameObject(gameObjectsInPrefab[i]) && meshFilter != null)
                {
                    meshFiltersInPrefab.Add(meshFilter);
                }
                else
                {
                    meshFiltersInPrefab.Add(null);
                }
            }
        }

        void GetMeshesInPrefab()
        {
            meshesInPrefab = new List<Mesh>();

            for (int i = 0; i < meshFiltersInPrefab.Count; i++)
            {
                if (meshFiltersInPrefab[i] != null)
                {
                    meshesInPrefab.Add(meshFiltersInPrefab[i].sharedMesh);
                }
                else
                {
                    meshesInPrefab.Add(null);
                }
            }
        }

        void CreateMeshInstances()
        {
            meshInstances = new List<Mesh>();

            for (int i = 0; i < meshFiltersInPrefab.Count; i++)
            {
                if (meshesInPrefab[i] != null)
                {
                    PreProcessMesh(meshesInPrefab[i]);

                    var meshInstance = Instantiate(meshesInPrefab[i]);
                    meshInstance.name = meshesInPrefab[i].name + " " + i.ToString("X2");
                    meshInstances.Add(meshInstance);
                }
                else
                {
                    meshInstances.Add(null);
                }
            }
        }

        void GetMeshCollidersInPrefab()
        {
            meshCollidersInPrefab = new List<MeshCollider>();

            for (int i = 0; i < gameObjectsInPrefab.Count; i++)
            {
                var allMeshCollider = gameObjectsInPrefab[i].GetComponents<MeshCollider>();

                if (IsValidGameObject(gameObjectsInPrefab[i]))
                {
                    for (int j = 0; j < allMeshCollider.Length; j++)
                    {
                        if (allMeshCollider[j] != null)
                        {
                            meshCollidersInPrefab.Add(allMeshCollider[j]);
                        }
                        else
                        {
                            meshCollidersInPrefab.Add(null);
                        }
                    }

                }
                else
                {
                    meshCollidersInPrefab.Add(null);
                }
            }
        }

        void GetCollidersInPrefab()
        {
            collidersInPrefab = new List<Mesh>();

            for (int i = 0; i < meshCollidersInPrefab.Count; i++)
            {
                if (meshCollidersInPrefab[i] != null)
                {
                    collidersInPrefab.Add(meshCollidersInPrefab[i].sharedMesh);
                }
                else
                {
                    collidersInPrefab.Add(null);
                }
            }
        }

        void CreateColliderInstances()
        {
            colliderInstances = new List<Mesh>();

            for (int i = 0; i < meshCollidersInPrefab.Count; i++)
            {
                if (collidersInPrefab[i] != null)
                {
                    var colliderInstance = Instantiate(collidersInPrefab[i]);
                    colliderInstance.name = collidersInPrefab[i].name + " " + i.ToString("X2");
                    colliderInstances.Add(colliderInstance);
                }
                else
                {
                    colliderInstances.Add(null);
                }
            }
        }

        void TransformMeshesToWorldSpace()
        {
            for (int i = 0; i < meshInstances.Count; i++)
            {
                if (meshInstances[i] != null)
                {
                    var instance = meshInstances[i];
                    var transforms = meshFiltersInPrefab[i].transform;

                    Vector3[] verticesOS = instance.vertices;
                    Vector3[] verticesWS = new Vector3[instance.vertices.Length];

                    // Transform vertioces OS pos to WS pos
                    for (int j = 0; j < verticesOS.Length; j++)
                    {
                        var trans = transforms.TransformDirection(verticesOS[j]);

                        verticesWS[j] = new Vector3(transforms.lossyScale.x * trans.x + transforms.position.x, transforms.lossyScale.y * trans.y + transforms.position.y, transforms.lossyScale.z * trans.z + transforms.position.z);
                    }

                    meshInstances[i].vertices = verticesWS;

                    //Some meshes don't have normals, check is needed
                    if (instance.normals != null && instance.normals.Length > 0)
                    {
                        Vector3[] normalsOS = instance.normals;
                        Vector3[] normalsWS = new Vector3[instance.vertices.Length];

                        for (int j = 0; j < normalsOS.Length; j++)
                        {
                            var trans = transforms.TransformDirection(normalsOS[j]);

                            normalsWS[j] = new Vector3(transforms.lossyScale.x * trans.x, transforms.lossyScale.y * trans.y, transforms.lossyScale.z * trans.z);
                        }

                        meshInstances[i].normals = normalsWS;
                    }

                    //Some meshes don't have tangenst, check is needed
                    if (instance.tangents != null && instance.tangents.Length > 0)
                    {
                        Vector4[] tangentsOS = instance.tangents;
                        Vector4[] tangentsWS = new Vector4[instance.vertices.Length];

                        for (int j = 0; j < tangentsOS.Length; j++)
                        {
                            tangentsWS[j] = transforms.TransformDirection(tangentsOS[j]);
                        }

                        meshInstances[i].tangents = tangentsWS;
                    }

                    meshInstances[i].RecalculateBounds();
                }
            }

            for (int i = 0; i < colliderInstances.Count; i++)
            {
                if (colliderInstances[i] != null)
                {
                    var instance = colliderInstances[i];
                    var transforms = meshCollidersInPrefab[i].gameObject.transform;

                    Vector3[] verticesOS = instance.vertices;
                    Vector3[] verticesWS = new Vector3[instance.vertices.Length];

                    // Transform vertioces OS pos to WS pos
                    for (int j = 0; j < verticesOS.Length; j++)
                    {
                        var trans = transforms.TransformDirection(verticesOS[j]);

                        verticesWS[j] = verticesWS[j] = new Vector3(transforms.lossyScale.x * trans.x + transforms.position.x, transforms.lossyScale.y * trans.y + transforms.position.y, transforms.lossyScale.z * trans.z + transforms.position.z);
                    }

                    colliderInstances[i].vertices = verticesWS;

                    // Some meshes don't have normals, check is needed
                    if (instance.normals != null && instance.normals.Length > 0)
                    {
                        Vector3[] normalsOS = instance.normals;
                        Vector3[] normalsWS = new Vector3[instance.vertices.Length];

                        for (int j = 0; j < normalsOS.Length; j++)
                        {
                            var trans = transforms.TransformDirection(normalsOS[j]);

                            normalsWS[j] = new Vector3(transforms.lossyScale.x * trans.x, transforms.lossyScale.y * trans.y, transforms.lossyScale.z * trans.z);
                        }

                        colliderInstances[i].normals = normalsWS;
                    }

                    colliderInstances[i].RecalculateTangents();
                    colliderInstances[i].RecalculateBounds();
                }
            }

            for (int i = 0; i < gameObjectsInPrefab.Count; i++)
            {
                //if (meshInstances[i] != null)
                {
                    gameObjectsInPrefab[i].transform.localPosition = Vector3.zero;
                    gameObjectsInPrefab[i].transform.localEulerAngles = Vector3.zero;
                    gameObjectsInPrefab[i].transform.localScale = Vector3.one;
                }
            }
        }

        void GetMaxBoundsInPrefab()
        {
            maxBoundsInfo = Vector4.zero;

            var bounds = new Bounds(Vector3.zero, Vector3.zero);

            for (int i = 0; i < meshInstances.Count; i++)
            {
                if (meshInstances[i] != null)
                {
                    bounds.Encapsulate(meshInstances[i].bounds);
                }
            }

            var maxX = Mathf.Max(Mathf.Abs(bounds.min.x), Mathf.Abs(bounds.max.x));
            var maxZ = Mathf.Max(Mathf.Abs(bounds.min.z), Mathf.Abs(bounds.max.z));

            var maxR = Mathf.Max(maxX, maxZ);
            var maxH = Mathf.Max(Mathf.Abs(bounds.min.y), Mathf.Abs(bounds.max.y));
            var maxS = Mathf.Max(maxR, maxH);

            maxBoundsInfo = new Vector4(maxR, maxH, maxS, 0.0f);
        }

        void PreProcessMesh(Mesh mesh)
        {
            var meshPath = AssetDatabase.GetAssetPath(mesh);

            //Workaround when the mesh is not readable (Unity Bug)
            ModelImporter modelImporter = AssetImporter.GetAtPath(meshPath) as ModelImporter;

            if (modelImporter != null)
            {
                modelImporter.isReadable = true;
                modelImporter.keepQuads = false;
                modelImporter.SaveAndReimport();
            }

            if (meshPath.EndsWith(".asset"))
            {
                //string filePath = Path.Combine(Directory.GetCurrentDirectory(), meshPath);
                //filePath = filePath.Replace("/", "\\");
                string filePath = meshPath;
                string fileText = File.ReadAllText(filePath);
                fileText = fileText.Replace("m_IsReadable: 0", "m_IsReadable: 1");
                File.WriteAllText(filePath, fileText);
                AssetDatabase.Refresh();
            }
        }

        bool IsValidGameObject(GameObject gameObject)
        {
            bool valid = true;

            if (gameObject.activeInHierarchy == false)
            {
                valid = false;
            }

            return valid;
        }

        /// <summary>
        /// Mesh Packing Macros
        /// </summary>

        void GetMeshConversionFromPreset()
        {
            for (int i = 0; i < presetLines.Count; i++)
            {
                if (presetLines[i].StartsWith("Mesh"))
                {
                    string[] splitLine = presetLines[i].Split(char.Parse(" "));
                    string name = "";
                    string source = "";
                    int sourceIndex = 0;
                    string option = "";
                    int optionIndex = 0;
                    string action = "";
                    int actionIndex = 0;

                    if (splitLine.Length > 1)
                    {
                        name = splitLine[1];
                    }

                    if (splitLine.Length > 2)
                    {
                        source = splitLine[2];

                        if (source == "NONE")
                        {
                            sourceIndex = 0;
                        }

                        if (source == "AUTO")
                        {
                            sourceIndex = 0;
                        }

                        // Available options for Float masks
                        if (source == "GET_MASK_FROM_CHANNEL")
                        {
                            sourceIndex = 1;
                        }

                        if (source == "GET_MASK_PROCEDURAL")
                        {
                            sourceIndex = 2;
                        }

                        if (source == "GET_MASK_FROM_TEXTURE")
                        {
                            sourceIndex = 3;
                        }

                        if (source == "GET_MASK_3RD_PARTY")
                        {
                            sourceIndex = 4;
                        }

                        // Available options for Coord masks
                        if (source == "GET_COORD_FROM_CHANNEL")
                        {
                            sourceIndex = 1;
                        }

                        if (source == "GET_COORD_PROCEDURAL")
                        {
                            sourceIndex = 2;
                        }

                        if (source == "GET_COORD_3RD_PARTY")
                        {
                            sourceIndex = 3;
                        }

                        // Available options for Coord masks
                        if (source == "GET_NORMALS_PROCEDURAL")
                        {
                            sourceIndex = 1;
                        }
                    }

                    if (splitLine.Length > 3)
                    {
                        option = splitLine[3];

                        optionIndex = int.Parse(option);
                    }

                    action = splitLine[splitLine.Length - 1];

                    if (action == "ACTION_INVERT")
                    {
                        actionIndex = 1;
                    }

                    if (action == "ACTION_NEGATE")
                    {
                        actionIndex = 2;
                    }

                    if (action == "ACTION_REMAP01")
                    {
                        actionIndex = 3;
                    }

                    if (action == "ACTION_MULTIPLY_BY_HEIGHT")
                    {
                        actionIndex = 4;
                    }

                    if (name == "SetVariation" || name == "SetRed")
                    {
                        sourceVariation = sourceIndex;
                        optionVariation = optionIndex;
                        actionVariation = actionIndex;
                    }

                    if (name == "SetOcclusion" || name == "SetGreen")
                    {
                        sourceOcclusion = sourceIndex;
                        optionOcclusion = optionIndex;
                        actionOcclusion = actionIndex;
                    }

                    if (name == "SetDetailMask" || name == "SetBlue")
                    {
                        sourceDetail = sourceIndex;
                        optionDetail = optionIndex;
                        actionDetail = actionIndex;
                    }

                    if (name == "SetMultiMask" || name == "SetAlpha")
                    {
                        sourceMulti = sourceIndex;
                        optionMulti = optionIndex;
                        actionMulti = actionIndex;
                    }

                    if (name == "SetDetailCoord" || name == "SetPivots")
                    {
                        sourceDetailCoord = sourceIndex;
                        optionDetailCoord = optionIndex;
                    }

                    if (name == "SetMotion1")
                    {
                        sourceMotion1 = sourceIndex;
                        optionMotion1 = optionIndex;
                        actionMotion1 = actionIndex;
                    }

                    if (name == "SetMotion2")
                    {
                        sourceMotion2 = sourceIndex;
                        optionMotion2 = optionIndex;
                        actionMotion2 = actionIndex;
                    }

                    if (name == "SetMotion3")
                    {
                        sourceMotion3 = sourceIndex;
                        optionMotion3 = optionIndex;
                        actionMotion3 = actionIndex;
                    }

                    if (name == "SetNormals")
                    {
                        sourceNormals = sourceIndex;
                        optionNormals = optionIndex;
                    }
                }
            }
        }

        void GetMeshConversionWithTextures(int meshIndex)
        {
            for (int i = 0; i < presetLines.Count; i++)
            {
                if (presetLines[i].StartsWith("Mesh"))
                {
                    string[] splitLine = presetLines[i].Split(char.Parse(" "));
                    string name = "";
                    string source = "";

                    string prop = "";
                    Texture2D texture = null;

                    if (splitLine.Length > 1)
                    {
                        name = splitLine[1];
                    }

                    if (splitLine.Length > 4)
                    {
                        source = splitLine[2];
                        prop = splitLine[4];

                        if (source == "GET_MASK_FROM_TEXTURE")
                        {
                            for (int t = 0; t < materialArraysInPrefab[meshIndex].Length; t++)
                            {
                                if (materialArraysInPrefab[meshIndex] != null)
                                {
                                    var material = materialArraysInPrefab[meshIndex][t];

                                    if (material != null && material.HasProperty(prop))
                                    {
                                        texture = (Texture2D)material.GetTexture(prop);
                                        break;
                                    }
                                }
                            }

                            if (name == "SetVariation" || name == "SetRed")
                            {
                                textureVariation = texture;
                            }

                            if (name == "SetOcclusion" || name == "SetGreen")
                            {
                                textureOcclusion = texture;
                            }

                            if (name == "SetDetailMask" || name == "SetBlue")
                            {
                                textureDetail = texture;
                            }

                            if (name == "SetMultiMask" || name == "SetAlpha")
                            {
                                textureMulti = texture;
                            }

                            if (name == "SetMotion1")
                            {
                                textureMotion1 = texture;
                            }

                            if (name == "SetMotion2")
                            {
                                textureMotion2 = texture;
                            }

                            if (name == "SetMotion3")
                            {
                                textureMotion3 = texture;
                            }
                        }
                    }
                }
            }
        }

        void ConvertColliders()
        {
            for (int i = 0; i < meshCollidersInPrefab.Count; i++)
            {
                if (meshCollidersInPrefab[i] != null && colliderInstances[i] != null)
                {
                    bool meshUnique = true;

                    for (int j = 0; j < meshesInPrefab.Count; j++)
                    {
                        if (collidersInPrefab[i] == meshesInPrefab[j])
                        {
                            meshCollidersInPrefab[i].sharedMesh = meshInstances[j];
                            meshUnique = false;

                            break;
                        }
                    }

                    if (meshUnique)
                    {
                        SaveMesh(colliderInstances[i]);
                        meshCollidersInPrefab[i].sharedMesh = convertedMesh;
                    }
                }
            }
        }

        void ConvertMeshes()
        {
            if (outputMeshIndex == OutputMesh.Off)
            {
                for (int i = 0; i < meshInstances.Count; i++)
                {
                    if (meshInstances[i] != null)
                    {
                        SaveMesh(meshInstances[i]);
                        meshFiltersInPrefab[i].sharedMesh = convertedMesh;
                    }
                }
            }
            else if (outputMeshIndex == OutputMesh.Default)
            {
                for (int i = 0; i < meshInstances.Count; i++)
                {
                    if (meshInstances[i] != null)
                    {
                        GetMeshConversionWithTextures(i);
                        ConvertMeshDefault(meshInstances[i]);
                        ConvertMeshNormals(meshInstances[i], i, sourceNormals, optionNormals);

                        SaveMesh(meshInstances[i]);
                        meshFiltersInPrefab[i].sharedMesh = convertedMesh;
                    }
                }
            }
            else if (outputMeshIndex == OutputMesh.Custom)
            {
                for (int i = 0; i < meshInstances.Count; i++)
                {
                    if (meshInstances[i] != null)
                    {
                        GetMeshConversionWithTextures(i);
                        ConvertMeshCustom(meshInstances[i]);
                        ConvertMeshNormals(meshInstances[i], i, sourceNormals, optionNormals);

                        SaveMesh(meshInstances[i]);
                        meshFiltersInPrefab[i].sharedMesh = convertedMesh;
                    }
                }
            }
            else if (outputMeshIndex == OutputMesh.Polygonal)
            {
                for (int i = 0; i < meshInstances.Count; i++)
                {
                    if (meshInstances[i] != null)
                    {
                        ConvertMeshPolygonal(meshInstances[i]);

                        SaveMesh(meshInstances[i]);
                        meshFiltersInPrefab[i].sharedMesh = convertedMesh;
                    }
                }
            }
            else if (outputMeshIndex == OutputMesh.DEEnvironment)
            {
                for (int i = 0; i < meshInstances.Count; i++)
                {
                    if (meshInstances[i] != null)
                    {
                        GetMeshConversionWithTextures(i);
                        ConvertMeshDEEnvironment(meshInstances[i]);
                        ConvertMeshNormals(meshInstances[i], i, sourceNormals, optionNormals);

                        SaveMesh(meshInstances[i]);
                        meshFiltersInPrefab[i].sharedMesh = convertedMesh;
                    }
                }
            }
        }

        void ConvertMeshDefault(Mesh mesh)
        {
            var vertexCount = mesh.vertexCount;

            var colors = new Color[vertexCount];

            var UV0 = GetCoordData(mesh, 0, 0);
            var UV4 = GetCoordData(mesh, 0, 0);

            var multiMask = new List<float>(vertexCount);

            if (sourceMulti == 0)
            {
                multiMask = GetMaskData(mesh, 2, 4, textureMulti, 0, 1.0f);
            }
            else
            {
                multiMask = GetMaskData(mesh, sourceMulti, optionMulti, textureMulti, actionMulti, 1.0f);
            }

            var occlusion = GetMaskData(mesh, sourceOcclusion, optionOcclusion, textureOcclusion, actionOcclusion, 1.0f);
            var detailMask = GetMaskData(mesh, sourceDetail, optionDetail, textureDetail, actionDetail, 1.0f);
            var variation = GetMaskData(mesh, sourceVariation, optionVariation, textureVariation, actionVariation, 1.0f);

            var detailCoordOrPivots = GetCoordData(mesh, sourceDetailCoord, optionDetailCoord);

            var motion1 = GetMaskData(mesh, sourceMotion1, optionMotion1, textureMotion1, actionMotion1, 1.0f);
            var motion2 = GetMaskData(mesh, sourceMotion2, optionMotion2, textureMotion2, actionMotion2, 1.0f);
            var motion3 = GetMaskData(mesh, sourceMotion3, optionMotion3, textureMotion3, actionMotion3, 1.0f);

            for (int i = 0; i < vertexCount; i++)
            {
                colors[i] = new Color(variation[i], occlusion[i], detailMask[i], multiMask[i]);
                UV0[i] = new Vector4(UV0[i].x, UV0[i].y, detailCoordOrPivots[i].x, detailCoordOrPivots[i].y);
                UV4[i] = new Vector4(motion1[i], motion2[i], motion3[i], 0);
            }

            mesh.colors = colors;
            mesh.SetUVs(0, UV0);
            mesh.SetUVs(3, UV4);
        }

        void ConvertMeshCustom(Mesh mesh)
        {
            var vertexCount = mesh.vertexCount;

            var colors = new Color[vertexCount];

            var red = GetMaskData(mesh, sourceVariation, optionVariation, textureVariation, actionVariation, 1.0f);
            var green = GetMaskData(mesh, sourceOcclusion, optionOcclusion, textureOcclusion, actionOcclusion, 1.0f);
            var blue = GetMaskData(mesh, sourceDetail, optionDetail, textureDetail, actionDetail, 1.0f);
            var alpha = GetMaskData(mesh, sourceMulti, optionMulti, textureMulti, actionMulti, 1.0f);

            for (int i = 0; i < vertexCount; i++)
            {
                colors[i] = new Color(red[i], green[i], blue[i], alpha[i]);
            }

            mesh.colors = colors;
        }

        void ConvertMeshPolygonal(Mesh mesh)
        {
            var vertexCount = mesh.vertexCount;

            var colors = new Color[vertexCount];

            if (mesh.colors == null || mesh.colors.Length == 0)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    colors[i] = Color.white;
                }
            }
            else
            {
                colors = mesh.colors;
            }

            var alpha = GetMaskData(mesh, 2, 4, textureMulti, 0, 1.0f);

            for (int i = 0; i < vertexCount; i++)
            {
                colors[i] = new Color(colors[i].r, colors[i].g, colors[i].b, alpha[i]);
            }

            mesh.colors = colors;
        }

        void ConvertMeshDEEnvironment(Mesh mesh)
        {
            var vertexCount = mesh.vertexCount;

            var colors = new Color[vertexCount];

            var occlusion = GetMaskData(mesh, sourceOcclusion, optionOcclusion, textureOcclusion, actionOcclusion, 1.0f);
            var variation = GetMaskData(mesh, sourceVariation, optionVariation, textureVariation, actionVariation, 1.0f);
            var motion1 = GetMaskData(mesh, sourceMotion1, optionMotion1, textureMotion1, actionMotion1, 1.0f);
            var motion2 = GetMaskData(mesh, sourceMotion2, optionMotion2, textureMotion2, actionMotion2, 1.0f);

            for (int i = 0; i < vertexCount; i++)
            {
                colors[i] = new Color(motion1[i], variation[i], motion2[i], occlusion[i]);
            }

            mesh.colors = colors;
        }

        void SaveMesh(Mesh mesh)
        {
            string dataPath;
            string savePath = "/" + mesh.name.Replace(":", "") + " (" + outputSuffix + " Mesh).asset";

            if (outputReadWrite == OutputReadWrite.MarkMeshesAsNonReadable)
            {
                mesh.UploadMeshData(true);
            }

            if (collectConvertedData)
            {
                dataPath = projectDataFolder + PREFABS_DATA_PATH + "/" + prefabName + savePath;
            }
            else
            {
                dataPath = prefabDataFolder + savePath;
            }

            if (!File.Exists(dataPath))
            {
                AssetDatabase.CreateAsset(mesh, dataPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else
            {
                var tveMeshFile = AssetDatabase.LoadAssetAtPath<Mesh>(dataPath);
                tveMeshFile.Clear();
                EditorUtility.CopySerialized(mesh, tveMeshFile);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            AssetDatabase.SetLabels(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(dataPath), new string[] { outputSuffix + " Mesh" });

            convertedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(dataPath);
        }

        // Get Float data
        List<float> GetMaskData(Mesh mesh, int source, int option, Texture2D texture, int action, float defaulValue)
        {
            var meshChannel = new List<float>();

            if (source == 0)
            {
                meshChannel = GetMaskDefaultValue(mesh, defaulValue);
            }

            else if (source == 1)
            {
                meshChannel = GetMaskMeshData(mesh, option, defaulValue);
            }

            else if (source == 2)
            {
                meshChannel = GetMaskProceduralData(mesh, option);
            }

            else if (source == 3)
            {
                meshChannel = GetMaskFromTextureData(mesh, option, texture);
            }

            else if (source == 4)
            {
                meshChannel = GetMask3rdPartyData(mesh, option);
            }

            if (action > 0)
            {
                meshChannel = MeshAction(meshChannel, mesh, action);
            }

            return meshChannel;
        }

        List<float> GetMaskDefaultValue(Mesh mesh, float defaulValue)
        {
            var vertexCount = mesh.vertexCount;

            var meshChannel = new List<float>(vertexCount);

            for (int i = 0; i < vertexCount; i++)
            {
                meshChannel.Add(defaulValue);
            }

            return meshChannel;
        }

        List<float> GetMaskMeshData(Mesh mesh, int option, float defaulValue)
        {
            var vertexCount = mesh.vertexCount;

            var meshChannel = new List<float>(vertexCount);

            // Vertex Color Data
            if (option == 0)
            {
                var channel = new List<Color>(vertexCount);
                mesh.GetColors(channel);
                var channelCount = channel.Count;

                for (int i = 0; i < channelCount; i++)
                {
                    meshChannel.Add(channel[i].r);
                }
            }

            else if (option == 1)
            {
                var channel = new List<Color>(vertexCount);
                mesh.GetColors(channel);
                var channelCount = channel.Count;

                for (int i = 0; i < channelCount; i++)
                {
                    meshChannel.Add(channel[i].g);
                }
            }

            else if (option == 2)
            {
                var channel = new List<Color>(vertexCount);
                mesh.GetColors(channel);
                var channelCount = channel.Count;

                for (int i = 0; i < channelCount; i++)
                {
                    meshChannel.Add(channel[i].b);
                }
            }

            else if (option == 3)
            {
                var channel = new List<Color>(vertexCount);
                mesh.GetColors(channel);
                var channelCount = channel.Count;

                for (int i = 0; i < channelCount; i++)
                {
                    meshChannel.Add(channel[i].a);
                }
            }

            // UV 0 Data
            else if (option == 4)
            {
                var channel = new List<Vector4>(vertexCount);
                mesh.GetUVs(0, channel);
                var channelCount = channel.Count;

                for (int i = 0; i < channelCount; i++)
                {
                    meshChannel.Add(channel[i].x);
                }
            }

            else if (option == 5)
            {
                var channel = new List<Vector4>(vertexCount);
                mesh.GetUVs(0, channel);
                var channelCount = channel.Count;

                for (int i = 0; i < channelCount; i++)
                {
                    meshChannel.Add(channel[i].y);
                }
            }

            else if (option == 6)
            {
                var channel = new List<Vector4>(vertexCount);
                mesh.GetUVs(0, channel);
                var channelCount = channel.Count;

                for (int i = 0; i < channelCount; i++)
                {
                    meshChannel.Add(channel[i].z);
                }
            }

            else if (option == 7)
            {
                var channel = new List<Vector4>(vertexCount);
                mesh.GetUVs(0, channel);
                var channelCount = channel.Count;

                for (int i = 0; i < channelCount; i++)
                {
                    meshChannel.Add(channel[i].w);
                }
            }

            // UV 2 Data
            else if (option == 8)
            {
                var channel = new List<Vector4>(vertexCount);
                mesh.GetUVs(1, channel);
                var channelCount = channel.Count;

                for (int i = 0; i < channelCount; i++)
                {
                    meshChannel.Add(channel[i].x);
                }
            }

            else if (option == 9)
            {
                var channel = new List<Vector4>(vertexCount);
                mesh.GetUVs(1, channel);
                var channelCount = channel.Count;

                for (int i = 0; i < channelCount; i++)
                {
                    meshChannel.Add(channel[i].y);
                }
            }

            else if (option == 10)
            {
                var channel = new List<Vector4>(vertexCount);
                mesh.GetUVs(1, channel);
                var channelCount = channel.Count;

                for (int i = 0; i < channelCount; i++)
                {
                    meshChannel.Add(channel[i].z);
                }
            }

            else if (option == 11)
            {
                var channel = new List<Vector4>(vertexCount);
                mesh.GetUVs(1, channel);
                var channelCount = channel.Count;

                for (int i = 0; i < channelCount; i++)
                {
                    meshChannel.Add(channel[i].w);
                }
            }

            // UV 3 Data
            else if (option == 12)
            {
                var channel = new List<Vector4>(vertexCount);
                mesh.GetUVs(2, channel);
                var channelCount = channel.Count;

                for (int i = 0; i < channelCount; i++)
                {
                    meshChannel.Add(channel[i].x);
                }
            }

            else if (option == 13)
            {
                var channel = new List<Vector4>(vertexCount);
                mesh.GetUVs(2, channel);
                var channelCount = channel.Count;

                for (int i = 0; i < channelCount; i++)
                {
                    meshChannel.Add(channel[i].y);
                }
            }

            else if (option == 14)
            {
                var channel = new List<Vector4>(vertexCount);
                mesh.GetUVs(2, channel);
                var channelCount = channel.Count;

                for (int i = 0; i < channelCount; i++)
                {
                    meshChannel.Add(channel[i].z);
                }
            }

            else if (option == 15)
            {
                var channel = new List<Vector4>(vertexCount);
                mesh.GetUVs(2, channel);
                var channelCount = channel.Count;

                for (int i = 0; i < channelCount; i++)
                {
                    meshChannel.Add(channel[i].w);
                }
            }

            // UV 4 Data
            else if (option == 16)
            {
                var channel = new List<Vector4>(vertexCount);
                mesh.GetUVs(3, channel);
                var channelCount = channel.Count;

                for (int i = 0; i < channelCount; i++)
                {
                    meshChannel.Add(channel[i].x);
                }
            }

            else if (option == 17)
            {
                var channel = new List<Vector4>(vertexCount);
                mesh.GetUVs(3, channel);
                var channelCount = channel.Count;

                for (int i = 0; i < channelCount; i++)
                {
                    meshChannel.Add(channel[i].y);
                }
            }

            else if (option == 18)
            {
                var channel = new List<Vector4>(vertexCount);
                mesh.GetUVs(3, channel);
                var channelCount = channel.Count;

                for (int i = 0; i < channelCount; i++)
                {
                    meshChannel.Add(channel[i].z);
                }
            }

            else if (option == 19)
            {
                var channel = new List<Vector4>(vertexCount);
                mesh.GetUVs(3, channel);
                var channelCount = channel.Count;

                for (int i = 0; i < channelCount; i++)
                {
                    meshChannel.Add(channel[i].w);
                }
            }

            if (meshChannel.Count == 0)
            {
                meshChannel = GetMaskDefaultValue(mesh, defaulValue);
            }

            return meshChannel;
        }

        List<float> GetMaskProceduralData(Mesh mesh, int option)
        {
            var vertexCount = mesh.vertexCount;
            var vertices = mesh.vertices;
            var normals = mesh.normals;

            var meshChannel = new List<float>(vertexCount);

            if (option == 0)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    meshChannel.Add(0.0f);
                }
            }
            else if (option == 1)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    meshChannel.Add(1.0f);
                }
            }
            // Random Variation
            else if (option == 2)
            {
                // Good Enough approach
                var triangles = mesh.triangles;
                var trianglesCount = mesh.triangles.Length;

                for (int i = 0; i < vertexCount; i++)
                {
                    meshChannel.Add(-99);
                }

                for (int i = 0; i < trianglesCount; i += 3)
                {
                    var index1 = triangles[i + 0];
                    var index2 = triangles[i + 1];
                    var index3 = triangles[i + 2];

                    float variation = 0;

                    if (meshChannel[index1] != -99)
                    {
                        variation = meshChannel[index1];
                    }
                    else if (meshChannel[index2] != -99)
                    {
                        variation = meshChannel[index2];
                    }
                    else if (meshChannel[index3] != -99)
                    {
                        variation = meshChannel[index3];
                    }
                    else
                    {
                        variation = UnityEngine.Random.Range(0.0f, 1.0f);
                    }

                    meshChannel[index1] = variation;
                    meshChannel[index2] = variation;
                    meshChannel[index3] = variation;
                }
            }
            // Predictive Variation
            else if (option == 3)
            {
                var triangles = mesh.triangles;
                var trianglesCount = mesh.triangles.Length;

                var elementIndices = new List<int>(vertexCount);
                int elementCount = 0;

                for (int i = 0; i < vertexCount; i++)
                {
                    elementIndices.Add(-99);
                }

                for (int i = 0; i < trianglesCount; i += 3)
                {
                    var index1 = triangles[i + 0];
                    var index2 = triangles[i + 1];
                    var index3 = triangles[i + 2];

                    int element = 0;

                    if (elementIndices[index1] != -99)
                    {
                        element = elementIndices[index1];
                    }
                    else if (elementIndices[index2] != -99)
                    {
                        element = elementIndices[index2];
                    }
                    else if (elementIndices[index3] != -99)
                    {
                        element = elementIndices[index3];
                    }
                    else
                    {
                        element = elementCount;
                        elementCount++;
                    }

                    elementIndices[index1] = element;
                    elementIndices[index2] = element;
                    elementIndices[index3] = element;
                }

                for (int i = 0; i < elementIndices.Count; i++)
                {
                    var variation = (float)elementIndices[i] / elementCount;
                    variation = Mathf.Repeat(variation * seed, 1.0f);
                    meshChannel.Add(variation);
                }
            }
            // Normalized in bounds height
            else if (option == 4)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    var mask = Mathf.Clamp01(vertices[i].y / maxBoundsInfo.y);

                    meshChannel.Add(mask);
                }
            }
            // Procedural Sphere
            else if (option == 5)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    var mask = Mathf.Clamp01(Vector3.Distance(vertices[i], Vector3.zero) / maxBoundsInfo.x);

                    meshChannel.Add(mask);
                }
            }
            // Procedural Cylinder no Cap
            else if (option == 6)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    var mask = Mathf.Clamp01(MathRemap(Vector3.Distance(vertices[i], new Vector3(0, vertices[i].y, 0)), maxBoundsInfo.x * 0.1f, maxBoundsInfo.x, 0f, 1f));

                    meshChannel.Add(mask);
                }
            }
            // Procedural Capsule
            else if (option == 7)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    var maskCyl = Mathf.Clamp01(MathRemap(Vector3.Distance(vertices[i], new Vector3(0, vertices[i].y, 0)), maxBoundsInfo.x * 0.1f, maxBoundsInfo.x, 0f, 1f));
                    var maskCap = Vector3.Magnitude(new Vector3(0, Mathf.Clamp01(MathRemap(vertices[i].y / maxBoundsInfo.y, 0.8f, 1f, 0f, 1f)), 0));
                    var maskBase = Mathf.Clamp01(MathRemap(vertices[i].y / maxBoundsInfo.y, 0f, 0.1f, 0f, 1f));
                    var mask = Mathf.Clamp01(maskCyl + maskCap) * maskBase;

                    meshChannel.Add(mask);
                }
            }
            // Bottom To Top
            else if (option == 8)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    var mask = 1.0f - Mathf.Clamp01(vertices[i].y / maxBoundsInfo.y);

                    meshChannel.Add(mask);
                }
            }
            // Top To Bottom
            else if (option == 9)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    var mask = Mathf.Clamp01(vertices[i].y / maxBoundsInfo.y);

                    meshChannel.Add(mask);
                }
            }
            // Bottom Projection
            else if (option == 10)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    var mask = Mathf.Clamp01(Vector3.Dot(new Vector3(0, -1, 0), normals[i]) * 0.5f + 0.5f);

                    meshChannel.Add(mask);
                }
            }
            // Top Projection
            else if (option == 11)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    var mask = Mathf.Clamp01(Vector3.Dot(new Vector3(0, 1, 0), normals[i]) * 0.5f + 0.5f);

                    meshChannel.Add(mask);
                }
            }
            // Height Exp
            else if (option == 12)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    var oneMinusMask = 1 - Mathf.Clamp01(vertices[i].y / maxBoundsInfo.y);
                    var powerMask = oneMinusMask * oneMinusMask * oneMinusMask * oneMinusMask;
                    var mask = 1 - powerMask;

                    meshChannel.Add(mask);
                }
            }
            //Hemi Sphere
            else if (option == 13)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    var height = Mathf.Clamp01(vertices[i].y / maxBoundsInfo.y);
                    var sphere = Mathf.Clamp01(Vector3.Distance(vertices[i], Vector3.zero) / maxBoundsInfo.x);
                    var mask = height * sphere;

                    meshChannel.Add(mask);
                }
            }
            //Hemi Cylinder
            else if (option == 14)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    var height = Mathf.Clamp01(vertices[i].y / maxBoundsInfo.y);
                    var cyl = Mathf.Clamp01(MathRemap(Vector3.Distance(vertices[i], new Vector3(0, vertices[i].y, 0)), maxBoundsInfo.x * 0.1f, maxBoundsInfo.x, 0f, 1f));
                    var mask = height * cyl;

                    meshChannel.Add(mask);
                }
            }
            //Hemi Capsule
            else if (option == 15)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    var height = Mathf.Clamp01(vertices[i].y / maxBoundsInfo.y);
                    var maskCyl = Mathf.Clamp01(MathRemap(Vector3.Distance(vertices[i], new Vector3(0, vertices[i].y, 0)), maxBoundsInfo.x * 0.1f, maxBoundsInfo.x, 0f, 1f));
                    var maskCap = Vector3.Magnitude(new Vector3(0, Mathf.Clamp01(MathRemap(vertices[i].y / maxBoundsInfo.y, 0.8f, 1f, 0f, 1f)), 0));
                    var maskBase = Mathf.Clamp01(MathRemap(vertices[i].y / maxBoundsInfo.y, 0f, 0.1f, 0f, 1f));
                    var mask = Mathf.Clamp01(maskCyl + maskCap) * maskBase * height;

                    meshChannel.Add(mask);
                }
            }
            // Normalized in bounds height with black Offset at the bottom
            else if (option == 16)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    var height = Mathf.Clamp01(vertices[i].y / maxBoundsInfo.y);
                    var mask = Mathf.Clamp01((height - 0.2f) / (1 - 0.2f));

                    meshChannel.Add(mask);
                }
            }
            // Normalized in bounds height with black Offset at the bottom
            else if (option == 17)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    var height = Mathf.Clamp01(vertices[i].y / maxBoundsInfo.y);
                    var mask = Mathf.Clamp01((height - 0.4f) / (1 - 0.4f));

                    meshChannel.Add(mask);
                }
            }
            // Normalized in bounds height with black Offset at the bottom
            else if (option == 18)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    var height = Mathf.Clamp01(vertices[i].y / maxBoundsInfo.y);
                    var mask = Mathf.Clamp01((height - 0.6f) / (1 - 0.6f));

                    meshChannel.Add(mask);
                }
            }

            return meshChannel;
        }

        List<float> GetMask3rdPartyData(Mesh mesh, int option)
        {
            var vertexCount = mesh.vertexCount;
            var vertices = mesh.vertices;

            var meshChannel = new List<float>();

            // CTI Leaves Mask
            if (option == 0)
            {
                var UV3 = mesh.uv3;

                for (int i = 0; i < vertexCount; i++)
                {
                    var pivotX = (Mathf.Repeat(UV3[i].x, 1.0f) * 2.0f) - 1.0f;
                    var pivotZ = (Mathf.Repeat(32768.0f * UV3[i].x, 1.0f) * 2.0f) - 1.0f;
                    var pivotY = Mathf.Sqrt(1.0f - Mathf.Clamp01(Vector2.Dot(new Vector2(pivotX, pivotZ), new Vector2(pivotX, pivotZ))));

                    var pivot = new Vector3(pivotX * UV3[i].y, pivotY * UV3[i].y, pivotZ * UV3[i].y);
                    var pos = vertices[i];

                    var mask = Vector3.Magnitude(pos - pivot) / (maxBoundsInfo.x * 1f);

                    meshChannel.Add(mask);
                }
            }
            // CTI Leaves Variation
            else if (option == 1)
            {
                var UV3 = mesh.uv3;

                for (int i = 0; i < vertexCount; i++)
                {
                    var pivotX = (Mathf.Repeat(UV3[i].x, 1.0f) * 2.0f) - 1.0f;
                    var pivotZ = (Mathf.Repeat(32768.0f * UV3[i].x, 1.0f) * 2.0f) - 1.0f;
                    var pivotY = Mathf.Sqrt(1.0f - Mathf.Clamp01(Vector2.Dot(new Vector2(pivotX, pivotZ), new Vector2(pivotX, pivotZ))));

                    var pivot = new Vector3(pivotX * UV3[i].y, pivotY * UV3[i].y, pivotZ * UV3[i].y);

                    var variX = Mathf.Repeat(pivot.x * 33.3f, 1.0f);
                    var variY = Mathf.Repeat(pivot.y * 33.3f, 1.0f);
                    var variZ = Mathf.Repeat(pivot.z * 33.3f, 1.0f);

                    var mask = variX + variY + variZ;

                    if (UV3[i].x < 0.01f)
                    {
                        mask = 0.0f;
                    }

                    meshChannel.Add(mask);
                }
            }
            // ST8 Leaves Mask
            else if (option == 2)
            {
                var UV2 = new List<Vector4>();
                var UV3 = new List<Vector4>();
                var UV4 = new List<Vector4>();

                mesh.GetUVs(1, UV2);
                mesh.GetUVs(2, UV3);
                mesh.GetUVs(3, UV4);

                if (UV4.Count != 0)
                {
                    for (int i = 0; i < vertexCount; i++)
                    {
                        var anchor = new Vector3(UV2[i].z - vertices[i].x, UV2[i].w - vertices[i].y, UV3[i].w - vertices[i].z);
                        var length = Vector3.Magnitude(anchor);
                        var leaves = UV2[i].w * UV4[i].w;

                        var mask = (length * leaves) / maxBoundsInfo.x;

                        meshChannel.Add(mask);
                    }
                }
                else
                {
                    for (int i = 0; i < vertexCount; i++)
                    {
                        var mask = Mathf.Clamp01(vertices[i].y / maxBoundsInfo.y);

                        meshChannel.Add(mask);
                    }
                }
            }
            // NM Leaves Mask
            else if (option == 3)
            {
                var tempColors = new List<Color>(vertexCount);
                mesh.GetColors(tempColors);

                if (tempColors.Count != 0)
                {
                    for (int i = 0; i < vertexCount; i++)
                    {
                        if (tempColors[i].a > 0.99f)
                        {
                            meshChannel.Add(0.0f);
                        }
                        else
                        {
                            meshChannel.Add(tempColors[i].a);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < vertexCount; i++)
                    {
                        var mask = Mathf.Clamp01(vertices[i].y / maxBoundsInfo.y);

                        meshChannel.Add(mask);
                    }
                }
            }

            return meshChannel;
        }

        List<float> GetMaskFromTextureData(Mesh mesh, int option, Texture2D texture)
        {
            var vertexCount = mesh.vertexCount;
            var meshChannel = new List<float>(vertexCount);

            if (texture == null)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    meshChannel.Add(1);
                }
            }
            else
            {
                string texPath = AssetDatabase.GetAssetPath(texture);
                TextureImporter texImporter = AssetImporter.GetAtPath(texPath) as TextureImporter;

                texImporter.isReadable = true;
                texImporter.SaveAndReimport();
                AssetDatabase.Refresh();

                var meshCoord = new List<Vector2>(vertexCount);

                mesh.GetUVs(0, meshCoord);

                if (option == 0)
                {
                    for (int i = 0; i < vertexCount; i++)
                    {
                        var pixel = texture.GetPixelBilinear(meshCoord[i].x, meshCoord[i].y);
                        meshChannel.Add(pixel.r);
                    }
                }

                else if (option == 1)
                {
                    for (int i = 0; i < vertexCount; i++)
                    {
                        var pixel = texture.GetPixelBilinear(meshCoord[i].x, meshCoord[i].y);
                        meshChannel.Add(pixel.g);
                    }
                }

                else if (option == 2)
                {
                    for (int i = 0; i < vertexCount; i++)
                    {
                        var pixel = texture.GetPixelBilinear(meshCoord[i].x, meshCoord[i].y);
                        meshChannel.Add(pixel.b);
                    }
                }

                else if (option == 3)
                {
                    for (int i = 0; i < vertexCount; i++)
                    {
                        var pixel = texture.GetPixelBilinear(meshCoord[i].x, meshCoord[i].y);
                        meshChannel.Add(pixel.a);
                    }
                }

                texImporter.isReadable = false;
                texImporter.SaveAndReimport();
                AssetDatabase.Refresh();
            }

            return meshChannel;
        }

        List<Vector4> GetCoordData(Mesh mesh, int source, int option)
        {
            var vertexCount = mesh.vertexCount;

            var meshCoord = new List<Vector4>(vertexCount);

            if (source == 0)
            {
                mesh.GetUVs(0, meshCoord);
            }
            else if (source == 1)
            {
                meshCoord = GetCoordMeshData(mesh, option);
            }
            else if (source == 2)
            {
                meshCoord = GetCoordProceduralData(mesh, option);
            }
            else if (source == 3)
            {
                meshCoord = GetCoord3rdPartyData(mesh, option);
            }

            if (meshCoord.Count == 0)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    meshCoord.Add(Vector4.zero);
                }
            }

            return meshCoord;
        }

        List<Vector4> GetCoordMeshData(Mesh mesh, int option)
        {
            var vertexCount = mesh.vertexCount;

            var meshCoord = new List<Vector4>(vertexCount);

            if (option == 0)
            {
                mesh.GetUVs(0, meshCoord);
            }

            else if (option == 1)
            {
                mesh.GetUVs(1, meshCoord);
            }

            else if (option == 2)
            {
                mesh.GetUVs(2, meshCoord);
            }

            else if (option == 3)
            {
                mesh.GetUVs(3, meshCoord);
            }

            return meshCoord;
        }

        List<Vector4> GetCoordProceduralData(Mesh mesh, int option)
        {
            var vertexCount = mesh.vertexCount;
            var vertices = mesh.vertices;

            var meshCoord = new List<Vector4>(vertexCount);

            // Planar XZ
            if (option == 0)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    meshCoord.Add(new Vector4(vertices[i].x, vertices[i].z, 0, 0));
                }
            }
            // Planar XY
            else if (option == 1)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    meshCoord.Add(new Vector4(vertices[i].x, vertices[i].y, 0, 0));
                }
            }
            // Planar ZY
            else if (option == 2)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    meshCoord.Add(new Vector4(vertices[i].z, vertices[i].y, 0, 0));
                }
            }
            // Procedural Pivots XZ
            else if (option == 3)
            {
                meshCoord = GenerateElementPivot(mesh);
            }

            return meshCoord;
        }

        List<Vector4> GetCoord3rdPartyData(Mesh mesh, int option)
        {
            var vertexCount = mesh.vertexCount;

            var meshCoord = new List<Vector4>(vertexCount);

            // NM Trunk Blend
            if (option == 0)
            {
                mesh.GetUVs(2, meshCoord);

                if (meshCoord.Count == 0)
                {
                    mesh.GetUVs(1, meshCoord);
                }
            }

            return meshCoord;
        }

        List<Vector4> GenerateElementPivot(Mesh mesh)
        {
            var vertexCount = mesh.vertexCount;
            var vertices = mesh.vertices;
            var triangles = mesh.triangles;
            var trianglesCount = mesh.triangles.Length;

            var elementIndices = new List<int>(vertexCount);
            var meshPivots = new List<Vector4>(vertexCount);
            int elementCount = 0;

            for (int i = 0; i < vertexCount; i++)
            {
                elementIndices.Add(-99);
            }

            for (int i = 0; i < vertexCount; i++)
            {
                meshPivots.Add(Vector3.zero);
            }

            for (int i = 0; i < trianglesCount; i += 3)
            {
                var index1 = triangles[i + 0];
                var index2 = triangles[i + 1];
                var index3 = triangles[i + 2];

                int element = 0;

                if (elementIndices[index1] != -99)
                {
                    element = elementIndices[index1];
                }
                else if (elementIndices[index2] != -99)
                {
                    element = elementIndices[index2];
                }
                else if (elementIndices[index3] != -99)
                {
                    element = elementIndices[index3];
                }
                else
                {
                    element = elementCount;
                    elementCount++;
                }

                elementIndices[index1] = element;
                elementIndices[index2] = element;
                elementIndices[index3] = element;
            }

            for (int e = 0; e < elementCount; e++)
            {
                var positions = new List<Vector3>();

                for (int i = 0; i < elementIndices.Count; i++)
                {
                    if (elementIndices[i] == e)
                    {
                        positions.Add(vertices[i]);
                    }
                }

                float x = 0;
                float z = 0;

                for (int p = 0; p < positions.Count; p++)
                {
                    x = x + positions[p].x;
                    z = z + positions[p].z;
                }

                for (int i = 0; i < elementIndices.Count; i++)
                {
                    if (elementIndices[i] == e)
                    {
                        meshPivots[i] = new Vector3(x / positions.Count, z / positions.Count);
                    }
                }
            }

            return meshPivots;
        }

        void ConvertMeshNormals(Mesh mesh, int index, int source, int option)
        {
            if (source == 1)
            {
                var vertexCount = mesh.vertexCount;
                var vertices = mesh.vertices;
                var normals = mesh.normals;
                var subMeshMaterials = materialArraysInstances[index];
                var subMeshIndices = new List<int>(subMeshMaterials.Length + 1);

                for (int i = 0; i < subMeshMaterials.Length; i++)
                {
                    var subMeshDescriptor = mesh.GetSubMesh(i);

                    subMeshIndices.Add(subMeshDescriptor.firstVertex);
                }

                subMeshIndices.Add(vertexCount);

                if (option == 0 || normals == null)
                {
                    mesh.RecalculateNormals();
                }

                Vector3[] customNormals = mesh.normals;

                for (int s = 0; s < subMeshIndices.Count - 1; s++)
                {
                    //Debug.Log(subMeshIndices[s] + "  " + subMeshIndices[s + 1] + "  " + subMeshMaterials[s].shader.name);

                    if (subMeshMaterials[s].shader.name.Contains("Bark") || subMeshMaterials[s].shader.name.Contains("Prop"))
                    {
                        for (int i = subMeshIndices[s]; i < subMeshIndices[s + 1]; i++)
                        {
                            customNormals[i] = normals[i];
                        }
                    }
                    else
                    {
                        // Flat Shading Low
                        if (option == 1)
                        {
                            for (int i = subMeshIndices[s]; i < subMeshIndices[s + 1]; i++)
                            {
                                var height = Mathf.Clamp01(vertices[i].y / maxBoundsInfo.y);

                                customNormals[i] = Vector3.Lerp(normals[i], new Vector3(0, 1, 0), height);
                            }
                        }

                        // Flat Shading Medium
                        else if (option == 2)
                        {
                            for (int i = subMeshIndices[s]; i < subMeshIndices[s + 1]; i++)
                            {
                                var height = Mathf.Clamp01(Mathf.Clamp01(vertices[i].y / maxBoundsInfo.y) + 0.5f);

                                customNormals[i] = Vector3.Lerp(normals[i], new Vector3(0, 1, 0), height);
                            }
                        }

                        // Flat Shading Full
                        else if (option == 3)
                        {
                            for (int i = subMeshIndices[s]; i < subMeshIndices[s + 1]; i++)
                            {
                                customNormals[i] = new Vector3(0, 1, 0);
                            }
                        }

                        // Spherical Shading Low
                        else if (option == 4)
                        {
                            for (int i = subMeshIndices[s]; i < subMeshIndices[s + 1]; i++)
                            {
                                var spherical = Vector3.Normalize(vertices[i]);

                                customNormals[i] = Vector3.Lerp(normals[i], spherical, 0.5f);
                            }
                        }

                        // Spherical Shading Medium
                        else if (option == 5)
                        {
                            for (int i = subMeshIndices[s]; i < subMeshIndices[s + 1]; i++)
                            {
                                var spherical = Vector3.Normalize(vertices[i]);

                                customNormals[i] = Vector3.Lerp(normals[i], spherical, 0.75f);
                            }
                        }

                        // Spherical Shading Full
                        else if (option == 6)
                        {
                            for (int i = subMeshIndices[s]; i < subMeshIndices[s + 1]; i++)
                            {
                                customNormals[i] = Vector3.Normalize(vertices[i]);
                            }
                        }
                    }
                }

                mesh.normals = customNormals;
                mesh.RecalculateTangents();
            }
        }

        //Encode Vector3 to 8bit per channel Float based on: https://developer.download.nvidia.com/cg/pack.html
        List<float> EncodeVector3ToFloat(int vertexCount, List<float> source1, List<float> source2, List<float> source3)
        {
            var encoded = new List<float>();

            for (int i = 0; i < vertexCount; i++)
            {
                var x = Mathf.RoundToInt(255.0f * Mathf.Clamp01(source1[i]));
                var y = Mathf.RoundToInt(255.0f * Mathf.Clamp01(source2[i]));
                var z = Mathf.RoundToInt(255.0f * Mathf.Clamp01(source3[i]));
                //int w = 0;

                int result = /*(w << 24) |*/ (z << 16) | (y << 8) | x;

                encoded.Add(result);
            }

            return encoded;
        }

        List<float> EncodeVector3ToFloat(int vertexCount, List<Vector3> source)
        {
            var encoded = new List<float>();

            for (int i = 0; i < vertexCount; i++)
            {
                var x = Mathf.RoundToInt(255.0f * Mathf.Clamp01(source[i].x));
                var y = Mathf.RoundToInt(255.0f * Mathf.Clamp01(source[i].y));
                var z = Mathf.RoundToInt(255.0f * Mathf.Clamp01(source[i].z));
                //int w = 0;

                int result = /*(w << 24) |*/ (z << 16) | (y << 8) | x;

                encoded.Add(result);
            }

            return encoded;
        }

        float GetMeshArea(Mesh mesh)
        {
            float result = 0;

            for (int p = mesh.vertices.Length - 1, q = 0; q < mesh.vertices.Length; p = q++)
            {
                result += Vector3.Cross(mesh.vertices[q], mesh.vertices[p]).magnitude;
            }
            return result * 0.5f;
        }

        /// <summary>
        /// Mesh Actions
        /// </summary>

        List<float> MeshAction(List<float> source, Mesh mesh, int action)
        {
            if (action == 1)
            {
                source = MeshActionInvert(source);
            }
            else if (action == 2)
            {
                source = MeshActionNegate(source);
            }
            else if (action == 3)
            {
                source = MeshActionRemap01(source);
            }
            else if (action == 4)
            {
                source = MeshActionMultiplyByHeight(source, mesh);
            }

            return source;
        }

        List<float> MeshActionInvert(List<float> source)
        {
            for (int i = 0; i < source.Count; i++)
            {
                source[i] = 1.0f - source[i];
            }

            return source;
        }

        List<float> MeshActionNegate(List<float> source)
        {
            for (int i = 0; i < source.Count; i++)
            {
                source[i] = source[i] * -1.0f;
            }

            return source;
        }

        List<float> MeshActionRemap01(List<float> source)
        {
            float min = source[0];
            float max = source[0];

            for (int i = 0; i < source.Count; i++)
            {
                if (source[i] < min)
                    min = source[i];

                if (source[i] > max)
                    max = source[i];
            }

            // Avoid divide by 0
            if (min != max)
            {
                for (int i = 0; i < source.Count; i++)
                {
                    source[i] = (source[i] - min) / (max - min);
                }
            }
            else
            {
                for (int i = 0; i < source.Count; i++)
                {
                    source[i] = 0.0f;
                }
            }

            return source;
        }

        List<float> MeshActionMultiplyByHeight(List<float> source, Mesh mesh)
        {
            var vertices = mesh.vertices;

            for (int i = 0; i < source.Count; i++)
            {
                var mask = Mathf.Clamp01(vertices[i].y / maxBoundsInfo.y);

                source[i] = source[i] * mask;
            }

            return source;
        }

        float MathRemap(float value, float low1, float high1, float low2, float high2)
        {
            return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
        }

        /// <summary>
        /// Convert Macros
        /// </summary>

        void GetDefaultShadersFromPreset()
        {
            for (int i = 0; i < presetLines.Count; i++)
            {
                if (presetLines[i].StartsWith("Shader"))
                {
                    string[] splitLine = presetLines[i].Split(char.Parse(" "));

                    var type = "";

                    if (splitLine.Length > 1)
                    {
                        type = splitLine[1];
                    }

                    if (type == "SHADER_DEFAULT_CROSS")
                    {
                        var shader = presetLines[i].Replace("Shader SHADER_DEFAULT_CROSS ", "");

                        if (Shader.Find(shader) != null)
                        {
                            shaderCross = Shader.Find(shader);
                        }
                    }
                    else if (type == "SHADER_DEFAULT_LEAF")
                    {
                        var shader = presetLines[i].Replace("Shader SHADER_DEFAULT_LEAF ", "");

                        if (Shader.Find(shader) != null)
                        {
                            shaderLeaf = Shader.Find(shader);
                        }
                    }
                    else if (type == "SHADER_DEFAULT_BARK")
                    {
                        var shader = presetLines[i].Replace("Shader SHADER_DEFAULT_BARK ", "");

                        if (Shader.Find(shader) != null)
                        {

                            shaderBark = Shader.Find(shader);
                        }
                    }
                    else if (type == "SHADER_DEFAULT_GRASS")
                    {
                        var shader = presetLines[i].Replace("Shader SHADER_DEFAULT_GRASS ", "");

                        if (Shader.Find(shader) != null)
                        {
                            shaderGrass = Shader.Find(shader);
                        }
                    }
                    else if (type == "SHADER_DEFAULT_PROP")
                    {
                        var shader = presetLines[i].Replace("Shader SHADER_DEFAULT_PROP ", "");

                        if (Shader.Find(shader) != null)
                        {
                            shaderProp = Shader.Find(shader);
                        }
                    }
                }
            }
        }

        void GetMaterialConversionFromPreset(Material materialOriginal, Material materialInstance)
        {
            var material = materialOriginal;
            var texName = "_MainMaskTex";
            var importType = TextureImporterType.Default;
            var importSRGB = true;

            var doPacking = false;

            int packChannel = 0;
            int maskIndex = 0;
            int action0Index = 0;
            int action1Index = 0;
            int action2Index = 0;

            InitConditionFromLine();
            InitTextureStorage();

            for (int i = 0; i < presetLines.Count; i++)
            {
                useLine = GetConditionFromLine(presetLines[i], material);

                if (useLine)
                {
                    if (presetLines[i].StartsWith("Utility"))
                    {
                        string[] splitLine = presetLines[i].Split(char.Parse(" "));

                        var type = "";
                        var file = "";

                        if (splitLine.Length > 1)
                        {
                            type = splitLine[1];
                        }

                        if (splitLine.Length > 2)
                        {
                            file = splitLine[2];
                        }

                        // Create a copy of the material instance at this point
                        if (type == "SET_CURRENT_MATERIAL_AS_BASE")
                        {
                            material = new Material(materialInstance);
                        }

                        // Reset material to original
                        if (type == "SET_ORIGINAL_MATERIAL_AS_BASE")
                        {
                            material = materialOriginal;
                        }

                        if (type == "START_TEXTURE_PACKING")
                        {
                            doPacking = true;
                        }

                        if (type == "DELETE_FILES_BY_NAME")
                        {
                            string dataPath;

                            if (collectConvertedData)
                            {
                                if (shareCommonMaterials)
                                {
                                    dataPath = projectDataFolder + SHARED_DATA_PATH;
                                }
                                else
                                {
                                    dataPath = projectDataFolder + PREFABS_DATA_PATH + "/" + prefabName;
                                }
                            }
                            else
                            {
                                dataPath = prefabDataFolder;
                            }

                            if (Directory.Exists(dataPath) && file != "")
                            {
                                var allFolderFiles = Directory.GetFiles(dataPath);

                                for (int f = 0; f < allFolderFiles.Length; f++)
                                {
                                    if (allFolderFiles[f].Contains(file))
                                    {
                                        FileUtil.DeleteFileOrDirectory(allFolderFiles[f]);
                                    }
                                }

                                AssetDatabase.Refresh();
                            }
                        }
                    }

                    if (presetLines[i].StartsWith("Material"))
                    {
                        string[] splitLine = presetLines[i].Split(char.Parse(" "));

                        var type = "";
                        var src = "";
                        var dst = "";
                        var val = "";
                        var set = "";

                        var x = "0";
                        var y = "0";
                        var z = "0";
                        var w = "0";

                        if (splitLine.Length > 1)
                        {
                            type = splitLine[1];
                        }

                        if (splitLine.Length > 2)
                        {
                            src = splitLine[2];
                            set = splitLine[2];
                        }

                        if (splitLine.Length > 3)
                        {
                            dst = splitLine[3];
                            x = splitLine[3];
                        }

                        if (splitLine.Length > 4)
                        {
                            val = splitLine[4];
                            y = splitLine[4];
                        }

                        if (splitLine.Length > 5)
                        {
                            z = splitLine[5];
                        }

                        if (splitLine.Length > 6)
                        {
                            w = splitLine[6];
                        }

                        if (type == "SET_SHADER")
                        {
                            materialInstance.shader = GetShaderFromPreset(set);
                        }
                        else if (type == "SET_SHADER_BY_NAME")
                        {
                            var shader = presetLines[i].Replace("Material SET_SHADER_BY_NAME ", "");

                            if (Shader.Find(shader) != null)
                            {
                                materialInstance.shader = Shader.Find(shader);
                            }
                        }
                        else if (type == "SET_FLOAT")
                        {
                            materialInstance.SetFloat(set, float.Parse(x, CultureInfo.InvariantCulture));
                        }
                        else if (type == "SET_COLOR")
                        {
                            materialInstance.SetColor(set, new Color(float.Parse(x, CultureInfo.InvariantCulture), float.Parse(y, CultureInfo.InvariantCulture), float.Parse(z, CultureInfo.InvariantCulture), float.Parse(w, CultureInfo.InvariantCulture)));
                        }
                        else if (type == "SET_VECTOR")
                        {
                            materialInstance.SetVector(set, new Vector4(float.Parse(x, CultureInfo.InvariantCulture), float.Parse(y, CultureInfo.InvariantCulture), float.Parse(z, CultureInfo.InvariantCulture), float.Parse(w, CultureInfo.InvariantCulture)));
                        }
                        else if (type == "COPY_FLOAT")
                        {
                            if (material.HasProperty(src))
                            {
                                materialInstance.SetFloat(dst, material.GetFloat(src));
                            }
                        }
                        else if (type == "COPY_TEX")
                        {
                            if (material.HasProperty(src))
                            {
                                var srcTex = material.GetTexture(src);

                                if (collectConvertedData)
                                {
                                    if (collectOriginalTextures)
                                    {
                                        var srcPath = AssetDatabase.GetAssetPath(srcTex);
                                        var dataPath = projectDataFolder + ORIGINAL_DATA_PATH + "/" + Path.GetFileName(srcPath);

                                        if (File.Exists(dataPath))
                                        {
                                            srcTex = AssetDatabase.LoadAssetAtPath<Texture>(dataPath);
                                            materialInstance.SetTexture(dst, srcTex);
                                        }
                                        else
                                        {
                                            AssetDatabase.CopyAsset(srcPath, dataPath);
                                            AssetDatabase.Refresh();

                                            srcTex = AssetDatabase.LoadAssetAtPath<Texture>(dataPath);
                                            materialInstance.SetTexture(dst, srcTex);
                                        }
                                    }
                                    else
                                    {
                                        materialInstance.SetTexture(dst, srcTex);
                                    }
                                }
                                else
                                {
                                    materialInstance.SetTexture(dst, srcTex);
                                }
                            }
                        }
                        else if (type == "COPY_COLOR")
                        {
                            if (material.HasProperty(src))
                            {
                                materialInstance.SetColor(dst, material.GetColor(src).linear);
                            }
                        }
                        else if (type == "COPY_VECTOR")
                        {
                            if (material.HasProperty(src))
                            {
                                materialInstance.SetVector(dst, material.GetVector(src));
                            }
                        }
                        else if (type == "COPY_ST_AS_VECTOR")
                        {
                            if (material.HasProperty(src))
                            {
                                Vector4 uvs = new Vector4(material.GetTextureScale(src).x, material.GetTextureScale(src).y,
                                                          material.GetTextureOffset(src).x, material.GetTextureOffset(src).y);

                                materialInstance.SetVector(dst, uvs);
                            }
                        }
                        else if (type == "COPY_FLOAT_AS_VECTOR_X")
                        {
                            if (material.HasProperty(src) && materialInstance.HasProperty(dst))
                            {
                                var vec = materialInstance.GetVector(dst);
                                vec.x = material.GetFloat(src);
                                materialInstance.SetVector(dst, vec);
                            }
                        }
                        else if (type == "COPY_FLOAT_AS_VECTOR_Y")
                        {
                            if (material.HasProperty(src) && materialInstance.HasProperty(dst))
                            {
                                var vec = materialInstance.GetVector(dst);
                                vec.y = material.GetFloat(src);
                                materialInstance.SetVector(dst, vec);
                            }
                        }
                        else if (type == "COPY_FLOAT_AS_VECTOR_Z")
                        {
                            if (material.HasProperty(src) && materialInstance.HasProperty(dst))
                            {
                                var vec = materialInstance.GetVector(dst);
                                vec.z = material.GetFloat(src);
                                materialInstance.SetVector(dst, vec);
                            }
                        }
                        else if (type == "COPY_FLOAT_AS_VECTOR_W")
                        {
                            if (material.HasProperty(src) && materialInstance.HasProperty(dst))
                            {
                                var vec = materialInstance.GetVector(dst);
                                vec.w = material.GetFloat(src);
                                materialInstance.SetVector(dst, vec);
                            }
                        }
                        else if (type == "COPY_VECTOR_X_AS_FLOAT")
                        {
                            if (material.HasProperty(src))
                            {
                                materialInstance.SetFloat(dst, material.GetVector(src).x);
                            }
                        }
                        else if (type == "COPY_VECTOR_Y_AS_FLOAT")
                        {
                            if (material.HasProperty(src))
                            {
                                materialInstance.SetFloat(dst, material.GetVector(src).y);
                            }
                        }
                        else if (type == "COPY_VECTOR_Z_AS_FLOAT")
                        {
                            if (material.HasProperty(src))
                            {
                                materialInstance.SetFloat(dst, material.GetVector(src).z);
                            }
                        }
                        else if (type == "COPY_VECTOR_W_AS_FLOAT")
                        {
                            if (material.HasProperty(src))
                            {
                                materialInstance.SetFloat(dst, material.GetVector(src).w);
                            }
                        }
                        else if (type == "ENABLE_KEYWORD")
                        {
                            materialInstance.EnableKeyword(set);
                        }
                        else if (type == "DISABLE_KEYWORD")
                        {
                            materialInstance.DisableKeyword(set);
                        }
                        else if (type == "ENABLE_INSTANCING")
                        {
                            materialInstance.enableInstancing = true;
                        }
                        else if (type == "DISABLE_INSTANCING")
                        {
                            materialInstance.enableInstancing = false;
                        }
                    }

                    if (presetLines[i].StartsWith("Texture"))
                    {
                        string[] splitLine = presetLines[i].Split(char.Parse(" "));
                        string type = "";
                        string value = "";
                        string pack = "";
                        string tex = "";

                        if (splitLine.Length > 2)
                        {
                            type = splitLine[1];
                            value = splitLine[2];

                            if (type == "PropName")
                            {
                                if (value != "")
                                {
                                    texName = value;
                                }
                            }

                            if (type == "ImportType")
                            {
                                if (value == "DEFAULT")
                                {
                                    importType = TextureImporterType.Default;
                                }
                                else if (value == "NORMALMAP")
                                {
                                    importType = TextureImporterType.NormalMap;
                                }
                            }

                            if (type == "ImportSpace")
                            {
                                if (value == "SRGB")
                                {
                                    importSRGB = true;
                                }
                                else if (value == "LINEAR")
                                {
                                    importSRGB = false;
                                }
                            }
                        }

                        if (splitLine.Length > 3)
                        {
                            tex = splitLine[3];
                        }

                        if (material.HasProperty(tex) && material.GetTexture(tex) != null)
                        {
                            if (splitLine.Length > 1)
                            {
                                type = splitLine[1];

                                if (type == "SetRed")
                                {
                                    maskIndex = 0;
                                }

                                if (type == "SetGreen")
                                {
                                    maskIndex = 1;
                                }

                                if (type == "SetBlue")
                                {
                                    maskIndex = 2;
                                }

                                if (type == "SetAlpha")
                                {
                                    maskIndex = 3;
                                }
                            }

                            if (splitLine.Length > 2)
                            {
                                pack = splitLine[2];

                                if (pack == "NONE")
                                {
                                    packChannel = 0;
                                }

                                if (pack == "GET_RED")
                                {
                                    packChannel = 1;
                                }

                                if (pack == "GET_GREEN")
                                {
                                    packChannel = 2;
                                }

                                if (pack == "GET_BLUE")
                                {
                                    packChannel = 3;
                                }

                                if (pack == "GET_ALPHA")
                                {
                                    packChannel = 4;
                                }

                                if (pack == "GET_GRAY")
                                {
                                    packChannel = 555;
                                }

                                if (pack == "GET_GREY")
                                {
                                    packChannel = 555;
                                }
                            }

                            if (presetLines[i].Contains("ACTION_INVERT"))
                            {
                                action0Index = 1;
                            }

                            if (presetLines[i].Contains("ACTION_MULTIPLY_0"))
                            {
                                action1Index = 1;
                            }

                            if (presetLines[i].Contains("ACTION_MULTIPLY_2"))
                            {
                                action1Index = 2;
                            }

                            if (presetLines[i].Contains("ACTION_MULTIPLY_3"))
                            {
                                action1Index = 3;
                            }

                            if (presetLines[i].Contains("ACTION_MULTIPLY_05"))
                            {
                                action1Index = 5;
                            }

                            if (presetLines[i].Contains("ACTION_POWER_0"))
                            {
                                action2Index = 1;
                            }

                            if (presetLines[i].Contains("ACTION_POWER_2"))
                            {
                                action2Index = 2;
                            }

                            if (presetLines[i].Contains("ACTION_POWER_3"))
                            {
                                action2Index = 3;
                            }

                            if (presetLines[i].Contains("ACTION_POWER_4"))
                            {
                                action2Index = 4;
                            }

                            maskChannels[maskIndex] = packChannel;
                            maskActions0[maskIndex] = action0Index;
                            maskActions1[maskIndex] = action1Index;
                            maskActions2[maskIndex] = action2Index;
                            maskTextures[maskIndex] = material.GetTexture(tex);
                        }
                    }
                }

                if (doPacking)
                {
                    var id = packedTextureNames.Count;

                    if (maskTextures[0] != null || maskTextures[1] != null || maskTextures[2] != null || maskTextures[3] != null)
                    {
                        var internalName = GetPackedTextureName(maskTextures[0], maskChannels[0], maskTextures[1], maskChannels[1], maskTextures[2], maskChannels[2], maskTextures[3], maskChannels[3]);
                        bool exist = false;

                        for (int n = 0; n < packedTextureNames.Count; n++)
                        {
                            if (packedTextureNames[n] == internalName)
                            {
                                materialInstance.SetTexture(texName, packedTextureObjcts[n]);
                                exist = true;
                            }
                        }

                        if (exist == false)
                        {
                            PackTexture(materialInstance, id, internalName, texName, importType, importSRGB);
                        }
                    }

                    InitTextureStorage();

                    doPacking = false;
                }
            }
        }

        Shader GetShaderFromPreset(string check)
        {
            var shader = shaderLeaf;

            if (check == "SHADER_DEFAULT_CROSS")
            {
                shader = shaderCross;
            }
            else if (check == "SHADER_DEFAULT_LEAF")
            {
                shader = shaderLeaf;
            }
            else if (check == "SHADER_DEFAULT_BARK")
            {
                shader = shaderBark;
            }
            else if (check == "SHADER_DEFAULT_GRASS")
            {
                shader = shaderGrass;
            }
            else if (check == "SHADER_DEFAULT_PROP")
            {
                shader = shaderProp;
            }

            return shader;
        }

        void ConvertMaterials()
        {
            packedTextureNames = new List<string>();
            packedTextureObjcts = new List<Texture>();

            for (int i = 0; i < materialArraysInPrefab.Count; i++)
            {
                if (materialArraysInPrefab[i] != null)
                {
                    for (int j = 0; j < materialArraysInPrefab[i].Length; j++)
                    {
                        var material = materialArraysInPrefab[i][j];
                        var materialInstance = materialArraysInstances[i][j];

                        if (IsValidMaterial(material))
                        {
                            string dataPath;
                            string savePath = "/" + materialInstance.name + ".mat";

                            if (collectConvertedData)
                            {
                                if (shareCommonMaterials)
                                {
                                    dataPath = projectDataFolder + SHARED_DATA_PATH + savePath;

                                    if (File.Exists(dataPath))
                                    {
                                        convertedMaterial = AssetDatabase.LoadAssetAtPath<Material>(dataPath);
                                    }
                                    else
                                    {
                                        ConvertMaterial(material, materialInstance);
                                    }
                                }
                                else
                                {
                                    if (keepConvertedMaterials)
                                    {
                                        dataPath = projectDataFolder + PREFABS_DATA_PATH + "/" + prefabName + savePath;

                                        if (File.Exists(dataPath))
                                        {
                                            convertedMaterial = AssetDatabase.LoadAssetAtPath<Material>(dataPath);
                                        }
                                        else
                                        {
                                            ConvertMaterial(material, materialInstance);
                                        }
                                    }
                                    else
                                    {
                                        ConvertMaterial(material, materialInstance);
                                    }
                                }
                            }
                            else
                            {
                                if (keepConvertedMaterials)
                                {
                                    dataPath = prefabDataFolder + savePath;

                                    if (File.Exists(dataPath))
                                    {
                                        convertedMaterial = AssetDatabase.LoadAssetAtPath<Material>(dataPath);
                                    }
                                    else
                                    {
                                        ConvertMaterial(material, materialInstance);
                                    }
                                }
                                else
                                {
                                    ConvertMaterial(material, materialInstance);
                                }
                            }

                            materialArraysInstances[i][j] = convertedMaterial;
                        }
                        else
                        {
                            materialArraysInstances[i][j] = material;
                        }
                    }
                }
            }
        }

        void ConvertMaterial(Material material, Material materialInstance)
        {
            materialInstance.enableInstancing = true;

            SetMaterialInitSettings(materialInstance);
            GetMaterialConversionFromPreset(material, materialInstance);
            SetMaterialPostSettings(materialInstance);

            SaveMaterial(materialInstance);
        }

        void SaveMaterial(Material materialInstance)
        {
            string dataPath;
            string savePath = "/" + materialInstance.name + ".mat";

            if (collectConvertedData)
            {
                if (shareCommonMaterials)
                {
                    dataPath = projectDataFolder + SHARED_DATA_PATH + savePath;
                }
                else
                {
                    dataPath = projectDataFolder + PREFABS_DATA_PATH + "/" + prefabName + savePath;
                }
            }
            else
            {
                dataPath = prefabDataFolder + savePath;
            }

            if (File.Exists(dataPath))
            {
                var materialFile = AssetDatabase.LoadAssetAtPath<Material>(dataPath);
                EditorUtility.CopySerialized(materialInstance, materialFile);
            }
            else
            {
                AssetDatabase.CreateAsset(materialInstance, dataPath);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            AssetDatabase.SetLabels(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(dataPath), new string[] { outputSuffix + " Material" });

            convertedMaterial = AssetDatabase.LoadAssetAtPath<Material>(dataPath);

            TVEMaterial.SetMaterialSettings(convertedMaterial);
        }

        void AssignConvertedMaterials()
        {
            for (int i = 0; i < meshRenderersInPrefab.Count; i++)
            {
                if (meshRenderersInPrefab[i] != null)
                {
                    meshRenderersInPrefab[i].sharedMaterials = materialArraysInstances[i];
                }
            }
        }

        void SetMaterialInitSettings(Material material)
        {
            // Set bounds info
            material.SetVector("_MaxBoundsInfo", maxBoundsInfo);

            // Set some initial motion settings
            material.SetFloat("_MotionAmplitude_10", 0.05f);
            material.SetFloat("_MotionSpeed_10", 2);
            material.SetFloat("_MotionScale_10", 0);
            material.SetFloat("_MotionVariation_10", 0);

            material.SetFloat("_MotionAmplitude_20", 0.1f);
            material.SetFloat("_MotionSpeed_20", 6);
            material.SetFloat("_MotionScale_20", 0.2f);
            material.SetFloat("_MotionVariation_20", 20f);

            material.SetFloat("_MotionAmplitude_32", 0.2f);
            material.SetFloat("_MotionSpeed_32", 20);
            material.SetFloat("_MotionScale_32", 20);
            material.SetFloat("_MotionVariation_32", 20);

            material.SetFloat("_InteractionAmplitude", 1f);
        }

        void SetMaterialPostSettings(Material material)
        {
            if (sourceDetailCoord > 0 && optionDetailCoord == 3)
            {
                material.SetInt("_VertexPivotMode", 1);
            }

            if (!material.shader.name.Contains("Objects"))
            {
                if (sourceVariation > 0 && (optionVariation == 2 || optionVariation == 3))
                {
                    material.SetInt("_VertexVariationMode", 1);
                }
            }

            // Leaves and branches are the same, disable rolling for bark
            if (sourceMotion2 == sourceMotion3 && optionMotion2 == optionMotion3 && actionMotion2 == actionMotion3)
            {
                if (material.shader.name.Contains("Bark"))
                {
                    material.SetInt("_MotionValue_20", 0);

                    // Legacy property
                    material.SetInt("_VertexRollingMode_20", 0);
                }
            }
        }

        bool IsValidMaterial(Material material)
        {
            bool valid = true;
            int i = 0;

            if (material == null)
            {
                i++;
            }

            if (material != null && material.HasProperty("_IsTVEShader") == true)
            {
                i++;
            }

            if (i > 0)
            {
                valid = false;
            }

            return valid;
        }

        /// <summary>
        /// Packed Texture Utils
        /// </summary>

        void InitTexturePacker()
        {
            blitTexture = Texture2D.whiteTexture;
            blitMaterial = new Material(Shader.Find("Hidden/BOXOPHOBIC/The Vegetation Engine/Helpers/Packer"));

            sourceTexCompressions = new TextureImporterCompression[4];
            sourceimportSizes = new int[4];
            sourceTexImporters = new TextureImporter[4];
            sourceTexSettings = new TextureImporterSettings[4];

            for (int i = 0; i < 4; i++)
            {
                sourceTexCompressions[i] = new TextureImporterCompression();
                sourceTexImporters[i] = new TextureImporter();
                sourceTexSettings[i] = new TextureImporterSettings();
            }
        }

        void InitTextureStorage()
        {
            maskChannels = new int[4];
            maskActions0 = new int[4];
            maskActions1 = new int[4];
            maskActions2 = new int[4];
            maskTextures = new Texture[4];

            for (int i = 0; i < 4; i++)
            {
                maskChannels[i] = 0;
                maskActions0[i] = 0;
                maskActions1[i] = 0;
                maskActions2[i] = 0;
                maskTextures[i] = null;
            }
        }

        void PackTexture(Material materialInstance, int id, string internalName, string texName, TextureImporterType importType, bool importSRGB)
        {
            string saveName = texName.Replace("_", "");

            string dataPath;
            string savePath = "/" + materialInstance.name + " - " + saveName /*+ " " + id.ToString()*/ + " (" + outputSuffix + " Texture).png";

            if (collectConvertedData)
            {
                if (shareCommonMaterials)
                {
                    dataPath = projectDataFolder + SHARED_DATA_PATH + savePath;
                }
                else
                {
                    dataPath = projectDataFolder + PREFABS_DATA_PATH + "/" + prefabName + savePath;
                }
            }
            else
            {
                dataPath = prefabDataFolder + savePath;
            }

            int initSize = GetPackedInitSize(maskTextures[0], maskTextures[1], maskTextures[2], maskTextures[3]);

            ResetBlitMaterial();

            //Set Packer Metallic channel
            if (maskTextures[0] != null)
            {
                PrepareSourceTexture(maskTextures[0], 0);
                ResetSourceTexture(0);

                blitMaterial.SetTexture("_Packer_TexR", maskTextures[0]);
                blitMaterial.SetInt("_Packer_ChannelR", maskChannels[0]);
                blitMaterial.SetInt("_Packer_Action0R", maskActions0[0]);
                blitMaterial.SetInt("_Packer_Action1R", maskActions1[0]);
                blitMaterial.SetInt("_Packer_Action2R", maskActions2[0]);
            }
            else
            {
                blitMaterial.SetInt("_Packer_ChannelR", NONE);
                blitMaterial.SetFloat("_Packer_FloatR", 1.0f);
            }

            //Set Packer Occlusion channel
            if (maskTextures[1] != null)
            {
                PrepareSourceTexture(maskTextures[1], 1);
                ResetSourceTexture(1);

                blitMaterial.SetTexture("_Packer_TexG", maskTextures[1]);
                blitMaterial.SetInt("_Packer_ChannelG", maskChannels[1]);
                blitMaterial.SetInt("_Packer_Action0G", maskActions0[1]);
                blitMaterial.SetInt("_Packer_Action1G", maskActions1[1]);
                blitMaterial.SetInt("_Packer_Action2G", maskActions2[1]);
            }
            else
            {
                blitMaterial.SetInt("_Packer_ChannelG", NONE);
                blitMaterial.SetFloat("_Packer_FloatG", 1.0f);
            }

            //Set Packer Mask channel
            if (maskTextures[2] != null)
            {
                PrepareSourceTexture(maskTextures[2], 2);
                ResetSourceTexture(2);

                blitMaterial.SetTexture("_Packer_TexB", maskTextures[2]);
                blitMaterial.SetInt("_Packer_ChannelB", maskChannels[2]);
                blitMaterial.SetInt("_Packer_Action0B", maskActions0[2]);
                blitMaterial.SetInt("_Packer_Action1B", maskActions1[2]);
                blitMaterial.SetInt("_Packer_Action2B", maskActions2[2]);
            }
            else
            {
                blitMaterial.SetInt("_Packer_ChannelB", NONE);
                blitMaterial.SetFloat("_Packer_FloatB", 1.0f);
            }

            //Set Packer Smothness channel
            if (maskTextures[3] != null)
            {
                PrepareSourceTexture(maskTextures[3], 3);
                ResetSourceTexture(3);

                blitMaterial.SetTexture("_Packer_TexA", maskTextures[3]);
                blitMaterial.SetInt("_Packer_ChannelA", maskChannels[3]);
                blitMaterial.SetInt("_Packer_Action0A", maskActions0[3]);
                blitMaterial.SetInt("_Packer_Action1A", maskActions1[3]);
                blitMaterial.SetInt("_Packer_Action2A", maskActions2[3]);
            }
            else
            {
                blitMaterial.SetInt("_Packer_ChannelA", NONE);
                blitMaterial.SetFloat("_Packer_FloatA", 1.0f);
            }

            Vector2 pixelSize = GetPackedPixelSize(maskTextures[0], maskTextures[1], maskTextures[2], maskTextures[3]);
            int importSize = GetPackedImportSize(initSize, pixelSize);

            Texture savedPacked = SavePackedTexture(dataPath, pixelSize);

            packedTextureNames.Add(internalName);
            packedTextureObjcts.Add(savedPacked);

            SetTextureImporterSettings(savedPacked, importSize, importType, importSRGB);

            materialInstance.SetTexture(texName, savedPacked);
        }

        string GetPackedTextureName(Texture tex1, int ch1, Texture tex2, int ch2, Texture tex3, int ch3, Texture tex4, int ch4)
        {
            var texName1 = "NULL";
            var texName2 = "NULL";
            var texName3 = "NULL";
            var texName4 = "NULL";

            if (tex1 != null)
            {
                texName1 = tex1.name;
            }

            if (tex2 != null)
            {
                texName2 = tex2.name;
            }

            if (tex3 != null)
            {
                texName3 = tex3.name;
            }

            if (tex4 != null)
            {
                texName4 = tex4.name;
            }

            var name = texName1 + ch1 + texName2 + ch2 + texName3 + ch3 + texName4 + ch4;

            return name;
        }

        Vector2 GetPackedPixelSize(Texture tex1, Texture tex2, Texture tex3, Texture tex4)
        {
            int x = 32;
            int y = 32;

            if (tex1 != null)
            {
                x = Mathf.Max(x, tex1.width);
                y = Mathf.Max(y, tex1.height);
            }

            if (tex2 != null)
            {
                x = Mathf.Max(x, tex2.width);
                y = Mathf.Max(y, tex2.height);
            }

            if (tex3 != null)
            {
                x = Mathf.Max(x, tex3.width);
                y = Mathf.Max(y, tex3.height);
            }

            if (tex4 != null)
            {
                x = Mathf.Max(x, tex4.width);
                y = Mathf.Max(y, tex4.height);
            }

            return new Vector2(x, y);
        }

        int GetPackedInitSize(Texture tex1, Texture tex2, Texture tex3, Texture tex4)
        {
            int initSize = 32;

            if (tex1 != null)
            {
                string texPath = AssetDatabase.GetAssetPath(tex1);
                TextureImporter texImporter = AssetImporter.GetAtPath(texPath) as TextureImporter;

                initSize = Mathf.Max(initSize, texImporter.maxTextureSize);
            }

            if (tex2 != null)
            {
                string texPath = AssetDatabase.GetAssetPath(tex2);
                TextureImporter texImporter = AssetImporter.GetAtPath(texPath) as TextureImporter;

                initSize = Mathf.Max(initSize, texImporter.maxTextureSize);
            }

            if (tex3 != null)
            {
                string texPath = AssetDatabase.GetAssetPath(tex3);
                TextureImporter texImporter = AssetImporter.GetAtPath(texPath) as TextureImporter;

                initSize = Mathf.Max(initSize, texImporter.maxTextureSize);
            }

            if (tex4 != null)
            {
                string texPath = AssetDatabase.GetAssetPath(tex4);
                TextureImporter texImporter = AssetImporter.GetAtPath(texPath) as TextureImporter;

                initSize = Mathf.Max(initSize, texImporter.maxTextureSize);
            }

            return initSize;
        }

        int GetPackedImportSize(int initTexImportSize, Vector2 pixelTexSize)
        {
            int pixelSize = (int)Mathf.Max(pixelTexSize.x, pixelTexSize.y);
            int importSize = initTexImportSize;

            if (pixelSize < importSize)
            {
                importSize = pixelSize;
            }

            for (int i = 1; i < MaxTextureSizes.Length - 1; i++)
            {
                int a;
                int b;

                if ((importSize > MaxTextureSizes[i]) && (importSize < MaxTextureSizes[i + 1]))
                {
                    a = Mathf.Abs(MaxTextureSizes[i] - importSize);
                    b = Mathf.Abs(MaxTextureSizes[i + 1] - importSize);

                    if (a < b)
                    {
                        importSize = MaxTextureSizes[i];
                    }
                    else
                    {
                        importSize = MaxTextureSizes[i + 1];
                    }

                    break;
                }
            }

            return importSize;
        }

        Texture SavePackedTexture(string path, Vector2 size)
        {
            if (File.Exists(path))
            {
                FileUtil.DeleteFileOrDirectory(path);
                FileUtil.DeleteFileOrDirectory(path + ".meta");
            }

            RenderTexture renderTexure = new RenderTexture((int)size.x, (int)size.y, 0, RenderTextureFormat.ARGB32);

            Graphics.Blit(blitTexture, renderTexure, blitMaterial, 0);

            RenderTexture.active = renderTexure;
            Texture2D packedTexure = new Texture2D(renderTexure.width, renderTexure.height, TextureFormat.ARGB32, false);
            packedTexure.ReadPixels(new Rect(0, 0, renderTexure.width, renderTexure.height), 0, 0);
            packedTexure.Apply();
            RenderTexture.active = null;

            renderTexure.Release();

            byte[] bytes;
            bytes = packedTexure.EncodeToPNG();

            File.WriteAllBytes(path, bytes);

            AssetDatabase.ImportAsset(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            AssetDatabase.SetLabels(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path), new string[] { outputSuffix + " Texture" });

            return AssetDatabase.LoadAssetAtPath<Texture>(path);
        }

        void SetTextureImporterSettings(Texture texture, int importSize, TextureImporterType importType, bool importSRGB)
        {
            string texPath = AssetDatabase.GetAssetPath(texture);
            TextureImporter texImporter = AssetImporter.GetAtPath(texPath) as TextureImporter;

            texImporter.textureType = importType;
            texImporter.maxTextureSize = importSize;
            texImporter.sRGBTexture = importSRGB;
            texImporter.alphaSource = TextureImporterAlphaSource.FromInput;


            texImporter.SaveAndReimport();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        void ResetBlitMaterial()
        {
            blitMaterial = new Material(Shader.Find("Hidden/BOXOPHOBIC/The Vegetation Engine/Helpers/Packer"));
        }

        void PrepareSourceTexture(Texture texture, int channel)
        {
            if (outputTextureIndex == OutputTexture.UseCurrentResolution)
            {
                return;
            }

            string texPath = AssetDatabase.GetAssetPath(texture);
            TextureImporter texImporter = AssetImporter.GetAtPath(texPath) as TextureImporter;

            sourceTexCompressions[channel] = texImporter.textureCompression;
            sourceimportSizes[channel] = texImporter.maxTextureSize;

            texImporter.ReadTextureSettings(sourceTexSettings[channel]);

            texImporter.textureType = TextureImporterType.Default;
            texImporter.sRGBTexture = false;
            texImporter.mipmapEnabled = false;
            texImporter.maxTextureSize = 8192;
            texImporter.textureCompression = TextureImporterCompression.Uncompressed;

            sourceTexImporters[channel] = texImporter;

            texImporter.SaveAndReimport();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        void ResetSourceTexture(int index)
        {
            if (outputTextureIndex == OutputTexture.UseCurrentResolution)
            {
                return;
            }

            sourceTexImporters[index].textureCompression = sourceTexCompressions[index];
            sourceTexImporters[index].maxTextureSize = sourceimportSizes[index];
            sourceTexImporters[index].SetTextureSettings(sourceTexSettings[index]);
            sourceTexImporters[index].SaveAndReimport();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Get Project Presets
        /// </summary>

        void GetDefaultShaders()
        {
            shaderProp = Shader.Find("BOXOPHOBIC/The Vegetation Engine/Objects/Prop Standard Lit");
            shaderBark = Shader.Find("BOXOPHOBIC/The Vegetation Engine/Vegetation/Bark Standard Lit");
            shaderCross = Shader.Find("BOXOPHOBIC/The Vegetation Engine/Vegetation/Cross Subsurface Lit");
            shaderGrass = Shader.Find("BOXOPHOBIC/The Vegetation Engine/Vegetation/Grass Subsurface Lit");
            shaderLeaf = Shader.Find("BOXOPHOBIC/The Vegetation Engine/Vegetation/Leaf Subsurface Lit");
        }

        void GetPresets()
        {
            presetPaths = new List<string>();
            presetPaths.Insert(0, "");

            overridePaths = new List<string>();
            overridePaths.Insert(0, "");

            detectLines = new List<string>();

            // FindObjectsOfTypeAll not working properly for unloaded assets
            allPresetPaths = Directory.GetFiles(Application.dataPath, "*.tvepreset", SearchOption.AllDirectories);

            for (int i = 0; i < allPresetPaths.Length; i++)
            {
                string assetPath = "Assets" + allPresetPaths[i].Replace(Application.dataPath, "").Replace('\\', '/');

                if (assetPath.Contains("[PRESET]"))
                {
                    presetPaths.Add(assetPath);
                }

                if (assetPath.Contains("[OVERRIDE]") == true)
                {
                    overridePaths.Add(assetPath);
                }

                if (assetPath.Contains("[DETECT]") == true)
                {
                    StreamReader reader = new StreamReader(assetPath);

                    while (!reader.EndOfStream)
                    {
                        detectLines.Add(reader.ReadLine());
                    }

                    reader.Close();
                }
            }

            PresetOptions = new string[presetPaths.Count];
            PresetOptions[0] = "Choose a preset";

            OverrideOptions = new string[overridePaths.Count];
            OverrideOptions[0] = "None";

            for (int i = 1; i < presetPaths.Count; i++)
            {
                PresetOptions[i] = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(presetPaths[i]).name;
                PresetOptions[i] = PresetOptions[i].Replace("[PRESET] ", "");
                PresetOptions[i] = PresetOptions[i].Replace(" - ", "/");
            }

            for (int i = 1; i < overridePaths.Count; i++)
            {
                OverrideOptions[i] = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(overridePaths[i]).name;
                OverrideOptions[i] = OverrideOptions[i].Replace("[OVERRIDE] ", "");
                OverrideOptions[i] = OverrideOptions[i].Replace(" - ", "/");
            }
        }

        void GetPresetLines()
        {
            presetLines = new List<string>();

            StreamReader reader = new StreamReader(presetPaths[presetIndex]);

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine().TrimStart();

                presetLines.Add(line);

                if (line.Contains("Include"))
                {
                    GetIncludeLines(line);
                }
            }

            reader.Close();

            for (int i = 0; i < overrideIndices.Count; i++)
            {
                if (overrideIndices[i] != 0)
                {
                    reader = new StreamReader(overridePaths[overrideIndices[i]]);

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine().TrimStart();

                        presetLines.Add(line);

                        if (line.Contains("Include"))
                        {
                            GetIncludeLines(line);
                        }
                    }

                    reader.Close();
                }
            }
        }

        void GetIncludeLines(string include)
        {
            var import = include.Replace("Include ", "");

            for (int i = 0; i < allPresetPaths.Length; i++)
            {
                if (allPresetPaths[i].Contains(import))
                {
                    StreamReader reader = new StreamReader(allPresetPaths[i]);

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine().TrimStart();

                        presetLines.Add(line);
                    }

                    reader.Close();
                }
            }
        }

        void GetDescriptionFromPreset()
        {
            infoTitle = "Preset";
            infoPreset = "";
            infoStatus = "";
            infoOnline = "";
            infoWarning = "";
            infoError = "";

            for (int i = 0; i < presetLines.Count; i++)
            {
                if (presetLines[i].StartsWith("InfoTitle"))
                {
                    infoTitle = presetLines[i].Replace("InfoTitle ", "");
                }

                if (presetLines[i].StartsWith("InfoPreset"))
                {
                    infoPreset = presetLines[i].Replace("InfoPreset ", "");
                }

                if (presetLines[i].StartsWith("InfoStatus"))
                {
                    infoStatus = presetLines[i].Replace("InfoStatus ", "");
                }

                if (presetLines[i].StartsWith("InfoOnline"))
                {
                    infoOnline = presetLines[i].Replace("InfoOnline ", "");
                }

                if (presetLines[i].StartsWith("InfoWarning"))
                {
                    infoWarning = presetLines[i].Replace("InfoWarning ", "");
                }

                if (presetLines[i].StartsWith("InfoError"))
                {
                    infoError = presetLines[i].Replace("InfoError ", "");
                }
            }

            if (presetAutoDetected)
            {
                infoTitle = infoTitle + " (Auto Detected)";
            }
        }

        void GetOutputsFromPreset()
        {
            if (presetIndex == 0)
            {
                return;
            }

            outputValid = true;

            for (int i = 0; i < presetLines.Count; i++)
            {
                if (presetLines[i].StartsWith("OutputMesh"))
                {
                    string source = presetLines[i].Replace("OutputMesh ", "");

                    if (source == "NONE")
                    {
                        outputMeshIndex = OutputMesh.Off;
                    }
                    else if (source == "DEFAULT")
                    {
                        outputMeshIndex = OutputMesh.Default;
                    }
                    else if (source == "CUSTOM")
                    {
                        outputMeshIndex = OutputMesh.Custom;
                    }
                    else if (source == "POLYGONAL")
                    {
                        outputMeshIndex = OutputMesh.Polygonal;
                    }
                    else if (source == "DEENVIRONMENT")
                    {
                        outputMeshIndex = OutputMesh.DEEnvironment;
                    }
                }

                if (presetLines[i].StartsWith("OutputReadWrite"))
                {
                    string source = presetLines[i].Replace("OutputReadWrite ", "");

                    if (source == "MARK_MESHES_AS_READABLE")
                    {
                        outputReadWrite = OutputReadWrite.MarkMeshesAsReadable;
                    }
                    else
                    {
                        outputReadWrite = OutputReadWrite.MarkMeshesAsNonReadable;
                    }
                }

                if (presetLines[i].StartsWith("OutputTransform"))
                {
                    string source = presetLines[i].Replace("OutputTransform ", "");

                    if (source == "KEEP_ORIGINAL_TRANSFORMS")
                    {
                        outputTransformIndex = OutputTransform.KeepOriginalTransforms;
                    }
                    else
                    {
                        outputTransformIndex = OutputTransform.TransformToWorldSpace;
                    }
                }

                if (presetLines[i].StartsWith("OutputMaterial"))
                {
                    string source = presetLines[i].Replace("OutputMaterial ", "");

                    if (source == "NONE")
                    {
                        outputMaterialIndex = OutputMaterial.Off;
                    }
                    else
                    {
                        outputMaterialIndex = OutputMaterial.Default;
                    }
                }

                if (presetLines[i].StartsWith("OutputTexture"))
                {
                    string source = presetLines[i].Replace("OutputTexture ", "");

                    if (source == "USE_CURRENT_RESOLUTION")
                    {
                        outputTextureIndex = OutputTexture.UseCurrentResolution;
                    }
                    else
                    {
                        outputTextureIndex = OutputTexture.UseHighestResolution;
                    }
                }

                if (presetLines[i].StartsWith("OutputSuffix"))
                {
                    outputSuffix = presetLines[i].Replace("OutputSuffix ", "");
                }

                if (presetLines[i].StartsWith("OutputValid"))
                {
                    string source = presetLines[i].Replace("OutputValid ", "");

                    if (source == "FALSE")
                    {
                        outputValid = false;
                    }
                }
            }
        }

        void GetAllPresetInfo()
        {
            if (presetIndex != 0)
            {
                outputMeshIndex = OutputMesh.Default;
                outputMaterialIndex = OutputMaterial.Default;
                outputSuffix = "TVE";

                sourceNormals = 0;

                GetDefaultShaders();

                GetPresetLines();
                GetDescriptionFromPreset();

                if (!hasOutputModifications)
                {
                    GetOutputsFromPreset();
                }

                if (!hasMeshModifications)
                {
                    GetMeshConversionFromPreset();
                }
                GetDefaultShadersFromPreset();
            }
        }

        void SaveGlobalOverrides()
        {
            var globalOverrides = "";

            for (int i = 0; i < overrideIndices.Count; i++)
            {
                if (overrideGlobals[i])
                {
                    globalOverrides = globalOverrides + OverrideOptions[overrideIndices[i]] + ";";
                }
            }

            globalOverrides.Replace("None", "");

            SettingsUtils.SaveSettingsData(userFolder + "Converter Overrides.asset", globalOverrides);
        }

        void GetGlobalOverrides()
        {
            var globalOverrides = SettingsUtils.LoadSettingsData(userFolder + "Converter Overrides.asset", "");

            if (globalOverrides != "")
            {
                var splitLine = globalOverrides.Split(char.Parse(";"));

                for (int o = 0; o < OverrideOptions.Length; o++)
                {
                    for (int s = 0; s < splitLine.Length; s++)
                    {
                        if (OverrideOptions[o] == splitLine[s])
                        {
                            if (!overrideIndices.Contains(o))
                            {
                                overrideIndices.Add(o);
                                overrideGlobals.Add(true);
                            }
                        }
                    }
                }
            }
        }

        void InitConditionFromLine()
        {
            useLines = new List<bool>();
            useLines.Add(true);
        }

        bool GetConditionFromLine(string line, Material material)
        {
            var valid = true;

            if (line.StartsWith("if"))
            {
                valid = false;

                string[] splitLine = line.Split(char.Parse(" "));

                var type = "";
                var check = "";
                var val = splitLine[splitLine.Length - 1];

                if (splitLine.Length > 1)
                {
                    type = splitLine[1];
                }

                if (splitLine.Length > 2)
                {
                    for (int i = 2; i < splitLine.Length; i++)
                    {
                        if (!float.TryParse(splitLine[i], out _))
                        {
                            check = check + splitLine[i] + " ";
                        }
                    }

                    check = check.TrimEnd();
                }

                if (type.Contains("PREFAB_PATH_CONTAINS"))
                {
                    var path = AssetDatabase.GetAssetPath(prefabObject).ToUpperInvariant();

                    if (path.Contains(check.ToUpperInvariant()))
                    {
                        valid = true;
                    }
                }
                else if (type.Contains("SHADER_NAME_CONTAINS"))
                {
                    var name = material.shader.name.ToUpperInvariant();

                    if (name.Contains(check.ToUpperInvariant()))
                    {
                        valid = true;
                    }
                }
                else if (type.Contains("SHADER_PIPELINE_IS_STANDARD"))
                {
                    if (material.GetTag("RenderPipeline", false) == "")
                    {
                        valid = true;
                    }
                }
                else if (type.Contains("SHADER_PIPELINE_IS_UNIVERSAL"))
                {
                    if (material.GetTag("RenderPipeline", false) == "UniversalPipeline")
                    {
                        valid = true;
                    }
                }
                else if (type.Contains("SHADER_PIPELINE_IS_HD"))
                {
                    if (material.GetTag("RenderPipeline", false) == "HDRenderPipeline")
                    {
                        valid = true;
                    }
                }
                else if (type.Contains("SHADER_PIPELINE_IS_SRP"))
                {
                    if (material.GetTag("RenderPipeline", false) == "HDRenderPipeline" || material.GetTag("RenderPipeline", false) == "UniversalPipeline")
                    {
                        valid = true;
                    }
                }
                else if (type.Contains("MATERIAL_NAME_CONTAINS"))
                {
                    var name = material.name.ToUpperInvariant();

                    if (name.Contains(check.ToUpperInvariant()))
                    {
                        valid = true;
                    }
                }
                else if (type.Contains("MATERIAL_RENDERTYPE_TAG_CONTAINS"))
                {
                    if (material.GetTag("RenderType", false).Contains(check))
                    {
                        valid = true;
                    }
                }
                else if (type.Contains("MATERIAL_HAS_PROP"))
                {
                    if (material.HasProperty(check))
                    {
                        valid = true;
                    }
                }
                else if (type.Contains("MATERIAL_HAS_TEX"))
                {
                    if (material.HasProperty(check))
                    {
                        if (material.GetTexture(check) != null)
                        {
                            valid = true;
                        }
                    }
                }
                else if (type.Contains("MATERIAL_FLOAT_EQUALS"))
                {
                    var min = float.Parse(val, CultureInfo.InvariantCulture) - 0.1f;
                    var max = float.Parse(val, CultureInfo.InvariantCulture) + 0.1f;

                    if (material.HasProperty(check) && material.GetFloat(check) > min && material.GetFloat(check) < max)
                    {
                        valid = true;
                    }
                }
                else if (type.Contains("MATERIAL_FLOAT_SMALLER"))
                {
                    var value = float.Parse(val, CultureInfo.InvariantCulture);

                    if (material.HasProperty(check) && material.GetFloat(check) < value)
                    {
                        valid = true;
                    }
                }
                else if (type.Contains("MATERIAL_FLOAT_HIGHER"))
                {
                    var value = float.Parse(val, CultureInfo.InvariantCulture);

                    if (material.HasProperty(check) && material.GetFloat(check) > value)
                    {
                        valid = true;
                    }
                }
                else if (type.Contains("MATERIAL_KEYWORD_ENABLED"))
                {
                    if (material.IsKeywordEnabled(check))
                    {
                        valid = true;
                    }
                }

                useLines.Add(valid);
            }

            if (line.StartsWith("if") && line.Contains("!"))
            {
                valid = !valid;
                useLines[useLines.Count - 1] = valid;
            }

            if (line.StartsWith("endif") || line.StartsWith("}"))
            {
                useLines.RemoveAt(useLines.Count - 1);
            }

            var useLine = true;

            for (int i = 1; i < useLines.Count; i++)
            {
                if (useLines[i] == false)
                {
                    useLine = false;
                    break;
                }
            }

            return useLine;
        }
    }
}
