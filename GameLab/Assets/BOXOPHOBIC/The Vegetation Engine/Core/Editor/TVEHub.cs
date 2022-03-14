﻿// Cristian Pop - https://boxophobic.com/

using UnityEngine;
using UnityEditor;
using Boxophobic.StyledGUI;
using Boxophobic.Utils;
using System.IO;

namespace TheVegetationEngine
{
    public class TVEHub : EditorWindow
    {
        const int GUI_HEIGHT = 18;

        string assetFolder = "Assets/BOXOPHOBIC/The Vegetation Engine";
        string userFolder = "Assets/BOXOPHOBIC/User";

        string[] pipelinePaths;
        string[] pipelineOptions;

        string[] allMaterialGUIDs;

        string pipelinesPath;
        int pipelineIndex = 0;

        int assetVersion;
        int userVersion;

        bool upgradeIsNeeded = false;
        bool finishIsNeeded = false;

        GUIStyle stylePopup;

        Color bannerColor;
        string bannerText;
        string bannerVersion;
        string helpURL;
        static TVEHub window;
        //Vector2 scrollPosition = Vector2.zero;

        [MenuItem("Window/BOXOPHOBIC/The Vegetation Engine/Hub", false, 1000)]
        public static void ShowWindow()
        {
            window = GetWindow<TVEHub>(false, "The Vegetation Engine Hub", true);
            window.minSize = new Vector2(400, 200);
        }

        void OnEnable()
        {
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

            searchFolders = AssetDatabase.FindAssets("User");

            for (int i = 0; i < searchFolders.Length; i++)
            {
                if (AssetDatabase.GUIDToAssetPath(searchFolders[i]).EndsWith("User.pdf"))
                {
                    userFolder = AssetDatabase.GUIDToAssetPath(searchFolders[i]);
                    userFolder = userFolder.Replace("/User.pdf", "");
                    userFolder += "/The Vegetation Engine";
                }
            }

            pipelinesPath = assetFolder + "/Core/Pipelines";

            GetPackages();
            GetAllMaterialGUIDs();

            // GetUser Settings
            assetVersion = SettingsUtils.LoadSettingsData(assetFolder + "/Core/Editor/Version.asset", -99);
            userVersion = SettingsUtils.LoadSettingsData(userFolder + "/Version.asset", -99);

            if (userVersion == -99)
            {
                // Looks like new install, but User folder might be deleted so check for TVE materials
                for (int i = 0; i < allMaterialGUIDs.Length; i++)
                {
                    var path = AssetDatabase.GUIDToAssetPath(allMaterialGUIDs[i]);

                    // Exclude TVE folder when checking
                    if (path.Contains("TVE Material") && path.Contains("The Vegetation Engine") == false)
                    {
                        upgradeIsNeeded = true;
                        break;
                    }
                }
            }

            // User Version exist and need upgrade
            if (userVersion != -99 && userVersion < assetVersion)
            {
                upgradeIsNeeded = true;
            }

            // Curent version was installed but deleted and reimported
            if (userVersion == assetVersion)
            {
                upgradeIsNeeded = false;
            }

            for (int i = 0; i < pipelineOptions.Length; i++)
            {
                if (pipelineOptions[i] == SettingsUtils.LoadSettingsData(userFolder + "/Pipeline.asset", ""))
                {
                    pipelineIndex = i;
                }
            }

            bannerVersion = assetVersion.ToString();
            bannerVersion = bannerVersion.Insert(1, ".");
            bannerVersion = bannerVersion.Insert(3, ".");

            bannerColor = new Color(0.890f, 0.745f, 0.309f);
            bannerText = "The Vegetation Engine " + bannerVersion;
            helpURL = "https://docs.google.com/document/d/145JOVlJ1tE-WODW45YoJ6Ixg23mFc56EnB_8Tbwloz8/edit#heading=h.pr0qp2u684tr";

            // Check for latest version
            //StartBackgroundTask(StartRequest("https://boxophobic.com/s/thevegetationengine", () =>
            //{
            //    int.TryParse(www.downloadHandler.text, out latestVersion);
            //    Debug.Log("hubhub" + latestVersion);
            //}));
        }

