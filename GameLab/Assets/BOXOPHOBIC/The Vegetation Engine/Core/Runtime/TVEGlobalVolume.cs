// Cristian Pop - https://boxophobic.com/

using UnityEngine;
using Boxophobic.StyledGUI;
using UnityEngine.Rendering;
using System.Collections.Generic;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TheVegetationEngine
{
    [ExecuteInEditMode]
    [AddComponentMenu("BOXOPHOBIC/The Vegetation Engine/TVE Global Volume")]
    public class TVEGlobalVolume : StyledMonoBehaviour
    {
        public enum ElementsVisibility
        {
            AlwaysHidden = 0,
            AlwaysVisible = 10,
            HiddenAtRuntime = 20,
        }

        public enum ElementsSorting
        {
            SortInEditMode = 0,
            SortAtRuntime = 10,
        }

        public enum RenderDataMode
        {
            Off = -1,
            FollowMainCamera256 = 8,
            FollowMainCamera512 = 9,
            FollowMainCamera1024 = 10,
            FollowMainCamera2048 = 11,
            FollowMainCamera4096 = 12,
            InsideGlobalVolume256 = 256,
            InsideGlobalVolume512 = 512,
            InsideGlobalVolume1024 = 1024,
            InsideGlobalVolume2048 = 2048,
            InsideGlobalVolume4096 = 4096,
        }

        [StyledBanner(0.890f, 0.745f, 0.309f, "Global Volume", "", "https://docs.google.com/document/d/145JOVlJ1tE-WODW45YoJ6Ixg23mFc56EnB_8Tbwloz8/edit#heading=h.a39m1w5ouu94")]
        public bool styledBanner;

        [StyledCategory("Camera Settings", 5, 10)]
        public bool cameraCat;

        [StyledMessage("Error", "Main Camera not found! Make sure you have a main camera with Main Camera tag in your scene! Particle elements updating will be skipped without it. Enter play mode to update the status!", 0, 10)]
        public bool styledCameraMessaage = false;

        public Camera mainCamera;

        [StyledCategory("Elements Settings")]
        public bool elementsCat;

#if UNITY_EDITOR
        [StyledMessage("Info", "Realtime Sorting is not supported for elements with GPU Instanceing enabled!", 0, 10)]
        public bool styledSortingMessaage = true;
#endif

        [Tooltip("Controls the elements visibility in scene and game view.")]
        public ElementsVisibility elementsVisibility = ElementsVisibility.HiddenAtRuntime;
        [HideInInspector]
        public ElementsVisibility elementsVisibilityOld = ElementsVisibility.HiddenAtRuntime;
        [Tooltip("Controls the elements sorting by element position. Always on in edit mode.")]
        public ElementsSorting elementsSorting = ElementsSorting.SortInEditMode;
        [Tooltip("Controls the elements fading at the volume edges if the Enable Volume Edge Fading support is toggled on the element material.")]
        [Range(0.0f, 1.0f)]
        public float elementsEdgeFade = 0.75f;

        [StyledCategory("Render Settings")]
        public bool dataCat;

        [Tooltip("Render mode used for Colors elements rendering.")]
        [FormerlySerializedAs("renderColorsData")]
        public RenderDataMode renderColors = RenderDataMode.InsideGlobalVolume1024;

        [Tooltip("Render mode used for Extras elements rendering.")]
        [FormerlySerializedAs("renderExtrasData")]
        public RenderDataMode renderExtras = RenderDataMode.InsideGlobalVolume1024;

        [Tooltip("Render mode used for Motion elements rendering.")]
        [FormerlySerializedAs("renderMotionData")]
        public RenderDataMode renderMotion = RenderDataMode.InsideGlobalVolume1024;

        [Tooltip("Render mode used for Size elements rendering.")]
        [FormerlySerializedAs("renderReactData")]
        public RenderDataMode renderVertex = RenderDataMode.InsideGlobalVolume1024;

        [Tooltip("Uses high precision render textures for Colors elements HDR support and for high quality Motion Interaction. Enter playmode to see the changes!")]
        [Space(10)]
        public bool useHighPrecisionRendering = true;

        [StyledInteractive()]
        public bool useFollowMainCamera = false;

        [Space(10)]
        [Tooltip("The volume scale used for follow main camera render data mode.")]
        public Vector3 followMainCameraVolume = new Vector3(100, 100, 100);
        [Tooltip("Pushes the follow main camera volume in the camera forward direction to avoid rendering elements behind the camera.")]
        [Range(0.0f, 1.0f)]
        public float followMainCameraOffset = 1;

        [StyledInteractive()]
        public bool usesFollowActive = true;

        [Tooltip("List containg Volume data entities.")]
        [System.NonSerialized]
        public List<TVERenderData> renderDataSet;

        [Tooltip("List containg all the Element entities.")]
        [System.NonSerialized]
        public List<TVEElementDefaultData> renderElements;

        [Tooltip("List containg all the Element entities with GPU Instancing enabled.")]
        [System.NonSerialized]
        public List<TVEElementInstancedData> renderInstanced;

        [StyledSpace(10)]
        public bool styledSpace0;

        TVERenderData colorsData;
        TVERenderData extrasData;
        TVERenderData motionData;
        TVERenderData vertexData;

        MaterialPropertyBlock properties;

        Matrix4x4 projectionMatrix;
        Matrix4x4 modelViewMatrix = new Matrix4x4
        (
            new Vector4(1f, 0f, 0f, 0f),
            new Vector4(0f, 0f, -1f, 0f),
            new Vector4(0f, -1f, 0f, 0f),
            new Vector4(0f, 0f, 0f, 1f)
        );

        void Start()
        {
            gameObject.name = "Global Volume";
            gameObject.transform.SetSiblingIndex(3);

            CreateRenderBuffers();

            SortElementObjects();
            SetElementsVisibility();

            if (Application.isPlaying)
            {
                BuildInstancedElements();
            }
        }

        void Update()
        {
            if (elementsSorting == ElementsSorting.SortAtRuntime)
            {
                SortElementObjects();
            }

            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            if (properties == null)
            {
                properties = new MaterialPropertyBlock();
            }

            if (elementsVisibilityOld != elementsVisibility)
            {
                SetElementsVisibility();

                elementsVisibilityOld = elementsVisibility;
            }

            CheckRenderBuffers();

            UpdateRenderBuffers();
            ExecuteRenderBuffers();

            SetGlobalShaderParameters();

#if UNITY_EDITOR
            if (elementsSorting == ElementsSorting.SortAtRuntime)
            {
                styledSortingMessaage = true;
            }
            else
            {
                styledSortingMessaage = false;
            }

            if (mainCamera == null)
            {
                styledCameraMessaage = true;
            }
            else
            {
                styledCameraMessaage = false;
            }

            useFollowMainCamera = false;

            for (int i = 0; i < renderDataSet.Count; i++)
            {
                var renderData = renderDataSet[i];

                if (renderData == null)
                {
                    continue;
                }

                if (renderData.isFollowing)
                {
                    useFollowMainCamera = true;
                }
            }
#endif
        }

        public void InitVolumeRendering()
        {
            renderDataSet = new List<TVERenderData>();
            renderElements = new List<TVEElementDefaultData>();
            renderInstanced = new List<TVEElementInstancedData>();

            colorsData = new TVERenderData();
            colorsData.isEnabled = true;
            colorsData.texName = "TVE_ColorsTex";
            colorsData.texParams = "TVE_ColorsParams";
            colorsData.texCoord = "TVE_ColorsCoords";
            colorsData.texUsage = "TVE_ColorsUsage";
            colorsData.volumePosition = "TVE_ColorsPosition";
            colorsData.volumeScale = "TVE_ColorsScale";

            colorsData.materialFilter = "_IsColorsElement";

            colorsData.texResolution = 1024;
            colorsData.bufferSize = -1;

            if (useHighPrecisionRendering)
            {
                colorsData.texFormat = RenderTextureFormat.ARGBHalf;
            }
            else
            {
                colorsData.texFormat = RenderTextureFormat.Default;
            }

            extrasData = new TVERenderData();
            extrasData.isEnabled = true;

            extrasData.texName = "TVE_ExtrasTex";
            extrasData.texParams = "TVE_ExtrasParams";
            extrasData.texCoord = "TVE_ExtrasCoords";
            extrasData.texUsage = "TVE_ExtrasUsage";
            extrasData.volumePosition = "TVE_ExtrasPosition";
            extrasData.volumeScale = "TVE_ExtrasScale";

            extrasData.materialFilter = "_IsExtrasElement";

            extrasData.texResolution = 1024;
            extrasData.bufferSize = -1;
            extrasData.texFormat = RenderTextureFormat.Default;

            motionData = new TVERenderData();
            motionData.isEnabled = true;

            motionData.texName = "TVE_MotionTex";
            motionData.texParams = "TVE_MotionParams";
            motionData.texCoord = "TVE_MotionCoords";
            motionData.texUsage = "TVE_MotionUsage";
            motionData.volumePosition = "TVE_MotionPosition";
            motionData.volumeScale = "TVE_MotionScale";

            motionData.materialFilter = "_IsMotionElement";

            motionData.texResolution = 1024;
            motionData.bufferSize = -1;

            if (useHighPrecisionRendering)
            {
                motionData.texFormat = RenderTextureFormat.ARGBHalf;
            }
            else
            {
                motionData.texFormat = RenderTextureFormat.Default;
            }

            vertexData = new TVERenderData();
            vertexData.isEnabled = true;
            vertexData.texName = "TVE_VertexTex";
            vertexData.texParams = "TVE_VertexParams";
            vertexData.texCoord = "TVE_VertexCoords";
            vertexData.texUsage = "TVE_VertexUsage";
            vertexData.volumePosition = "TVE_VertexPosition";
            vertexData.volumeScale = "TVE_VertexScale";

            vertexData.materialFilter = "_IsVertexElement";

            vertexData.texResolution = 1024;
            vertexData.bufferSize = -1;
            vertexData.texFormat = RenderTextureFormat.Default;

            UpdateRenderData(colorsData, renderColors);
            UpdateRenderData(extrasData, renderExtras);
            UpdateRenderData(motionData, renderMotion);
            UpdateRenderData(vertexData, renderVertex);

            renderDataSet.Add(colorsData);
            renderDataSet.Add(extrasData);
            renderDataSet.Add(motionData);
            renderDataSet.Add(vertexData);
        }

        void UpdateRenderData(TVERenderData renderData, RenderDataMode renderDataMode)
        {
            if (renderDataMode == RenderDataMode.Off)
            {
                renderData.isEnabled = false;
                renderData.texResolution = 32;
                renderData.bufferSize = -1;
                renderData.isFollowing = false;
            }
            else if (renderDataMode == RenderDataMode.FollowMainCamera256)
            {
                renderData.isEnabled = true;
                renderData.texResolution = 256;
                renderData.isFollowing = true;
            }
            else if (renderDataMode == RenderDataMode.FollowMainCamera512)
            {
                renderData.isEnabled = true;
                renderData.texResolution = 512;
                renderData.isFollowing = true;
            }
            else if (renderDataMode == RenderDataMode.FollowMainCamera1024)
            {
                renderData.isEnabled = true;
                renderData.texResolution = 1024;
                renderData.isFollowing = true;
            }
            else if (renderDataMode == RenderDataMode.FollowMainCamera2048)
            {
                renderData.isEnabled = true;
                renderData.texResolution = 2048;
                renderData.isFollowing = true;
            }
            else if (renderDataMode == RenderDataMode.FollowMainCamera4096)
            {
                renderData.isEnabled = true;
                renderData.texResolution = 4096;
                renderData.isFollowing = true;
            }
            else if (renderDataMode == RenderDataMode.InsideGlobalVolume256)
            {
                renderData.isEnabled = true;
                renderData.texResolution = 256;
                renderData.isFollowing = false;
            }
            else if (renderDataMode == RenderDataMode.InsideGlobalVolume512)
            {
                renderData.isEnabled = true;
                renderData.texResolution = 512;
                renderData.isFollowing = false;
            }
            else if (renderDataMode == RenderDataMode.InsideGlobalVolume1024)
            {
                renderData.isEnabled = true;
                renderData.texResolution = 1024;
                renderData.isFollowing = false;
            }
            else if (renderDataMode == RenderDataMode.InsideGlobalVolume2048)
            {
                renderData.isEnabled = true;
                renderData.texResolution = 2048;
                renderData.isFollowing = false;
            }
            else if (renderDataMode == RenderDataMode.InsideGlobalVolume4096)
            {
                renderData.isEnabled = true;
                renderData.texResolution = 4096;
                renderData.isFollowing = false;
            }
        }

        void CreateRenderBuffers()
        {
            for (int i = 0; i < renderDataSet.Count; i++)
            {
                var renderData = renderDataSet[i];

                if (renderData == null)
                {
                    continue;
                }

                if (renderData.texObject != null)
                {
                    renderData.texObject.Release();
                }

                if (renderData.commandBuffers != null)
                {
                    for (int b = 0; b < renderData.commandBuffers.Length; b++)
                    {
                        renderData.commandBuffers[b].Clear();
                    }
                }

                renderData.bufferUsage = new float[9];
                Shader.SetGlobalFloatArray(renderData.texUsage, renderData.bufferUsage);

                if (renderData.isEnabled && renderData.bufferSize > -1)
                {
                    renderData.texObject = new RenderTexture(renderData.texResolution, renderData.texResolution, 0, renderData.texFormat);
                    renderData.texObject.dimension = TextureDimension.Tex2DArray;
                    renderData.texObject.volumeDepth = renderData.bufferSize + 1;
                    renderData.texObject.name = renderData.texName;
                    renderData.texObject.wrapMode = TextureWrapMode.Clamp;

                    renderData.commandBuffers = new CommandBuffer[renderData.bufferSize + 1];

                    for (int b = 0; b < renderData.commandBuffers.Length; b++)
                    {
                        renderData.commandBuffers[b] = new CommandBuffer();
                        renderData.commandBuffers[b].name = renderData.texName;
                    }

                    Shader.SetGlobalTexture(renderData.texName, renderData.texObject);
                }
                else
                {
                    Shader.SetGlobalTexture(renderData.texName, Resources.Load<Texture2DArray>("Internal ArrayTex"));
                }
            }
        }

        void CheckRenderBuffers()
        {
            for (int i = 0; i < renderDataSet.Count; i++)
            {
                var renderData = renderDataSet[i];

                if (renderData == null)
                {
                    continue;
                }

                if (renderData.isUpdated)
                {
                    CreateRenderBuffers();
                    renderData.isUpdated = false;
                }
            }
        }

        void UpdateRenderBuffers()
        {
            for (int i = 0; i < renderDataSet.Count; i++)
            {
                var renderData = renderDataSet[i];

                if (renderData == null || renderData.commandBuffers == null || !renderData.isEnabled)
                {
                    continue;
                }

                var bufferParams = Shader.GetGlobalVector(renderData.texParams);

                for (int b = 0; b < renderData.commandBuffers.Length; b++)
                {
                    renderData.commandBuffers[b].Clear();
                    renderData.commandBuffers[b].ClearRenderTarget(true, true, bufferParams);
                    renderData.bufferUsage[b] = 0;

                    for (int e = 0; e < renderElements.Count; e++)
                    {
                        var elementData = renderElements[e];

                        if (elementData.renderer.sharedMaterial.HasProperty(renderData.materialFilter))
                        {
                            //bufferUsage[b] = 0;

                            if (elementData.layers[b] == 1)
                            {
                                Camera.SetupCurrent(mainCamera);

                                properties.SetFloat("_RaycastFadeValue", elementData.fadeValue);
                                elementData.renderer.SetPropertyBlock(properties);

                                renderData.commandBuffers[b].DrawRenderer(elementData.renderer, elementData.renderer.sharedMaterial, 0, 0);
                                renderData.bufferUsage[b] = 1;
                            }
                        }
                    }

                    if (!Application.isPlaying)
                    {
                        continue;
                    }

                    for (int e = 0; e < renderInstanced.Count; e++)
                    {
                        var elementData = renderInstanced[e];

                        if (elementData.material.HasProperty(renderData.materialFilter))
                        {
                            //bufferUsage[b] = 0;

                            if (elementData.layers[b] == 1)
                            {
                                Matrix4x4[] matrix4X4s = new Matrix4x4[elementData.renderers.Count];

                                for (int m = 0; m < elementData.renderers.Count; m++)
                                {
                                    matrix4X4s[m] = elementData.renderers[m].localToWorldMatrix;
                                }

                                renderData.commandBuffers[b].DrawMeshInstanced(elementData.mesh, 0, elementData.material, 0, matrix4X4s);
                                renderData.bufferUsage[b] = 1;
                            }
                        }
                    }
                }

                Shader.SetGlobalFloatArray(renderData.texUsage, renderData.bufferUsage);

                //for (int u = 0; u < renderData.bufferUsage.Length; u++)
                //{
                //    Debug.Log(renderData.texUsage + " Index: " + u + " Usage: " + renderData.bufferUsage[u]);
                //}
            }
        }

        void ExecuteRenderBuffers()
        {
            GL.PushMatrix();
            RenderTexture currentRenderTexture = RenderTexture.active;

            for (int i = 0; i < renderDataSet.Count; i++)
            {
                var renderData = renderDataSet[i];

                if (renderData == null || renderData.commandBuffers == null || !renderData.isEnabled)
                {
                    continue;
                }

                var position = Vector3.zero;
                var scale = Vector3.zero;

                if (renderData.isEnabled)
                {
                    if (renderData.isFollowing)
                    {
                        if (mainCamera != null)
                        {
                            var offsetX = followMainCameraVolume.x / 2 * mainCamera.transform.forward.x * followMainCameraOffset;
                            var offsetZ = followMainCameraVolume.z / 2 * mainCamera.transform.forward.z * followMainCameraOffset;
                            var cameraPos = mainCamera.transform.position + new Vector3(offsetX, 1, offsetZ);

                            float gridX = followMainCameraVolume.x / renderData.texResolution;
                            float gridZ = followMainCameraVolume.z / renderData.texResolution;
                            float posX = Mathf.Round(cameraPos.x / gridX) * gridX;
                            float posZ = Mathf.Round(cameraPos.z / gridZ) * gridZ;

                            position = new Vector3(posX, mainCamera.transform.position.y, posZ);
                            scale = new Vector3(followMainCameraVolume.x, followMainCameraVolume.y, followMainCameraVolume.z);
                        }
                    }
                    else
                    {
                        position = gameObject.transform.position;
                        scale = gameObject.transform.lossyScale;
                    }
                }

                projectionMatrix = Matrix4x4.Ortho(-scale.x / 2 + position.x,
                                                    scale.x / 2 + position.x,
                                                    scale.z / 2 + -position.z,
                                                    -scale.z / 2 + -position.z,
                                                    -scale.y / 2 + position.y,
                                                    scale.y / 2 + position.y);

                var x = 1 / scale.x;
                var y = 1 / scale.z;
                var z = 1 / scale.x * position.x - 0.5f;
                var w = 1 / scale.z * position.z - 0.5f;
                var coord = new Vector4(x, y, -z, -w);

                GL.LoadProjectionMatrix(projectionMatrix);
                GL.modelview = modelViewMatrix;

                Shader.SetGlobalVector(renderData.texCoord, coord);
                Shader.SetGlobalVector(renderData.volumePosition, position);
                Shader.SetGlobalVector(renderData.volumeScale, scale);

                for (int b = 0; b < renderData.commandBuffers.Length; b++)
                {
                    Graphics.SetRenderTarget(renderData.texObject, 0, CubemapFace.Unknown, b);
                    Graphics.ExecuteCommandBuffer(renderData.commandBuffers[b]);
                }
            }

            RenderTexture.active = currentRenderTexture;
            GL.PopMatrix();
        }

        void SetGlobalShaderParameters()
        {
            Shader.SetGlobalFloat("TVE_ElementsFadeValue", elementsEdgeFade);
        }

        public void SortElementObjects()
        {
            for (int i = 0; i < renderElements.Count - 1; i++)
            {
                for (int j = 0; j < renderElements.Count - 1; j++)
                {
                    if (renderElements[j] != null && renderElements[j].element.transform.position.y > renderElements[j + 1].element.transform.position.y)
                    {
                        var next = renderElements[j + 1];
                        renderElements[j + 1] = renderElements[j];
                        renderElements[j] = next;
                    }
                }
            }
        }

        public void BuildInstancedElements()
        {
            if (renderElements.Count == 0)
            {
                return;
            }

            var instanced = new List<TVEElementInstancedData>();

            for (int i = 0; i < renderElements.Count; i++)
            {
                if (renderElements[i].renderer.sharedMaterial.enableInstancing == true)
                {
                    var element = renderElements[i];

                    var elementData = new TVEElementInstancedData();
                    //elementData.bufferFilter = element.bufferFilter;
                    elementData.layers = element.layers;
                    elementData.material = element.renderer.sharedMaterial;
                    elementData.mesh = element.mesh;

                    instanced.Add(elementData);
                }
            }

            for (int i = 0; i < instanced.Count; i++)
            {
                var renderersList = new List<Renderer>();

                for (int j = 0; j < renderElements.Count; j++)
                {
                    if (renderersList.Count > 1022)
                    {
                        break;
                    }

                    if (instanced[i].material == renderElements[j].renderer.sharedMaterial && instanced[i].mesh == renderElements[j].mesh)
                    {
                        renderersList.Add(renderElements[j].renderer);
                        renderElements.Remove(renderElements[j]);
                        j--;
                    }
                }

                instanced[i].renderers = renderersList;
            }

            for (int i = 0; i < instanced.Count; i++)
            {
                if (instanced[i].renderers.Count == 0)
                {
                    instanced.RemoveAt(i);
                    i--;
                }
            }

            renderInstanced.AddRange(instanced);
            instanced.Clear();
        }

        void SetElementsVisibility()
        {
            if (elementsVisibility == ElementsVisibility.AlwaysHidden)
            {
                DisableElementsVisibility();
            }
            else if (elementsVisibility == ElementsVisibility.AlwaysVisible)
            {
                EnableElementsVisibility();
            }
            else if (elementsVisibility == ElementsVisibility.HiddenAtRuntime)
            {
                if (Application.isPlaying)
                {
                    DisableElementsVisibility();
                }
                else
                {
                    EnableElementsVisibility();
                }
            }
        }

        void EnableElementsVisibility()
        {
            for (int i = 0; i < renderElements.Count; i++)
            {
                if (renderElements[i] != null)
                {
#if UNITY_2019_3_OR_NEWER
                    renderElements[i].renderer.forceRenderingOff = false;
#else
                    volumeElements[i].renderer.enabled = true;
#endif
                }
            }
        }

        void DisableElementsVisibility()
        {
            for (int i = 0; i < renderElements.Count; i++)
            {
                if (renderElements[i] != null)
                {
#if UNITY_2019_3_OR_NEWER
                    renderElements[i].renderer.forceRenderingOff = true;
#else
                    volumeElements[i].renderer.enabled = false;
#endif
                }
            }
        }

#if UNITY_EDITOR        
        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.890f, 0.745f, 0.309f, 1f);
            Gizmos.DrawWireCube(transform.position, transform.lossyScale);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.0f, 0.0f, 0.0f, 0.11f);
            Gizmos.DrawWireCube(transform.position, transform.lossyScale);

            if (useFollowMainCamera)
            {
                if (mainCamera != null)
                {
                    if (Selection.Contains(mainCamera.gameObject))
                    {
                        Gizmos.color = new Color(0.890f, 0.745f, 0.309f, 1f);
                    }

                    var offsetX = followMainCameraVolume.x / 2 * mainCamera.transform.forward.x * followMainCameraOffset;
                    var offsetZ = followMainCameraVolume.z / 2 * mainCamera.transform.forward.z * followMainCameraOffset;
                    var cameraPos = mainCamera.transform.position + new Vector3(offsetX, 1, offsetZ);

                    Gizmos.DrawWireCube(new Vector3(cameraPos.x, mainCamera.transform.position.y, cameraPos.z), followMainCameraVolume);
                }
            }
        }

        void OnValidate()
        {
            if (renderDataSet == null)
            {
                return;
            }

            if (colorsData == null || extrasData == null || motionData == null || vertexData == null)
            {
                return;
            }

            UpdateRenderData(colorsData, renderColors);
            UpdateRenderData(extrasData, renderExtras);
            UpdateRenderData(motionData, renderMotion);
            UpdateRenderData(vertexData, renderVertex);

            CreateRenderBuffers();
        }
#endif
    }
}
