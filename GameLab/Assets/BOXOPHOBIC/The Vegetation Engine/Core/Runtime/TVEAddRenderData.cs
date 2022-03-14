using UnityEngine;

namespace TheVegetationEngine
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
    [AddComponentMenu("BOXOPHOBIC/The Vegetation Engine/TVE Add Render Data")]
#endif
    public class TVEAddRenderData : StyledMonoBehaviour
    {
        public TVERenderData renderData;
        public Color renderColor;

        void OnEnable()
        {
            if (TVEManager.Instance == null)
            {
                return;
            }

            var renderDataSet = TVEManager.Instance.globalVolume.renderDataSet;

            if (!renderDataSet.Contains(renderData))
            {
                TVEManager.Instance.globalVolume.renderDataSet.Add(renderData);
            }
        }

        void Update()
        {
            if (QualitySettings.activeColorSpace == ColorSpace.Linear)
            {
                Shader.SetGlobalColor(renderData.texParams, renderColor.linear);
            }
            else
            {
                Shader.SetGlobalColor(renderData.texParams, renderColor);
            }
        }
    }
}