        void OnGUI()
        {
            SetGUIStyles();
            DrawToolbar();

            StyledGUI.DrawWindowBanner(bannerColor, bannerText, helpURL);

            GUILayout.BeginHorizontal();
            GUILayout.Space(20);

            GUILayout.BeginVertical();

            //scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(this.position.width - 28), GUILayout.Height(this.position.height - 80));

            if (EditorApplication.isCompiling)
            {
                GUI.enabled = false;
            }

            if (File.Exists(assetFolder + "/Core/Editor/TVEHubAutoRun.cs"))
            {
                if (upgradeIsNeeded)
                {
                    EditorGUILayout.HelpBox("Previous version detected! The Vegetation Engine will check all project materials and upgrade them if needed. " +
                                            "Make sure you read the Upgrading Steps to upgrade to a new version. Do not close Unity during the upgrade!", MessageType.Info, true);

                    GUILayout.Space(10);

                    if (GUILayout.Button("Check Materials And Install", GUILayout.Height(24)))
                    {
                        SetMaterialSettings();
                        InstallAsset();
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Welcome to the Vegetation Engine! The installer will set up the asset and prepare the shaders for the current Unity version!", MessageType.Info, true);

                    GUILayout.Space(15);

                    if (GUILayout.Button("Install", GUILayout.Height(24)))
                    {
                        InstallAsset();
                    }
                }
            }
            // TVE is installed
            else
            {
                if (finishIsNeeded)
                {
                    EditorGUILayout.HelpBox("The setup is not done yet, click the button below to finish the setup!", MessageType.Info, true);

                    GUILayout.Space(10);

                    if (GUILayout.Button("Finish Setup", GUILayout.Height(24)))
                    {
                        SetMaterialSettings();

                        finishIsNeeded = false;

                        GUIUtility.ExitGUI();
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Click the Render Pipeline Import button to install the shaders for the choosed render pipeline!", MessageType.Info, true);

                    GUILayout.Space(10);

                    GUILayout.BeginHorizontal();

                    GUILayout.Label("Render Pipeline Support", GUILayout.Width(220));
                    pipelineIndex = EditorGUILayout.Popup(pipelineIndex, pipelineOptions, stylePopup);

                    if (GUILayout.Button("Import", GUILayout.Width(80), GUILayout.Height(GUI_HEIGHT)))
                    {
                        SettingsUtils.SaveSettingsData(userFolder + "/Pipeline.asset", pipelineOptions[pipelineIndex]);

                        SetDefineSymbols(pipelineOptions[pipelineIndex]);
                        ImportPackage();

                        if (pipelineOptions[pipelineIndex].Contains("High"))
                        {
                            finishIsNeeded = true;
                        }

                        GUIUtility.ExitGUI();
                    }

                    GUILayout.EndHorizontal();
                }

                GUILayout.Space(10);

                GUILayout.FlexibleSpace();

                GUILayout.Space(20);
            }

            GUI.enabled = true;

            GUILayout.EndVertical();

            //GUILayout.EndScrollView();

            GUILayout.Space(13);
            GUILayout.EndHorizontal();
        }

        void SetGUIStyles()
        {
            stylePopup = new GUIStyle(EditorStyles.popup)
            {
                alignment = TextAnchor.MiddleCenter
            };
        }

        void DrawToolbar()
        {
            var GUI_TOOLBAR_EDITOR_WIDTH = this.position.width / 4.0f + 1;

            var styledToolbar = new GUIStyle(EditorStyles.toolbarButton)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Normal,
                fontSize = 11,
            };

            GUILayout.Space(1);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Discord Server", styledToolbar, GUILayout.Width(GUI_TOOLBAR_EDITOR_WIDTH)))
            {
                Application.OpenURL("https://discord.com/invite/znxuXET");
            }
            GUILayout.Space(-1);

            if (GUILayout.Button("Documentation", styledToolbar, GUILayout.Width(GUI_TOOLBAR_EDITOR_WIDTH)))
            {
                Application.OpenURL("https://docs.google.com/document/d/145JOVlJ1tE-WODW45YoJ6Ixg23mFc56EnB_8Tbwloz8/edit#");
            }
            GUILayout.Space(-1);

            if (GUILayout.Button("Changelog", styledToolbar, GUILayout.Width(GUI_TOOLBAR_EDITOR_WIDTH)))
            {
                Application.OpenURL("https://docs.google.com/document/d/145JOVlJ1tE-WODW45YoJ6Ixg23mFc56EnB_8Tbwloz8/edit#heading=h.1rbujejuzjce");
            }
            GUILayout.Space(-1);

            if (GUILayout.Button("Write A Review", styledToolbar, GUILayout.Width(GUI_TOOLBAR_EDITOR_WIDTH)))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/tools/utilities/the-vegetation-engine-159647#reviews");
            }
            GUILayout.Space(-1);

            GUILayout.EndHorizontal();
            GUILayout.Space(4);
        }

