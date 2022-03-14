// Cristian Pop - https://boxophobic.com/

using UnityEngine;
using Boxophobic.StyledGUI;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TheVegetationEngine
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
    [AddComponentMenu("BOXOPHOBIC/The Vegetation Engine/TVE Element")]
#endif
    public class TVEElement : StyledMonoBehaviour
    {
        const string elementLayerMask = "_ElementLayerMask";

        [StyledBanner(0.890f, 0.745f, 0.309f, "Element", "", "https://docs.google.com/document/d/145JOVlJ1tE-WODW45YoJ6Ixg23mFc56EnB_8Tbwloz8/edit#heading=h.fd5y8rbb7aia")]
        public bool styledBanner;

        [HideInInspector]
        public TVEElementMaterialData materialData;
        TVEElementDefaultData elementData;

        Renderer meshRenderer;
        Material sharedMaterial;
        new ParticleSystem particleSystem;

        int useVertexColorDirection = 0;
        int useRaycastFading = 0;
        Vector3 lastPosition;

        LayerMask raycastMask;
        float raycastEnd = 0;

        bool isSelected;

        void OnEnable()
        {
            meshRenderer = gameObject.GetComponent<Renderer>();
            sharedMaterial = meshRenderer.sharedMaterial;
            particleSystem = gameObject.GetComponent<ParticleSystem>();

            if (sharedMaterial == null || sharedMaterial.name == "Element")
            {
                if (materialData == null)
                {
                    materialData = new TVEElementMaterialData();
                }

                if (materialData.shader == null)
                {
#if UNITY_EDITOR
                    sharedMaterial = new Material(Resources.Load<Material>("Internal Colors"));
                    SaveMaterialData(sharedMaterial);
#endif
                }
                else
                {
                    sharedMaterial = new Material(materialData.shader);
                    LoadMaterialData(sharedMaterial);
                }

                sharedMaterial.name = "Element";
                gameObject.GetComponent<Renderer>().sharedMaterial = sharedMaterial;
            }

            if (sharedMaterial != null)
            {
                TVEMaterial.SetElementSettings(sharedMaterial);

                GetMaterialParameters();
            }

            AddElementToVolume();
        }

        void OnDestroy()
        {
            RemoveElementFromVolume();
        }

        void OnDisable()
        {
            RemoveElementFromVolume();
        }

        void Update()
        {
#if UNITY_EDITOR

            if (Selection.Contains(gameObject))
            {
                isSelected = true;
            }
            else
            {
                isSelected = false;
            }

            if (isSelected)
            {
                sharedMaterial = meshRenderer.sharedMaterial;

                GetMaterialParameters();

                if (!EditorUtility.IsPersistent(sharedMaterial))
                {
                    SaveMaterialData(sharedMaterial);
                }

                RemoveElementFromVolume();
                AddElementToVolume();

                if (TVEManager.Instance != null)
                {
                    TVEManager.Instance.globalVolume.SortElementObjects();
                }
            }
#endif

            if (particleSystem != null)
            {
                var particleModule = particleSystem.main;
                var particleColor = particleModule.startColor.color;

                if (useVertexColorDirection > 0)
                {
                    var direction = transform.position - lastPosition;
                    var localDirection = transform.InverseTransformDirection(direction);
                    var worldDirection = transform.TransformVector(localDirection);
                    lastPosition = transform.position;

                    var worldDirectionX = Mathf.Clamp01(worldDirection.x * 10 * 0.5f + 0.5f);
                    var worldDirectionZ = Mathf.Clamp01(worldDirection.z * 10 * 0.5f + 0.5f);

                    particleColor = new Color(worldDirectionX, worldDirectionZ, particleColor.b, particleColor.a);
                }

                if (useRaycastFading > 0)
                {
                    var fade = GetRacastFading();
                    particleColor = new Color(particleColor.r, particleColor.g, particleColor.b, fade);
                }
                else
                {
                    particleColor = new Color(particleColor.r, particleColor.g, particleColor.b, particleColor.a);
                }

                particleModule.startColor = particleColor;
            }
            else
            {
                if (useRaycastFading > 0)
                {
                    var fade = GetRacastFading();

                    elementData.fadeValue = fade;
                }
                else
                {
                    elementData.fadeValue = 1.0f;
                }
            }
        }


#if UNITY_EDITOR
        void SaveMaterialData(Material material)
        {
            materialData = new TVEElementMaterialData();
            materialData.props = new List<TVEElementPropertyData>();

            materialData.shader = material.shader;

            for (int i = 0; i < ShaderUtil.GetPropertyCount(material.shader); i++)
            {
                var type = ShaderUtil.GetPropertyType(material.shader, i);
                var prop = ShaderUtil.GetPropertyName(material.shader, i);

                if (type == ShaderUtil.ShaderPropertyType.TexEnv)
                {
                    var propData = new TVEElementPropertyData();
                    propData.type = PropertyType.Texture;
                    propData.prop = prop;
                    propData.texture = material.GetTexture(prop);

                    materialData.props.Add(propData);
                }

                if (type == ShaderUtil.ShaderPropertyType.Vector || type == ShaderUtil.ShaderPropertyType.Color)
                {
                    var propData = new TVEElementPropertyData();
                    propData.type = PropertyType.Vector;
                    propData.prop = prop;
                    propData.vector = material.GetVector(prop);

                    materialData.props.Add(propData);
                }

                if (type == ShaderUtil.ShaderPropertyType.Float || type == ShaderUtil.ShaderPropertyType.Range)
                {
                    var propData = new TVEElementPropertyData();
                    propData.type = PropertyType.Value;
                    propData.prop = prop;
                    propData.value = material.GetFloat(prop);

                    materialData.props.Add(propData);
                }
            }
        }
#endif

        void LoadMaterialData(Material material)
        {
            material.shader = materialData.shader;

            for (int i = 0; i < materialData.props.Count; i++)
            {
                if (materialData.props[i].type == PropertyType.Texture)
                {
                    material.SetTexture(materialData.props[i].prop, materialData.props[i].texture);
                }

                if (materialData.props[i].type == PropertyType.Vector)
                {
                    material.SetVector(materialData.props[i].prop, materialData.props[i].vector);
                }

                if (materialData.props[i].type == PropertyType.Value)
                {
                    material.SetFloat(materialData.props[i].prop, materialData.props[i].value);
                }
            }
        }

        void GetMaterialParameters()
        {
            if (sharedMaterial.HasProperty("_ElementDirectionMode"))
            {
                useVertexColorDirection = sharedMaterial.GetInt("_ElementDirectionMode");
            }

            if (sharedMaterial.HasProperty("_ElementRaycastMode"))
            {
                useRaycastFading = sharedMaterial.GetInt("_ElementRaycastMode");
                raycastMask = sharedMaterial.GetInt("_RaycastLayerMask");
                raycastEnd = sharedMaterial.GetInt("_RaycastDistanceEndValue");
            }
        }

        float GetRacastFading()
        {
            raycastEnd = sharedMaterial.GetInt("_RaycastDistanceEndValue");

            RaycastHit hit;
            bool raycastHit = Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, raycastMask);

            if (hit.distance > 0)
            {
                return 1 - Mathf.Clamp01(hit.distance / raycastEnd);
            }
            else
            {
                return 0;
            }
        }

        void AddElementToVolume()
        {
            if (TVEManager.Instance == null)
                return;

            if (gameObject.GetComponent<MeshRenderer>() != null && gameObject.GetComponent<MeshRenderer>().sharedMaterial != null)
            {
                var renderer = gameObject.GetComponent<MeshRenderer>();

                elementData = new TVEElementDefaultData();
                elementData.element = gameObject;
                elementData.type = RendererType.Mesh;
                elementData.renderer = renderer;
                elementData.mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
                elementData.fadeValue = 1.0f;

                AddElement();
                SetElementVisibility(renderer);
            }
            else if (gameObject.GetComponent<ParticleSystemRenderer>() != null && gameObject.GetComponent<ParticleSystemRenderer>().sharedMaterial != null)
            {
                var renderer = gameObject.GetComponent<ParticleSystemRenderer>();

                elementData = new TVEElementDefaultData();
                elementData.element = gameObject;
                elementData.type = RendererType.Particle;
                elementData.renderer = renderer;
                elementData.mesh = null;
                elementData.fadeValue = 1.0f;

                AddElement();
                SetElementVisibility(renderer);
            }
            else if (gameObject.GetComponent<TrailRenderer>() != null && gameObject.GetComponent<TrailRenderer>().sharedMaterial != null)
            {
                var renderer = gameObject.GetComponent<TrailRenderer>();

                elementData = new TVEElementDefaultData();
                elementData.element = gameObject;
                elementData.type = RendererType.Trail;
                elementData.renderer = renderer;
                elementData.mesh = null;
                elementData.fadeValue = 1.0f;

                AddElement();
                SetElementVisibility(renderer);
            }
            //else if (gameObject.GetComponent<LineRenderer>() != null && gameObject.GetComponent<LineRenderer>().sharedMaterial != null)
            //{
            //    var material = gameObject.GetComponent<LineRenderer>().sharedMaterial;
            //    var data = new TVEElementDrawerData(ElementType.Undefined, ElementLayer.Any, RendererType.Line, gameObject, new Mesh(), gameObject.GetComponent<Renderer>());
            //    data.mesh.name = "Line";

            //    AddElementByType(material, data);
            //}
        }

        void AddElement()
        {
            var renderDataSet = TVEManager.Instance.globalVolume.renderDataSet;
            var renderElements = TVEManager.Instance.globalVolume.renderElements;

            for (int i = 0; i < renderDataSet.Count; i++)
            {
                var renderData = renderDataSet[i];

                if (renderData == null)
                {
                    continue;
                }

                if (sharedMaterial.HasProperty(renderData.materialFilter))
                {
                    if (sharedMaterial.HasProperty(elementLayerMask))
                    {
                        var bitmask = sharedMaterial.GetInt(elementLayerMask);
                        var maxLayer = 0;
                        elementData.layers = new List<int>(9);

                        for (int m = 0; m < 9; m++)
                        {
                            if (((1 << m) & bitmask) != 0)
                            {
                                elementData.layers.Add(1);
                                maxLayer = m;
                            }
                            else
                            {
                                elementData.layers.Add(0);
                            }
                        }

                        if (maxLayer > renderData.bufferSize)
                        {
                            renderData.bufferSize = maxLayer;
                            renderData.isUpdated = true;
                        }
                    }
                    else
                    {
                        elementData.layers = new List<int>(9);
                        elementData.layers.Add(1);

                        for (int m = 1; m < 9; m++)
                        {
                            elementData.layers.Add(0);
                        }
                    }

                    if (!renderElements.Contains(elementData))
                    {
                        TVEManager.Instance.globalVolume.renderElements.Add(elementData);
                    }
                }
            }
        }

        void RemoveElementFromVolume()
        {
            if (TVEManager.Instance == null)
                return;

            var renderElements = TVEManager.Instance.globalVolume.renderElements;

            if (renderElements != null)
            {
                for (int i = 0; i < renderElements.Count; i++)
                {
                    if (renderElements[i].element == gameObject)
                    {
                        renderElements.RemoveAt(i);
                    }
                }
            }

            var renderInstanced = TVEManager.Instance.globalVolume.renderInstanced;

            if (renderInstanced != null)
            {
                for (int i = 0; i < renderInstanced.Count; i++)
                {
                    for (int j = 0; j < renderInstanced[i].renderers.Count; j++)
                    {
                        if (renderInstanced[i].renderers[j] == meshRenderer)
                        {
                            renderInstanced[i].renderers.RemoveAt(j);
                        }
                    }
                }
            }
        }

        void SetElementVisibility(Renderer renderer)
        {
            if (TVEManager.Instance.globalVolume.elementsVisibility == TVEGlobalVolume.ElementsVisibility.AlwaysHidden)
            {
#if UNITY_2019_3_OR_NEWER
                renderer.forceRenderingOff = true;
#else
                renderer.enabled = false;
#endif
            }

            if (TVEManager.Instance.globalVolume.elementsVisibility == TVEGlobalVolume.ElementsVisibility.AlwaysVisible)
            {
#if UNITY_2019_3_OR_NEWER
                renderer.forceRenderingOff = false;
#else
                renderer.enabled = true;
#endif
            }

            if (TVEManager.Instance.globalVolume.elementsVisibility == TVEGlobalVolume.ElementsVisibility.HiddenAtRuntime)
            {
                if (Application.isPlaying)
                {
#if UNITY_2019_3_OR_NEWER
                    renderer.forceRenderingOff = true;
#else
                    renderer.enabled = false;
#endif
                }
                else
                {
#if UNITY_2019_3_OR_NEWER
                    renderer.forceRenderingOff = false;
#else
                    renderer.enabled = true;
#endif
                }
            }
        }

        void OnDrawGizmosSelected()
        {
            DrawGizmos(true);
        }

        void OnDrawGizmos()
        {
            DrawGizmos(false);
        }

        void DrawGizmos(bool selected)
        {
            if (TVEManager.Instance == null || elementData == null)
            {
                return;
            }

            var sin = Mathf.SmoothStep(0, 1, Mathf.Sin(Time.realtimeSinceStartup * 4) * 0.5f + 0.5f);

            var genericColor = new Color(0.0f, 0.0f, 0.0f, 0.1f);
            var invalidColor = new Color(1.0f, 0.0f, 0.0f, 0.1f);

            if (selected)
            {
                genericColor = new Color(0.890f, 0.745f, 0.309f, 1.0f);
                invalidColor = new Color(1.0f, 0.05f, 0.05f, sin);
            }

            var renderDataSet = TVEManager.Instance.globalVolume.renderDataSet;
            Bounds elementBounds = elementData.renderer.bounds;

            for (int i = 0; i < renderDataSet.Count; i++)
            {
                var renderData = renderDataSet[i];

                if (renderData == null)
                {
                    continue;
                }

                if (sharedMaterial.HasProperty(renderData.materialFilter))
                {
                    if (IsElementInVolume(renderData.volumePosition, renderData.volumeScale, elementBounds))
                    {
                        Gizmos.color = genericColor;
                    }
                    else
                    {
                        Gizmos.color = invalidColor;
                    }
                }
            }

            if (isSelected)
            {
                if (useRaycastFading > 0)
                {
                    Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - raycastEnd, transform.position.z));
                }

                if (elementData.type == RendererType.Mesh)
                {
                    if (sharedMaterial.shader.name.Contains("Control") || sharedMaterial.shader.name.Contains("Direction") || sharedMaterial.shader.name.Contains("Turbulence"))
                    {
                        Gizmos.DrawLine(new Vector3(transform.position.x + transform.forward.x * transform.lossyScale.x * 0.1f, transform.position.y, transform.position.z + transform.forward.z * transform.lossyScale.x * 0.1f), new Vector3(transform.position.x + transform.forward.x * transform.lossyScale.x * 0.3f, transform.position.y, transform.position.z + transform.forward.z * transform.lossyScale.x * 0.3f));
                    }
                }
            }

            Bounds gizmoBounds;

            if (elementData.type == RendererType.Mesh)
            {
                gizmoBounds = elementData.mesh.bounds;
                Gizmos.matrix = transform.localToWorldMatrix;
            }
            else
            {
                gizmoBounds = elementData.renderer.bounds;
            }

            Gizmos.DrawWireCube(gizmoBounds.center, gizmoBounds.size);
        }

        //void DrawTextGizmo()
        //{
        //    var label = "Outside Volume";

        //    var styleLabel = new GUIStyle(EditorStyles.whiteLabel)
        //    {
        //        richText = true,
        //        alignment = UnityEngine.TextAnchor.MiddleCenter,
        //        fontSize = 9,
        //    };

        //    Handles.BeginGUI();
        //    var size = styleLabel.CalcSize(new GUIContent(label));
        //    var pos2D = HandleUtility.WorldToGUIPoint(transform.position);

        //    GUI.color = Color.red;
        //    GUI.Label(new Rect(pos2D.x - (size.x / 2), pos2D.y - 48, size.x, size.y), label, styleLabel);

        //    Handles.EndGUI();
        //}

        bool IsElementInVolume(string globalPosition, string globalScale, Bounds elementBounds)
        {
            var inVolume = false;

            var position = Shader.GetGlobalVector(globalPosition);
            var scale = Shader.GetGlobalVector(globalScale);

            var volumeBounds = new Bounds(position, scale);

            if (volumeBounds.Intersects(elementBounds))
            {
                inVolume = true;
            }

            return inVolume;
        }
    }
}
