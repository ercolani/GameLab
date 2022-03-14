// Cristian Pop - https://boxophobic.com/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace TheVegetationEngine
{
    public enum BufferType
    {
        Undefined = -1,
        Colors = 10,
        Extras = 20,
        Motion = 30,
        Vertex = 40,
        Custom = 100,
    }

    public enum RendererType
    {
        Mesh = 0,
        Particle = 1,
        Trail = 2,
        //Line = 3,
    }

    public enum PropertyType
    {
        Texture = 0,
        Vector = 1,
        Value = 2,
    }

    [System.Serializable]
    public class TVEElementMaterialData
    {
        public Shader shader;
        public List<TVEElementPropertyData> props;

        public TVEElementMaterialData()
        {

        }
    }

    [System.Serializable]
    public class TVEElementPropertyData
    {
        public PropertyType type;
        public string prop;
        public Texture texture;
        public Vector4 vector;
        public float value;

        public TVEElementPropertyData()
        {

        }
    }

    [System.Serializable]
    public class TVEElementDefaultData
    {
        //public string bufferFilter;
        public List<int> layers;
        public GameObject element;
        public Mesh mesh;
        public RendererType type;
        public Renderer renderer;
        public float fadeValue;

        public TVEElementDefaultData()
        {

        }
    }

    [System.Serializable]
    public class TVEElementInstancedData
    {
        //public string bufferFilter;
        public List<int> layers;
        public Material material;
        public Mesh mesh;
        public List<Renderer> renderers;

        public TVEElementInstancedData()
        {

        }
    }

    [System.Serializable]
    public class TVERenderData
    {
        public bool isEnabled = true;
        public bool isFollowing = false;
        [HideInInspector]
        public bool isUpdated = false;

        public RenderTextureFormat texFormat = RenderTextureFormat.Default;
        public int texResolution = 1024;
        public string texName = "TVE_CustomTex";
        public string texParams = "TVE_CustomParams";
        public string texCoord = "TVE_CustomCoord";
        public string texUsage = "TVE_CustomUsage";
        public string volumePosition = "TVE_CustomPosition";
        public string volumeScale = "TVE_CustomScale";
        public string materialFilter = "_IsCustomElement";
        
        [System.NonSerialized]
        public int bufferSize = 0;
        [System.NonSerialized]
        public float[] bufferUsage;
        [System.NonSerialized]
        public RenderTexture texObject;
        [System.NonSerialized]
        public CommandBuffer[] commandBuffers;

        public TVERenderData()
        {

        }
    }
}