        void InstallAsset()
        {
            SettingsUtils.SaveSettingsData(userFolder + "/Version.asset", assetVersion);
            SettingsUtils.SaveSettingsData(userFolder + "/Pipeline.asset", "Standard");
            SettingsUtils.SaveSettingsData(userFolder + "/Engine.asset", "Unity Default Renderer");

            FileUtil.DeleteFileOrDirectory(assetFolder + "/Core/Editor/TVEHubAutorun.cs");

            if (File.Exists(assetFolder + "/Core/Editor/TVEHubAutoRun.cs.meta"))
            {
                FileUtil.DeleteFileOrDirectory(assetFolder + "/Core/Editor/TVEHubAutorun.cs.meta");
            }

            AssetDatabase.Refresh();

            SetDefineSymbols("Standard");
            SetScriptExecutionOrder();

            Debug.Log("<b>[The Vegetation Engine Installer]</b> " + "The Vegetation Engine " + bannerVersion + " is installed!");

            GUIUtility.ExitGUI();
        }

        void GetPackages()
        {
            pipelinePaths = Directory.GetFiles(pipelinesPath, "*.unitypackage", SearchOption.TopDirectoryOnly);

            pipelineOptions = new string[pipelinePaths.Length];

            for (int i = 0; i < pipelineOptions.Length; i++)
            {
                pipelineOptions[i] = Path.GetFileNameWithoutExtension(pipelinePaths[i].Replace("Built-in Pipeline", "Standard"));
            }
        }

        void GetAllMaterialGUIDs()
        {
            allMaterialGUIDs = AssetDatabase.FindAssets("t:material", null);
        }

        void SetMaterialSettings()
        {
            int length = allMaterialGUIDs.Length;
            int count = 0;

            foreach (var asset in allMaterialGUIDs)
            {
                var path = AssetDatabase.GUIDToAssetPath(asset);
                var message = "Checking ";

                if (path.Contains("TVE Material"))
                {
                    var material = AssetDatabase.LoadAssetAtPath<Material>(path);
                    TVEMaterial.SetMaterialSettings(material);
                }

                if (path.Contains("TVE Element"))
                {
                    var material = AssetDatabase.LoadAssetAtPath<Material>(path);
                    TVEMaterial.SetElementSettings(material);
                }

                if (path.Contains("_Impostor"))
                {
                    var material = AssetDatabase.LoadAssetAtPath<Material>(path);

                    if (material.HasProperty("_IsVersion"))
                    {
                        if (material.GetInt("_IsVersion") < 600)
                        {
                            if (material.HasProperty("_LayerReactValue"))
                            {
                                material.SetInt("_LayerVertexValue", material.GetInt("_LayerReactValue"));
                            }

                            material.SetInt("_IsVersion", 600);
                        }
                    }
                }

                EditorUtility.DisplayProgressBar("The Vegetatin Engine", message + Path.GetFileName(path), (float)count * (1.0f / (float)length));
            }

            EditorUtility.ClearProgressBar();
        }

        void ImportPackage()
        {
            AssetDatabase.ImportPackage(pipelinePaths[pipelineIndex], false);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("<b>[The Vegetation Engine Installer]</b> " + pipelineOptions[pipelineIndex] + " package is imported!");
        }

        void SetDefineSymbols(string pipeline)
        {
            var defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

            if (!defineSymbols.Contains("THE_VEGETATION_ENGINE"))
            {
                defineSymbols += ";THE_VEGETATION_ENGINE;";
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defineSymbols);
        }

        void SetScriptExecutionOrder()
        {
            MonoScript[] scripts = (MonoScript[])Resources.FindObjectsOfTypeAll(typeof(MonoScript));
            
            foreach (MonoScript script in scripts)
            {
                if (script.GetClass() == typeof(TVEManager))
                {
                    MonoImporter.SetExecutionOrder(script, -122);
                }
            }
        }

        // Check for latest version
        //UnityWebRequest www;

        //IEnumerator StartRequest(string url, Action success = null)
        //{
        //    using (www = UnityWebRequest.Get(url))
        //    {
        //        yield return www.Send();

        //        while (www.isDone == false)
        //            yield return null;

        //        if (success != null)
        //            success();
        //    }
        //}

        //public static void StartBackgroundTask(IEnumerator update, Action end = null)
        //{
        //    EditorApplication.CallbackFunction closureCallback = null;

        //    closureCallback = () =>
        //    {
        //        try
        //        {
        //            if (update.MoveNext() == false)
        //            {
        //                if (end != null)
        //                    end();
        //                EditorApplication.update -= closureCallback;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            if (end != null)
        //                end();
        //            Debug.LogException(ex);
        //            EditorApplication.update -= closureCallback;
        //        }
        //    };

        //    EditorApplication.update += closureCallback;
        //}
    }
}
