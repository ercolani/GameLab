// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BOXOPHOBIC/The Vegetation Engine/Vegetation/Leaf Subsurface Lit"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[StyledCategory(Render Settings, 5, 10)]_RenderingCat("[ Rendering Cat ]", Float) = 0
		[Enum(Opaque,0,Transparent,1)]_RenderMode("Render Mode", Float) = 0
		[Enum(Off,0,On,1)]_RenderZWrite("Render ZWrite", Float) = 1
		[Enum(Both,0,Back,1,Front,2)]_RenderCull("Render Faces", Float) = 0
		[Enum(Flip,0,Mirror,1,Same,2)]_RenderNormals("Render Normals", Float) = 0
		[HideInInspector]_RenderQueue("Render Queue", Float) = 0
		[HideInInspector]_RenderPriority("Render Priority", Float) = 0
		[StyledSpace(10)]_ReceiveSpace("# Receive Space", Float) = 0
		[Enum(Off,0,On,1)]_RenderSpecular("Receive Specular", Float) = 1
		[Enum(Off,0,On,1)]_RenderDecals("Receive Decals", Float) = 0
		[Enum(Off,0,On,1)]_RenderSSR("Receive SSR/SSGI", Float) = 0
		[Enum(Off,0,On,1)][Space(10)]_RenderClip("Alpha Clipping", Float) = 1
		_Cutoff("Alpha Treshold", Range( 0 , 1)) = 0.5
		[StyledSpace(10)]_FadeSpace("# Fade Space", Float) = 0
		_FadeGlancingValue("Fade by Glancing Angle", Range( 0 , 1)) = 0
		_FadeCameraValue("Fade by Camera Distance", Range( 0 , 1)) = 1
		[StyledCategory(Global Settings)]_GlobalCat("[ Global Cat ]", Float) = 0
		[StyledEnum(TVELayers, Default 0 Layer_1 1 Layer_2 2 Layer_3 3 Layer_4 4 Layer_5 5 Layer_6 6 Layer_7 7 Layer_8 8, 0, 0)]_LayerColorsValue("Layer Colors", Float) = 0
		[StyledEnum(TVELayers, Default 0 Layer_1 1 Layer_2 2 Layer_3 3 Layer_4 4 Layer_5 5 Layer_6 6 Layer_7 7 Layer_8 8, 0, 0)]_LayerExtrasValue("Layer Extras", Float) = 0
		[StyledEnum(TVELayers, Default 0 Layer_1 1 Layer_2 2 Layer_3 3 Layer_4 4 Layer_5 5 Layer_6 6 Layer_7 7 Layer_8 8, 0, 0)]_LayerMotionValue("Layer Motion", Float) = 0
		[StyledEnum(TVELayers, Default 0 Layer_1 1 Layer_2 2 Layer_3 3 Layer_4 4 Layer_5 5 Layer_6 6 Layer_7 7 Layer_8 8, 0, 0)]_LayerVertexValue("Layer Vertex", Float) = 0
		[StyledSpace(10)]_LayersSpace("# Layers Space", Float) = 0
		[StyledMessage(Info, Procedural Variation in use. The Variation might not work as expected when switching from one LOD to another., _VertexVariationMode, 1 , 0, 10)]_VariationGlobalsMessage("# Variation Globals Message", Float) = 0
		_GlobalColors("Global Color", Range( 0 , 1)) = 1
		_GlobalAlpha("Global Alpha", Range( 0 , 1)) = 1
		_GlobalOverlay("Global Overlay", Range( 0 , 1)) = 1
		_GlobalWetness("Global Wetness", Range( 0 , 1)) = 1
		_GlobalEmissive("Global Emissive", Range( 0 , 1)) = 1
		_GlobalSize("Global Size Fade", Range( 0 , 1)) = 1
		[StyledRemapSlider(_ColorsMaskMinValue, _ColorsMaskMaxValue, 0, 1, 10, 0)]_ColorsMaskRemap("Color Mask", Vector) = (0,0,0,0)
		[HideInInspector]_ColorsMaskMinValue("Color Mask Min Value", Range( 0 , 1)) = 0
		[HideInInspector]_ColorsMaskMaxValue("Color Mask Max Value", Range( 0 , 1)) = 0
		_ColorsVariationValue("Color Variation", Range( 0 , 1)) = 0
		[StyledRemapSlider(_OverlayMaskMinValue, _OverlayMaskMaxValue, 0, 1, 10, 0)]_OverlayMaskRemap("Overlay Mask", Vector) = (0,0,0,0)
		[HideInInspector]_OverlayMaskMinValue("Overlay Mask Min Value", Range( 0 , 1)) = 0.45
		[HideInInspector]_OverlayMaskMaxValue("Overlay Mask Max Value", Range( 0 , 1)) = 0.55
		_OverlayVariationValue("Overlay Variation", Range( 0 , 1)) = 0
		[StyledRemapSlider(_AlphaMaskMinValue, _AlphaMaskMaxValue, 0, 1, 10, 0)]_AlphaMaskRemap("Alpha Mask", Vector) = (0,0,0,0)
		[StyledCategory(Main Settings)]_MainCat("[ Main Cat ]", Float) = 0
		[NoScaleOffset][StyledTextureSingleLine]_MainAlbedoTex("Main Albedo", 2D) = "white" {}
		[NoScaleOffset][StyledTextureSingleLine]_MainNormalTex("Main Normal", 2D) = "bump" {}
		[NoScaleOffset][StyledTextureSingleLine]_MainMaskTex("Main Mask", 2D) = "white" {}
		[Space(10)][StyledVector(9)]_MainUVs("Main UVs", Vector) = (1,1,0,0)
		[HDR]_MainColor("Main Color", Color) = (1,1,1,1)
		_MainNormalValue("Main Normal", Range( -8 , 8)) = 1
		_MainOcclusionValue("Main Occlusion", Range( 0 , 1)) = 1
		_MainSmoothnessValue("Main Smoothness", Range( 0 , 1)) = 1
		[StyledRemapSlider(_LeavesMaskMinValue, _LeavesMaskMaxValue, 0, 1)]_LeavesMaskRemap("Main Leaves Mask", Vector) = (0,0,0,2)
		[StyledCategory(Detail Settings)]_DetailCat("[ Detail Cat ]", Float) = 0
		[Enum(Off,0,On,1)]_DetailMode("Detail Mode", Float) = 0
		[Enum(Overlay,0,Replace,1)]_DetailBlendMode("Detail Blend", Float) = 1
		[Enum(Vertex Blue,0,Projection,1)]_DetailTypeMode("Detail Type", Float) = 0
		[StyledRemapSlider(_DetailBlendMinValue, _DetailBlendMaxValue,0,1)]_DetailBlendRemap("Detail Blending", Vector) = (0,0,0,0)
		[StyledCategory(Occlusion Settings)]_OcclusionCat("[ Occlusion Cat ]", Float) = 0
		[HDR]_VertexOcclusionColor("Vertex Occlusion Color", Color) = (1,1,1,1)
		[StyledRemapSlider(_VertexOcclusionMinValue, _VertexOcclusionMaxValue, 0, 1)]_VertexOcclusionRemap("Vertex Occlusion Mask", Vector) = (0,0,0,0)
		[HideInInspector]_VertexOcclusionMinValue("Vertex Occlusion Min Value", Range( 0 , 1)) = 0
		[HideInInspector]_VertexOcclusionMaxValue("Vertex Occlusion Max Value", Range( 0 , 1)) = 1
		[StyledCategory(Subsurface Settings)]_SubsurfaceCat("[ Subsurface Cat ]", Float) = 0
		_SubsurfaceValue("Subsurface Intensity", Range( 0 , 1)) = 1
		[HDR]_SubsurfaceColor("Subsurface Color", Color) = (0.4,0.4,0.1,1)
		[StyledRemapSlider(_SubsurfaceMaskMinValue, _SubsurfaceMaskMaxValue,0,1)]_SubsurfaceMaskRemap("Subsurface Mask", Vector) = (0,0,0,0)
		[HideInInspector]_SubsurfaceMaskMinValue("Subsurface Mask Min Value", Range( 0 , 1)) = 0
		[HideInInspector]_SubsurfaceMaskMaxValue("Subsurface Mask Max Value", Range( 0 , 1)) = 1
		[Space(10)][DiffusionProfile]_SubsurfaceDiffusion("Subsurface Diffusion", Float) = 0
		[HideInInspector]_SubsurfaceDiffusion_Asset("Subsurface Diffusion", Vector) = (0,0,0,0)
		[HideInInspector][Space(10)][ASEDiffusionProfile(_SubsurfaceDiffusion)]_SubsurfaceDiffusion_asset("Subsurface Diffusion", Vector) = (0,0,0,0)
		[Space(10)]_MainLightScatteringValue("Subsurface Scattering Intensity", Range( 0 , 16)) = 8
		_MainLightNormalValue("Subsurface Scattering Normal", Range( 0 , 1)) = 0.5
		_MainLightAngleValue("Subsurface Scattering Angle", Range( 0 , 16)) = 8
		[Space(10)]_TranslucencyIntensityValue("Translucency Intensity", Range( 0 , 50)) = 1
		_TranslucencyNormalValue("Translucency Normal", Range( 0 , 1)) = 0.1
		_TranslucencyScatteringValue("Translucency Scattering", Range( 1 , 50)) = 2
		_TranslucencyDirectValue("Translucency Direct", Range( 0 , 1)) = 1
		_TranslucencyAmbientValue("Translucency Ambient", Range( 0 , 1)) = 0.2
		_TranslucencyShadowValue("Translucency Shadow", Range( 0 , 1)) = 1
		[StyledMessage(Warning,  Translucency is not supported in HDRP. Diffusion Profiles will be used instead., 10, 5)]_TranslucencyHDMessage("# Translucency HD Message", Float) = 0
		[StyledCategory(Gradient Settings)]_GradientCat("[ Gradient Cat ]", Float) = 0
		[HDR]_GradientColorOne("Gradient Color One", Color) = (1,1,1,1)
		[HDR]_GradientColorTwo("Gradient Color Two", Color) = (1,1,1,1)
		[StyledRemapSlider(_GradientMinValue, _GradientMaxValue, 0, 1)]_GradientMaskRemap("Gradient Mask", Vector) = (0,0,0,0)
		[HideInInspector]_GradientMinValue("Gradient Mask Min", Range( 0 , 1)) = 0
		[HideInInspector]_GradientMaxValue("Gradient Mask Max ", Range( 0 , 1)) = 1
		[StyledCategory(Noise Settings)]_NoiseCat("[ Noise Cat ]", Float) = 0
		[HDR]_NoiseColorOne("Noise Color One", Color) = (1,1,1,1)
		[HDR]_NoiseColorTwo("Noise Color Two", Color) = (1,1,1,1)
		[StyledRemapSlider(_NoiseMinValue, _NoiseMaxValue, 0, 1)]_NoiseMaskRemap("Noise Mask", Vector) = (0,0,0,0)
		[HideInInspector]_NoiseMinValue("Noise Mask Min", Range( 0 , 1)) = 0
		[HideInInspector]_NoiseMaxValue("Noise Mask Max ", Range( 0 , 1)) = 1
		_NoiseScaleValue("Noise Scale", Range( 0 , 1)) = 0.01
		[StyledCategory(Emissive Settings)]_EmissiveCat("[ Emissive Cat]", Float) = 0
		[NoScaleOffset][StyledTextureSingleLine]_EmissiveTex("Emissive Texture", 2D) = "white" {}
		[Space(10)][StyledVector(9)]_EmissiveUVs("Emissive UVs", Vector) = (1,1,0,0)
		[Enum(None,0,Any,10,Baked,20,Realtime,30)]_EmissiveFlagMode("Emissive Baking", Float) = 0
		[HDR]_EmissiveColor("Emissive Color", Color) = (0,0,0,0)
		[StyledEmissiveIntensity]_EmissiveIntensityParams("Emissive Intensity", Vector) = (1,1,1,0)
		[StyledCategory(Perspective Settings)]_PerspectiveCat("[ Perspective Cat ]", Float) = 0
		[StyledCategory(Size Fade Settings)]_SizeFadeCat("[ Size Fade Cat ]", Float) = 0
		[StyledMessage(Info, The Size Fade feature is recommended to be used to fade out vegetation at a distance in combination with the LOD Groups or with a 3rd party culling system., _SizeFadeMode, 1, 0, 10)]_SizeFadeMessage("# Size Fade Message", Float) = 0
		[Enum(Off,0,On,1)]_SizeFadeMode("Size Fade Mode", Float) = 0
		_SizeFadeStartValue("Size Fade Start", Range( 0 , 1000)) = 200
		_SizeFadeEndValue("Size Fade End", Range( 0 , 1000)) = 300
		[StyledCategory(Motion Settings)]_MotionCat("[ Motion Cat ]", Float) = 0
		[StyledMessage(Info, Procedural variation in use. Use the Scale settings if the Variation is breaking the bending and rolling animation., _VertexVariationMode, 1 , 0, 10)]_VariationMotionMessage("# Variation Motion Message", Float) = 0
		[StyledSpace(10)]_MotionSpace("# Motion Space", Float) = 0
		_MotionAmplitude_10("Primary Bending", Range( 0 , 1)) = 0.05
		[IntRange]_MotionSpeed_10("Primary Speed", Range( 0 , 40)) = 2
		_MotionScale_10("Primary Scale", Range( 0 , 20)) = 0
		_MotionVariation_10("Primary Variation", Range( 0 , 20)) = 0
		[Space(10)]_MotionAmplitude_20("Second Rolling", Range( 0 , 1)) = 0.1
		_MotionAmplitude_21("Second Vertical", Range( 0 , 1)) = 0
		_MotionAmplitude_22("Second Squash", Range( 0 , 1)) = 0
		[IntRange]_MotionSpeed_20("Second Speed", Range( 0 , 40)) = 4
		_MotionScale_20("Second Scale", Range( 0 , 60)) = 0
		_MotionVariation_20("Second Variation", Range( 0 , 60)) = 5
		[Space(10)]_MotionAmplitude_32("Details Flutter", Range( 0 , 1)) = 0.2
		[IntRange]_MotionSpeed_32("Details Speed", Range( 0 , 40)) = 4
		_MotionScale_32("Details Scale", Range( 0 , 20)) = 0
		_MotionVariation_32("Details Variation", Range( 0 , 20)) = 2
		[Space(10)]_InteractionAmplitude("Interaction Amplitude", Range( 0 , 10)) = 1
		_InteractionMaskValue("Interaction Use Mask", Range( 0 , 1)) = 0
		[ASEEnd][Space(10)][StyledToggle]_MotionValue_20("Use Second Motion Settings", Range( 0 , 1)) = 1
		[HideInInspector]_MaxBoundsInfo("_MaxBoundsInfo", Vector) = (1,1,1,1)
		[HideInInspector]_render_normals_options("_render_normals_options", Vector) = (1,1,1,0)
		[HideInInspector]_Color("Legacy Color", Color) = (0,0,0,0)
		[HideInInspector]_MainTex("Legacy MainTex", 2D) = "white" {}
		[HideInInspector]_BumpMap("Legacy BumpMap", 2D) = "white" {}
		[HideInInspector]_LayerReactValue("Legacy Layer React", Float) = 0
		[HideInInspector]_VertexRollingMode("Legacy Vertex Rolling", Float) = 1
		[HideInInspector][StyledToggle]_VertexDataMode("_VertexDataMode", Float) = 0
		[HideInInspector][StyledToggle]_VertexDynamicMode("_VertexDynamicMode", Float) = 0
		[HideInInspector]_VertexVariationMode("_VertexVariationMode", Float) = 0
		[HideInInspector]_VertexMasksMode("_VertexMasksMode", Float) = 0
		[HideInInspector]_IsTVEShader("_IsTVEShader", Float) = 1
		[HideInInspector]_IsVersion("_IsVersion", Float) = 620
		[HideInInspector]_HasEmissive("_HasEmissive", Float) = 0
		[HideInInspector]_HasGradient("_HasGradient", Float) = 0
		[HideInInspector]_HasOcclusion("_HasOcclusion", Float) = 0
		[HideInInspector]_subsurface_shadow("_subsurface_shadow", Float) = 1
		[HideInInspector]_IsLeafShader("_IsLeafShader", Float) = 1
		[HideInInspector]_IsSubsurfaceShader("_IsSubsurfaceShader", Float) = 1
		[HideInInspector]_render_cull("_render_cull", Float) = 0
		[HideInInspector]_render_src("_render_src", Float) = 5
		[HideInInspector]_render_dst("_render_dst", Float) = 10
		[HideInInspector]_render_zw("_render_zw", Float) = 1

		//_TransmissionShadow( "Transmission Shadow", Range( 0, 1 ) ) = 0.5
		//_TransStrength( "Trans Strength", Range( 0, 50 ) ) = 1
		//_TransNormal( "Trans Normal Distortion", Range( 0, 1 ) ) = 0.5
		//_TransScattering( "Trans Scattering", Range( 1, 50 ) ) = 2
		//_TransDirect( "Trans Direct", Range( 0, 1 ) ) = 0.9
		//_TransAmbient( "Trans Ambient", Range( 0, 1 ) ) = 0.1
		//_TransShadow( "Trans Shadow", Range( 0, 1 ) ) = 0.5
		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry" }
		Cull [_render_cull]
		AlphaToMask Off
		HLSLINCLUDE
		#pragma target 4.0

		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}
		
		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS
		ENDHLSL

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }
			
			Blend [_render_src] [_render_dst], One Zero
			ZWrite [_render_zw]
			ZTest LEqual
			Offset 0,0
			ColorMask RGBA
			

			HLSLPROGRAM
			#define _SPECULAR_SETUP 1
			#define _NORMAL_DROPOFF_TS 1
			#define _TRANSMISSION_ASE 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#pragma multi_compile _ DOTS_INSTANCING_ON
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _EMISSION
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70403

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x 

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
			
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_FORWARD

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			
			#if ASE_SRP_VERSION <= 70108
			#define REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
			#endif

			#if defined(UNITY_INSTANCING_ENABLED) && defined(_TERRAIN_INSTANCED_PERPIXEL_NORMAL)
			    #define ENABLE_TERRAIN_PERPIXEL_NORMAL
			#endif

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR
			#define ASE_NEEDS_FRAG_WORLD_TANGENT
			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_BITANGENT
			#define ASE_NEEDS_FRAG_COLOR
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#pragma shader_feature_local TVE_ALPHA_CLIP
			#pragma shader_feature_local TVE_VERTEX_DATA_BATCHED
			//TVE Pipeline Defines
			#define THE_VEGETATION_ENGINE
			#define TVE_IS_UNIVERSAL_PIPELINE
			//TVE Shader Type Defines
			#define TVE_IS_VEGETATION_SHADER
			//TVE Injection Defines
			//SHADER INJECTION POINT BEGIN
			//SHADER INJECTION POINT END


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord : TEXCOORD0;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 lightmapUVOrVertexSH : TEXCOORD0;
				half4 fogFactorAndVertexLight : TEXCOORD1;
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord : TEXCOORD2;
				#endif
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 screenPos : TEXCOORD6;
				#endif
				float4 ase_texcoord7 : TEXCOORD7;
				float4 ase_texcoord8 : TEXCOORD8;
				float4 ase_color : COLOR;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord9 : TEXCOORD9;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			half4 _DetailBlendRemap;
			half4 _OverlayMaskRemap;
			half4 _ColorsMaskRemap;
			half4 _SubsurfaceMaskRemap;
			half4 _LeavesMaskRemap;
			half4 _NoiseColorOne;
			half4 _SubsurfaceColor;
			half4 _AlphaMaskRemap;
			half4 _VertexOcclusionRemap;
			half4 _VertexOcclusionColor;
			half4 _GradientColorTwo;
			float4 _NoiseMaskRemap;
			half4 _GradientColorOne;
			half4 _MainColor;
			half4 _NoiseColorTwo;
			float4 _GradientMaskRemap;
			float4 _SubsurfaceDiffusion_asset;
			float4 _MaxBoundsInfo;
			half4 _EmissiveColor;
			half4 _MainUVs;
			float4 _SubsurfaceDiffusion_Asset;
			half4 _EmissiveUVs;
			float4 _EmissiveIntensityParams;
			float4 _Color;
			half3 _render_normals_options;
			half _SizeFadeStartValue;
			half _SizeFadeMode;
			half _GradientMinValue;
			half _GradientMaxValue;
			half _SizeFadeEndValue;
			half _VertexDataMode;
			half _LayerVertexValue;
			half _MotionAmplitude_32;
			float _MotionVariation_32;
			float _MotionSpeed_32;
			float _MotionScale_32;
			half _MotionAmplitude_22;
			half _MotionAmplitude_21;
			half _MotionScale_20;
			half _MotionSpeed_20;
			half _MotionValue_20;
			half _MotionVariation_20;
			half _MotionAmplitude_20;
			half _GlobalSize;
			half _render_cull;
			half _LayerColorsValue;
			half _NoiseMinValue;
			half _FadeGlancingValue;
			half _MainOcclusionValue;
			half _GlobalWetness;
			half _MainSmoothnessValue;
			half _RenderSpecular;
			half _GlobalEmissive;
			half _VertexOcclusionMaxValue;
			half _VertexOcclusionMinValue;
			half _OverlayMaskMaxValue;
			half _OverlayMaskMinValue;
			half _OverlayVariationValue;
			half _LayerExtrasValue;
			half _NoiseScaleValue;
			half _GlobalOverlay;
			half _MainNormalValue;
			half _MainLightScatteringValue;
			half _MainLightAngleValue;
			half _SubsurfaceMaskMaxValue;
			half _SubsurfaceMaskMinValue;
			half _SubsurfaceValue;
			half _ColorsMaskMaxValue;
			half _ColorsMaskMinValue;
			half _ColorsVariationValue;
			half _GlobalColors;
			half _InteractionMaskValue;
			half _NoiseMaxValue;
			half _MainLightNormalValue;
			half _InteractionAmplitude;
			half _IsSubsurfaceShader;
			half _VertexDynamicMode;
			half _SizeFadeCat;
			float _SubsurfaceDiffusion;
			half _VariationGlobalsMessage;
			half _RenderCull;
			half _HasEmissive;
			half _FadeSpace;
			half _MotionCat;
			half _IsVersion;
			half _DetailCat;
			half _HasGradient;
			half _SubsurfaceCat;
			half _VertexMasksMode;
			half _PerspectiveCat;
			half _TranslucencyNormalValue;
			half _TranslucencyHDMessage;
			half _RenderMode;
			half _RenderDecals;
			half _TranslucencyAmbientValue;
			half _RenderingCat;
			half _DetailBlendMode;
			half _GradientCat;
			half _DetailTypeMode;
			half _RenderQueue;
			half _TranslucencyIntensityValue;
			half _RenderPriority;
			half _EmissiveCat;
			half _subsurface_shadow;
			half _render_src;
			half _VariationMotionMessage;
			float _MotionScale_10;
			half _TranslucencyScatteringValue;
			half _DetailMode;
			half _MotionVariation_10;
			float _MotionSpeed_10;
			half _MotionAmplitude_10;
			half _LayerMotionValue;
			half _FadeCameraValue;
			half _IsLeafShader;
			half _render_dst;
			half _render_zw;
			half _RenderClip;
			half _VertexRollingMode;
			half _VertexVariationMode;
			half _SizeFadeMessage;
			half _Cutoff;
			half _MotionSpace;
			half _TranslucencyShadowValue;
			half _LayersSpace;
			half _LayerReactValue;
			half _MainCat;
			half _ReceiveSpace;
			half _RenderSSR;
			half _IsTVEShader;
			half _EmissiveFlagMode;
			half _RenderZWrite;
			half _HasOcclusion;
			half _OcclusionCat;
			half _GlobalCat;
			half _RenderNormals;
			half _TranslucencyDirectValue;
			half _NoiseCat;
			half _GlobalAlpha;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			half TVE_Enabled;
			sampler2D _BumpMap;
			sampler2D _MainTex;
			half4 TVE_MotionParams;
			TEXTURE2D_ARRAY(TVE_MotionTex);
			half4 TVE_MotionCoords;
			SAMPLER(samplerTVE_MotionTex);
			float TVE_MotionUsage[10];
			half4 TVE_MotionParamsMin;
			half4 TVE_MotionParamsMax;
			sampler2D TVE_NoiseTex;
			half4 TVE_NoiseParams;
			half TVE_MotionFadeEnd;
			half TVE_MotionFadeStart;
			half4 TVE_VertexParams;
			TEXTURE2D_ARRAY(TVE_VertexTex);
			half4 TVE_VertexCoords;
			SAMPLER(samplerTVE_VertexTex);
			float TVE_VertexUsage[10];
			half TVE_DistanceFadeBias;
			half _DisableSRPBatcher;
			sampler3D TVE_WorldTex3D;
			sampler2D _MainAlbedoTex;
			half4 TVE_ColorsParams;
			TEXTURE2D_ARRAY(TVE_ColorsTex);
			half4 TVE_ColorsCoords;
			SAMPLER(samplerTVE_ColorsTex);
			float TVE_ColorsUsage[10];
			sampler2D _MainMaskTex;
			half4 TVE_MainLightParams;
			half3 TVE_MainLightDirection;
			sampler2D _MainNormalTex;
			half4 TVE_OverlayColor;
			half4 TVE_ExtrasParams;
			TEXTURE2D_ARRAY(TVE_ExtrasTex);
			half4 TVE_ExtrasCoords;
			SAMPLER(samplerTVE_ExtrasTex);
			float TVE_ExtrasUsage[10];
			sampler2D _EmissiveTex;
			half TVE_OverlaySmoothness;
			half TVE_CameraFadeStart;
			half TVE_CameraFadeEnd;
			sampler3D TVE_ScreenTex3D;
			half TVE_ScreenTexCoord;


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 VertexPosition3588_g61524 = v.vertex.xyz;
				half3 Mesh_PivotsOS2291_g61524 = half3(0,0,0);
				float3 temp_output_2283_0_g61524 = ( VertexPosition3588_g61524 - Mesh_PivotsOS2291_g61524 );
				half3 VertexPos40_g62096 = temp_output_2283_0_g61524;
				float3 appendResult74_g62096 = (float3(VertexPos40_g62096.x , 0.0 , 0.0));
				half3 VertexPosRotationAxis50_g62096 = appendResult74_g62096;
				float3 break84_g62096 = VertexPos40_g62096;
				float3 appendResult81_g62096 = (float3(0.0 , break84_g62096.y , break84_g62096.z));
				half3 VertexPosOtherAxis82_g62096 = appendResult81_g62096;
				float4 temp_output_91_19_g62127 = TVE_MotionCoords;
				float4x4 break19_g62080 = GetObjectToWorldMatrix();
				float3 appendResult20_g62080 = (float3(break19_g62080[ 0 ][ 3 ] , break19_g62080[ 1 ][ 3 ] , break19_g62080[ 2 ][ 3 ]));
				half3 ObjectData20_g62081 = appendResult20_g62080;
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				half3 WorldData19_g62081 = ase_worldPos;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g62081 = WorldData19_g62081;
				#else
				float3 staticSwitch14_g62081 = ObjectData20_g62081;
				#endif
				float3 temp_output_114_0_g62080 = staticSwitch14_g62081;
				float3 vertexToFrag4224_g61524 = temp_output_114_0_g62080;
				half3 ObjectData20_g62075 = vertexToFrag4224_g61524;
				float3 vertexToFrag3890_g61524 = ase_worldPos;
				half3 WorldData19_g62075 = vertexToFrag3890_g61524;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g62075 = WorldData19_g62075;
				#else
				float3 staticSwitch14_g62075 = ObjectData20_g62075;
				#endif
				float3 ObjectPosition4223_g61524 = staticSwitch14_g62075;
				float3 Position83_g62127 = ObjectPosition4223_g61524;
				float temp_output_84_0_g62127 = _LayerMotionValue;
				float4 lerpResult87_g62127 = lerp( TVE_MotionParams , SAMPLE_TEXTURE2D_ARRAY_LOD( TVE_MotionTex, samplerTVE_MotionTex, ( (temp_output_91_19_g62127).zw + ( (temp_output_91_19_g62127).xy * (Position83_g62127).xz ) ),temp_output_84_0_g62127, 0.0 ) , TVE_MotionUsage[(int)temp_output_84_0_g62127]);
				half4 Global_Motion_Params3909_g61524 = lerpResult87_g62127;
				float4 break322_g62099 = Global_Motion_Params3909_g61524;
				float3 appendResult397_g62099 = (float3(break322_g62099.x , 0.0 , break322_g62099.y));
				float3 temp_output_398_0_g62099 = (appendResult397_g62099*2.0 + -1.0);
				float3 ase_parentObjectScale = ( 1.0 / float3( length( GetWorldToObjectMatrix()[ 0 ].xyz ), length( GetWorldToObjectMatrix()[ 1 ].xyz ), length( GetWorldToObjectMatrix()[ 2 ].xyz ) ) );
				half2 Global_MotionDirectionOS39_g61524 = (( mul( GetWorldToObjectMatrix(), float4( temp_output_398_0_g62099 , 0.0 ) ).xyz * ase_parentObjectScale )).xz;
				half ObjectData20_g62106 = 3.14;
				float Bounds_Height374_g61524 = _MaxBoundsInfo.y;
				half WorldData19_g62106 = ( Bounds_Height374_g61524 * 3.14 );
				#ifdef TVE_VERTEX_DATA_BATCHED
				float staticSwitch14_g62106 = WorldData19_g62106;
				#else
				float staticSwitch14_g62106 = ObjectData20_g62106;
				#endif
				float Motion_Max_Bending1133_g61524 = staticSwitch14_g62106;
				half Mesh_Motion_1082_g61524 = v.ase_texcoord3.x;
				half Motion_10_Mask4617_g61524 = ( _MotionAmplitude_10 * Motion_Max_Bending1133_g61524 * Mesh_Motion_1082_g61524 );
				half Input_Speed62_g62087 = _MotionSpeed_10;
				float mulTime373_g62087 = _TimeParameters.x * Input_Speed62_g62087;
				float3 break111_g62098 = ObjectPosition4223_g61524;
				float Mesh_Variation16_g61524 = v.ase_color.r;
				half VertexDynamicMode4798_g61524 = _VertexDynamicMode;
				float lerpResult4795_g61524 = lerp( frac( ( v.ase_color.r + ( break111_g62098.x + break111_g62098.z ) ) ) , Mesh_Variation16_g61524 , VertexDynamicMode4798_g61524);
				half ObjectData20_g62094 = lerpResult4795_g61524;
				half WorldData19_g62094 = Mesh_Variation16_g61524;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float staticSwitch14_g62094 = WorldData19_g62094;
				#else
				float staticSwitch14_g62094 = ObjectData20_g62094;
				#endif
				half Motion_Variation3073_g61524 = staticSwitch14_g62094;
				half Motion_10_Variation4581_g61524 = ( _MotionVariation_10 * Motion_Variation3073_g61524 );
				half Motion_Variation284_g62087 = Motion_10_Variation4581_g61524;
				float Motion_Scale287_g62087 = ( _MotionScale_10 * ( ase_worldPos.x + ase_worldPos.y + ase_worldPos.z ) );
				half Sine_MinusOneToOne281_g62087 = sin( ( mulTime373_g62087 + Motion_Variation284_g62087 + Motion_Scale287_g62087 ) );
				half Input_WindSquash419_g62087 = 0.2;
				half Wind_Power369_g62099 = break322_g62099.z;
				float lerpResult376_g62099 = lerp( TVE_MotionParamsMin.x , TVE_MotionParamsMax.x , Wind_Power369_g62099);
				half Wind_Stop420_g62099 = saturate( ( Wind_Power369_g62099 * 10.0 ) );
				half Global_MotionPower_103106_g61524 = ( lerpResult376_g62099 * Wind_Stop420_g62099 );
				half Input_WindPower327_g62087 = Global_MotionPower_103106_g61524;
				float lerpResult321_g62087 = lerp( Sine_MinusOneToOne281_g62087 , (Sine_MinusOneToOne281_g62087*Input_WindSquash419_g62087 + 1.0) , Input_WindPower327_g62087);
				half Motion_10_SinWaveAm4570_g61524 = lerpResult321_g62087;
				float2 panner437_g62099 = ( _TimeParameters.x * (TVE_NoiseParams).xy + ( (ObjectPosition4223_g61524).xz * TVE_NoiseParams.z ));
				float saferPower446_g62099 = abs( abs( tex2Dlod( TVE_NoiseTex, float4( panner437_g62099, 0, 0.0) ).r ) );
				float lerpResult448_g62099 = lerp( TVE_MotionParamsMin.w , TVE_MotionParamsMax.w , Wind_Power369_g62099);
				half Global_MotionNoise34_g61524 = pow( saferPower446_g62099 , lerpResult448_g62099 );
				half Motion_10_Bending2258_g61524 = ( Motion_10_Mask4617_g61524 * Motion_10_SinWaveAm4570_g61524 * Global_MotionPower_103106_g61524 * Global_MotionNoise34_g61524 );
				half Interaction_Amplitude4137_g61524 = _InteractionAmplitude;
				float lerpResult4494_g61524 = lerp( 1.0 , Mesh_Motion_1082_g61524 , _InteractionMaskValue);
				half ObjectData20_g62102 = lerpResult4494_g61524;
				half WorldData19_g62102 = Mesh_Motion_1082_g61524;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float staticSwitch14_g62102 = WorldData19_g62102;
				#else
				float staticSwitch14_g62102 = ObjectData20_g62102;
				#endif
				half Motion_10_Interaction53_g61524 = ( Interaction_Amplitude4137_g61524 * Motion_Max_Bending1133_g61524 * staticSwitch14_g62102 );
				half Global_InteractionMask66_g61524 = ( break322_g62099.w * break322_g62099.w );
				float lerpResult4685_g61524 = lerp( Motion_10_Bending2258_g61524 , Motion_10_Interaction53_g61524 , saturate( ( Global_InteractionMask66_g61524 * Interaction_Amplitude4137_g61524 ) ));
				float2 break4603_g61524 = ( Global_MotionDirectionOS39_g61524 * lerpResult4685_g61524 );
				half Vertex_ZAxisRotatin190_g61524 = break4603_g61524.y;
				half Angle44_g62096 = Vertex_ZAxisRotatin190_g61524;
				half3 VertexPos40_g62104 = ( VertexPosRotationAxis50_g62096 + ( VertexPosOtherAxis82_g62096 * cos( Angle44_g62096 ) ) + ( cross( float3(1,0,0) , VertexPosOtherAxis82_g62096 ) * sin( Angle44_g62096 ) ) );
				float3 appendResult74_g62104 = (float3(0.0 , 0.0 , VertexPos40_g62104.z));
				half3 VertexPosRotationAxis50_g62104 = appendResult74_g62104;
				float3 break84_g62104 = VertexPos40_g62104;
				float3 appendResult81_g62104 = (float3(break84_g62104.x , break84_g62104.y , 0.0));
				half3 VertexPosOtherAxis82_g62104 = appendResult81_g62104;
				half Vertex_XAxisRotation216_g61524 = break4603_g61524.x;
				half Angle44_g62104 = -Vertex_XAxisRotation216_g61524;
				half3 VertexPos40_g62072 = ( VertexPosRotationAxis50_g62104 + ( VertexPosOtherAxis82_g62104 * cos( Angle44_g62104 ) ) + ( cross( float3(0,0,1) , VertexPosOtherAxis82_g62104 ) * sin( Angle44_g62104 ) ) );
				float3 appendResult74_g62072 = (float3(0.0 , VertexPos40_g62072.y , 0.0));
				float3 VertexPosRotationAxis50_g62072 = appendResult74_g62072;
				float3 break84_g62072 = VertexPos40_g62072;
				float3 appendResult81_g62072 = (float3(break84_g62072.x , 0.0 , break84_g62072.z));
				float3 VertexPosOtherAxis82_g62072 = appendResult81_g62072;
				half Mesh_Motion_2060_g61524 = v.ase_texcoord3.y;
				float lerpResult410_g62099 = lerp( TVE_MotionParamsMin.y , TVE_MotionParamsMax.y , Wind_Power369_g62099);
				half Global_MotionPower_203109_g61524 = ( lerpResult410_g62099 * Wind_Stop420_g62099 );
				half Motion_20_Variation4255_g61524 = ( _MotionVariation_20 * Motion_Variation3073_g61524 );
				half Variation127_g62091 = ( Motion_20_Variation4255_g61524 * Motion_Variation3073_g61524 );
				float mulTime131_g62091 = _TimeParameters.x * 0.5;
				float temp_output_134_0_g62091 = (sin( ( Variation127_g62091 + mulTime131_g62091 ) )*0.5 + 0.5);
				half Global_MotionPower2223_g61524 = ( Wind_Power369_g62099 * Wind_Stop420_g62099 );
				float temp_output_112_0_g62091 = Global_MotionPower2223_g61524;
				float lerpResult136_g62091 = lerp( ( temp_output_134_0_g62091 * temp_output_134_0_g62091 * temp_output_134_0_g62091 * temp_output_134_0_g62091 ) , 1.0 , ( temp_output_112_0_g62091 * temp_output_112_0_g62091 ));
				float lerpResult126_g62091 = lerp( lerpResult136_g62091 , 1.0 , ( 1.0 - saturate( Variation127_g62091 ) ));
				half Motion_Selective4260_g61524 = lerpResult126_g62091;
				half Motion_20_Mode162_g61524 = _MotionValue_20;
				half Motion_20_Commons4381_g61524 = ( Mesh_Motion_2060_g61524 * Global_MotionPower_203109_g61524 * Global_MotionNoise34_g61524 * Motion_Selective4260_g61524 * Motion_20_Mode162_g61524 );
				half Motion_20_Speed4257_g61524 = _MotionSpeed_20;
				half Input_Speed62_g62085 = Motion_20_Speed4257_g61524;
				float mulTime354_g62085 = _TimeParameters.x * Input_Speed62_g62085;
				float Motion_Variation284_g62085 = Motion_20_Variation4255_g61524;
				half Motion_20_Scale4256_g61524 = _MotionScale_20;
				float Motion_Scale287_g62085 = ( Motion_20_Scale4256_g61524 * ( ase_worldPos.x + ase_worldPos.y + ase_worldPos.z ) );
				half Motion_20_SineWaveA4382_g61524 = sin( ( mulTime354_g62085 + Motion_Variation284_g62085 + Motion_Scale287_g62085 ) );
				half ObjectData20_g62086 = 3.14;
				float Bounds_Radius121_g61524 = _MaxBoundsInfo.x;
				half WorldData19_g62086 = Bounds_Radius121_g61524;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float staticSwitch14_g62086 = WorldData19_g62086;
				#else
				float staticSwitch14_g62086 = ObjectData20_g62086;
				#endif
				float Motion_Max_Rolling1137_g61524 = staticSwitch14_g62086;
				half Motion_20_Rolling138_g61524 = ( _MotionAmplitude_20 * Motion_20_Commons4381_g61524 * Motion_20_SineWaveA4382_g61524 * Motion_Max_Rolling1137_g61524 );
				half Angle44_g62072 = Motion_20_Rolling138_g61524;
				half Input_Speed62_g62141 = ( Motion_20_Speed4257_g61524 - 1.0 );
				float mulTime354_g62141 = _TimeParameters.x * Input_Speed62_g62141;
				float Motion_Variation284_g62141 = Motion_20_Variation4255_g61524;
				float Motion_Scale287_g62141 = ( Motion_20_Scale4256_g61524 * ( ase_worldPos.x + ase_worldPos.y + ase_worldPos.z ) );
				half Motion_20_SineWaveB4460_g61524 = sin( ( mulTime354_g62141 + Motion_Variation284_g62141 + Motion_Scale287_g62141 ) );
				float3 appendResult4393_g61524 = (float3(0.0 , ( _MotionAmplitude_21 * Motion_20_Commons4381_g61524 * Motion_20_SineWaveB4460_g61524 * Bounds_Radius121_g61524 ) , 0.0));
				half3 Motion_20_Vertical4280_g61524 = appendResult4393_g61524;
				float2 break4421_g61524 = ( ( _MotionAmplitude_22 * Motion_20_Commons4381_g61524 * ( Bounds_Radius121_g61524 * 2.0 ) * (Motion_20_SineWaveA4382_g61524*0.2 + 1.0) ) * Global_MotionDirectionOS39_g61524 );
				float3 appendResult4417_g61524 = (float3(break4421_g61524.x , 0.0 , break4421_g61524.y));
				half3 Motion_20_Squash4418_g61524 = appendResult4417_g61524;
				half Motion_Scale321_g62079 = ( _MotionScale_32 * 10.0 );
				half Input_Speed62_g62079 = _MotionSpeed_32;
				float mulTime349_g62079 = _TimeParameters.x * Input_Speed62_g62079;
				float Motion_Variation330_g62079 = ( _MotionVariation_32 * Motion_Variation3073_g61524 );
				half Input_Amplitude58_g62079 = ( _MotionAmplitude_32 * Bounds_Radius121_g61524 * 0.1 );
				float3 temp_output_299_0_g62079 = ( sin( ( ( ase_worldPos * Motion_Scale321_g62079 ) + mulTime349_g62079 + Motion_Variation330_g62079 ) ) * Input_Amplitude58_g62079 );
				half Mesh_Motion_30144_g61524 = v.ase_texcoord3.z;
				float lerpResult378_g62099 = lerp( TVE_MotionParamsMin.z , TVE_MotionParamsMax.z , Wind_Power369_g62099);
				half Global_MotionPower_303115_g61524 = ( lerpResult378_g62099 * Wind_Stop420_g62099 );
				float temp_output_7_0_g62093 = TVE_MotionFadeEnd;
				half Wind_FadeOut4005_g61524 = saturate( ( ( distance( ase_worldPos , _WorldSpaceCameraPos ) - temp_output_7_0_g62093 ) / ( TVE_MotionFadeStart - temp_output_7_0_g62093 ) ) );
				half3 Motion_30_Details263_g61524 = ( temp_output_299_0_g62079 * ( Global_MotionNoise34_g61524 * Mesh_Motion_30144_g61524 * Global_MotionPower_303115_g61524 * Wind_FadeOut4005_g61524 * Motion_Selective4260_g61524 ) );
				float3 Vertex_Motion_Object833_g61524 = ( ( ( VertexPosRotationAxis50_g62072 + ( VertexPosOtherAxis82_g62072 * cos( Angle44_g62072 ) ) + ( cross( float3(0,1,0) , VertexPosOtherAxis82_g62072 ) * sin( Angle44_g62072 ) ) ) + Motion_20_Vertical4280_g61524 + Motion_20_Squash4418_g61524 ) + Motion_30_Details263_g61524 );
				float3 temp_output_3474_0_g61524 = ( VertexPosition3588_g61524 - Mesh_PivotsOS2291_g61524 );
				float3 appendResult2043_g61524 = (float3(Vertex_XAxisRotation216_g61524 , 0.0 , Vertex_ZAxisRotatin190_g61524));
				float3 appendResult2047_g61524 = (float3(Motion_20_Rolling138_g61524 , 0.0 , -Motion_20_Rolling138_g61524));
				float3 Vertex_Motion_World1118_g61524 = ( ( ( temp_output_3474_0_g61524 + appendResult2043_g61524 ) + appendResult2047_g61524 + Motion_20_Vertical4280_g61524 + Motion_20_Squash4418_g61524 ) + Motion_30_Details263_g61524 );
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch3312_g61524 = Vertex_Motion_World1118_g61524;
				#else
				float3 staticSwitch3312_g61524 = ( Vertex_Motion_Object833_g61524 + ( 0.0 * _VertexDataMode ) );
				#endif
				float4 temp_output_94_19_g62115 = TVE_VertexCoords;
				float3 Position83_g62115 = ObjectPosition4223_g61524;
				float temp_output_84_0_g62115 = _LayerVertexValue;
				float4 lerpResult87_g62115 = lerp( TVE_VertexParams , SAMPLE_TEXTURE2D_ARRAY_LOD( TVE_VertexTex, samplerTVE_VertexTex, ( (temp_output_94_19_g62115).zw + ( (temp_output_94_19_g62115).xy * (Position83_g62115).xz ) ),temp_output_84_0_g62115, 0.0 ) , TVE_VertexUsage[(int)temp_output_84_0_g62115]);
				half4 Global_Object_Params4173_g61524 = lerpResult87_g62115;
				half Global_VertexSize174_g61524 = Global_Object_Params4173_g61524.w;
				float lerpResult16_g62149 = lerp( 0.0 , _GlobalSize , TVE_Enabled);
				float lerpResult346_g61524 = lerp( 1.0 , Global_VertexSize174_g61524 , lerpResult16_g62149);
				float3 appendResult3480_g61524 = (float3(lerpResult346_g61524 , lerpResult346_g61524 , lerpResult346_g61524));
				half3 ObjectData20_g62147 = appendResult3480_g61524;
				half3 _Vector11 = half3(1,1,1);
				half3 WorldData19_g62147 = _Vector11;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g62147 = WorldData19_g62147;
				#else
				float3 staticSwitch14_g62147 = ObjectData20_g62147;
				#endif
				half3 Vertex_Size1741_g61524 = staticSwitch14_g62147;
				half3 _Vector5 = half3(1,1,1);
				float temp_output_7_0_g62100 = _SizeFadeEndValue;
				float temp_output_335_0_g61524 = saturate( ( ( ( distance( _WorldSpaceCameraPos , ObjectPosition4223_g61524 ) * ( 1.0 / TVE_DistanceFadeBias ) ) - temp_output_7_0_g62100 ) / ( _SizeFadeStartValue - temp_output_7_0_g62100 ) ) );
				float3 appendResult3482_g61524 = (float3(temp_output_335_0_g61524 , temp_output_335_0_g61524 , temp_output_335_0_g61524));
				float3 lerpResult3556_g61524 = lerp( _Vector5 , appendResult3482_g61524 , _SizeFadeMode);
				half3 ObjectData20_g62097 = lerpResult3556_g61524;
				half3 WorldData19_g62097 = _Vector5;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g62097 = WorldData19_g62097;
				#else
				float3 staticSwitch14_g62097 = ObjectData20_g62097;
				#endif
				float3 Vertex_SizeFade1740_g61524 = staticSwitch14_g62097;
				half3 Grass_Perspective2661_g61524 = half3(0,0,0);
				float3 Final_VertexPosition890_g61524 = ( ( ( staticSwitch3312_g61524 * Vertex_Size1741_g61524 * Vertex_SizeFade1740_g61524 ) + Mesh_PivotsOS2291_g61524 + Grass_Perspective2661_g61524 ) + _DisableSRPBatcher );
				
				half Mesh_Height1524_g61524 = v.ase_color.a;
				float temp_output_7_0_g62140 = _GradientMinValue;
				half Gradient_Tint2784_g61524 = saturate( ( ( Mesh_Height1524_g61524 - temp_output_7_0_g62140 ) / ( _GradientMaxValue - temp_output_7_0_g62140 ) ) );
				float vertexToFrag11_g62137 = Gradient_Tint2784_g61524;
				o.ase_texcoord7.x = vertexToFrag11_g62137;
				float3 temp_cast_8 = (_NoiseScaleValue).xxx;
				float3 WorldPosition3905_g61524 = vertexToFrag3890_g61524;
				float temp_output_7_0_g62120 = _NoiseMinValue;
				half Noise_Tint2802_g61524 = saturate( ( ( tex3Dlod( TVE_WorldTex3D, float4( ( temp_cast_8 * WorldPosition3905_g61524 * 0.1 ), 0.0) ).r - temp_output_7_0_g62120 ) / ( _NoiseMaxValue - temp_output_7_0_g62120 ) ) );
				float vertexToFrag11_g62143 = Noise_Tint2802_g61524;
				o.ase_texcoord7.y = vertexToFrag11_g62143;
				float2 vertexToFrag11_g62084 = ( ( v.texcoord.xy * (_MainUVs).xy ) + (_MainUVs).zw );
				o.ase_texcoord7.zw = vertexToFrag11_g62084;
				o.ase_texcoord8.xyz = vertexToFrag3890_g61524;
				float Mesh_Occlusion318_g61524 = v.ase_color.g;
				float temp_output_7_0_g62136 = _VertexOcclusionMinValue;
				float temp_output_3377_0_g61524 = saturate( ( ( Mesh_Occlusion318_g61524 - temp_output_7_0_g62136 ) / ( _VertexOcclusionMaxValue - temp_output_7_0_g62136 ) ) );
				float vertexToFrag11_g62138 = temp_output_3377_0_g61524;
				o.ase_texcoord8.w = vertexToFrag11_g62138;
				
				float2 vertexToFrag11_g62119 = ( ( v.texcoord.xy * (_EmissiveUVs).xy ) + (_EmissiveUVs).zw );
				o.ase_texcoord9.xy = vertexToFrag11_g62119;
				
				float temp_output_7_0_g62146 = TVE_CameraFadeStart;
				float lerpResult4755_g61524 = lerp( 1.0 , saturate( ( ( distance( ase_worldPos , _WorldSpaceCameraPos ) - temp_output_7_0_g62146 ) / ( TVE_CameraFadeEnd - temp_output_7_0_g62146 ) ) ) , _FadeCameraValue);
				float vertexToFrag11_g62121 = lerpResult4755_g61524;
				o.ase_texcoord9.z = vertexToFrag11_g62121;
				
				o.ase_color = v.ase_color;
				o.ase_normal = v.ase_normal;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord9.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = Final_VertexPosition890_g61524;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 positionVS = TransformWorldToView( positionWS );
				float4 positionCS = TransformWorldToHClip( positionWS );

				VertexNormalInputs normalInput = GetVertexNormalInputs( v.ase_normal, v.ase_tangent );

				o.tSpace0 = float4( normalInput.normalWS, positionWS.x);
				o.tSpace1 = float4( normalInput.tangentWS, positionWS.y);
				o.tSpace2 = float4( normalInput.bitangentWS, positionWS.z);

				OUTPUT_LIGHTMAP_UV( v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy );
				OUTPUT_SH( normalInput.normalWS.xyz, o.lightmapUVOrVertexSH.xyz );

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					o.lightmapUVOrVertexSH.zw = v.texcoord;
					o.lightmapUVOrVertexSH.xy = v.texcoord * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				half3 vertexLight = VertexLighting( positionWS, normalInput.normalWS );
				#ifdef ASE_FOG
					half fogFactor = ComputeFogFactor( positionCS.z );
				#else
					half fogFactor = 0;
				#endif
				o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
				
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = positionWS;
				vertexInput.positionCS = positionCS;
				o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				
				o.clipPos = positionCS;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				o.screenPos = ComputeScreenPos(positionCS);
				#endif
				return o;
			}
			
			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_tangent = v.ase_tangent;
				o.texcoord = v.texcoord;
				o.texcoord1 = v.texcoord1;
				o.texcoord = v.texcoord;
				o.ase_texcoord3 = v.ase_texcoord3;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.texcoord = patch[0].texcoord * bary.x + patch[1].texcoord * bary.y + patch[2].texcoord * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord = patch[0].texcoord * bary.x + patch[1].texcoord * bary.y + patch[2].texcoord * bary.z;
				o.ase_texcoord3 = patch[0].ase_texcoord3 * bary.x + patch[1].ase_texcoord3 * bary.y + patch[2].ase_texcoord3 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif

			half4 frag ( VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						, FRONT_FACE_TYPE ase_vface : FRONT_FACE_SEMANTIC ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float2 sampleCoords = (IN.lightmapUVOrVertexSH.zw / _TerrainHeightmapRecipSize.zw + 0.5f) * _TerrainHeightmapRecipSize.xy;
					float3 WorldNormal = TransformObjectToWorldNormal(normalize(SAMPLE_TEXTURE2D(_TerrainNormalmapTexture, sampler_TerrainNormalmapTexture, sampleCoords).rgb * 2 - 1));
					float3 WorldTangent = -cross(GetObjectToWorldMatrix()._13_23_33, WorldNormal);
					float3 WorldBiTangent = cross(WorldNormal, -WorldTangent);
				#else
					float3 WorldNormal = normalize( IN.tSpace0.xyz );
					float3 WorldTangent = IN.tSpace1.xyz;
					float3 WorldBiTangent = IN.tSpace2.xyz;
				#endif
				float3 WorldPosition = float3(IN.tSpace0.w,IN.tSpace1.w,IN.tSpace2.w);
				float3 WorldViewDirection = _WorldSpaceCameraPos.xyz  - WorldPosition;
				float4 ShadowCoords = float4( 0, 0, 0, 0 );
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 ScreenPos = IN.screenPos;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					ShadowCoords = IN.shadowCoord;
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
				#endif
	
				WorldViewDirection = SafeNormalize( WorldViewDirection );

				float vertexToFrag11_g62137 = IN.ase_texcoord7.x;
				float3 lerpResult2779_g61524 = lerp( (_GradientColorTwo).rgb , (_GradientColorOne).rgb , vertexToFrag11_g62137);
				float vertexToFrag11_g62143 = IN.ase_texcoord7.y;
				float3 lerpResult2800_g61524 = lerp( (_NoiseColorTwo).rgb , (_NoiseColorOne).rgb , vertexToFrag11_g62143);
				half Leaves_Mask4511_g61524 = 1.0;
				float3 lerpResult4521_g61524 = lerp( float3( 1,1,1 ) , ( lerpResult2779_g61524 * lerpResult2800_g61524 * float3(1,1,1) ) , Leaves_Mask4511_g61524);
				float3 lerpResult4519_g61524 = lerp( float3( 1,1,1 ) , (_MainColor).rgb , Leaves_Mask4511_g61524);
				float2 vertexToFrag11_g62084 = IN.ase_texcoord7.zw;
				half2 Main_UVs15_g61524 = vertexToFrag11_g62084;
				float4 tex2DNode29_g61524 = tex2D( _MainAlbedoTex, Main_UVs15_g61524 );
				half3 Main_Albedo99_g61524 = ( lerpResult4519_g61524 * (tex2DNode29_g61524).rgb );
				half3 Blend_Albedo265_g61524 = Main_Albedo99_g61524;
				half3 Blend_AlbedoTinted2808_g61524 = ( lerpResult4521_g61524 * Blend_Albedo265_g61524 );
				float dotResult3616_g61524 = dot( Blend_AlbedoTinted2808_g61524 , float3(0.2126,0.7152,0.0722) );
				float3 temp_cast_0 = (dotResult3616_g61524).xxx;
				float4 temp_output_91_19_g62122 = TVE_ColorsCoords;
				float3 vertexToFrag3890_g61524 = IN.ase_texcoord8.xyz;
				float3 WorldPosition3905_g61524 = vertexToFrag3890_g61524;
				float3 Position58_g62122 = WorldPosition3905_g61524;
				float temp_output_82_0_g62122 = _LayerColorsValue;
				float4 lerpResult88_g62122 = lerp( TVE_ColorsParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ColorsTex, samplerTVE_ColorsTex, ( (temp_output_91_19_g62122).zw + ( (temp_output_91_19_g62122).xy * (Position58_g62122).xz ) ),temp_output_82_0_g62122 ) , TVE_ColorsUsage[(int)temp_output_82_0_g62122]);
				half Global_ColorsTex_A1701_g61524 = (lerpResult88_g62122).a;
				half Global_Colors_Influence3668_g61524 = saturate( Global_ColorsTex_A1701_g61524 );
				float3 lerpResult3618_g61524 = lerp( Blend_AlbedoTinted2808_g61524 , temp_cast_0 , Global_Colors_Influence3668_g61524);
				half3 Global_ColorsTex_RGB1700_g61524 = (lerpResult88_g62122).rgb;
				#ifdef UNITY_COLORSPACE_GAMMA
				float staticSwitch1_g62151 = 2.0;
				#else
				float staticSwitch1_g62151 = 4.594794;
				#endif
				half3 Global_Colors1954_g61524 = ( Global_ColorsTex_RGB1700_g61524 * staticSwitch1_g62151 );
				float lerpResult3870_g61524 = lerp( 1.0 , IN.ase_color.r , _ColorsVariationValue);
				half Global_Colors_Value3650_g61524 = ( _GlobalColors * lerpResult3870_g61524 );
				float4 tex2DNode35_g61524 = tex2D( _MainMaskTex, Main_UVs15_g61524 );
				half Main_Mask57_g61524 = tex2DNode35_g61524.b;
				float temp_output_7_0_g62131 = _ColorsMaskMinValue;
				half Global_Colors_Mask3692_g61524 = saturate( ( ( Main_Mask57_g61524 - temp_output_7_0_g62131 ) / ( _ColorsMaskMaxValue - temp_output_7_0_g62131 ) ) );
				float lerpResult16_g62101 = lerp( 0.0 , ( Global_Colors_Value3650_g61524 * Global_Colors_Mask3692_g61524 ) , TVE_Enabled);
				float3 lerpResult3628_g61524 = lerp( Blend_AlbedoTinted2808_g61524 , ( lerpResult3618_g61524 * Global_Colors1954_g61524 ) , lerpResult16_g62101);
				half3 Blend_AlbedoColored863_g61524 = lerpResult3628_g61524;
				float3 temp_output_799_0_g61524 = (_SubsurfaceColor).rgb;
				float dotResult3930_g61524 = dot( temp_output_799_0_g61524 , float3(0.2126,0.7152,0.0722) );
				float3 temp_cast_3 = (dotResult3930_g61524).xxx;
				float3 lerpResult3932_g61524 = lerp( temp_output_799_0_g61524 , temp_cast_3 , Global_Colors_Influence3668_g61524);
				float3 lerpResult3942_g61524 = lerp( temp_output_799_0_g61524 , ( lerpResult3932_g61524 * Global_Colors1954_g61524 ) , ( Global_Colors_Value3650_g61524 * Global_Colors_Mask3692_g61524 ));
				half3 Subsurface_Color1722_g61524 = lerpResult3942_g61524;
				half MainLight_Subsurface4041_g61524 = TVE_MainLightParams.a;
				half Subsurface_Intensity1752_g61524 = ( _SubsurfaceValue * MainLight_Subsurface4041_g61524 );
				float temp_output_7_0_g62148 = _SubsurfaceMaskMinValue;
				half Subsurface_Mask1557_g61524 = saturate( ( ( Main_Mask57_g61524 - temp_output_7_0_g62148 ) / ( _SubsurfaceMaskMaxValue - temp_output_7_0_g62148 ) ) );
				half3 Subsurface_Transmission884_g61524 = ( Subsurface_Color1722_g61524 * Subsurface_Intensity1752_g61524 * Subsurface_Mask1557_g61524 );
				half3 MainLight_Direction3926_g61524 = TVE_MainLightDirection;
				float3 normalizeResult2169_g61524 = normalize( WorldViewDirection );
				float3 ViewDir_Normalized3963_g61524 = normalizeResult2169_g61524;
				float dotResult785_g61524 = dot( -MainLight_Direction3926_g61524 , ViewDir_Normalized3963_g61524 );
				float saferPower1624_g61524 = abs( (dotResult785_g61524*0.5 + 0.5) );
				#ifdef UNITY_PASS_FORWARDADD
				float staticSwitch1602_g61524 = 0.0;
				#else
				float staticSwitch1602_g61524 = ( pow( saferPower1624_g61524 , _MainLightAngleValue ) * _MainLightScatteringValue );
				#endif
				half Mask_Subsurface_View782_g61524 = staticSwitch1602_g61524;
				float3 unpack4112_g61524 = UnpackNormalScale( tex2D( _MainNormalTex, Main_UVs15_g61524 ), _MainNormalValue );
				unpack4112_g61524.z = lerp( 1, unpack4112_g61524.z, saturate(_MainNormalValue) );
				half3 Main_Normal137_g61524 = unpack4112_g61524;
				float3 tanToWorld0 = float3( WorldTangent.x, WorldBiTangent.x, WorldNormal.x );
				float3 tanToWorld1 = float3( WorldTangent.y, WorldBiTangent.y, WorldNormal.y );
				float3 tanToWorld2 = float3( WorldTangent.z, WorldBiTangent.z, WorldNormal.z );
				float3 tanNormal4099_g61524 = Main_Normal137_g61524;
				float3 worldNormal4099_g61524 = float3(dot(tanToWorld0,tanNormal4099_g61524), dot(tanToWorld1,tanNormal4099_g61524), dot(tanToWorld2,tanNormal4099_g61524));
				float3 Main_Normal_WS4101_g61524 = worldNormal4099_g61524;
				float dotResult777_g61524 = dot( MainLight_Direction3926_g61524 , Main_Normal_WS4101_g61524 );
				float lerpResult4198_g61524 = lerp( 1.0 , saturate( dotResult777_g61524 ) , _MainLightNormalValue);
				#ifdef UNITY_PASS_FORWARDADD
				float staticSwitch1604_g61524 = 0.0;
				#else
				float staticSwitch1604_g61524 = lerpResult4198_g61524;
				#endif
				half Mask_Subsurface_Normal870_g61524 = staticSwitch1604_g61524;
				half3 Subsurface_Scattering1693_g61524 = ( Subsurface_Transmission884_g61524 * Blend_AlbedoColored863_g61524 * ( Mask_Subsurface_View782_g61524 * Mask_Subsurface_Normal870_g61524 ) );
				half3 Blend_AlbedoAndSubsurface149_g61524 = ( Blend_AlbedoColored863_g61524 + Subsurface_Scattering1693_g61524 );
				half3 Global_OverlayColor1758_g61524 = (TVE_OverlayColor).rgb;
				half VertexDynamicMode4798_g61524 = _VertexDynamicMode;
				float lerpResult4801_g61524 = lerp( Main_Normal_WS4101_g61524.y , IN.ase_normal.y , VertexDynamicMode4798_g61524);
				float lerpResult3567_g61524 = lerp( 0.5 , 1.0 , lerpResult4801_g61524);
				half Main_AlbedoTex_G3526_g61524 = tex2DNode29_g61524.g;
				float4 temp_output_93_19_g62109 = TVE_ExtrasCoords;
				float3 Position82_g62109 = WorldPosition3905_g61524;
				float temp_output_84_0_g62109 = _LayerExtrasValue;
				float4 lerpResult88_g62109 = lerp( TVE_ExtrasParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ExtrasTex, samplerTVE_ExtrasTex, ( (temp_output_93_19_g62109).zw + ( (temp_output_93_19_g62109).xy * (Position82_g62109).xz ) ),temp_output_84_0_g62109 ) , TVE_ExtrasUsage[(int)temp_output_84_0_g62109]);
				float4 break89_g62109 = lerpResult88_g62109;
				half Global_Extras_Overlay156_g61524 = break89_g62109.b;
				float lerpResult1065_g61524 = lerp( 1.0 , IN.ase_color.r , _OverlayVariationValue);
				half Overlay_Variation4560_g61524 = lerpResult1065_g61524;
				half Overlay_Commons1365_g61524 = ( _GlobalOverlay * Global_Extras_Overlay156_g61524 * Overlay_Variation4560_g61524 );
				float temp_output_7_0_g62145 = _OverlayMaskMinValue;
				half Overlay_Mask269_g61524 = saturate( ( ( ( ( ( lerpResult3567_g61524 * 0.5 ) + Main_AlbedoTex_G3526_g61524 ) * Overlay_Commons1365_g61524 ) - temp_output_7_0_g62145 ) / ( _OverlayMaskMaxValue - temp_output_7_0_g62145 ) ) );
				float3 lerpResult336_g61524 = lerp( Blend_AlbedoAndSubsurface149_g61524 , Global_OverlayColor1758_g61524 , Overlay_Mask269_g61524);
				half3 Final_Albedo359_g61524 = lerpResult336_g61524;
				float3 temp_cast_7 = (1.0).xxx;
				float vertexToFrag11_g62138 = IN.ase_texcoord8.w;
				float3 lerpResult2945_g61524 = lerp( (_VertexOcclusionColor).rgb , temp_cast_7 , vertexToFrag11_g62138);
				float3 Vertex_Occlusion648_g61524 = lerpResult2945_g61524;
				
				float3 temp_output_13_0_g62074 = Main_Normal137_g61524;
				float3 switchResult12_g62074 = (((ase_vface>0)?(temp_output_13_0_g62074):(( temp_output_13_0_g62074 * _render_normals_options ))));
				half3 Blend_Normal312_g61524 = switchResult12_g62074;
				half3 Final_Normal366_g61524 = Blend_Normal312_g61524;
				
				float4 temp_output_4214_0_g61524 = ( _EmissiveColor * _EmissiveIntensityParams.x );
				float2 vertexToFrag11_g62119 = IN.ase_texcoord9.xy;
				half2 Emissive_UVs2468_g61524 = vertexToFrag11_g62119;
				half Global_Extras_Emissive4203_g61524 = break89_g62109.r;
				float lerpResult4206_g61524 = lerp( 1.0 , Global_Extras_Emissive4203_g61524 , _GlobalEmissive);
				half3 Final_Emissive2476_g61524 = ( (( temp_output_4214_0_g61524 * tex2D( _EmissiveTex, Emissive_UVs2468_g61524 ) )).rgb * lerpResult4206_g61524 );
				
				float3 temp_cast_8 = (( 0.04 * _RenderSpecular )).xxx;
				
				half Main_Smoothness227_g61524 = ( tex2DNode35_g61524.a * _MainSmoothnessValue );
				half Blend_Smoothness314_g61524 = Main_Smoothness227_g61524;
				half Global_OverlaySmoothness311_g61524 = TVE_OverlaySmoothness;
				float lerpResult343_g61524 = lerp( Blend_Smoothness314_g61524 , Global_OverlaySmoothness311_g61524 , Overlay_Mask269_g61524);
				half Final_Smoothness371_g61524 = lerpResult343_g61524;
				half Global_Extras_Wetness305_g61524 = break89_g62109.g;
				float lerpResult3673_g61524 = lerp( 0.0 , Global_Extras_Wetness305_g61524 , _GlobalWetness);
				half Final_SmoothnessAndWetness4130_g61524 = saturate( ( Final_Smoothness371_g61524 + lerpResult3673_g61524 ) );
				
				float lerpResult240_g61524 = lerp( 1.0 , tex2DNode35_g61524.g , _MainOcclusionValue);
				half Main_Occlusion247_g61524 = lerpResult240_g61524;
				half Blend_Occlusion323_g61524 = Main_Occlusion247_g61524;
				
				float localCustomAlphaClip3735_g61524 = ( 0.0 );
				float3 normalizeResult3971_g61524 = normalize( cross( ddy( WorldPosition ) , ddx( WorldPosition ) ) );
				float3 NormalsWS_Derivates3972_g61524 = normalizeResult3971_g61524;
				float dotResult3851_g61524 = dot( ViewDir_Normalized3963_g61524 , NormalsWS_Derivates3972_g61524 );
				float lerpResult3993_g61524 = lerp( 1.0 , saturate( abs( dotResult3851_g61524 ) ) , _FadeGlancingValue);
				half Fade_Glancing3853_g61524 = lerpResult3993_g61524;
				float vertexToFrag11_g62121 = IN.ase_texcoord9.z;
				half Fade_Camera3743_g61524 = vertexToFrag11_g62121;
				float temp_output_41_0_g62144 = ( Fade_Glancing3853_g61524 * Fade_Camera3743_g61524 );
				half Final_AlphaFade3727_g61524 = saturate( ( temp_output_41_0_g62144 + ( temp_output_41_0_g62144 * tex3D( TVE_ScreenTex3D, ( TVE_ScreenTexCoord * WorldPosition3905_g61524 ) ).r ) ) );
				float Main_Alpha316_g61524 = ( _MainColor.a * tex2DNode29_g61524.a );
				float Mesh_Variation16_g61524 = IN.ase_color.r;
				float temp_output_4023_0_g61524 = (Mesh_Variation16_g61524*0.5 + 0.5);
				half Global_Extras_Alpha1033_g61524 = break89_g62109.a;
				float temp_output_4022_0_g61524 = ( temp_output_4023_0_g61524 - ( 1.0 - Global_Extras_Alpha1033_g61524 ) );
				half AlphaTreshold2132_g61524 = _Cutoff;
				#ifdef TVE_ALPHA_CLIP
				float staticSwitch4017_g61524 = ( temp_output_4022_0_g61524 + AlphaTreshold2132_g61524 );
				#else
				float staticSwitch4017_g61524 = temp_output_4022_0_g61524;
				#endif
				float lerpResult16_g62113 = lerp( 0.0 , _GlobalAlpha , TVE_Enabled);
				float lerpResult4011_g61524 = lerp( 1.0 , staticSwitch4017_g61524 , lerpResult16_g62113);
				half Global_Alpha315_g61524 = saturate( lerpResult4011_g61524 );
				#ifdef TVE_ALPHA_CLIP
				float staticSwitch3792_g61524 = ( ( Main_Alpha316_g61524 * Global_Alpha315_g61524 ) - ( AlphaTreshold2132_g61524 - 0.5 ) );
				#else
				float staticSwitch3792_g61524 = ( Main_Alpha316_g61524 * Global_Alpha315_g61524 );
				#endif
				half Final_Alpha3754_g61524 = staticSwitch3792_g61524;
				float temp_output_661_0_g61524 = ( Final_AlphaFade3727_g61524 * Final_Alpha3754_g61524 );
				float Alpha3735_g61524 = temp_output_661_0_g61524;
				float Treshold3735_g61524 = 0.5;
				{
				#if TVE_ALPHA_CLIP
				clip(Alpha3735_g61524 - Treshold3735_g61524);
				#endif
				}
				half Final_Clip914_g61524 = saturate( Alpha3735_g61524 );
				
				float3 Albedo = ( Final_Albedo359_g61524 * Vertex_Occlusion648_g61524 );
				float3 Normal = Final_Normal366_g61524;
				float3 Emission = Final_Emissive2476_g61524;
				float3 Specular = temp_cast_8;
				float Metallic = 0;
				float Smoothness = Final_SmoothnessAndWetness4130_g61524;
				float Occlusion = Blend_Occlusion323_g61524;
				float Alpha = Final_Clip914_g61524;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;
				float3 BakedGI = 0;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = Subsurface_Transmission884_g61524;
				float3 Translucency = 1;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				InputData inputData;
				inputData.positionWS = WorldPosition;
				inputData.viewDirectionWS = WorldViewDirection;
				inputData.shadowCoord = ShadowCoords;

				#ifdef _NORMALMAP
					#if _NORMAL_DROPOFF_TS
					inputData.normalWS = TransformTangentToWorld(Normal, half3x3( WorldTangent, WorldBiTangent, WorldNormal ));
					#elif _NORMAL_DROPOFF_OS
					inputData.normalWS = TransformObjectToWorldNormal(Normal);
					#elif _NORMAL_DROPOFF_WS
					inputData.normalWS = Normal;
					#endif
					inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
				#else
					inputData.normalWS = WorldNormal;
				#endif

				#ifdef ASE_FOG
					inputData.fogCoord = IN.fogFactorAndVertexLight.x;
				#endif

				inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float3 SH = SampleSH(inputData.normalWS.xyz);
				#else
					float3 SH = IN.lightmapUVOrVertexSH.xyz;
				#endif

				inputData.bakedGI = SAMPLE_GI( IN.lightmapUVOrVertexSH.xy, SH, inputData.normalWS );
				#ifdef _ASE_BAKEDGI
					inputData.bakedGI = BakedGI;
				#endif
				half4 color = UniversalFragmentPBR(
					inputData, 
					Albedo, 
					Metallic, 
					Specular, 
					Smoothness, 
					Occlusion, 
					Emission, 
					Alpha);

				#ifdef _TRANSMISSION_ASE
				{
					float shadow = _subsurface_shadow;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );
					half3 mainTransmission = max(0 , -dot(inputData.normalWS, mainLight.direction)) * mainAtten * Transmission;
					color.rgb += Albedo * mainTransmission;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 transmission = max(0 , -dot(inputData.normalWS, light.direction)) * atten * Transmission;
							color.rgb += Albedo * transmission;
						}
					#endif
				}
				#endif

				#ifdef _TRANSLUCENCY_ASE
				{
					float shadow = _TransShadow;
					float normal = _TransNormal;
					float scattering = _TransScattering;
					float direct = _TransDirect;
					float ambient = _TransAmbient;
					float strength = _TransStrength;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );

					half3 mainLightDir = mainLight.direction + inputData.normalWS * normal;
					half mainVdotL = pow( saturate( dot( inputData.viewDirectionWS, -mainLightDir ) ), scattering );
					half3 mainTranslucency = mainAtten * ( mainVdotL * direct + inputData.bakedGI * ambient ) * Translucency;
					color.rgb += Albedo * mainTranslucency * strength;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 lightDir = light.direction + inputData.normalWS * normal;
							half VdotL = pow( saturate( dot( inputData.viewDirectionWS, -lightDir ) ), scattering );
							half3 translucency = atten * ( VdotL * direct + inputData.bakedGI * ambient ) * Translucency;
							color.rgb += Albedo * translucency * strength;
						}
					#endif
				}
				#endif

				#ifdef _REFRACTION_ASE
					float4 projScreenPos = ScreenPos / ScreenPos.w;
					float3 refractionOffset = ( RefractionIndex - 1.0 ) * mul( UNITY_MATRIX_V, WorldNormal ).xyz * ( 1.0 - dot( WorldNormal, WorldViewDirection ) );
					projScreenPos.xy += refractionOffset.xy;
					float3 refraction = SHADERGRAPH_SAMPLE_SCENE_COLOR( projScreenPos ) * RefractionColor;
					color.rgb = lerp( refraction, color.rgb, color.a );
					color.a = 1;
				#endif

				#ifdef ASE_FINAL_COLOR_ALPHA_MULTIPLY
					color.rgb *= color.a;
				#endif

				#ifdef ASE_FOG
					#ifdef TERRAIN_SPLAT_ADDPASS
						color.rgb = MixFogColor(color.rgb, half3( 0, 0, 0 ), IN.fogFactorAndVertexLight.x );
					#else
						color.rgb = MixFog(color.rgb, IN.fogFactorAndVertexLight.x);
					#endif
				#endif
				
				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif

				return color;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			ZWrite On
			ZTest LEqual
			AlphaToMask Off

			HLSLPROGRAM
			#define _SPECULAR_SETUP 1
			#define _NORMAL_DROPOFF_TS 1
			#define _TRANSMISSION_ASE 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#pragma multi_compile _ DOTS_INSTANCING_ON
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _EMISSION
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70403

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x 

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_SHADOWCASTER

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#pragma shader_feature_local TVE_ALPHA_CLIP
			#pragma shader_feature_local TVE_VERTEX_DATA_BATCHED
			//TVE Pipeline Defines
			#define THE_VEGETATION_ENGINE
			#define TVE_IS_UNIVERSAL_PIPELINE
			//TVE Shader Type Defines
			#define TVE_IS_VEGETATION_SHADER
			//TVE Injection Defines
			//SHADER INJECTION POINT BEGIN
			//SHADER INJECTION POINT END


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			half4 _DetailBlendRemap;
			half4 _OverlayMaskRemap;
			half4 _ColorsMaskRemap;
			half4 _SubsurfaceMaskRemap;
			half4 _LeavesMaskRemap;
			half4 _NoiseColorOne;
			half4 _SubsurfaceColor;
			half4 _AlphaMaskRemap;
			half4 _VertexOcclusionRemap;
			half4 _VertexOcclusionColor;
			half4 _GradientColorTwo;
			float4 _NoiseMaskRemap;
			half4 _GradientColorOne;
			half4 _MainColor;
			half4 _NoiseColorTwo;
			float4 _GradientMaskRemap;
			float4 _SubsurfaceDiffusion_asset;
			float4 _MaxBoundsInfo;
			half4 _EmissiveColor;
			half4 _MainUVs;
			float4 _SubsurfaceDiffusion_Asset;
			half4 _EmissiveUVs;
			float4 _EmissiveIntensityParams;
			float4 _Color;
			half3 _render_normals_options;
			half _SizeFadeStartValue;
			half _SizeFadeMode;
			half _GradientMinValue;
			half _GradientMaxValue;
			half _SizeFadeEndValue;
			half _VertexDataMode;
			half _LayerVertexValue;
			half _MotionAmplitude_32;
			float _MotionVariation_32;
			float _MotionSpeed_32;
			float _MotionScale_32;
			half _MotionAmplitude_22;
			half _MotionAmplitude_21;
			half _MotionScale_20;
			half _MotionSpeed_20;
			half _MotionValue_20;
			half _MotionVariation_20;
			half _MotionAmplitude_20;
			half _GlobalSize;
			half _render_cull;
			half _LayerColorsValue;
			half _NoiseMinValue;
			half _FadeGlancingValue;
			half _MainOcclusionValue;
			half _GlobalWetness;
			half _MainSmoothnessValue;
			half _RenderSpecular;
			half _GlobalEmissive;
			half _VertexOcclusionMaxValue;
			half _VertexOcclusionMinValue;
			half _OverlayMaskMaxValue;
			half _OverlayMaskMinValue;
			half _OverlayVariationValue;
			half _LayerExtrasValue;
			half _NoiseScaleValue;
			half _GlobalOverlay;
			half _MainNormalValue;
			half _MainLightScatteringValue;
			half _MainLightAngleValue;
			half _SubsurfaceMaskMaxValue;
			half _SubsurfaceMaskMinValue;
			half _SubsurfaceValue;
			half _ColorsMaskMaxValue;
			half _ColorsMaskMinValue;
			half _ColorsVariationValue;
			half _GlobalColors;
			half _InteractionMaskValue;
			half _NoiseMaxValue;
			half _MainLightNormalValue;
			half _InteractionAmplitude;
			half _IsSubsurfaceShader;
			half _VertexDynamicMode;
			half _SizeFadeCat;
			float _SubsurfaceDiffusion;
			half _VariationGlobalsMessage;
			half _RenderCull;
			half _HasEmissive;
			half _FadeSpace;
			half _MotionCat;
			half _IsVersion;
			half _DetailCat;
			half _HasGradient;
			half _SubsurfaceCat;
			half _VertexMasksMode;
			half _PerspectiveCat;
			half _TranslucencyNormalValue;
			half _TranslucencyHDMessage;
			half _RenderMode;
			half _RenderDecals;
			half _TranslucencyAmbientValue;
			half _RenderingCat;
			half _DetailBlendMode;
			half _GradientCat;
			half _DetailTypeMode;
			half _RenderQueue;
			half _TranslucencyIntensityValue;
			half _RenderPriority;
			half _EmissiveCat;
			half _subsurface_shadow;
			half _render_src;
			half _VariationMotionMessage;
			float _MotionScale_10;
			half _TranslucencyScatteringValue;
			half _DetailMode;
			half _MotionVariation_10;
			float _MotionSpeed_10;
			half _MotionAmplitude_10;
			half _LayerMotionValue;
			half _FadeCameraValue;
			half _IsLeafShader;
			half _render_dst;
			half _render_zw;
			half _RenderClip;
			half _VertexRollingMode;
			half _VertexVariationMode;
			half _SizeFadeMessage;
			half _Cutoff;
			half _MotionSpace;
			half _TranslucencyShadowValue;
			half _LayersSpace;
			half _LayerReactValue;
			half _MainCat;
			half _ReceiveSpace;
			half _RenderSSR;
			half _IsTVEShader;
			half _EmissiveFlagMode;
			half _RenderZWrite;
			half _HasOcclusion;
			half _OcclusionCat;
			half _GlobalCat;
			half _RenderNormals;
			half _TranslucencyDirectValue;
			half _NoiseCat;
			half _GlobalAlpha;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			half TVE_Enabled;
			sampler2D _BumpMap;
			sampler2D _MainTex;
			half4 TVE_MotionParams;
			TEXTURE2D_ARRAY(TVE_MotionTex);
			half4 TVE_MotionCoords;
			SAMPLER(samplerTVE_MotionTex);
			float TVE_MotionUsage[10];
			half4 TVE_MotionParamsMin;
			half4 TVE_MotionParamsMax;
			sampler2D TVE_NoiseTex;
			half4 TVE_NoiseParams;
			half TVE_MotionFadeEnd;
			half TVE_MotionFadeStart;
			half4 TVE_VertexParams;
			TEXTURE2D_ARRAY(TVE_VertexTex);
			half4 TVE_VertexCoords;
			SAMPLER(samplerTVE_VertexTex);
			float TVE_VertexUsage[10];
			half TVE_DistanceFadeBias;
			half _DisableSRPBatcher;
			half TVE_CameraFadeStart;
			half TVE_CameraFadeEnd;
			sampler3D TVE_ScreenTex3D;
			half TVE_ScreenTexCoord;
			sampler2D _MainAlbedoTex;
			half4 TVE_ExtrasParams;
			TEXTURE2D_ARRAY(TVE_ExtrasTex);
			half4 TVE_ExtrasCoords;
			SAMPLER(samplerTVE_ExtrasTex);
			float TVE_ExtrasUsage[10];


			
			float3 _LightDirection;

			VertexOutput VertexFunction( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float3 VertexPosition3588_g61524 = v.vertex.xyz;
				half3 Mesh_PivotsOS2291_g61524 = half3(0,0,0);
				float3 temp_output_2283_0_g61524 = ( VertexPosition3588_g61524 - Mesh_PivotsOS2291_g61524 );
				half3 VertexPos40_g62096 = temp_output_2283_0_g61524;
				float3 appendResult74_g62096 = (float3(VertexPos40_g62096.x , 0.0 , 0.0));
				half3 VertexPosRotationAxis50_g62096 = appendResult74_g62096;
				float3 break84_g62096 = VertexPos40_g62096;
				float3 appendResult81_g62096 = (float3(0.0 , break84_g62096.y , break84_g62096.z));
				half3 VertexPosOtherAxis82_g62096 = appendResult81_g62096;
				float4 temp_output_91_19_g62127 = TVE_MotionCoords;
				float4x4 break19_g62080 = GetObjectToWorldMatrix();
				float3 appendResult20_g62080 = (float3(break19_g62080[ 0 ][ 3 ] , break19_g62080[ 1 ][ 3 ] , break19_g62080[ 2 ][ 3 ]));
				half3 ObjectData20_g62081 = appendResult20_g62080;
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				half3 WorldData19_g62081 = ase_worldPos;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g62081 = WorldData19_g62081;
				#else
				float3 staticSwitch14_g62081 = ObjectData20_g62081;
				#endif
				float3 temp_output_114_0_g62080 = staticSwitch14_g62081;
				float3 vertexToFrag4224_g61524 = temp_output_114_0_g62080;
				half3 ObjectData20_g62075 = vertexToFrag4224_g61524;
				float3 vertexToFrag3890_g61524 = ase_worldPos;
				half3 WorldData19_g62075 = vertexToFrag3890_g61524;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g62075 = WorldData19_g62075;
				#else
				float3 staticSwitch14_g62075 = ObjectData20_g62075;
				#endif
				float3 ObjectPosition4223_g61524 = staticSwitch14_g62075;
				float3 Position83_g62127 = ObjectPosition4223_g61524;
				float temp_output_84_0_g62127 = _LayerMotionValue;
				float4 lerpResult87_g62127 = lerp( TVE_MotionParams , SAMPLE_TEXTURE2D_ARRAY_LOD( TVE_MotionTex, samplerTVE_MotionTex, ( (temp_output_91_19_g62127).zw + ( (temp_output_91_19_g62127).xy * (Position83_g62127).xz ) ),temp_output_84_0_g62127, 0.0 ) , TVE_MotionUsage[(int)temp_output_84_0_g62127]);
				half4 Global_Motion_Params3909_g61524 = lerpResult87_g62127;
				float4 break322_g62099 = Global_Motion_Params3909_g61524;
				float3 appendResult397_g62099 = (float3(break322_g62099.x , 0.0 , break322_g62099.y));
				float3 temp_output_398_0_g62099 = (appendResult397_g62099*2.0 + -1.0);
				float3 ase_parentObjectScale = ( 1.0 / float3( length( GetWorldToObjectMatrix()[ 0 ].xyz ), length( GetWorldToObjectMatrix()[ 1 ].xyz ), length( GetWorldToObjectMatrix()[ 2 ].xyz ) ) );
				half2 Global_MotionDirectionOS39_g61524 = (( mul( GetWorldToObjectMatrix(), float4( temp_output_398_0_g62099 , 0.0 ) ).xyz * ase_parentObjectScale )).xz;
				half ObjectData20_g62106 = 3.14;
				float Bounds_Height374_g61524 = _MaxBoundsInfo.y;
				half WorldData19_g62106 = ( Bounds_Height374_g61524 * 3.14 );
				#ifdef TVE_VERTEX_DATA_BATCHED
				float staticSwitch14_g62106 = WorldData19_g62106;
				#else
				float staticSwitch14_g62106 = ObjectData20_g62106;
				#endif
				float Motion_Max_Bending1133_g61524 = staticSwitch14_g62106;
				half Mesh_Motion_1082_g61524 = v.ase_texcoord3.x;
				half Motion_10_Mask4617_g61524 = ( _MotionAmplitude_10 * Motion_Max_Bending1133_g61524 * Mesh_Motion_1082_g61524 );
				half Input_Speed62_g62087 = _MotionSpeed_10;
				float mulTime373_g62087 = _TimeParameters.x * Input_Speed62_g62087;
				float3 break111_g62098 = ObjectPosition4223_g61524;
				float Mesh_Variation16_g61524 = v.ase_color.r;
				half VertexDynamicMode4798_g61524 = _VertexDynamicMode;
				float lerpResult4795_g61524 = lerp( frac( ( v.ase_color.r + ( break111_g62098.x + break111_g62098.z ) ) ) , Mesh_Variation16_g61524 , VertexDynamicMode4798_g61524);
				half ObjectData20_g62094 = lerpResult4795_g61524;
				half WorldData19_g62094 = Mesh_Variation16_g61524;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float staticSwitch14_g62094 = WorldData19_g62094;
				#else
				float staticSwitch14_g62094 = ObjectData20_g62094;
				#endif
				half Motion_Variation3073_g61524 = staticSwitch14_g62094;
				half Motion_10_Variation4581_g61524 = ( _MotionVariation_10 * Motion_Variation3073_g61524 );
				half Motion_Variation284_g62087 = Motion_10_Variation4581_g61524;
				float Motion_Scale287_g62087 = ( _MotionScale_10 * ( ase_worldPos.x + ase_worldPos.y + ase_worldPos.z ) );
				half Sine_MinusOneToOne281_g62087 = sin( ( mulTime373_g62087 + Motion_Variation284_g62087 + Motion_Scale287_g62087 ) );
				half Input_WindSquash419_g62087 = 0.2;
				half Wind_Power369_g62099 = break322_g62099.z;
				float lerpResult376_g62099 = lerp( TVE_MotionParamsMin.x , TVE_MotionParamsMax.x , Wind_Power369_g62099);
				half Wind_Stop420_g62099 = saturate( ( Wind_Power369_g62099 * 10.0 ) );
				half Global_MotionPower_103106_g61524 = ( lerpResult376_g62099 * Wind_Stop420_g62099 );
				half Input_WindPower327_g62087 = Global_MotionPower_103106_g61524;
				float lerpResult321_g62087 = lerp( Sine_MinusOneToOne281_g62087 , (Sine_MinusOneToOne281_g62087*Input_WindSquash419_g62087 + 1.0) , Input_WindPower327_g62087);
				half Motion_10_SinWaveAm4570_g61524 = lerpResult321_g62087;
				float2 panner437_g62099 = ( _TimeParameters.x * (TVE_NoiseParams).xy + ( (ObjectPosition4223_g61524).xz * TVE_NoiseParams.z ));
				float saferPower446_g62099 = abs( abs( tex2Dlod( TVE_NoiseTex, float4( panner437_g62099, 0, 0.0) ).r ) );
				float lerpResult448_g62099 = lerp( TVE_MotionParamsMin.w , TVE_MotionParamsMax.w , Wind_Power369_g62099);
				half Global_MotionNoise34_g61524 = pow( saferPower446_g62099 , lerpResult448_g62099 );
				half Motion_10_Bending2258_g61524 = ( Motion_10_Mask4617_g61524 * Motion_10_SinWaveAm4570_g61524 * Global_MotionPower_103106_g61524 * Global_MotionNoise34_g61524 );
				half Interaction_Amplitude4137_g61524 = _InteractionAmplitude;
				float lerpResult4494_g61524 = lerp( 1.0 , Mesh_Motion_1082_g61524 , _InteractionMaskValue);
				half ObjectData20_g62102 = lerpResult4494_g61524;
				half WorldData19_g62102 = Mesh_Motion_1082_g61524;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float staticSwitch14_g62102 = WorldData19_g62102;
				#else
				float staticSwitch14_g62102 = ObjectData20_g62102;
				#endif
				half Motion_10_Interaction53_g61524 = ( Interaction_Amplitude4137_g61524 * Motion_Max_Bending1133_g61524 * staticSwitch14_g62102 );
				half Global_InteractionMask66_g61524 = ( break322_g62099.w * break322_g62099.w );
				float lerpResult4685_g61524 = lerp( Motion_10_Bending2258_g61524 , Motion_10_Interaction53_g61524 , saturate( ( Global_InteractionMask66_g61524 * Interaction_Amplitude4137_g61524 ) ));
				float2 break4603_g61524 = ( Global_MotionDirectionOS39_g61524 * lerpResult4685_g61524 );
				half Vertex_ZAxisRotatin190_g61524 = break4603_g61524.y;
				half Angle44_g62096 = Vertex_ZAxisRotatin190_g61524;
				half3 VertexPos40_g62104 = ( VertexPosRotationAxis50_g62096 + ( VertexPosOtherAxis82_g62096 * cos( Angle44_g62096 ) ) + ( cross( float3(1,0,0) , VertexPosOtherAxis82_g62096 ) * sin( Angle44_g62096 ) ) );
				float3 appendResult74_g62104 = (float3(0.0 , 0.0 , VertexPos40_g62104.z));
				half3 VertexPosRotationAxis50_g62104 = appendResult74_g62104;
				float3 break84_g62104 = VertexPos40_g62104;
				float3 appendResult81_g62104 = (float3(break84_g62104.x , break84_g62104.y , 0.0));
				half3 VertexPosOtherAxis82_g62104 = appendResult81_g62104;
				half Vertex_XAxisRotation216_g61524 = break4603_g61524.x;
				half Angle44_g62104 = -Vertex_XAxisRotation216_g61524;
				half3 VertexPos40_g62072 = ( VertexPosRotationAxis50_g62104 + ( VertexPosOtherAxis82_g62104 * cos( Angle44_g62104 ) ) + ( cross( float3(0,0,1) , VertexPosOtherAxis82_g62104 ) * sin( Angle44_g62104 ) ) );
				float3 appendResult74_g62072 = (float3(0.0 , VertexPos40_g62072.y , 0.0));
				float3 VertexPosRotationAxis50_g62072 = appendResult74_g62072;
				float3 break84_g62072 = VertexPos40_g62072;
				float3 appendResult81_g62072 = (float3(break84_g62072.x , 0.0 , break84_g62072.z));
				float3 VertexPosOtherAxis82_g62072 = appendResult81_g62072;
				half Mesh_Motion_2060_g61524 = v.ase_texcoord3.y;
				float lerpResult410_g62099 = lerp( TVE_MotionParamsMin.y , TVE_MotionParamsMax.y , Wind_Power369_g62099);
				half Global_MotionPower_203109_g61524 = ( lerpResult410_g62099 * Wind_Stop420_g62099 );
				half Motion_20_Variation4255_g61524 = ( _MotionVariation_20 * Motion_Variation3073_g61524 );
				half Variation127_g62091 = ( Motion_20_Variation4255_g61524 * Motion_Variation3073_g61524 );
				float mulTime131_g62091 = _TimeParameters.x * 0.5;
				float temp_output_134_0_g62091 = (sin( ( Variation127_g62091 + mulTime131_g62091 ) )*0.5 + 0.5);
				half Global_MotionPower2223_g61524 = ( Wind_Power369_g62099 * Wind_Stop420_g62099 );
				float temp_output_112_0_g62091 = Global_MotionPower2223_g61524;
				float lerpResult136_g62091 = lerp( ( temp_output_134_0_g62091 * temp_output_134_0_g62091 * temp_output_134_0_g62091 * temp_output_134_0_g62091 ) , 1.0 , ( temp_output_112_0_g62091 * temp_output_112_0_g62091 ));
				float lerpResult126_g62091 = lerp( lerpResult136_g62091 , 1.0 , ( 1.0 - saturate( Variation127_g62091 ) ));
				half Motion_Selective4260_g61524 = lerpResult126_g62091;
				half Motion_20_Mode162_g61524 = _MotionValue_20;
				half Motion_20_Commons4381_g61524 = ( Mesh_Motion_2060_g61524 * Global_MotionPower_203109_g61524 * Global_MotionNoise34_g61524 * Motion_Selective4260_g61524 * Motion_20_Mode162_g61524 );
				half Motion_20_Speed4257_g61524 = _MotionSpeed_20;
				half Input_Speed62_g62085 = Motion_20_Speed4257_g61524;
				float mulTime354_g62085 = _TimeParameters.x * Input_Speed62_g62085;
				float Motion_Variation284_g62085 = Motion_20_Variation4255_g61524;
				half Motion_20_Scale4256_g61524 = _MotionScale_20;
				float Motion_Scale287_g62085 = ( Motion_20_Scale4256_g61524 * ( ase_worldPos.x + ase_worldPos.y + ase_worldPos.z ) );
				half Motion_20_SineWaveA4382_g61524 = sin( ( mulTime354_g62085 + Motion_Variation284_g62085 + Motion_Scale287_g62085 ) );
				half ObjectData20_g62086 = 3.14;
				float Bounds_Radius121_g61524 = _MaxBoundsInfo.x;
				half WorldData19_g62086 = Bounds_Radius121_g61524;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float staticSwitch14_g62086 = WorldData19_g62086;
				#else
				float staticSwitch14_g62086 = ObjectData20_g62086;
				#endif
				float Motion_Max_Rolling1137_g61524 = staticSwitch14_g62086;
				half Motion_20_Rolling138_g61524 = ( _MotionAmplitude_20 * Motion_20_Commons4381_g61524 * Motion_20_SineWaveA4382_g61524 * Motion_Max_Rolling1137_g61524 );
				half Angle44_g62072 = Motion_20_Rolling138_g61524;
				half Input_Speed62_g62141 = ( Motion_20_Speed4257_g61524 - 1.0 );
				float mulTime354_g62141 = _TimeParameters.x * Input_Speed62_g62141;
				float Motion_Variation284_g62141 = Motion_20_Variation4255_g61524;
				float Motion_Scale287_g62141 = ( Motion_20_Scale4256_g61524 * ( ase_worldPos.x + ase_worldPos.y + ase_worldPos.z ) );
				half Motion_20_SineWaveB4460_g61524 = sin( ( mulTime354_g62141 + Motion_Variation284_g62141 + Motion_Scale287_g62141 ) );
				float3 appendResult4393_g61524 = (float3(0.0 , ( _MotionAmplitude_21 * Motion_20_Commons4381_g61524 * Motion_20_SineWaveB4460_g61524 * Bounds_Radius121_g61524 ) , 0.0));
				half3 Motion_20_Vertical4280_g61524 = appendResult4393_g61524;
				float2 break4421_g61524 = ( ( _MotionAmplitude_22 * Motion_20_Commons4381_g61524 * ( Bounds_Radius121_g61524 * 2.0 ) * (Motion_20_SineWaveA4382_g61524*0.2 + 1.0) ) * Global_MotionDirectionOS39_g61524 );
				float3 appendResult4417_g61524 = (float3(break4421_g61524.x , 0.0 , break4421_g61524.y));
				half3 Motion_20_Squash4418_g61524 = appendResult4417_g61524;
				half Motion_Scale321_g62079 = ( _MotionScale_32 * 10.0 );
				half Input_Speed62_g62079 = _MotionSpeed_32;
				float mulTime349_g62079 = _TimeParameters.x * Input_Speed62_g62079;
				float Motion_Variation330_g62079 = ( _MotionVariation_32 * Motion_Variation3073_g61524 );
				half Input_Amplitude58_g62079 = ( _MotionAmplitude_32 * Bounds_Radius121_g61524 * 0.1 );
				float3 temp_output_299_0_g62079 = ( sin( ( ( ase_worldPos * Motion_Scale321_g62079 ) + mulTime349_g62079 + Motion_Variation330_g62079 ) ) * Input_Amplitude58_g62079 );
				half Mesh_Motion_30144_g61524 = v.ase_texcoord3.z;
				float lerpResult378_g62099 = lerp( TVE_MotionParamsMin.z , TVE_MotionParamsMax.z , Wind_Power369_g62099);
				half Global_MotionPower_303115_g61524 = ( lerpResult378_g62099 * Wind_Stop420_g62099 );
				float temp_output_7_0_g62093 = TVE_MotionFadeEnd;
				half Wind_FadeOut4005_g61524 = saturate( ( ( distance( ase_worldPos , _WorldSpaceCameraPos ) - temp_output_7_0_g62093 ) / ( TVE_MotionFadeStart - temp_output_7_0_g62093 ) ) );
				half3 Motion_30_Details263_g61524 = ( temp_output_299_0_g62079 * ( Global_MotionNoise34_g61524 * Mesh_Motion_30144_g61524 * Global_MotionPower_303115_g61524 * Wind_FadeOut4005_g61524 * Motion_Selective4260_g61524 ) );
				float3 Vertex_Motion_Object833_g61524 = ( ( ( VertexPosRotationAxis50_g62072 + ( VertexPosOtherAxis82_g62072 * cos( Angle44_g62072 ) ) + ( cross( float3(0,1,0) , VertexPosOtherAxis82_g62072 ) * sin( Angle44_g62072 ) ) ) + Motion_20_Vertical4280_g61524 + Motion_20_Squash4418_g61524 ) + Motion_30_Details263_g61524 );
				float3 temp_output_3474_0_g61524 = ( VertexPosition3588_g61524 - Mesh_PivotsOS2291_g61524 );
				float3 appendResult2043_g61524 = (float3(Vertex_XAxisRotation216_g61524 , 0.0 , Vertex_ZAxisRotatin190_g61524));
				float3 appendResult2047_g61524 = (float3(Motion_20_Rolling138_g61524 , 0.0 , -Motion_20_Rolling138_g61524));
				float3 Vertex_Motion_World1118_g61524 = ( ( ( temp_output_3474_0_g61524 + appendResult2043_g61524 ) + appendResult2047_g61524 + Motion_20_Vertical4280_g61524 + Motion_20_Squash4418_g61524 ) + Motion_30_Details263_g61524 );
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch3312_g61524 = Vertex_Motion_World1118_g61524;
				#else
				float3 staticSwitch3312_g61524 = ( Vertex_Motion_Object833_g61524 + ( 0.0 * _VertexDataMode ) );
				#endif
				float4 temp_output_94_19_g62115 = TVE_VertexCoords;
				float3 Position83_g62115 = ObjectPosition4223_g61524;
				float temp_output_84_0_g62115 = _LayerVertexValue;
				float4 lerpResult87_g62115 = lerp( TVE_VertexParams , SAMPLE_TEXTURE2D_ARRAY_LOD( TVE_VertexTex, samplerTVE_VertexTex, ( (temp_output_94_19_g62115).zw + ( (temp_output_94_19_g62115).xy * (Position83_g62115).xz ) ),temp_output_84_0_g62115, 0.0 ) , TVE_VertexUsage[(int)temp_output_84_0_g62115]);
				half4 Global_Object_Params4173_g61524 = lerpResult87_g62115;
				half Global_VertexSize174_g61524 = Global_Object_Params4173_g61524.w;
				float lerpResult16_g62149 = lerp( 0.0 , _GlobalSize , TVE_Enabled);
				float lerpResult346_g61524 = lerp( 1.0 , Global_VertexSize174_g61524 , lerpResult16_g62149);
				float3 appendResult3480_g61524 = (float3(lerpResult346_g61524 , lerpResult346_g61524 , lerpResult346_g61524));
				half3 ObjectData20_g62147 = appendResult3480_g61524;
				half3 _Vector11 = half3(1,1,1);
				half3 WorldData19_g62147 = _Vector11;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g62147 = WorldData19_g62147;
				#else
				float3 staticSwitch14_g62147 = ObjectData20_g62147;
				#endif
				half3 Vertex_Size1741_g61524 = staticSwitch14_g62147;
				half3 _Vector5 = half3(1,1,1);
				float temp_output_7_0_g62100 = _SizeFadeEndValue;
				float temp_output_335_0_g61524 = saturate( ( ( ( distance( _WorldSpaceCameraPos , ObjectPosition4223_g61524 ) * ( 1.0 / TVE_DistanceFadeBias ) ) - temp_output_7_0_g62100 ) / ( _SizeFadeStartValue - temp_output_7_0_g62100 ) ) );
				float3 appendResult3482_g61524 = (float3(temp_output_335_0_g61524 , temp_output_335_0_g61524 , temp_output_335_0_g61524));
				float3 lerpResult3556_g61524 = lerp( _Vector5 , appendResult3482_g61524 , _SizeFadeMode);
				half3 ObjectData20_g62097 = lerpResult3556_g61524;
				half3 WorldData19_g62097 = _Vector5;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g62097 = WorldData19_g62097;
				#else
				float3 staticSwitch14_g62097 = ObjectData20_g62097;
				#endif
				float3 Vertex_SizeFade1740_g61524 = staticSwitch14_g62097;
				half3 Grass_Perspective2661_g61524 = half3(0,0,0);
				float3 Final_VertexPosition890_g61524 = ( ( ( staticSwitch3312_g61524 * Vertex_Size1741_g61524 * Vertex_SizeFade1740_g61524 ) + Mesh_PivotsOS2291_g61524 + Grass_Perspective2661_g61524 ) + _DisableSRPBatcher );
				
				float temp_output_7_0_g62146 = TVE_CameraFadeStart;
				float lerpResult4755_g61524 = lerp( 1.0 , saturate( ( ( distance( ase_worldPos , _WorldSpaceCameraPos ) - temp_output_7_0_g62146 ) / ( TVE_CameraFadeEnd - temp_output_7_0_g62146 ) ) ) , _FadeCameraValue);
				float vertexToFrag11_g62121 = lerpResult4755_g61524;
				o.ase_texcoord2.x = vertexToFrag11_g62121;
				o.ase_texcoord2.yzw = vertexToFrag3890_g61524;
				float2 vertexToFrag11_g62084 = ( ( v.ase_texcoord.xy * (_MainUVs).xy ) + (_MainUVs).zw );
				o.ase_texcoord3.xy = vertexToFrag11_g62084;
				
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = Final_VertexPosition890_g61524;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif
				float3 normalWS = TransformObjectToWorldDir(v.ase_normal);

				float4 clipPos = TransformWorldToHClip( ApplyShadowBias( positionWS, normalWS, _LightDirection ) );

				#if UNITY_REVERSED_Z
					clipPos.z = min(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#else
					clipPos.z = max(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = clipPos;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord3 = v.ase_texcoord3;
				o.ase_color = v.ase_color;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord3 = patch[0].ase_texcoord3 * bary.x + patch[1].ase_texcoord3 * bary.y + patch[2].ase_texcoord3 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif

			half4 frag(	VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );
				
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float localCustomAlphaClip3735_g61524 = ( 0.0 );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 normalizeResult2169_g61524 = normalize( ase_worldViewDir );
				float3 ViewDir_Normalized3963_g61524 = normalizeResult2169_g61524;
				float3 normalizeResult3971_g61524 = normalize( cross( ddy( WorldPosition ) , ddx( WorldPosition ) ) );
				float3 NormalsWS_Derivates3972_g61524 = normalizeResult3971_g61524;
				float dotResult3851_g61524 = dot( ViewDir_Normalized3963_g61524 , NormalsWS_Derivates3972_g61524 );
				float lerpResult3993_g61524 = lerp( 1.0 , saturate( abs( dotResult3851_g61524 ) ) , _FadeGlancingValue);
				half Fade_Glancing3853_g61524 = lerpResult3993_g61524;
				float vertexToFrag11_g62121 = IN.ase_texcoord2.x;
				half Fade_Camera3743_g61524 = vertexToFrag11_g62121;
				float temp_output_41_0_g62144 = ( Fade_Glancing3853_g61524 * Fade_Camera3743_g61524 );
				float3 vertexToFrag3890_g61524 = IN.ase_texcoord2.yzw;
				float3 WorldPosition3905_g61524 = vertexToFrag3890_g61524;
				half Final_AlphaFade3727_g61524 = saturate( ( temp_output_41_0_g62144 + ( temp_output_41_0_g62144 * tex3D( TVE_ScreenTex3D, ( TVE_ScreenTexCoord * WorldPosition3905_g61524 ) ).r ) ) );
				float2 vertexToFrag11_g62084 = IN.ase_texcoord3.xy;
				half2 Main_UVs15_g61524 = vertexToFrag11_g62084;
				float4 tex2DNode29_g61524 = tex2D( _MainAlbedoTex, Main_UVs15_g61524 );
				float Main_Alpha316_g61524 = ( _MainColor.a * tex2DNode29_g61524.a );
				float Mesh_Variation16_g61524 = IN.ase_color.r;
				float temp_output_4023_0_g61524 = (Mesh_Variation16_g61524*0.5 + 0.5);
				float4 temp_output_93_19_g62109 = TVE_ExtrasCoords;
				float3 Position82_g62109 = WorldPosition3905_g61524;
				float temp_output_84_0_g62109 = _LayerExtrasValue;
				float4 lerpResult88_g62109 = lerp( TVE_ExtrasParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ExtrasTex, samplerTVE_ExtrasTex, ( (temp_output_93_19_g62109).zw + ( (temp_output_93_19_g62109).xy * (Position82_g62109).xz ) ),temp_output_84_0_g62109 ) , TVE_ExtrasUsage[(int)temp_output_84_0_g62109]);
				float4 break89_g62109 = lerpResult88_g62109;
				half Global_Extras_Alpha1033_g61524 = break89_g62109.a;
				float temp_output_4022_0_g61524 = ( temp_output_4023_0_g61524 - ( 1.0 - Global_Extras_Alpha1033_g61524 ) );
				half AlphaTreshold2132_g61524 = _Cutoff;
				#ifdef TVE_ALPHA_CLIP
				float staticSwitch4017_g61524 = ( temp_output_4022_0_g61524 + AlphaTreshold2132_g61524 );
				#else
				float staticSwitch4017_g61524 = temp_output_4022_0_g61524;
				#endif
				float lerpResult16_g62113 = lerp( 0.0 , _GlobalAlpha , TVE_Enabled);
				float lerpResult4011_g61524 = lerp( 1.0 , staticSwitch4017_g61524 , lerpResult16_g62113);
				half Global_Alpha315_g61524 = saturate( lerpResult4011_g61524 );
				#ifdef TVE_ALPHA_CLIP
				float staticSwitch3792_g61524 = ( ( Main_Alpha316_g61524 * Global_Alpha315_g61524 ) - ( AlphaTreshold2132_g61524 - 0.5 ) );
				#else
				float staticSwitch3792_g61524 = ( Main_Alpha316_g61524 * Global_Alpha315_g61524 );
				#endif
				half Final_Alpha3754_g61524 = staticSwitch3792_g61524;
				float temp_output_661_0_g61524 = ( Final_AlphaFade3727_g61524 * Final_Alpha3754_g61524 );
				float Alpha3735_g61524 = temp_output_661_0_g61524;
				float Treshold3735_g61524 = 0.5;
				{
				#if TVE_ALPHA_CLIP
				clip(Alpha3735_g61524 - Treshold3735_g61524);
				#endif
				}
				half Final_Clip914_g61524 = saturate( Alpha3735_g61524 );
				
				float Alpha = Final_Clip914_g61524;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

				#ifdef _ALPHATEST_ON
					#ifdef _ALPHATEST_SHADOW_ON
						clip(Alpha - AlphaClipThresholdShadow);
					#else
						clip(Alpha - AlphaClipThreshold);
					#endif
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif
				return 0;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM
			#define _SPECULAR_SETUP 1
			#define _NORMAL_DROPOFF_TS 1
			#define _TRANSMISSION_ASE 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#pragma multi_compile _ DOTS_INSTANCING_ON
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _EMISSION
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70403

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x 

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_DEPTHONLY

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#pragma shader_feature_local TVE_ALPHA_CLIP
			#pragma shader_feature_local TVE_VERTEX_DATA_BATCHED
			//TVE Pipeline Defines
			#define THE_VEGETATION_ENGINE
			#define TVE_IS_UNIVERSAL_PIPELINE
			//TVE Shader Type Defines
			#define TVE_IS_VEGETATION_SHADER
			//TVE Injection Defines
			//SHADER INJECTION POINT BEGIN
			//SHADER INJECTION POINT END


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			half4 _DetailBlendRemap;
			half4 _OverlayMaskRemap;
			half4 _ColorsMaskRemap;
			half4 _SubsurfaceMaskRemap;
			half4 _LeavesMaskRemap;
			half4 _NoiseColorOne;
			half4 _SubsurfaceColor;
			half4 _AlphaMaskRemap;
			half4 _VertexOcclusionRemap;
			half4 _VertexOcclusionColor;
			half4 _GradientColorTwo;
			float4 _NoiseMaskRemap;
			half4 _GradientColorOne;
			half4 _MainColor;
			half4 _NoiseColorTwo;
			float4 _GradientMaskRemap;
			float4 _SubsurfaceDiffusion_asset;
			float4 _MaxBoundsInfo;
			half4 _EmissiveColor;
			half4 _MainUVs;
			float4 _SubsurfaceDiffusion_Asset;
			half4 _EmissiveUVs;
			float4 _EmissiveIntensityParams;
			float4 _Color;
			half3 _render_normals_options;
			half _SizeFadeStartValue;
			half _SizeFadeMode;
			half _GradientMinValue;
			half _GradientMaxValue;
			half _SizeFadeEndValue;
			half _VertexDataMode;
			half _LayerVertexValue;
			half _MotionAmplitude_32;
			float _MotionVariation_32;
			float _MotionSpeed_32;
			float _MotionScale_32;
			half _MotionAmplitude_22;
			half _MotionAmplitude_21;
			half _MotionScale_20;
			half _MotionSpeed_20;
			half _MotionValue_20;
			half _MotionVariation_20;
			half _MotionAmplitude_20;
			half _GlobalSize;
			half _render_cull;
			half _LayerColorsValue;
			half _NoiseMinValue;
			half _FadeGlancingValue;
			half _MainOcclusionValue;
			half _GlobalWetness;
			half _MainSmoothnessValue;
			half _RenderSpecular;
			half _GlobalEmissive;
			half _VertexOcclusionMaxValue;
			half _VertexOcclusionMinValue;
			half _OverlayMaskMaxValue;
			half _OverlayMaskMinValue;
			half _OverlayVariationValue;
			half _LayerExtrasValue;
			half _NoiseScaleValue;
			half _GlobalOverlay;
			half _MainNormalValue;
			half _MainLightScatteringValue;
			half _MainLightAngleValue;
			half _SubsurfaceMaskMaxValue;
			half _SubsurfaceMaskMinValue;
			half _SubsurfaceValue;
			half _ColorsMaskMaxValue;
			half _ColorsMaskMinValue;
			half _ColorsVariationValue;
			half _GlobalColors;
			half _InteractionMaskValue;
			half _NoiseMaxValue;
			half _MainLightNormalValue;
			half _InteractionAmplitude;
			half _IsSubsurfaceShader;
			half _VertexDynamicMode;
			half _SizeFadeCat;
			float _SubsurfaceDiffusion;
			half _VariationGlobalsMessage;
			half _RenderCull;
			half _HasEmissive;
			half _FadeSpace;
			half _MotionCat;
			half _IsVersion;
			half _DetailCat;
			half _HasGradient;
			half _SubsurfaceCat;
			half _VertexMasksMode;
			half _PerspectiveCat;
			half _TranslucencyNormalValue;
			half _TranslucencyHDMessage;
			half _RenderMode;
			half _RenderDecals;
			half _TranslucencyAmbientValue;
			half _RenderingCat;
			half _DetailBlendMode;
			half _GradientCat;
			half _DetailTypeMode;
			half _RenderQueue;
			half _TranslucencyIntensityValue;
			half _RenderPriority;
			half _EmissiveCat;
			half _subsurface_shadow;
			half _render_src;
			half _VariationMotionMessage;
			float _MotionScale_10;
			half _TranslucencyScatteringValue;
			half _DetailMode;
			half _MotionVariation_10;
			float _MotionSpeed_10;
			half _MotionAmplitude_10;
			half _LayerMotionValue;
			half _FadeCameraValue;
			half _IsLeafShader;
			half _render_dst;
			half _render_zw;
			half _RenderClip;
			half _VertexRollingMode;
			half _VertexVariationMode;
			half _SizeFadeMessage;
			half _Cutoff;
			half _MotionSpace;
			half _TranslucencyShadowValue;
			half _LayersSpace;
			half _LayerReactValue;
			half _MainCat;
			half _ReceiveSpace;
			half _RenderSSR;
			half _IsTVEShader;
			half _EmissiveFlagMode;
			half _RenderZWrite;
			half _HasOcclusion;
			half _OcclusionCat;
			half _GlobalCat;
			half _RenderNormals;
			half _TranslucencyDirectValue;
			half _NoiseCat;
			half _GlobalAlpha;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			half TVE_Enabled;
			sampler2D _BumpMap;
			sampler2D _MainTex;
			half4 TVE_MotionParams;
			TEXTURE2D_ARRAY(TVE_MotionTex);
			half4 TVE_MotionCoords;
			SAMPLER(samplerTVE_MotionTex);
			float TVE_MotionUsage[10];
			half4 TVE_MotionParamsMin;
			half4 TVE_MotionParamsMax;
			sampler2D TVE_NoiseTex;
			half4 TVE_NoiseParams;
			half TVE_MotionFadeEnd;
			half TVE_MotionFadeStart;
			half4 TVE_VertexParams;
			TEXTURE2D_ARRAY(TVE_VertexTex);
			half4 TVE_VertexCoords;
			SAMPLER(samplerTVE_VertexTex);
			float TVE_VertexUsage[10];
			half TVE_DistanceFadeBias;
			half _DisableSRPBatcher;
			half TVE_CameraFadeStart;
			half TVE_CameraFadeEnd;
			sampler3D TVE_ScreenTex3D;
			half TVE_ScreenTexCoord;
			sampler2D _MainAlbedoTex;
			half4 TVE_ExtrasParams;
			TEXTURE2D_ARRAY(TVE_ExtrasTex);
			half4 TVE_ExtrasCoords;
			SAMPLER(samplerTVE_ExtrasTex);
			float TVE_ExtrasUsage[10];


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 VertexPosition3588_g61524 = v.vertex.xyz;
				half3 Mesh_PivotsOS2291_g61524 = half3(0,0,0);
				float3 temp_output_2283_0_g61524 = ( VertexPosition3588_g61524 - Mesh_PivotsOS2291_g61524 );
				half3 VertexPos40_g62096 = temp_output_2283_0_g61524;
				float3 appendResult74_g62096 = (float3(VertexPos40_g62096.x , 0.0 , 0.0));
				half3 VertexPosRotationAxis50_g62096 = appendResult74_g62096;
				float3 break84_g62096 = VertexPos40_g62096;
				float3 appendResult81_g62096 = (float3(0.0 , break84_g62096.y , break84_g62096.z));
				half3 VertexPosOtherAxis82_g62096 = appendResult81_g62096;
				float4 temp_output_91_19_g62127 = TVE_MotionCoords;
				float4x4 break19_g62080 = GetObjectToWorldMatrix();
				float3 appendResult20_g62080 = (float3(break19_g62080[ 0 ][ 3 ] , break19_g62080[ 1 ][ 3 ] , break19_g62080[ 2 ][ 3 ]));
				half3 ObjectData20_g62081 = appendResult20_g62080;
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				half3 WorldData19_g62081 = ase_worldPos;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g62081 = WorldData19_g62081;
				#else
				float3 staticSwitch14_g62081 = ObjectData20_g62081;
				#endif
				float3 temp_output_114_0_g62080 = staticSwitch14_g62081;
				float3 vertexToFrag4224_g61524 = temp_output_114_0_g62080;
				half3 ObjectData20_g62075 = vertexToFrag4224_g61524;
				float3 vertexToFrag3890_g61524 = ase_worldPos;
				half3 WorldData19_g62075 = vertexToFrag3890_g61524;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g62075 = WorldData19_g62075;
				#else
				float3 staticSwitch14_g62075 = ObjectData20_g62075;
				#endif
				float3 ObjectPosition4223_g61524 = staticSwitch14_g62075;
				float3 Position83_g62127 = ObjectPosition4223_g61524;
				float temp_output_84_0_g62127 = _LayerMotionValue;
				float4 lerpResult87_g62127 = lerp( TVE_MotionParams , SAMPLE_TEXTURE2D_ARRAY_LOD( TVE_MotionTex, samplerTVE_MotionTex, ( (temp_output_91_19_g62127).zw + ( (temp_output_91_19_g62127).xy * (Position83_g62127).xz ) ),temp_output_84_0_g62127, 0.0 ) , TVE_MotionUsage[(int)temp_output_84_0_g62127]);
				half4 Global_Motion_Params3909_g61524 = lerpResult87_g62127;
				float4 break322_g62099 = Global_Motion_Params3909_g61524;
				float3 appendResult397_g62099 = (float3(break322_g62099.x , 0.0 , break322_g62099.y));
				float3 temp_output_398_0_g62099 = (appendResult397_g62099*2.0 + -1.0);
				float3 ase_parentObjectScale = ( 1.0 / float3( length( GetWorldToObjectMatrix()[ 0 ].xyz ), length( GetWorldToObjectMatrix()[ 1 ].xyz ), length( GetWorldToObjectMatrix()[ 2 ].xyz ) ) );
				half2 Global_MotionDirectionOS39_g61524 = (( mul( GetWorldToObjectMatrix(), float4( temp_output_398_0_g62099 , 0.0 ) ).xyz * ase_parentObjectScale )).xz;
				half ObjectData20_g62106 = 3.14;
				float Bounds_Height374_g61524 = _MaxBoundsInfo.y;
				half WorldData19_g62106 = ( Bounds_Height374_g61524 * 3.14 );
				#ifdef TVE_VERTEX_DATA_BATCHED
				float staticSwitch14_g62106 = WorldData19_g62106;
				#else
				float staticSwitch14_g62106 = ObjectData20_g62106;
				#endif
				float Motion_Max_Bending1133_g61524 = staticSwitch14_g62106;
				half Mesh_Motion_1082_g61524 = v.ase_texcoord3.x;
				half Motion_10_Mask4617_g61524 = ( _MotionAmplitude_10 * Motion_Max_Bending1133_g61524 * Mesh_Motion_1082_g61524 );
				half Input_Speed62_g62087 = _MotionSpeed_10;
				float mulTime373_g62087 = _TimeParameters.x * Input_Speed62_g62087;
				float3 break111_g62098 = ObjectPosition4223_g61524;
				float Mesh_Variation16_g61524 = v.ase_color.r;
				half VertexDynamicMode4798_g61524 = _VertexDynamicMode;
				float lerpResult4795_g61524 = lerp( frac( ( v.ase_color.r + ( break111_g62098.x + break111_g62098.z ) ) ) , Mesh_Variation16_g61524 , VertexDynamicMode4798_g61524);
				half ObjectData20_g62094 = lerpResult4795_g61524;
				half WorldData19_g62094 = Mesh_Variation16_g61524;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float staticSwitch14_g62094 = WorldData19_g62094;
				#else
				float staticSwitch14_g62094 = ObjectData20_g62094;
				#endif
				half Motion_Variation3073_g61524 = staticSwitch14_g62094;
				half Motion_10_Variation4581_g61524 = ( _MotionVariation_10 * Motion_Variation3073_g61524 );
				half Motion_Variation284_g62087 = Motion_10_Variation4581_g61524;
				float Motion_Scale287_g62087 = ( _MotionScale_10 * ( ase_worldPos.x + ase_worldPos.y + ase_worldPos.z ) );
				half Sine_MinusOneToOne281_g62087 = sin( ( mulTime373_g62087 + Motion_Variation284_g62087 + Motion_Scale287_g62087 ) );
				half Input_WindSquash419_g62087 = 0.2;
				half Wind_Power369_g62099 = break322_g62099.z;
				float lerpResult376_g62099 = lerp( TVE_MotionParamsMin.x , TVE_MotionParamsMax.x , Wind_Power369_g62099);
				half Wind_Stop420_g62099 = saturate( ( Wind_Power369_g62099 * 10.0 ) );
				half Global_MotionPower_103106_g61524 = ( lerpResult376_g62099 * Wind_Stop420_g62099 );
				half Input_WindPower327_g62087 = Global_MotionPower_103106_g61524;
				float lerpResult321_g62087 = lerp( Sine_MinusOneToOne281_g62087 , (Sine_MinusOneToOne281_g62087*Input_WindSquash419_g62087 + 1.0) , Input_WindPower327_g62087);
				half Motion_10_SinWaveAm4570_g61524 = lerpResult321_g62087;
				float2 panner437_g62099 = ( _TimeParameters.x * (TVE_NoiseParams).xy + ( (ObjectPosition4223_g61524).xz * TVE_NoiseParams.z ));
				float saferPower446_g62099 = abs( abs( tex2Dlod( TVE_NoiseTex, float4( panner437_g62099, 0, 0.0) ).r ) );
				float lerpResult448_g62099 = lerp( TVE_MotionParamsMin.w , TVE_MotionParamsMax.w , Wind_Power369_g62099);
				half Global_MotionNoise34_g61524 = pow( saferPower446_g62099 , lerpResult448_g62099 );
				half Motion_10_Bending2258_g61524 = ( Motion_10_Mask4617_g61524 * Motion_10_SinWaveAm4570_g61524 * Global_MotionPower_103106_g61524 * Global_MotionNoise34_g61524 );
				half Interaction_Amplitude4137_g61524 = _InteractionAmplitude;
				float lerpResult4494_g61524 = lerp( 1.0 , Mesh_Motion_1082_g61524 , _InteractionMaskValue);
				half ObjectData20_g62102 = lerpResult4494_g61524;
				half WorldData19_g62102 = Mesh_Motion_1082_g61524;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float staticSwitch14_g62102 = WorldData19_g62102;
				#else
				float staticSwitch14_g62102 = ObjectData20_g62102;
				#endif
				half Motion_10_Interaction53_g61524 = ( Interaction_Amplitude4137_g61524 * Motion_Max_Bending1133_g61524 * staticSwitch14_g62102 );
				half Global_InteractionMask66_g61524 = ( break322_g62099.w * break322_g62099.w );
				float lerpResult4685_g61524 = lerp( Motion_10_Bending2258_g61524 , Motion_10_Interaction53_g61524 , saturate( ( Global_InteractionMask66_g61524 * Interaction_Amplitude4137_g61524 ) ));
				float2 break4603_g61524 = ( Global_MotionDirectionOS39_g61524 * lerpResult4685_g61524 );
				half Vertex_ZAxisRotatin190_g61524 = break4603_g61524.y;
				half Angle44_g62096 = Vertex_ZAxisRotatin190_g61524;
				half3 VertexPos40_g62104 = ( VertexPosRotationAxis50_g62096 + ( VertexPosOtherAxis82_g62096 * cos( Angle44_g62096 ) ) + ( cross( float3(1,0,0) , VertexPosOtherAxis82_g62096 ) * sin( Angle44_g62096 ) ) );
				float3 appendResult74_g62104 = (float3(0.0 , 0.0 , VertexPos40_g62104.z));
				half3 VertexPosRotationAxis50_g62104 = appendResult74_g62104;
				float3 break84_g62104 = VertexPos40_g62104;
				float3 appendResult81_g62104 = (float3(break84_g62104.x , break84_g62104.y , 0.0));
				half3 VertexPosOtherAxis82_g62104 = appendResult81_g62104;
				half Vertex_XAxisRotation216_g61524 = break4603_g61524.x;
				half Angle44_g62104 = -Vertex_XAxisRotation216_g61524;
				half3 VertexPos40_g62072 = ( VertexPosRotationAxis50_g62104 + ( VertexPosOtherAxis82_g62104 * cos( Angle44_g62104 ) ) + ( cross( float3(0,0,1) , VertexPosOtherAxis82_g62104 ) * sin( Angle44_g62104 ) ) );
				float3 appendResult74_g62072 = (float3(0.0 , VertexPos40_g62072.y , 0.0));
				float3 VertexPosRotationAxis50_g62072 = appendResult74_g62072;
				float3 break84_g62072 = VertexPos40_g62072;
				float3 appendResult81_g62072 = (float3(break84_g62072.x , 0.0 , break84_g62072.z));
				float3 VertexPosOtherAxis82_g62072 = appendResult81_g62072;
				half Mesh_Motion_2060_g61524 = v.ase_texcoord3.y;
				float lerpResult410_g62099 = lerp( TVE_MotionParamsMin.y , TVE_MotionParamsMax.y , Wind_Power369_g62099);
				half Global_MotionPower_203109_g61524 = ( lerpResult410_g62099 * Wind_Stop420_g62099 );
				half Motion_20_Variation4255_g61524 = ( _MotionVariation_20 * Motion_Variation3073_g61524 );
				half Variation127_g62091 = ( Motion_20_Variation4255_g61524 * Motion_Variation3073_g61524 );
				float mulTime131_g62091 = _TimeParameters.x * 0.5;
				float temp_output_134_0_g62091 = (sin( ( Variation127_g62091 + mulTime131_g62091 ) )*0.5 + 0.5);
				half Global_MotionPower2223_g61524 = ( Wind_Power369_g62099 * Wind_Stop420_g62099 );
				float temp_output_112_0_g62091 = Global_MotionPower2223_g61524;
				float lerpResult136_g62091 = lerp( ( temp_output_134_0_g62091 * temp_output_134_0_g62091 * temp_output_134_0_g62091 * temp_output_134_0_g62091 ) , 1.0 , ( temp_output_112_0_g62091 * temp_output_112_0_g62091 ));
				float lerpResult126_g62091 = lerp( lerpResult136_g62091 , 1.0 , ( 1.0 - saturate( Variation127_g62091 ) ));
				half Motion_Selective4260_g61524 = lerpResult126_g62091;
				half Motion_20_Mode162_g61524 = _MotionValue_20;
				half Motion_20_Commons4381_g61524 = ( Mesh_Motion_2060_g61524 * Global_MotionPower_203109_g61524 * Global_MotionNoise34_g61524 * Motion_Selective4260_g61524 * Motion_20_Mode162_g61524 );
				half Motion_20_Speed4257_g61524 = _MotionSpeed_20;
				half Input_Speed62_g62085 = Motion_20_Speed4257_g61524;
				float mulTime354_g62085 = _TimeParameters.x * Input_Speed62_g62085;
				float Motion_Variation284_g62085 = Motion_20_Variation4255_g61524;
				half Motion_20_Scale4256_g61524 = _MotionScale_20;
				float Motion_Scale287_g62085 = ( Motion_20_Scale4256_g61524 * ( ase_worldPos.x + ase_worldPos.y + ase_worldPos.z ) );
				half Motion_20_SineWaveA4382_g61524 = sin( ( mulTime354_g62085 + Motion_Variation284_g62085 + Motion_Scale287_g62085 ) );
				half ObjectData20_g62086 = 3.14;
				float Bounds_Radius121_g61524 = _MaxBoundsInfo.x;
				half WorldData19_g62086 = Bounds_Radius121_g61524;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float staticSwitch14_g62086 = WorldData19_g62086;
				#else
				float staticSwitch14_g62086 = ObjectData20_g62086;
				#endif
				float Motion_Max_Rolling1137_g61524 = staticSwitch14_g62086;
				half Motion_20_Rolling138_g61524 = ( _MotionAmplitude_20 * Motion_20_Commons4381_g61524 * Motion_20_SineWaveA4382_g61524 * Motion_Max_Rolling1137_g61524 );
				half Angle44_g62072 = Motion_20_Rolling138_g61524;
				half Input_Speed62_g62141 = ( Motion_20_Speed4257_g61524 - 1.0 );
				float mulTime354_g62141 = _TimeParameters.x * Input_Speed62_g62141;
				float Motion_Variation284_g62141 = Motion_20_Variation4255_g61524;
				float Motion_Scale287_g62141 = ( Motion_20_Scale4256_g61524 * ( ase_worldPos.x + ase_worldPos.y + ase_worldPos.z ) );
				half Motion_20_SineWaveB4460_g61524 = sin( ( mulTime354_g62141 + Motion_Variation284_g62141 + Motion_Scale287_g62141 ) );
				float3 appendResult4393_g61524 = (float3(0.0 , ( _MotionAmplitude_21 * Motion_20_Commons4381_g61524 * Motion_20_SineWaveB4460_g61524 * Bounds_Radius121_g61524 ) , 0.0));
				half3 Motion_20_Vertical4280_g61524 = appendResult4393_g61524;
				float2 break4421_g61524 = ( ( _MotionAmplitude_22 * Motion_20_Commons4381_g61524 * ( Bounds_Radius121_g61524 * 2.0 ) * (Motion_20_SineWaveA4382_g61524*0.2 + 1.0) ) * Global_MotionDirectionOS39_g61524 );
				float3 appendResult4417_g61524 = (float3(break4421_g61524.x , 0.0 , break4421_g61524.y));
				half3 Motion_20_Squash4418_g61524 = appendResult4417_g61524;
				half Motion_Scale321_g62079 = ( _MotionScale_32 * 10.0 );
				half Input_Speed62_g62079 = _MotionSpeed_32;
				float mulTime349_g62079 = _TimeParameters.x * Input_Speed62_g62079;
				float Motion_Variation330_g62079 = ( _MotionVariation_32 * Motion_Variation3073_g61524 );
				half Input_Amplitude58_g62079 = ( _MotionAmplitude_32 * Bounds_Radius121_g61524 * 0.1 );
				float3 temp_output_299_0_g62079 = ( sin( ( ( ase_worldPos * Motion_Scale321_g62079 ) + mulTime349_g62079 + Motion_Variation330_g62079 ) ) * Input_Amplitude58_g62079 );
				half Mesh_Motion_30144_g61524 = v.ase_texcoord3.z;
				float lerpResult378_g62099 = lerp( TVE_MotionParamsMin.z , TVE_MotionParamsMax.z , Wind_Power369_g62099);
				half Global_MotionPower_303115_g61524 = ( lerpResult378_g62099 * Wind_Stop420_g62099 );
				float temp_output_7_0_g62093 = TVE_MotionFadeEnd;
				half Wind_FadeOut4005_g61524 = saturate( ( ( distance( ase_worldPos , _WorldSpaceCameraPos ) - temp_output_7_0_g62093 ) / ( TVE_MotionFadeStart - temp_output_7_0_g62093 ) ) );
				half3 Motion_30_Details263_g61524 = ( temp_output_299_0_g62079 * ( Global_MotionNoise34_g61524 * Mesh_Motion_30144_g61524 * Global_MotionPower_303115_g61524 * Wind_FadeOut4005_g61524 * Motion_Selective4260_g61524 ) );
				float3 Vertex_Motion_Object833_g61524 = ( ( ( VertexPosRotationAxis50_g62072 + ( VertexPosOtherAxis82_g62072 * cos( Angle44_g62072 ) ) + ( cross( float3(0,1,0) , VertexPosOtherAxis82_g62072 ) * sin( Angle44_g62072 ) ) ) + Motion_20_Vertical4280_g61524 + Motion_20_Squash4418_g61524 ) + Motion_30_Details263_g61524 );
				float3 temp_output_3474_0_g61524 = ( VertexPosition3588_g61524 - Mesh_PivotsOS2291_g61524 );
				float3 appendResult2043_g61524 = (float3(Vertex_XAxisRotation216_g61524 , 0.0 , Vertex_ZAxisRotatin190_g61524));
				float3 appendResult2047_g61524 = (float3(Motion_20_Rolling138_g61524 , 0.0 , -Motion_20_Rolling138_g61524));
				float3 Vertex_Motion_World1118_g61524 = ( ( ( temp_output_3474_0_g61524 + appendResult2043_g61524 ) + appendResult2047_g61524 + Motion_20_Vertical4280_g61524 + Motion_20_Squash4418_g61524 ) + Motion_30_Details263_g61524 );
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch3312_g61524 = Vertex_Motion_World1118_g61524;
				#else
				float3 staticSwitch3312_g61524 = ( Vertex_Motion_Object833_g61524 + ( 0.0 * _VertexDataMode ) );
				#endif
				float4 temp_output_94_19_g62115 = TVE_VertexCoords;
				float3 Position83_g62115 = ObjectPosition4223_g61524;
				float temp_output_84_0_g62115 = _LayerVertexValue;
				float4 lerpResult87_g62115 = lerp( TVE_VertexParams , SAMPLE_TEXTURE2D_ARRAY_LOD( TVE_VertexTex, samplerTVE_VertexTex, ( (temp_output_94_19_g62115).zw + ( (temp_output_94_19_g62115).xy * (Position83_g62115).xz ) ),temp_output_84_0_g62115, 0.0 ) , TVE_VertexUsage[(int)temp_output_84_0_g62115]);
				half4 Global_Object_Params4173_g61524 = lerpResult87_g62115;
				half Global_VertexSize174_g61524 = Global_Object_Params4173_g61524.w;
				float lerpResult16_g62149 = lerp( 0.0 , _GlobalSize , TVE_Enabled);
				float lerpResult346_g61524 = lerp( 1.0 , Global_VertexSize174_g61524 , lerpResult16_g62149);
				float3 appendResult3480_g61524 = (float3(lerpResult346_g61524 , lerpResult346_g61524 , lerpResult346_g61524));
				half3 ObjectData20_g62147 = appendResult3480_g61524;
				half3 _Vector11 = half3(1,1,1);
				half3 WorldData19_g62147 = _Vector11;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g62147 = WorldData19_g62147;
				#else
				float3 staticSwitch14_g62147 = ObjectData20_g62147;
				#endif
				half3 Vertex_Size1741_g61524 = staticSwitch14_g62147;
				half3 _Vector5 = half3(1,1,1);
				float temp_output_7_0_g62100 = _SizeFadeEndValue;
				float temp_output_335_0_g61524 = saturate( ( ( ( distance( _WorldSpaceCameraPos , ObjectPosition4223_g61524 ) * ( 1.0 / TVE_DistanceFadeBias ) ) - temp_output_7_0_g62100 ) / ( _SizeFadeStartValue - temp_output_7_0_g62100 ) ) );
				float3 appendResult3482_g61524 = (float3(temp_output_335_0_g61524 , temp_output_335_0_g61524 , temp_output_335_0_g61524));
				float3 lerpResult3556_g61524 = lerp( _Vector5 , appendResult3482_g61524 , _SizeFadeMode);
				half3 ObjectData20_g62097 = lerpResult3556_g61524;
				half3 WorldData19_g62097 = _Vector5;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g62097 = WorldData19_g62097;
				#else
				float3 staticSwitch14_g62097 = ObjectData20_g62097;
				#endif
				float3 Vertex_SizeFade1740_g61524 = staticSwitch14_g62097;
				half3 Grass_Perspective2661_g61524 = half3(0,0,0);
				float3 Final_VertexPosition890_g61524 = ( ( ( staticSwitch3312_g61524 * Vertex_Size1741_g61524 * Vertex_SizeFade1740_g61524 ) + Mesh_PivotsOS2291_g61524 + Grass_Perspective2661_g61524 ) + _DisableSRPBatcher );
				
				float temp_output_7_0_g62146 = TVE_CameraFadeStart;
				float lerpResult4755_g61524 = lerp( 1.0 , saturate( ( ( distance( ase_worldPos , _WorldSpaceCameraPos ) - temp_output_7_0_g62146 ) / ( TVE_CameraFadeEnd - temp_output_7_0_g62146 ) ) ) , _FadeCameraValue);
				float vertexToFrag11_g62121 = lerpResult4755_g61524;
				o.ase_texcoord2.x = vertexToFrag11_g62121;
				o.ase_texcoord2.yzw = vertexToFrag3890_g61524;
				float2 vertexToFrag11_g62084 = ( ( v.ase_texcoord.xy * (_MainUVs).xy ) + (_MainUVs).zw );
				o.ase_texcoord3.xy = vertexToFrag11_g62084;
				
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = Final_VertexPosition890_g61524;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;
				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord3 = v.ase_texcoord3;
				o.ase_color = v.ase_color;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord3 = patch[0].ase_texcoord3 * bary.x + patch[1].ase_texcoord3 * bary.y + patch[2].ase_texcoord3 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif
			half4 frag(	VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float localCustomAlphaClip3735_g61524 = ( 0.0 );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 normalizeResult2169_g61524 = normalize( ase_worldViewDir );
				float3 ViewDir_Normalized3963_g61524 = normalizeResult2169_g61524;
				float3 normalizeResult3971_g61524 = normalize( cross( ddy( WorldPosition ) , ddx( WorldPosition ) ) );
				float3 NormalsWS_Derivates3972_g61524 = normalizeResult3971_g61524;
				float dotResult3851_g61524 = dot( ViewDir_Normalized3963_g61524 , NormalsWS_Derivates3972_g61524 );
				float lerpResult3993_g61524 = lerp( 1.0 , saturate( abs( dotResult3851_g61524 ) ) , _FadeGlancingValue);
				half Fade_Glancing3853_g61524 = lerpResult3993_g61524;
				float vertexToFrag11_g62121 = IN.ase_texcoord2.x;
				half Fade_Camera3743_g61524 = vertexToFrag11_g62121;
				float temp_output_41_0_g62144 = ( Fade_Glancing3853_g61524 * Fade_Camera3743_g61524 );
				float3 vertexToFrag3890_g61524 = IN.ase_texcoord2.yzw;
				float3 WorldPosition3905_g61524 = vertexToFrag3890_g61524;
				half Final_AlphaFade3727_g61524 = saturate( ( temp_output_41_0_g62144 + ( temp_output_41_0_g62144 * tex3D( TVE_ScreenTex3D, ( TVE_ScreenTexCoord * WorldPosition3905_g61524 ) ).r ) ) );
				float2 vertexToFrag11_g62084 = IN.ase_texcoord3.xy;
				half2 Main_UVs15_g61524 = vertexToFrag11_g62084;
				float4 tex2DNode29_g61524 = tex2D( _MainAlbedoTex, Main_UVs15_g61524 );
				float Main_Alpha316_g61524 = ( _MainColor.a * tex2DNode29_g61524.a );
				float Mesh_Variation16_g61524 = IN.ase_color.r;
				float temp_output_4023_0_g61524 = (Mesh_Variation16_g61524*0.5 + 0.5);
				float4 temp_output_93_19_g62109 = TVE_ExtrasCoords;
				float3 Position82_g62109 = WorldPosition3905_g61524;
				float temp_output_84_0_g62109 = _LayerExtrasValue;
				float4 lerpResult88_g62109 = lerp( TVE_ExtrasParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ExtrasTex, samplerTVE_ExtrasTex, ( (temp_output_93_19_g62109).zw + ( (temp_output_93_19_g62109).xy * (Position82_g62109).xz ) ),temp_output_84_0_g62109 ) , TVE_ExtrasUsage[(int)temp_output_84_0_g62109]);
				float4 break89_g62109 = lerpResult88_g62109;
				half Global_Extras_Alpha1033_g61524 = break89_g62109.a;
				float temp_output_4022_0_g61524 = ( temp_output_4023_0_g61524 - ( 1.0 - Global_Extras_Alpha1033_g61524 ) );
				half AlphaTreshold2132_g61524 = _Cutoff;
				#ifdef TVE_ALPHA_CLIP
				float staticSwitch4017_g61524 = ( temp_output_4022_0_g61524 + AlphaTreshold2132_g61524 );
				#else
				float staticSwitch4017_g61524 = temp_output_4022_0_g61524;
				#endif
				float lerpResult16_g62113 = lerp( 0.0 , _GlobalAlpha , TVE_Enabled);
				float lerpResult4011_g61524 = lerp( 1.0 , staticSwitch4017_g61524 , lerpResult16_g62113);
				half Global_Alpha315_g61524 = saturate( lerpResult4011_g61524 );
				#ifdef TVE_ALPHA_CLIP
				float staticSwitch3792_g61524 = ( ( Main_Alpha316_g61524 * Global_Alpha315_g61524 ) - ( AlphaTreshold2132_g61524 - 0.5 ) );
				#else
				float staticSwitch3792_g61524 = ( Main_Alpha316_g61524 * Global_Alpha315_g61524 );
				#endif
				half Final_Alpha3754_g61524 = staticSwitch3792_g61524;
				float temp_output_661_0_g61524 = ( Final_AlphaFade3727_g61524 * Final_Alpha3754_g61524 );
				float Alpha3735_g61524 = temp_output_661_0_g61524;
				float Treshold3735_g61524 = 0.5;
				{
				#if TVE_ALPHA_CLIP
				clip(Alpha3735_g61524 - Treshold3735_g61524);
				#endif
				}
				half Final_Clip914_g61524 = saturate( Alpha3735_g61524 );
				
				float Alpha = Final_Clip914_g61524;
				float AlphaClipThreshold = 0.5;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				#ifdef ASE_DEPTH_WRITE_ON
				outputDepth = DepthValue;
				#endif
				return 0;
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "Meta"
			Tags { "LightMode"="Meta" }

			Cull Off

			HLSLPROGRAM
			#define _SPECULAR_SETUP 1
			#define _NORMAL_DROPOFF_TS 1
			#define _TRANSMISSION_ASE 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#pragma multi_compile _ DOTS_INSTANCING_ON
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _EMISSION
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70403

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x 

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_META

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature_local TVE_ALPHA_CLIP
			#pragma shader_feature_local TVE_VERTEX_DATA_BATCHED
			//TVE Pipeline Defines
			#define THE_VEGETATION_ENGINE
			#define TVE_IS_UNIVERSAL_PIPELINE
			//TVE Shader Type Defines
			#define TVE_IS_VEGETATION_SHADER
			//TVE Injection Defines
			//SHADER INJECTION POINT BEGIN
			//SHADER INJECTION POINT END


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_tangent : TANGENT;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				float4 ase_texcoord6 : TEXCOORD6;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord7 : TEXCOORD7;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			half4 _DetailBlendRemap;
			half4 _OverlayMaskRemap;
			half4 _ColorsMaskRemap;
			half4 _SubsurfaceMaskRemap;
			half4 _LeavesMaskRemap;
			half4 _NoiseColorOne;
			half4 _SubsurfaceColor;
			half4 _AlphaMaskRemap;
			half4 _VertexOcclusionRemap;
			half4 _VertexOcclusionColor;
			half4 _GradientColorTwo;
			float4 _NoiseMaskRemap;
			half4 _GradientColorOne;
			half4 _MainColor;
			half4 _NoiseColorTwo;
			float4 _GradientMaskRemap;
			float4 _SubsurfaceDiffusion_asset;
			float4 _MaxBoundsInfo;
			half4 _EmissiveColor;
			half4 _MainUVs;
			float4 _SubsurfaceDiffusion_Asset;
			half4 _EmissiveUVs;
			float4 _EmissiveIntensityParams;
			float4 _Color;
			half3 _render_normals_options;
			half _SizeFadeStartValue;
			half _SizeFadeMode;
			half _GradientMinValue;
			half _GradientMaxValue;
			half _SizeFadeEndValue;
			half _VertexDataMode;
			half _LayerVertexValue;
			half _MotionAmplitude_32;
			float _MotionVariation_32;
			float _MotionSpeed_32;
			float _MotionScale_32;
			half _MotionAmplitude_22;
			half _MotionAmplitude_21;
			half _MotionScale_20;
			half _MotionSpeed_20;
			half _MotionValue_20;
			half _MotionVariation_20;
			half _MotionAmplitude_20;
			half _GlobalSize;
			half _render_cull;
			half _LayerColorsValue;
			half _NoiseMinValue;
			half _FadeGlancingValue;
			half _MainOcclusionValue;
			half _GlobalWetness;
			half _MainSmoothnessValue;
			half _RenderSpecular;
			half _GlobalEmissive;
			half _VertexOcclusionMaxValue;
			half _VertexOcclusionMinValue;
			half _OverlayMaskMaxValue;
			half _OverlayMaskMinValue;
			half _OverlayVariationValue;
			half _LayerExtrasValue;
			half _NoiseScaleValue;
			half _GlobalOverlay;
			half _MainNormalValue;
			half _MainLightScatteringValue;
			half _MainLightAngleValue;
			half _SubsurfaceMaskMaxValue;
			half _SubsurfaceMaskMinValue;
			half _SubsurfaceValue;
			half _ColorsMaskMaxValue;
			half _ColorsMaskMinValue;
			half _ColorsVariationValue;
			half _GlobalColors;
			half _InteractionMaskValue;
			half _NoiseMaxValue;
			half _MainLightNormalValue;
			half _InteractionAmplitude;
			half _IsSubsurfaceShader;
			half _VertexDynamicMode;
			half _SizeFadeCat;
			float _SubsurfaceDiffusion;
			half _VariationGlobalsMessage;
			half _RenderCull;
			half _HasEmissive;
			half _FadeSpace;
			half _MotionCat;
			half _IsVersion;
			half _DetailCat;
			half _HasGradient;
			half _SubsurfaceCat;
			half _VertexMasksMode;
			half _PerspectiveCat;
			half _TranslucencyNormalValue;
			half _TranslucencyHDMessage;
			half _RenderMode;
			half _RenderDecals;
			half _TranslucencyAmbientValue;
			half _RenderingCat;
			half _DetailBlendMode;
			half _GradientCat;
			half _DetailTypeMode;
			half _RenderQueue;
			half _TranslucencyIntensityValue;
			half _RenderPriority;
			half _EmissiveCat;
			half _subsurface_shadow;
			half _render_src;
			half _VariationMotionMessage;
			float _MotionScale_10;
			half _TranslucencyScatteringValue;
			half _DetailMode;
			half _MotionVariation_10;
			float _MotionSpeed_10;
			half _MotionAmplitude_10;
			half _LayerMotionValue;
			half _FadeCameraValue;
			half _IsLeafShader;
			half _render_dst;
			half _render_zw;
			half _RenderClip;
			half _VertexRollingMode;
			half _VertexVariationMode;
			half _SizeFadeMessage;
			half _Cutoff;
			half _MotionSpace;
			half _TranslucencyShadowValue;
			half _LayersSpace;
			half _LayerReactValue;
			half _MainCat;
			half _ReceiveSpace;
			half _RenderSSR;
			half _IsTVEShader;
			half _EmissiveFlagMode;
			half _RenderZWrite;
			half _HasOcclusion;
			half _OcclusionCat;
			half _GlobalCat;
			half _RenderNormals;
			half _TranslucencyDirectValue;
			half _NoiseCat;
			half _GlobalAlpha;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			half TVE_Enabled;
			sampler2D _BumpMap;
			sampler2D _MainTex;
			half4 TVE_MotionParams;
			TEXTURE2D_ARRAY(TVE_MotionTex);
			half4 TVE_MotionCoords;
			SAMPLER(samplerTVE_MotionTex);
			float TVE_MotionUsage[10];
			half4 TVE_MotionParamsMin;
			half4 TVE_MotionParamsMax;
			sampler2D TVE_NoiseTex;
			half4 TVE_NoiseParams;
			half TVE_MotionFadeEnd;
			half TVE_MotionFadeStart;
			half4 TVE_VertexParams;
			TEXTURE2D_ARRAY(TVE_VertexTex);
			half4 TVE_VertexCoords;
			SAMPLER(samplerTVE_VertexTex);
			float TVE_VertexUsage[10];
			half TVE_DistanceFadeBias;
			half _DisableSRPBatcher;
			sampler3D TVE_WorldTex3D;
			sampler2D _MainAlbedoTex;
			half4 TVE_ColorsParams;
			TEXTURE2D_ARRAY(TVE_ColorsTex);
			half4 TVE_ColorsCoords;
			SAMPLER(samplerTVE_ColorsTex);
			float TVE_ColorsUsage[10];
			sampler2D _MainMaskTex;
			half4 TVE_MainLightParams;
			half3 TVE_MainLightDirection;
			sampler2D _MainNormalTex;
			half4 TVE_OverlayColor;
			half4 TVE_ExtrasParams;
			TEXTURE2D_ARRAY(TVE_ExtrasTex);
			half4 TVE_ExtrasCoords;
			SAMPLER(samplerTVE_ExtrasTex);
			float TVE_ExtrasUsage[10];
			sampler2D _EmissiveTex;
			half TVE_CameraFadeStart;
			half TVE_CameraFadeEnd;
			sampler3D TVE_ScreenTex3D;
			half TVE_ScreenTexCoord;


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 VertexPosition3588_g61524 = v.vertex.xyz;
				half3 Mesh_PivotsOS2291_g61524 = half3(0,0,0);
				float3 temp_output_2283_0_g61524 = ( VertexPosition3588_g61524 - Mesh_PivotsOS2291_g61524 );
				half3 VertexPos40_g62096 = temp_output_2283_0_g61524;
				float3 appendResult74_g62096 = (float3(VertexPos40_g62096.x , 0.0 , 0.0));
				half3 VertexPosRotationAxis50_g62096 = appendResult74_g62096;
				float3 break84_g62096 = VertexPos40_g62096;
				float3 appendResult81_g62096 = (float3(0.0 , break84_g62096.y , break84_g62096.z));
				half3 VertexPosOtherAxis82_g62096 = appendResult81_g62096;
				float4 temp_output_91_19_g62127 = TVE_MotionCoords;
				float4x4 break19_g62080 = GetObjectToWorldMatrix();
				float3 appendResult20_g62080 = (float3(break19_g62080[ 0 ][ 3 ] , break19_g62080[ 1 ][ 3 ] , break19_g62080[ 2 ][ 3 ]));
				half3 ObjectData20_g62081 = appendResult20_g62080;
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				half3 WorldData19_g62081 = ase_worldPos;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g62081 = WorldData19_g62081;
				#else
				float3 staticSwitch14_g62081 = ObjectData20_g62081;
				#endif
				float3 temp_output_114_0_g62080 = staticSwitch14_g62081;
				float3 vertexToFrag4224_g61524 = temp_output_114_0_g62080;
				half3 ObjectData20_g62075 = vertexToFrag4224_g61524;
				float3 vertexToFrag3890_g61524 = ase_worldPos;
				half3 WorldData19_g62075 = vertexToFrag3890_g61524;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g62075 = WorldData19_g62075;
				#else
				float3 staticSwitch14_g62075 = ObjectData20_g62075;
				#endif
				float3 ObjectPosition4223_g61524 = staticSwitch14_g62075;
				float3 Position83_g62127 = ObjectPosition4223_g61524;
				float temp_output_84_0_g62127 = _LayerMotionValue;
				float4 lerpResult87_g62127 = lerp( TVE_MotionParams , SAMPLE_TEXTURE2D_ARRAY_LOD( TVE_MotionTex, samplerTVE_MotionTex, ( (temp_output_91_19_g62127).zw + ( (temp_output_91_19_g62127).xy * (Position83_g62127).xz ) ),temp_output_84_0_g62127, 0.0 ) , TVE_MotionUsage[(int)temp_output_84_0_g62127]);
				half4 Global_Motion_Params3909_g61524 = lerpResult87_g62127;
				float4 break322_g62099 = Global_Motion_Params3909_g61524;
				float3 appendResult397_g62099 = (float3(break322_g62099.x , 0.0 , break322_g62099.y));
				float3 temp_output_398_0_g62099 = (appendResult397_g62099*2.0 + -1.0);
				float3 ase_parentObjectScale = ( 1.0 / float3( length( GetWorldToObjectMatrix()[ 0 ].xyz ), length( GetWorldToObjectMatrix()[ 1 ].xyz ), length( GetWorldToObjectMatrix()[ 2 ].xyz ) ) );
				half2 Global_MotionDirectionOS39_g61524 = (( mul( GetWorldToObjectMatrix(), float4( temp_output_398_0_g62099 , 0.0 ) ).xyz * ase_parentObjectScale )).xz;
				half ObjectData20_g62106 = 3.14;
				float Bounds_Height374_g61524 = _MaxBoundsInfo.y;
				half WorldData19_g62106 = ( Bounds_Height374_g61524 * 3.14 );
				#ifdef TVE_VERTEX_DATA_BATCHED
				float staticSwitch14_g62106 = WorldData19_g62106;
				#else
				float staticSwitch14_g62106 = ObjectData20_g62106;
				#endif
				float Motion_Max_Bending1133_g61524 = staticSwitch14_g62106;
				half Mesh_Motion_1082_g61524 = v.ase_texcoord3.x;
				half Motion_10_Mask4617_g61524 = ( _MotionAmplitude_10 * Motion_Max_Bending1133_g61524 * Mesh_Motion_1082_g61524 );
				half Input_Speed62_g62087 = _MotionSpeed_10;
				float mulTime373_g62087 = _TimeParameters.x * Input_Speed62_g62087;
				float3 break111_g62098 = ObjectPosition4223_g61524;
				float Mesh_Variation16_g61524 = v.ase_color.r;
				half VertexDynamicMode4798_g61524 = _VertexDynamicMode;
				float lerpResult4795_g61524 = lerp( frac( ( v.ase_color.r + ( break111_g62098.x + break111_g62098.z ) ) ) , Mesh_Variation16_g61524 , VertexDynamicMode4798_g61524);
				half ObjectData20_g62094 = lerpResult4795_g61524;
				half WorldData19_g62094 = Mesh_Variation16_g61524;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float staticSwitch14_g62094 = WorldData19_g62094;
				#else
				float staticSwitch14_g62094 = ObjectData20_g62094;
				#endif
				half Motion_Variation3073_g61524 = staticSwitch14_g62094;
				half Motion_10_Variation4581_g61524 = ( _MotionVariation_10 * Motion_Variation3073_g61524 );
				half Motion_Variation284_g62087 = Motion_10_Variation4581_g61524;
				float Motion_Scale287_g62087 = ( _MotionScale_10 * ( ase_worldPos.x + ase_worldPos.y + ase_worldPos.z ) );
				half Sine_MinusOneToOne281_g62087 = sin( ( mulTime373_g62087 + Motion_Variation284_g62087 + Motion_Scale287_g62087 ) );
				half Input_WindSquash419_g62087 = 0.2;
				half Wind_Power369_g62099 = break322_g62099.z;
				float lerpResult376_g62099 = lerp( TVE_MotionParamsMin.x , TVE_MotionParamsMax.x , Wind_Power369_g62099);
				half Wind_Stop420_g62099 = saturate( ( Wind_Power369_g62099 * 10.0 ) );
				half Global_MotionPower_103106_g61524 = ( lerpResult376_g62099 * Wind_Stop420_g62099 );
				half Input_WindPower327_g62087 = Global_MotionPower_103106_g61524;
				float lerpResult321_g62087 = lerp( Sine_MinusOneToOne281_g62087 , (Sine_MinusOneToOne281_g62087*Input_WindSquash419_g62087 + 1.0) , Input_WindPower327_g62087);
				half Motion_10_SinWaveAm4570_g61524 = lerpResult321_g62087;
				float2 panner437_g62099 = ( _TimeParameters.x * (TVE_NoiseParams).xy + ( (ObjectPosition4223_g61524).xz * TVE_NoiseParams.z ));
				float saferPower446_g62099 = abs( abs( tex2Dlod( TVE_NoiseTex, float4( panner437_g62099, 0, 0.0) ).r ) );
				float lerpResult448_g62099 = lerp( TVE_MotionParamsMin.w , TVE_MotionParamsMax.w , Wind_Power369_g62099);
				half Global_MotionNoise34_g61524 = pow( saferPower446_g62099 , lerpResult448_g62099 );
				half Motion_10_Bending2258_g61524 = ( Motion_10_Mask4617_g61524 * Motion_10_SinWaveAm4570_g61524 * Global_MotionPower_103106_g61524 * Global_MotionNoise34_g61524 );
				half Interaction_Amplitude4137_g61524 = _InteractionAmplitude;
				float lerpResult4494_g61524 = lerp( 1.0 , Mesh_Motion_1082_g61524 , _InteractionMaskValue);
				half ObjectData20_g62102 = lerpResult4494_g61524;
				half WorldData19_g62102 = Mesh_Motion_1082_g61524;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float staticSwitch14_g62102 = WorldData19_g62102;
				#else
				float staticSwitch14_g62102 = ObjectData20_g62102;
				#endif
				half Motion_10_Interaction53_g61524 = ( Interaction_Amplitude4137_g61524 * Motion_Max_Bending1133_g61524 * staticSwitch14_g62102 );
				half Global_InteractionMask66_g61524 = ( break322_g62099.w * break322_g62099.w );
				float lerpResult4685_g61524 = lerp( Motion_10_Bending2258_g61524 , Motion_10_Interaction53_g61524 , saturate( ( Global_InteractionMask66_g61524 * Interaction_Amplitude4137_g61524 ) ));
				float2 break4603_g61524 = ( Global_MotionDirectionOS39_g61524 * lerpResult4685_g61524 );
				half Vertex_ZAxisRotatin190_g61524 = break4603_g61524.y;
				half Angle44_g62096 = Vertex_ZAxisRotatin190_g61524;
				half3 VertexPos40_g62104 = ( VertexPosRotationAxis50_g62096 + ( VertexPosOtherAxis82_g62096 * cos( Angle44_g62096 ) ) + ( cross( float3(1,0,0) , VertexPosOtherAxis82_g62096 ) * sin( Angle44_g62096 ) ) );
				float3 appendResult74_g62104 = (float3(0.0 , 0.0 , VertexPos40_g62104.z));
				half3 VertexPosRotationAxis50_g62104 = appendResult74_g62104;
				float3 break84_g62104 = VertexPos40_g62104;
				float3 appendResult81_g62104 = (float3(break84_g62104.x , break84_g62104.y , 0.0));
				half3 VertexPosOtherAxis82_g62104 = appendResult81_g62104;
				half Vertex_XAxisRotation216_g61524 = break4603_g61524.x;
				half Angle44_g62104 = -Vertex_XAxisRotation216_g61524;
				half3 VertexPos40_g62072 = ( VertexPosRotationAxis50_g62104 + ( VertexPosOtherAxis82_g62104 * cos( Angle44_g62104 ) ) + ( cross( float3(0,0,1) , VertexPosOtherAxis82_g62104 ) * sin( Angle44_g62104 ) ) );
				float3 appendResult74_g62072 = (float3(0.0 , VertexPos40_g62072.y , 0.0));
				float3 VertexPosRotationAxis50_g62072 = appendResult74_g62072;
				float3 break84_g62072 = VertexPos40_g62072;
				float3 appendResult81_g62072 = (float3(break84_g62072.x , 0.0 , break84_g62072.z));
				float3 VertexPosOtherAxis82_g62072 = appendResult81_g62072;
				half Mesh_Motion_2060_g61524 = v.ase_texcoord3.y;
				float lerpResult410_g62099 = lerp( TVE_MotionParamsMin.y , TVE_MotionParamsMax.y , Wind_Power369_g62099);
				half Global_MotionPower_203109_g61524 = ( lerpResult410_g62099 * Wind_Stop420_g62099 );
				half Motion_20_Variation4255_g61524 = ( _MotionVariation_20 * Motion_Variation3073_g61524 );
				half Variation127_g62091 = ( Motion_20_Variation4255_g61524 * Motion_Variation3073_g61524 );
				float mulTime131_g62091 = _TimeParameters.x * 0.5;
				float temp_output_134_0_g62091 = (sin( ( Variation127_g62091 + mulTime131_g62091 ) )*0.5 + 0.5);
				half Global_MotionPower2223_g61524 = ( Wind_Power369_g62099 * Wind_Stop420_g62099 );
				float temp_output_112_0_g62091 = Global_MotionPower2223_g61524;
				float lerpResult136_g62091 = lerp( ( temp_output_134_0_g62091 * temp_output_134_0_g62091 * temp_output_134_0_g62091 * temp_output_134_0_g62091 ) , 1.0 , ( temp_output_112_0_g62091 * temp_output_112_0_g62091 ));
				float lerpResult126_g62091 = lerp( lerpResult136_g62091 , 1.0 , ( 1.0 - saturate( Variation127_g62091 ) ));
				half Motion_Selective4260_g61524 = lerpResult126_g62091;
				half Motion_20_Mode162_g61524 = _MotionValue_20;
				half Motion_20_Commons4381_g61524 = ( Mesh_Motion_2060_g61524 * Global_MotionPower_203109_g61524 * Global_MotionNoise34_g61524 * Motion_Selective4260_g61524 * Motion_20_Mode162_g61524 );
				half Motion_20_Speed4257_g61524 = _MotionSpeed_20;
				half Input_Speed62_g62085 = Motion_20_Speed4257_g61524;
				float mulTime354_g62085 = _TimeParameters.x * Input_Speed62_g62085;
				float Motion_Variation284_g62085 = Motion_20_Variation4255_g61524;
				half Motion_20_Scale4256_g61524 = _MotionScale_20;
				float Motion_Scale287_g62085 = ( Motion_20_Scale4256_g61524 * ( ase_worldPos.x + ase_worldPos.y + ase_worldPos.z ) );
				half Motion_20_SineWaveA4382_g61524 = sin( ( mulTime354_g62085 + Motion_Variation284_g62085 + Motion_Scale287_g62085 ) );
				half ObjectData20_g62086 = 3.14;
				float Bounds_Radius121_g61524 = _MaxBoundsInfo.x;
				half WorldData19_g62086 = Bounds_Radius121_g61524;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float staticSwitch14_g62086 = WorldData19_g62086;
				#else
				float staticSwitch14_g62086 = ObjectData20_g62086;
				#endif
				float Motion_Max_Rolling1137_g61524 = staticSwitch14_g62086;
				half Motion_20_Rolling138_g61524 = ( _MotionAmplitude_20 * Motion_20_Commons4381_g61524 * Motion_20_SineWaveA4382_g61524 * Motion_Max_Rolling1137_g61524 );
				half Angle44_g62072 = Motion_20_Rolling138_g61524;
				half Input_Speed62_g62141 = ( Motion_20_Speed4257_g61524 - 1.0 );
				float mulTime354_g62141 = _TimeParameters.x * Input_Speed62_g62141;
				float Motion_Variation284_g62141 = Motion_20_Variation4255_g61524;
				float Motion_Scale287_g62141 = ( Motion_20_Scale4256_g61524 * ( ase_worldPos.x + ase_worldPos.y + ase_worldPos.z ) );
				half Motion_20_SineWaveB4460_g61524 = sin( ( mulTime354_g62141 + Motion_Variation284_g62141 + Motion_Scale287_g62141 ) );
				float3 appendResult4393_g61524 = (float3(0.0 , ( _MotionAmplitude_21 * Motion_20_Commons4381_g61524 * Motion_20_SineWaveB4460_g61524 * Bounds_Radius121_g61524 ) , 0.0));
				half3 Motion_20_Vertical4280_g61524 = appendResult4393_g61524;
				float2 break4421_g61524 = ( ( _MotionAmplitude_22 * Motion_20_Commons4381_g61524 * ( Bounds_Radius121_g61524 * 2.0 ) * (Motion_20_SineWaveA4382_g61524*0.2 + 1.0) ) * Global_MotionDirectionOS39_g61524 );
				float3 appendResult4417_g61524 = (float3(break4421_g61524.x , 0.0 , break4421_g61524.y));
				half3 Motion_20_Squash4418_g61524 = appendResult4417_g61524;
				half Motion_Scale321_g62079 = ( _MotionScale_32 * 10.0 );
				half Input_Speed62_g62079 = _MotionSpeed_32;
				float mulTime349_g62079 = _TimeParameters.x * Input_Speed62_g62079;
				float Motion_Variation330_g62079 = ( _MotionVariation_32 * Motion_Variation3073_g61524 );
				half Input_Amplitude58_g62079 = ( _MotionAmplitude_32 * Bounds_Radius121_g61524 * 0.1 );
				float3 temp_output_299_0_g62079 = ( sin( ( ( ase_worldPos * Motion_Scale321_g62079 ) + mulTime349_g62079 + Motion_Variation330_g62079 ) ) * Input_Amplitude58_g62079 );
				half Mesh_Motion_30144_g61524 = v.ase_texcoord3.z;
				float lerpResult378_g62099 = lerp( TVE_MotionParamsMin.z , TVE_MotionParamsMax.z , Wind_Power369_g62099);
				half Global_MotionPower_303115_g61524 = ( lerpResult378_g62099 * Wind_Stop420_g62099 );
				float temp_output_7_0_g62093 = TVE_MotionFadeEnd;
				half Wind_FadeOut4005_g61524 = saturate( ( ( distance( ase_worldPos , _WorldSpaceCameraPos ) - temp_output_7_0_g62093 ) / ( TVE_MotionFadeStart - temp_output_7_0_g62093 ) ) );
				half3 Motion_30_Details263_g61524 = ( temp_output_299_0_g62079 * ( Global_MotionNoise34_g61524 * Mesh_Motion_30144_g61524 * Global_MotionPower_303115_g61524 * Wind_FadeOut4005_g61524 * Motion_Selective4260_g61524 ) );
				float3 Vertex_Motion_Object833_g61524 = ( ( ( VertexPosRotationAxis50_g62072 + ( VertexPosOtherAxis82_g62072 * cos( Angle44_g62072 ) ) + ( cross( float3(0,1,0) , VertexPosOtherAxis82_g62072 ) * sin( Angle44_g62072 ) ) ) + Motion_20_Vertical4280_g61524 + Motion_20_Squash4418_g61524 ) + Motion_30_Details263_g61524 );
				float3 temp_output_3474_0_g61524 = ( VertexPosition3588_g61524 - Mesh_PivotsOS2291_g61524 );
				float3 appendResult2043_g61524 = (float3(Vertex_XAxisRotation216_g61524 , 0.0 , Vertex_ZAxisRotatin190_g61524));
				float3 appendResult2047_g61524 = (float3(Motion_20_Rolling138_g61524 , 0.0 , -Motion_20_Rolling138_g61524));
				float3 Vertex_Motion_World1118_g61524 = ( ( ( temp_output_3474_0_g61524 + appendResult2043_g61524 ) + appendResult2047_g61524 + Motion_20_Vertical4280_g61524 + Motion_20_Squash4418_g61524 ) + Motion_30_Details263_g61524 );
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch3312_g61524 = Vertex_Motion_World1118_g61524;
				#else
				float3 staticSwitch3312_g61524 = ( Vertex_Motion_Object833_g61524 + ( 0.0 * _VertexDataMode ) );
				#endif
				float4 temp_output_94_19_g62115 = TVE_VertexCoords;
				float3 Position83_g62115 = ObjectPosition4223_g61524;
				float temp_output_84_0_g62115 = _LayerVertexValue;
				float4 lerpResult87_g62115 = lerp( TVE_VertexParams , SAMPLE_TEXTURE2D_ARRAY_LOD( TVE_VertexTex, samplerTVE_VertexTex, ( (temp_output_94_19_g62115).zw + ( (temp_output_94_19_g62115).xy * (Position83_g62115).xz ) ),temp_output_84_0_g62115, 0.0 ) , TVE_VertexUsage[(int)temp_output_84_0_g62115]);
				half4 Global_Object_Params4173_g61524 = lerpResult87_g62115;
				half Global_VertexSize174_g61524 = Global_Object_Params4173_g61524.w;
				float lerpResult16_g62149 = lerp( 0.0 , _GlobalSize , TVE_Enabled);
				float lerpResult346_g61524 = lerp( 1.0 , Global_VertexSize174_g61524 , lerpResult16_g62149);
				float3 appendResult3480_g61524 = (float3(lerpResult346_g61524 , lerpResult346_g61524 , lerpResult346_g61524));
				half3 ObjectData20_g62147 = appendResult3480_g61524;
				half3 _Vector11 = half3(1,1,1);
				half3 WorldData19_g62147 = _Vector11;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g62147 = WorldData19_g62147;
				#else
				float3 staticSwitch14_g62147 = ObjectData20_g62147;
				#endif
				half3 Vertex_Size1741_g61524 = staticSwitch14_g62147;
				half3 _Vector5 = half3(1,1,1);
				float temp_output_7_0_g62100 = _SizeFadeEndValue;
				float temp_output_335_0_g61524 = saturate( ( ( ( distance( _WorldSpaceCameraPos , ObjectPosition4223_g61524 ) * ( 1.0 / TVE_DistanceFadeBias ) ) - temp_output_7_0_g62100 ) / ( _SizeFadeStartValue - temp_output_7_0_g62100 ) ) );
				float3 appendResult3482_g61524 = (float3(temp_output_335_0_g61524 , temp_output_335_0_g61524 , temp_output_335_0_g61524));
				float3 lerpResult3556_g61524 = lerp( _Vector5 , appendResult3482_g61524 , _SizeFadeMode);
				half3 ObjectData20_g62097 = lerpResult3556_g61524;
				half3 WorldData19_g62097 = _Vector5;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g62097 = WorldData19_g62097;
				#else
				float3 staticSwitch14_g62097 = ObjectData20_g62097;
				#endif
				float3 Vertex_SizeFade1740_g61524 = staticSwitch14_g62097;
				half3 Grass_Perspective2661_g61524 = half3(0,0,0);
				float3 Final_VertexPosition890_g61524 = ( ( ( staticSwitch3312_g61524 * Vertex_Size1741_g61524 * Vertex_SizeFade1740_g61524 ) + Mesh_PivotsOS2291_g61524 + Grass_Perspective2661_g61524 ) + _DisableSRPBatcher );
				
				half Mesh_Height1524_g61524 = v.ase_color.a;
				float temp_output_7_0_g62140 = _GradientMinValue;
				half Gradient_Tint2784_g61524 = saturate( ( ( Mesh_Height1524_g61524 - temp_output_7_0_g62140 ) / ( _GradientMaxValue - temp_output_7_0_g62140 ) ) );
				float vertexToFrag11_g62137 = Gradient_Tint2784_g61524;
				o.ase_texcoord2.x = vertexToFrag11_g62137;
				float3 temp_cast_8 = (_NoiseScaleValue).xxx;
				float3 WorldPosition3905_g61524 = vertexToFrag3890_g61524;
				float temp_output_7_0_g62120 = _NoiseMinValue;
				half Noise_Tint2802_g61524 = saturate( ( ( tex3Dlod( TVE_WorldTex3D, float4( ( temp_cast_8 * WorldPosition3905_g61524 * 0.1 ), 0.0) ).r - temp_output_7_0_g62120 ) / ( _NoiseMaxValue - temp_output_7_0_g62120 ) ) );
				float vertexToFrag11_g62143 = Noise_Tint2802_g61524;
				o.ase_texcoord2.y = vertexToFrag11_g62143;
				float2 vertexToFrag11_g62084 = ( ( v.ase_texcoord.xy * (_MainUVs).xy ) + (_MainUVs).zw );
				o.ase_texcoord2.zw = vertexToFrag11_g62084;
				o.ase_texcoord3.xyz = vertexToFrag3890_g61524;
				float3 ase_worldTangent = TransformObjectToWorldDir(v.ase_tangent.xyz);
				o.ase_texcoord4.xyz = ase_worldTangent;
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord5.xyz = ase_worldNormal;
				float ase_vertexTangentSign = v.ase_tangent.w * unity_WorldTransformParams.w;
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				o.ase_texcoord6.xyz = ase_worldBitangent;
				float Mesh_Occlusion318_g61524 = v.ase_color.g;
				float temp_output_7_0_g62136 = _VertexOcclusionMinValue;
				float temp_output_3377_0_g61524 = saturate( ( ( Mesh_Occlusion318_g61524 - temp_output_7_0_g62136 ) / ( _VertexOcclusionMaxValue - temp_output_7_0_g62136 ) ) );
				float vertexToFrag11_g62138 = temp_output_3377_0_g61524;
				o.ase_texcoord3.w = vertexToFrag11_g62138;
				
				float2 vertexToFrag11_g62119 = ( ( v.ase_texcoord.xy * (_EmissiveUVs).xy ) + (_EmissiveUVs).zw );
				o.ase_texcoord7.xy = vertexToFrag11_g62119;
				
				float temp_output_7_0_g62146 = TVE_CameraFadeStart;
				float lerpResult4755_g61524 = lerp( 1.0 , saturate( ( ( distance( ase_worldPos , _WorldSpaceCameraPos ) - temp_output_7_0_g62146 ) / ( TVE_CameraFadeEnd - temp_output_7_0_g62146 ) ) ) , _FadeCameraValue);
				float vertexToFrag11_g62121 = lerpResult4755_g61524;
				o.ase_texcoord4.w = vertexToFrag11_g62121;
				
				o.ase_color = v.ase_color;
				o.ase_normal = v.ase_normal;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord5.w = 0;
				o.ase_texcoord6.w = 0;
				o.ase_texcoord7.zw = 0;
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = Final_VertexPosition890_g61524;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				o.clipPos = MetaVertexPosition( v.vertex, v.texcoord1.xy, v.texcoord1.xy, unity_LightmapST, unity_DynamicLightmapST );
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = o.clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_tangent : TANGENT;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.texcoord1 = v.texcoord1;
				o.texcoord2 = v.texcoord2;
				o.ase_texcoord3 = v.ase_texcoord3;
				o.ase_color = v.ase_color;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_tangent = v.ase_tangent;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord2 = patch[0].texcoord2 * bary.x + patch[1].texcoord2 * bary.y + patch[2].texcoord2 * bary.z;
				o.ase_texcoord3 = patch[0].ase_texcoord3 * bary.x + patch[1].ase_texcoord3 * bary.y + patch[2].ase_texcoord3 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float vertexToFrag11_g62137 = IN.ase_texcoord2.x;
				float3 lerpResult2779_g61524 = lerp( (_GradientColorTwo).rgb , (_GradientColorOne).rgb , vertexToFrag11_g62137);
				float vertexToFrag11_g62143 = IN.ase_texcoord2.y;
				float3 lerpResult2800_g61524 = lerp( (_NoiseColorTwo).rgb , (_NoiseColorOne).rgb , vertexToFrag11_g62143);
				half Leaves_Mask4511_g61524 = 1.0;
				float3 lerpResult4521_g61524 = lerp( float3( 1,1,1 ) , ( lerpResult2779_g61524 * lerpResult2800_g61524 * float3(1,1,1) ) , Leaves_Mask4511_g61524);
				float3 lerpResult4519_g61524 = lerp( float3( 1,1,1 ) , (_MainColor).rgb , Leaves_Mask4511_g61524);
				float2 vertexToFrag11_g62084 = IN.ase_texcoord2.zw;
				half2 Main_UVs15_g61524 = vertexToFrag11_g62084;
				float4 tex2DNode29_g61524 = tex2D( _MainAlbedoTex, Main_UVs15_g61524 );
				half3 Main_Albedo99_g61524 = ( lerpResult4519_g61524 * (tex2DNode29_g61524).rgb );
				half3 Blend_Albedo265_g61524 = Main_Albedo99_g61524;
				half3 Blend_AlbedoTinted2808_g61524 = ( lerpResult4521_g61524 * Blend_Albedo265_g61524 );
				float dotResult3616_g61524 = dot( Blend_AlbedoTinted2808_g61524 , float3(0.2126,0.7152,0.0722) );
				float3 temp_cast_0 = (dotResult3616_g61524).xxx;
				float4 temp_output_91_19_g62122 = TVE_ColorsCoords;
				float3 vertexToFrag3890_g61524 = IN.ase_texcoord3.xyz;
				float3 WorldPosition3905_g61524 = vertexToFrag3890_g61524;
				float3 Position58_g62122 = WorldPosition3905_g61524;
				float temp_output_82_0_g62122 = _LayerColorsValue;
				float4 lerpResult88_g62122 = lerp( TVE_ColorsParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ColorsTex, samplerTVE_ColorsTex, ( (temp_output_91_19_g62122).zw + ( (temp_output_91_19_g62122).xy * (Position58_g62122).xz ) ),temp_output_82_0_g62122 ) , TVE_ColorsUsage[(int)temp_output_82_0_g62122]);
				half Global_ColorsTex_A1701_g61524 = (lerpResult88_g62122).a;
				half Global_Colors_Influence3668_g61524 = saturate( Global_ColorsTex_A1701_g61524 );
				float3 lerpResult3618_g61524 = lerp( Blend_AlbedoTinted2808_g61524 , temp_cast_0 , Global_Colors_Influence3668_g61524);
				half3 Global_ColorsTex_RGB1700_g61524 = (lerpResult88_g62122).rgb;
				#ifdef UNITY_COLORSPACE_GAMMA
				float staticSwitch1_g62151 = 2.0;
				#else
				float staticSwitch1_g62151 = 4.594794;
				#endif
				half3 Global_Colors1954_g61524 = ( Global_ColorsTex_RGB1700_g61524 * staticSwitch1_g62151 );
				float lerpResult3870_g61524 = lerp( 1.0 , IN.ase_color.r , _ColorsVariationValue);
				half Global_Colors_Value3650_g61524 = ( _GlobalColors * lerpResult3870_g61524 );
				float4 tex2DNode35_g61524 = tex2D( _MainMaskTex, Main_UVs15_g61524 );
				half Main_Mask57_g61524 = tex2DNode35_g61524.b;
				float temp_output_7_0_g62131 = _ColorsMaskMinValue;
				half Global_Colors_Mask3692_g61524 = saturate( ( ( Main_Mask57_g61524 - temp_output_7_0_g62131 ) / ( _ColorsMaskMaxValue - temp_output_7_0_g62131 ) ) );
				float lerpResult16_g62101 = lerp( 0.0 , ( Global_Colors_Value3650_g61524 * Global_Colors_Mask3692_g61524 ) , TVE_Enabled);
				float3 lerpResult3628_g61524 = lerp( Blend_AlbedoTinted2808_g61524 , ( lerpResult3618_g61524 * Global_Colors1954_g61524 ) , lerpResult16_g62101);
				half3 Blend_AlbedoColored863_g61524 = lerpResult3628_g61524;
				float3 temp_output_799_0_g61524 = (_SubsurfaceColor).rgb;
				float dotResult3930_g61524 = dot( temp_output_799_0_g61524 , float3(0.2126,0.7152,0.0722) );
				float3 temp_cast_3 = (dotResult3930_g61524).xxx;
				float3 lerpResult3932_g61524 = lerp( temp_output_799_0_g61524 , temp_cast_3 , Global_Colors_Influence3668_g61524);
				float3 lerpResult3942_g61524 = lerp( temp_output_799_0_g61524 , ( lerpResult3932_g61524 * Global_Colors1954_g61524 ) , ( Global_Colors_Value3650_g61524 * Global_Colors_Mask3692_g61524 ));
				half3 Subsurface_Color1722_g61524 = lerpResult3942_g61524;
				half MainLight_Subsurface4041_g61524 = TVE_MainLightParams.a;
				half Subsurface_Intensity1752_g61524 = ( _SubsurfaceValue * MainLight_Subsurface4041_g61524 );
				float temp_output_7_0_g62148 = _SubsurfaceMaskMinValue;
				half Subsurface_Mask1557_g61524 = saturate( ( ( Main_Mask57_g61524 - temp_output_7_0_g62148 ) / ( _SubsurfaceMaskMaxValue - temp_output_7_0_g62148 ) ) );
				half3 Subsurface_Transmission884_g61524 = ( Subsurface_Color1722_g61524 * Subsurface_Intensity1752_g61524 * Subsurface_Mask1557_g61524 );
				half3 MainLight_Direction3926_g61524 = TVE_MainLightDirection;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 normalizeResult2169_g61524 = normalize( ase_worldViewDir );
				float3 ViewDir_Normalized3963_g61524 = normalizeResult2169_g61524;
				float dotResult785_g61524 = dot( -MainLight_Direction3926_g61524 , ViewDir_Normalized3963_g61524 );
				float saferPower1624_g61524 = abs( (dotResult785_g61524*0.5 + 0.5) );
				#ifdef UNITY_PASS_FORWARDADD
				float staticSwitch1602_g61524 = 0.0;
				#else
				float staticSwitch1602_g61524 = ( pow( saferPower1624_g61524 , _MainLightAngleValue ) * _MainLightScatteringValue );
				#endif
				half Mask_Subsurface_View782_g61524 = staticSwitch1602_g61524;
				float3 unpack4112_g61524 = UnpackNormalScale( tex2D( _MainNormalTex, Main_UVs15_g61524 ), _MainNormalValue );
				unpack4112_g61524.z = lerp( 1, unpack4112_g61524.z, saturate(_MainNormalValue) );
				half3 Main_Normal137_g61524 = unpack4112_g61524;
				float3 ase_worldTangent = IN.ase_texcoord4.xyz;
				float3 ase_worldNormal = IN.ase_texcoord5.xyz;
				float3 ase_worldBitangent = IN.ase_texcoord6.xyz;
				float3 tanToWorld0 = float3( ase_worldTangent.x, ase_worldBitangent.x, ase_worldNormal.x );
				float3 tanToWorld1 = float3( ase_worldTangent.y, ase_worldBitangent.y, ase_worldNormal.y );
				float3 tanToWorld2 = float3( ase_worldTangent.z, ase_worldBitangent.z, ase_worldNormal.z );
				float3 tanNormal4099_g61524 = Main_Normal137_g61524;
				float3 worldNormal4099_g61524 = float3(dot(tanToWorld0,tanNormal4099_g61524), dot(tanToWorld1,tanNormal4099_g61524), dot(tanToWorld2,tanNormal4099_g61524));
				float3 Main_Normal_WS4101_g61524 = worldNormal4099_g61524;
				float dotResult777_g61524 = dot( MainLight_Direction3926_g61524 , Main_Normal_WS4101_g61524 );
				float lerpResult4198_g61524 = lerp( 1.0 , saturate( dotResult777_g61524 ) , _MainLightNormalValue);
				#ifdef UNITY_PASS_FORWARDADD
				float staticSwitch1604_g61524 = 0.0;
				#else
				float staticSwitch1604_g61524 = lerpResult4198_g61524;
				#endif
				half Mask_Subsurface_Normal870_g61524 = staticSwitch1604_g61524;
				half3 Subsurface_Scattering1693_g61524 = ( Subsurface_Transmission884_g61524 * Blend_AlbedoColored863_g61524 * ( Mask_Subsurface_View782_g61524 * Mask_Subsurface_Normal870_g61524 ) );
				half3 Blend_AlbedoAndSubsurface149_g61524 = ( Blend_AlbedoColored863_g61524 + Subsurface_Scattering1693_g61524 );
				half3 Global_OverlayColor1758_g61524 = (TVE_OverlayColor).rgb;
				half VertexDynamicMode4798_g61524 = _VertexDynamicMode;
				float lerpResult4801_g61524 = lerp( Main_Normal_WS4101_g61524.y , IN.ase_normal.y , VertexDynamicMode4798_g61524);
				float lerpResult3567_g61524 = lerp( 0.5 , 1.0 , lerpResult4801_g61524);
				half Main_AlbedoTex_G3526_g61524 = tex2DNode29_g61524.g;
				float4 temp_output_93_19_g62109 = TVE_ExtrasCoords;
				float3 Position82_g62109 = WorldPosition3905_g61524;
				float temp_output_84_0_g62109 = _LayerExtrasValue;
				float4 lerpResult88_g62109 = lerp( TVE_ExtrasParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ExtrasTex, samplerTVE_ExtrasTex, ( (temp_output_93_19_g62109).zw + ( (temp_output_93_19_g62109).xy * (Position82_g62109).xz ) ),temp_output_84_0_g62109 ) , TVE_ExtrasUsage[(int)temp_output_84_0_g62109]);
				float4 break89_g62109 = lerpResult88_g62109;
				half Global_Extras_Overlay156_g61524 = break89_g62109.b;
				float lerpResult1065_g61524 = lerp( 1.0 , IN.ase_color.r , _OverlayVariationValue);
				half Overlay_Variation4560_g61524 = lerpResult1065_g61524;
				half Overlay_Commons1365_g61524 = ( _GlobalOverlay * Global_Extras_Overlay156_g61524 * Overlay_Variation4560_g61524 );
				float temp_output_7_0_g62145 = _OverlayMaskMinValue;
				half Overlay_Mask269_g61524 = saturate( ( ( ( ( ( lerpResult3567_g61524 * 0.5 ) + Main_AlbedoTex_G3526_g61524 ) * Overlay_Commons1365_g61524 ) - temp_output_7_0_g62145 ) / ( _OverlayMaskMaxValue - temp_output_7_0_g62145 ) ) );
				float3 lerpResult336_g61524 = lerp( Blend_AlbedoAndSubsurface149_g61524 , Global_OverlayColor1758_g61524 , Overlay_Mask269_g61524);
				half3 Final_Albedo359_g61524 = lerpResult336_g61524;
				float3 temp_cast_7 = (1.0).xxx;
				float vertexToFrag11_g62138 = IN.ase_texcoord3.w;
				float3 lerpResult2945_g61524 = lerp( (_VertexOcclusionColor).rgb , temp_cast_7 , vertexToFrag11_g62138);
				float3 Vertex_Occlusion648_g61524 = lerpResult2945_g61524;
				
				float4 temp_output_4214_0_g61524 = ( _EmissiveColor * _EmissiveIntensityParams.x );
				float2 vertexToFrag11_g62119 = IN.ase_texcoord7.xy;
				half2 Emissive_UVs2468_g61524 = vertexToFrag11_g62119;
				half Global_Extras_Emissive4203_g61524 = break89_g62109.r;
				float lerpResult4206_g61524 = lerp( 1.0 , Global_Extras_Emissive4203_g61524 , _GlobalEmissive);
				half3 Final_Emissive2476_g61524 = ( (( temp_output_4214_0_g61524 * tex2D( _EmissiveTex, Emissive_UVs2468_g61524 ) )).rgb * lerpResult4206_g61524 );
				
				float localCustomAlphaClip3735_g61524 = ( 0.0 );
				float3 normalizeResult3971_g61524 = normalize( cross( ddy( WorldPosition ) , ddx( WorldPosition ) ) );
				float3 NormalsWS_Derivates3972_g61524 = normalizeResult3971_g61524;
				float dotResult3851_g61524 = dot( ViewDir_Normalized3963_g61524 , NormalsWS_Derivates3972_g61524 );
				float lerpResult3993_g61524 = lerp( 1.0 , saturate( abs( dotResult3851_g61524 ) ) , _FadeGlancingValue);
				half Fade_Glancing3853_g61524 = lerpResult3993_g61524;
				float vertexToFrag11_g62121 = IN.ase_texcoord4.w;
				half Fade_Camera3743_g61524 = vertexToFrag11_g62121;
				float temp_output_41_0_g62144 = ( Fade_Glancing3853_g61524 * Fade_Camera3743_g61524 );
				half Final_AlphaFade3727_g61524 = saturate( ( temp_output_41_0_g62144 + ( temp_output_41_0_g62144 * tex3D( TVE_ScreenTex3D, ( TVE_ScreenTexCoord * WorldPosition3905_g61524 ) ).r ) ) );
				float Main_Alpha316_g61524 = ( _MainColor.a * tex2DNode29_g61524.a );
				float Mesh_Variation16_g61524 = IN.ase_color.r;
				float temp_output_4023_0_g61524 = (Mesh_Variation16_g61524*0.5 + 0.5);
				half Global_Extras_Alpha1033_g61524 = break89_g62109.a;
				float temp_output_4022_0_g61524 = ( temp_output_4023_0_g61524 - ( 1.0 - Global_Extras_Alpha1033_g61524 ) );
				half AlphaTreshold2132_g61524 = _Cutoff;
				#ifdef TVE_ALPHA_CLIP
				float staticSwitch4017_g61524 = ( temp_output_4022_0_g61524 + AlphaTreshold2132_g61524 );
				#else
				float staticSwitch4017_g61524 = temp_output_4022_0_g61524;
				#endif
				float lerpResult16_g62113 = lerp( 0.0 , _GlobalAlpha , TVE_Enabled);
				float lerpResult4011_g61524 = lerp( 1.0 , staticSwitch4017_g61524 , lerpResult16_g62113);
				half Global_Alpha315_g61524 = saturate( lerpResult4011_g61524 );
				#ifdef TVE_ALPHA_CLIP
				float staticSwitch3792_g61524 = ( ( Main_Alpha316_g61524 * Global_Alpha315_g61524 ) - ( AlphaTreshold2132_g61524 - 0.5 ) );
				#else
				float staticSwitch3792_g61524 = ( Main_Alpha316_g61524 * Global_Alpha315_g61524 );
				#endif
				half Final_Alpha3754_g61524 = staticSwitch3792_g61524;
				float temp_output_661_0_g61524 = ( Final_AlphaFade3727_g61524 * Final_Alpha3754_g61524 );
				float Alpha3735_g61524 = temp_output_661_0_g61524;
				float Treshold3735_g61524 = 0.5;
				{
				#if TVE_ALPHA_CLIP
				clip(Alpha3735_g61524 - Treshold3735_g61524);
				#endif
				}
				half Final_Clip914_g61524 = saturate( Alpha3735_g61524 );
				
				
				float3 Albedo = ( Final_Albedo359_g61524 * Vertex_Occlusion648_g61524 );
				float3 Emission = Final_Emissive2476_g61524;
				float Alpha = Final_Clip914_g61524;
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				MetaInput metaInput = (MetaInput)0;
				metaInput.Albedo = Albedo;
				metaInput.Emission = Emission;
				
				return MetaFragment(metaInput);
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "Universal2D"
			Tags { "LightMode"="Universal2D" }

			Blend [_render_src] [_render_dst], One Zero
			ZWrite [_render_zw]
			ZTest LEqual
			Offset 0,0
			ColorMask RGBA

			HLSLPROGRAM
			#define _SPECULAR_SETUP 1
			#define _NORMAL_DROPOFF_TS 1
			#define _TRANSMISSION_ASE 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#pragma multi_compile _ DOTS_INSTANCING_ON
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _EMISSION
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70403

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x 

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_2D

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			
			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature_local TVE_ALPHA_CLIP
			#pragma shader_feature_local TVE_VERTEX_DATA_BATCHED
			//TVE Pipeline Defines
			#define THE_VEGETATION_ENGINE
			#define TVE_IS_UNIVERSAL_PIPELINE
			//TVE Shader Type Defines
			#define TVE_IS_VEGETATION_SHADER
			//TVE Injection Defines
			//SHADER INJECTION POINT BEGIN
			//SHADER INJECTION POINT END


			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_tangent : TANGENT;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				float4 ase_texcoord6 : TEXCOORD6;
				float3 ase_normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			half4 _DetailBlendRemap;
			half4 _OverlayMaskRemap;
			half4 _ColorsMaskRemap;
			half4 _SubsurfaceMaskRemap;
			half4 _LeavesMaskRemap;
			half4 _NoiseColorOne;
			half4 _SubsurfaceColor;
			half4 _AlphaMaskRemap;
			half4 _VertexOcclusionRemap;
			half4 _VertexOcclusionColor;
			half4 _GradientColorTwo;
			float4 _NoiseMaskRemap;
			half4 _GradientColorOne;
			half4 _MainColor;
			half4 _NoiseColorTwo;
			float4 _GradientMaskRemap;
			float4 _SubsurfaceDiffusion_asset;
			float4 _MaxBoundsInfo;
			half4 _EmissiveColor;
			half4 _MainUVs;
			float4 _SubsurfaceDiffusion_Asset;
			half4 _EmissiveUVs;
			float4 _EmissiveIntensityParams;
			float4 _Color;
			half3 _render_normals_options;
			half _SizeFadeStartValue;
			half _SizeFadeMode;
			half _GradientMinValue;
			half _GradientMaxValue;
			half _SizeFadeEndValue;
			half _VertexDataMode;
			half _LayerVertexValue;
			half _MotionAmplitude_32;
			float _MotionVariation_32;
			float _MotionSpeed_32;
			float _MotionScale_32;
			half _MotionAmplitude_22;
			half _MotionAmplitude_21;
			half _MotionScale_20;
			half _MotionSpeed_20;
			half _MotionValue_20;
			half _MotionVariation_20;
			half _MotionAmplitude_20;
			half _GlobalSize;
			half _render_cull;
			half _LayerColorsValue;
			half _NoiseMinValue;
			half _FadeGlancingValue;
			half _MainOcclusionValue;
			half _GlobalWetness;
			half _MainSmoothnessValue;
			half _RenderSpecular;
			half _GlobalEmissive;
			half _VertexOcclusionMaxValue;
			half _VertexOcclusionMinValue;
			half _OverlayMaskMaxValue;
			half _OverlayMaskMinValue;
			half _OverlayVariationValue;
			half _LayerExtrasValue;
			half _NoiseScaleValue;
			half _GlobalOverlay;
			half _MainNormalValue;
			half _MainLightScatteringValue;
			half _MainLightAngleValue;
			half _SubsurfaceMaskMaxValue;
			half _SubsurfaceMaskMinValue;
			half _SubsurfaceValue;
			half _ColorsMaskMaxValue;
			half _ColorsMaskMinValue;
			half _ColorsVariationValue;
			half _GlobalColors;
			half _InteractionMaskValue;
			half _NoiseMaxValue;
			half _MainLightNormalValue;
			half _InteractionAmplitude;
			half _IsSubsurfaceShader;
			half _VertexDynamicMode;
			half _SizeFadeCat;
			float _SubsurfaceDiffusion;
			half _VariationGlobalsMessage;
			half _RenderCull;
			half _HasEmissive;
			half _FadeSpace;
			half _MotionCat;
			half _IsVersion;
			half _DetailCat;
			half _HasGradient;
			half _SubsurfaceCat;
			half _VertexMasksMode;
			half _PerspectiveCat;
			half _TranslucencyNormalValue;
			half _TranslucencyHDMessage;
			half _RenderMode;
			half _RenderDecals;
			half _TranslucencyAmbientValue;
			half _RenderingCat;
			half _DetailBlendMode;
			half _GradientCat;
			half _DetailTypeMode;
			half _RenderQueue;
			half _TranslucencyIntensityValue;
			half _RenderPriority;
			half _EmissiveCat;
			half _subsurface_shadow;
			half _render_src;
			half _VariationMotionMessage;
			float _MotionScale_10;
			half _TranslucencyScatteringValue;
			half _DetailMode;
			half _MotionVariation_10;
			float _MotionSpeed_10;
			half _MotionAmplitude_10;
			half _LayerMotionValue;
			half _FadeCameraValue;
			half _IsLeafShader;
			half _render_dst;
			half _render_zw;
			half _RenderClip;
			half _VertexRollingMode;
			half _VertexVariationMode;
			half _SizeFadeMessage;
			half _Cutoff;
			half _MotionSpace;
			half _TranslucencyShadowValue;
			half _LayersSpace;
			half _LayerReactValue;
			half _MainCat;
			half _ReceiveSpace;
			half _RenderSSR;
			half _IsTVEShader;
			half _EmissiveFlagMode;
			half _RenderZWrite;
			half _HasOcclusion;
			half _OcclusionCat;
			half _GlobalCat;
			half _RenderNormals;
			half _TranslucencyDirectValue;
			half _NoiseCat;
			half _GlobalAlpha;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			half TVE_Enabled;
			sampler2D _BumpMap;
			sampler2D _MainTex;
			half4 TVE_MotionParams;
			TEXTURE2D_ARRAY(TVE_MotionTex);
			half4 TVE_MotionCoords;
			SAMPLER(samplerTVE_MotionTex);
			float TVE_MotionUsage[10];
			half4 TVE_MotionParamsMin;
			half4 TVE_MotionParamsMax;
			sampler2D TVE_NoiseTex;
			half4 TVE_NoiseParams;
			half TVE_MotionFadeEnd;
			half TVE_MotionFadeStart;
			half4 TVE_VertexParams;
			TEXTURE2D_ARRAY(TVE_VertexTex);
			half4 TVE_VertexCoords;
			SAMPLER(samplerTVE_VertexTex);
			float TVE_VertexUsage[10];
			half TVE_DistanceFadeBias;
			half _DisableSRPBatcher;
			sampler3D TVE_WorldTex3D;
			sampler2D _MainAlbedoTex;
			half4 TVE_ColorsParams;
			TEXTURE2D_ARRAY(TVE_ColorsTex);
			half4 TVE_ColorsCoords;
			SAMPLER(samplerTVE_ColorsTex);
			float TVE_ColorsUsage[10];
			sampler2D _MainMaskTex;
			half4 TVE_MainLightParams;
			half3 TVE_MainLightDirection;
			sampler2D _MainNormalTex;
			half4 TVE_OverlayColor;
			half4 TVE_ExtrasParams;
			TEXTURE2D_ARRAY(TVE_ExtrasTex);
			half4 TVE_ExtrasCoords;
			SAMPLER(samplerTVE_ExtrasTex);
			float TVE_ExtrasUsage[10];
			half TVE_CameraFadeStart;
			half TVE_CameraFadeEnd;
			sampler3D TVE_ScreenTex3D;
			half TVE_ScreenTexCoord;


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float3 VertexPosition3588_g61524 = v.vertex.xyz;
				half3 Mesh_PivotsOS2291_g61524 = half3(0,0,0);
				float3 temp_output_2283_0_g61524 = ( VertexPosition3588_g61524 - Mesh_PivotsOS2291_g61524 );
				half3 VertexPos40_g62096 = temp_output_2283_0_g61524;
				float3 appendResult74_g62096 = (float3(VertexPos40_g62096.x , 0.0 , 0.0));
				half3 VertexPosRotationAxis50_g62096 = appendResult74_g62096;
				float3 break84_g62096 = VertexPos40_g62096;
				float3 appendResult81_g62096 = (float3(0.0 , break84_g62096.y , break84_g62096.z));
				half3 VertexPosOtherAxis82_g62096 = appendResult81_g62096;
				float4 temp_output_91_19_g62127 = TVE_MotionCoords;
				float4x4 break19_g62080 = GetObjectToWorldMatrix();
				float3 appendResult20_g62080 = (float3(break19_g62080[ 0 ][ 3 ] , break19_g62080[ 1 ][ 3 ] , break19_g62080[ 2 ][ 3 ]));
				half3 ObjectData20_g62081 = appendResult20_g62080;
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				half3 WorldData19_g62081 = ase_worldPos;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g62081 = WorldData19_g62081;
				#else
				float3 staticSwitch14_g62081 = ObjectData20_g62081;
				#endif
				float3 temp_output_114_0_g62080 = staticSwitch14_g62081;
				float3 vertexToFrag4224_g61524 = temp_output_114_0_g62080;
				half3 ObjectData20_g62075 = vertexToFrag4224_g61524;
				float3 vertexToFrag3890_g61524 = ase_worldPos;
				half3 WorldData19_g62075 = vertexToFrag3890_g61524;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g62075 = WorldData19_g62075;
				#else
				float3 staticSwitch14_g62075 = ObjectData20_g62075;
				#endif
				float3 ObjectPosition4223_g61524 = staticSwitch14_g62075;
				float3 Position83_g62127 = ObjectPosition4223_g61524;
				float temp_output_84_0_g62127 = _LayerMotionValue;
				float4 lerpResult87_g62127 = lerp( TVE_MotionParams , SAMPLE_TEXTURE2D_ARRAY_LOD( TVE_MotionTex, samplerTVE_MotionTex, ( (temp_output_91_19_g62127).zw + ( (temp_output_91_19_g62127).xy * (Position83_g62127).xz ) ),temp_output_84_0_g62127, 0.0 ) , TVE_MotionUsage[(int)temp_output_84_0_g62127]);
				half4 Global_Motion_Params3909_g61524 = lerpResult87_g62127;
				float4 break322_g62099 = Global_Motion_Params3909_g61524;
				float3 appendResult397_g62099 = (float3(break322_g62099.x , 0.0 , break322_g62099.y));
				float3 temp_output_398_0_g62099 = (appendResult397_g62099*2.0 + -1.0);
				float3 ase_parentObjectScale = ( 1.0 / float3( length( GetWorldToObjectMatrix()[ 0 ].xyz ), length( GetWorldToObjectMatrix()[ 1 ].xyz ), length( GetWorldToObjectMatrix()[ 2 ].xyz ) ) );
				half2 Global_MotionDirectionOS39_g61524 = (( mul( GetWorldToObjectMatrix(), float4( temp_output_398_0_g62099 , 0.0 ) ).xyz * ase_parentObjectScale )).xz;
				half ObjectData20_g62106 = 3.14;
				float Bounds_Height374_g61524 = _MaxBoundsInfo.y;
				half WorldData19_g62106 = ( Bounds_Height374_g61524 * 3.14 );
				#ifdef TVE_VERTEX_DATA_BATCHED
				float staticSwitch14_g62106 = WorldData19_g62106;
				#else
				float staticSwitch14_g62106 = ObjectData20_g62106;
				#endif
				float Motion_Max_Bending1133_g61524 = staticSwitch14_g62106;
				half Mesh_Motion_1082_g61524 = v.ase_texcoord3.x;
				half Motion_10_Mask4617_g61524 = ( _MotionAmplitude_10 * Motion_Max_Bending1133_g61524 * Mesh_Motion_1082_g61524 );
				half Input_Speed62_g62087 = _MotionSpeed_10;
				float mulTime373_g62087 = _TimeParameters.x * Input_Speed62_g62087;
				float3 break111_g62098 = ObjectPosition4223_g61524;
				float Mesh_Variation16_g61524 = v.ase_color.r;
				half VertexDynamicMode4798_g61524 = _VertexDynamicMode;
				float lerpResult4795_g61524 = lerp( frac( ( v.ase_color.r + ( break111_g62098.x + break111_g62098.z ) ) ) , Mesh_Variation16_g61524 , VertexDynamicMode4798_g61524);
				half ObjectData20_g62094 = lerpResult4795_g61524;
				half WorldData19_g62094 = Mesh_Variation16_g61524;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float staticSwitch14_g62094 = WorldData19_g62094;
				#else
				float staticSwitch14_g62094 = ObjectData20_g62094;
				#endif
				half Motion_Variation3073_g61524 = staticSwitch14_g62094;
				half Motion_10_Variation4581_g61524 = ( _MotionVariation_10 * Motion_Variation3073_g61524 );
				half Motion_Variation284_g62087 = Motion_10_Variation4581_g61524;
				float Motion_Scale287_g62087 = ( _MotionScale_10 * ( ase_worldPos.x + ase_worldPos.y + ase_worldPos.z ) );
				half Sine_MinusOneToOne281_g62087 = sin( ( mulTime373_g62087 + Motion_Variation284_g62087 + Motion_Scale287_g62087 ) );
				half Input_WindSquash419_g62087 = 0.2;
				half Wind_Power369_g62099 = break322_g62099.z;
				float lerpResult376_g62099 = lerp( TVE_MotionParamsMin.x , TVE_MotionParamsMax.x , Wind_Power369_g62099);
				half Wind_Stop420_g62099 = saturate( ( Wind_Power369_g62099 * 10.0 ) );
				half Global_MotionPower_103106_g61524 = ( lerpResult376_g62099 * Wind_Stop420_g62099 );
				half Input_WindPower327_g62087 = Global_MotionPower_103106_g61524;
				float lerpResult321_g62087 = lerp( Sine_MinusOneToOne281_g62087 , (Sine_MinusOneToOne281_g62087*Input_WindSquash419_g62087 + 1.0) , Input_WindPower327_g62087);
				half Motion_10_SinWaveAm4570_g61524 = lerpResult321_g62087;
				float2 panner437_g62099 = ( _TimeParameters.x * (TVE_NoiseParams).xy + ( (ObjectPosition4223_g61524).xz * TVE_NoiseParams.z ));
				float saferPower446_g62099 = abs( abs( tex2Dlod( TVE_NoiseTex, float4( panner437_g62099, 0, 0.0) ).r ) );
				float lerpResult448_g62099 = lerp( TVE_MotionParamsMin.w , TVE_MotionParamsMax.w , Wind_Power369_g62099);
				half Global_MotionNoise34_g61524 = pow( saferPower446_g62099 , lerpResult448_g62099 );
				half Motion_10_Bending2258_g61524 = ( Motion_10_Mask4617_g61524 * Motion_10_SinWaveAm4570_g61524 * Global_MotionPower_103106_g61524 * Global_MotionNoise34_g61524 );
				half Interaction_Amplitude4137_g61524 = _InteractionAmplitude;
				float lerpResult4494_g61524 = lerp( 1.0 , Mesh_Motion_1082_g61524 , _InteractionMaskValue);
				half ObjectData20_g62102 = lerpResult4494_g61524;
				half WorldData19_g62102 = Mesh_Motion_1082_g61524;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float staticSwitch14_g62102 = WorldData19_g62102;
				#else
				float staticSwitch14_g62102 = ObjectData20_g62102;
				#endif
				half Motion_10_Interaction53_g61524 = ( Interaction_Amplitude4137_g61524 * Motion_Max_Bending1133_g61524 * staticSwitch14_g62102 );
				half Global_InteractionMask66_g61524 = ( break322_g62099.w * break322_g62099.w );
				float lerpResult4685_g61524 = lerp( Motion_10_Bending2258_g61524 , Motion_10_Interaction53_g61524 , saturate( ( Global_InteractionMask66_g61524 * Interaction_Amplitude4137_g61524 ) ));
				float2 break4603_g61524 = ( Global_MotionDirectionOS39_g61524 * lerpResult4685_g61524 );
				half Vertex_ZAxisRotatin190_g61524 = break4603_g61524.y;
				half Angle44_g62096 = Vertex_ZAxisRotatin190_g61524;
				half3 VertexPos40_g62104 = ( VertexPosRotationAxis50_g62096 + ( VertexPosOtherAxis82_g62096 * cos( Angle44_g62096 ) ) + ( cross( float3(1,0,0) , VertexPosOtherAxis82_g62096 ) * sin( Angle44_g62096 ) ) );
				float3 appendResult74_g62104 = (float3(0.0 , 0.0 , VertexPos40_g62104.z));
				half3 VertexPosRotationAxis50_g62104 = appendResult74_g62104;
				float3 break84_g62104 = VertexPos40_g62104;
				float3 appendResult81_g62104 = (float3(break84_g62104.x , break84_g62104.y , 0.0));
				half3 VertexPosOtherAxis82_g62104 = appendResult81_g62104;
				half Vertex_XAxisRotation216_g61524 = break4603_g61524.x;
				half Angle44_g62104 = -Vertex_XAxisRotation216_g61524;
				half3 VertexPos40_g62072 = ( VertexPosRotationAxis50_g62104 + ( VertexPosOtherAxis82_g62104 * cos( Angle44_g62104 ) ) + ( cross( float3(0,0,1) , VertexPosOtherAxis82_g62104 ) * sin( Angle44_g62104 ) ) );
				float3 appendResult74_g62072 = (float3(0.0 , VertexPos40_g62072.y , 0.0));
				float3 VertexPosRotationAxis50_g62072 = appendResult74_g62072;
				float3 break84_g62072 = VertexPos40_g62072;
				float3 appendResult81_g62072 = (float3(break84_g62072.x , 0.0 , break84_g62072.z));
				float3 VertexPosOtherAxis82_g62072 = appendResult81_g62072;
				half Mesh_Motion_2060_g61524 = v.ase_texcoord3.y;
				float lerpResult410_g62099 = lerp( TVE_MotionParamsMin.y , TVE_MotionParamsMax.y , Wind_Power369_g62099);
				half Global_MotionPower_203109_g61524 = ( lerpResult410_g62099 * Wind_Stop420_g62099 );
				half Motion_20_Variation4255_g61524 = ( _MotionVariation_20 * Motion_Variation3073_g61524 );
				half Variation127_g62091 = ( Motion_20_Variation4255_g61524 * Motion_Variation3073_g61524 );
				float mulTime131_g62091 = _TimeParameters.x * 0.5;
				float temp_output_134_0_g62091 = (sin( ( Variation127_g62091 + mulTime131_g62091 ) )*0.5 + 0.5);
				half Global_MotionPower2223_g61524 = ( Wind_Power369_g62099 * Wind_Stop420_g62099 );
				float temp_output_112_0_g62091 = Global_MotionPower2223_g61524;
				float lerpResult136_g62091 = lerp( ( temp_output_134_0_g62091 * temp_output_134_0_g62091 * temp_output_134_0_g62091 * temp_output_134_0_g62091 ) , 1.0 , ( temp_output_112_0_g62091 * temp_output_112_0_g62091 ));
				float lerpResult126_g62091 = lerp( lerpResult136_g62091 , 1.0 , ( 1.0 - saturate( Variation127_g62091 ) ));
				half Motion_Selective4260_g61524 = lerpResult126_g62091;
				half Motion_20_Mode162_g61524 = _MotionValue_20;
				half Motion_20_Commons4381_g61524 = ( Mesh_Motion_2060_g61524 * Global_MotionPower_203109_g61524 * Global_MotionNoise34_g61524 * Motion_Selective4260_g61524 * Motion_20_Mode162_g61524 );
				half Motion_20_Speed4257_g61524 = _MotionSpeed_20;
				half Input_Speed62_g62085 = Motion_20_Speed4257_g61524;
				float mulTime354_g62085 = _TimeParameters.x * Input_Speed62_g62085;
				float Motion_Variation284_g62085 = Motion_20_Variation4255_g61524;
				half Motion_20_Scale4256_g61524 = _MotionScale_20;
				float Motion_Scale287_g62085 = ( Motion_20_Scale4256_g61524 * ( ase_worldPos.x + ase_worldPos.y + ase_worldPos.z ) );
				half Motion_20_SineWaveA4382_g61524 = sin( ( mulTime354_g62085 + Motion_Variation284_g62085 + Motion_Scale287_g62085 ) );
				half ObjectData20_g62086 = 3.14;
				float Bounds_Radius121_g61524 = _MaxBoundsInfo.x;
				half WorldData19_g62086 = Bounds_Radius121_g61524;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float staticSwitch14_g62086 = WorldData19_g62086;
				#else
				float staticSwitch14_g62086 = ObjectData20_g62086;
				#endif
				float Motion_Max_Rolling1137_g61524 = staticSwitch14_g62086;
				half Motion_20_Rolling138_g61524 = ( _MotionAmplitude_20 * Motion_20_Commons4381_g61524 * Motion_20_SineWaveA4382_g61524 * Motion_Max_Rolling1137_g61524 );
				half Angle44_g62072 = Motion_20_Rolling138_g61524;
				half Input_Speed62_g62141 = ( Motion_20_Speed4257_g61524 - 1.0 );
				float mulTime354_g62141 = _TimeParameters.x * Input_Speed62_g62141;
				float Motion_Variation284_g62141 = Motion_20_Variation4255_g61524;
				float Motion_Scale287_g62141 = ( Motion_20_Scale4256_g61524 * ( ase_worldPos.x + ase_worldPos.y + ase_worldPos.z ) );
				half Motion_20_SineWaveB4460_g61524 = sin( ( mulTime354_g62141 + Motion_Variation284_g62141 + Motion_Scale287_g62141 ) );
				float3 appendResult4393_g61524 = (float3(0.0 , ( _MotionAmplitude_21 * Motion_20_Commons4381_g61524 * Motion_20_SineWaveB4460_g61524 * Bounds_Radius121_g61524 ) , 0.0));
				half3 Motion_20_Vertical4280_g61524 = appendResult4393_g61524;
				float2 break4421_g61524 = ( ( _MotionAmplitude_22 * Motion_20_Commons4381_g61524 * ( Bounds_Radius121_g61524 * 2.0 ) * (Motion_20_SineWaveA4382_g61524*0.2 + 1.0) ) * Global_MotionDirectionOS39_g61524 );
				float3 appendResult4417_g61524 = (float3(break4421_g61524.x , 0.0 , break4421_g61524.y));
				half3 Motion_20_Squash4418_g61524 = appendResult4417_g61524;
				half Motion_Scale321_g62079 = ( _MotionScale_32 * 10.0 );
				half Input_Speed62_g62079 = _MotionSpeed_32;
				float mulTime349_g62079 = _TimeParameters.x * Input_Speed62_g62079;
				float Motion_Variation330_g62079 = ( _MotionVariation_32 * Motion_Variation3073_g61524 );
				half Input_Amplitude58_g62079 = ( _MotionAmplitude_32 * Bounds_Radius121_g61524 * 0.1 );
				float3 temp_output_299_0_g62079 = ( sin( ( ( ase_worldPos * Motion_Scale321_g62079 ) + mulTime349_g62079 + Motion_Variation330_g62079 ) ) * Input_Amplitude58_g62079 );
				half Mesh_Motion_30144_g61524 = v.ase_texcoord3.z;
				float lerpResult378_g62099 = lerp( TVE_MotionParamsMin.z , TVE_MotionParamsMax.z , Wind_Power369_g62099);
				half Global_MotionPower_303115_g61524 = ( lerpResult378_g62099 * Wind_Stop420_g62099 );
				float temp_output_7_0_g62093 = TVE_MotionFadeEnd;
				half Wind_FadeOut4005_g61524 = saturate( ( ( distance( ase_worldPos , _WorldSpaceCameraPos ) - temp_output_7_0_g62093 ) / ( TVE_MotionFadeStart - temp_output_7_0_g62093 ) ) );
				half3 Motion_30_Details263_g61524 = ( temp_output_299_0_g62079 * ( Global_MotionNoise34_g61524 * Mesh_Motion_30144_g61524 * Global_MotionPower_303115_g61524 * Wind_FadeOut4005_g61524 * Motion_Selective4260_g61524 ) );
				float3 Vertex_Motion_Object833_g61524 = ( ( ( VertexPosRotationAxis50_g62072 + ( VertexPosOtherAxis82_g62072 * cos( Angle44_g62072 ) ) + ( cross( float3(0,1,0) , VertexPosOtherAxis82_g62072 ) * sin( Angle44_g62072 ) ) ) + Motion_20_Vertical4280_g61524 + Motion_20_Squash4418_g61524 ) + Motion_30_Details263_g61524 );
				float3 temp_output_3474_0_g61524 = ( VertexPosition3588_g61524 - Mesh_PivotsOS2291_g61524 );
				float3 appendResult2043_g61524 = (float3(Vertex_XAxisRotation216_g61524 , 0.0 , Vertex_ZAxisRotatin190_g61524));
				float3 appendResult2047_g61524 = (float3(Motion_20_Rolling138_g61524 , 0.0 , -Motion_20_Rolling138_g61524));
				float3 Vertex_Motion_World1118_g61524 = ( ( ( temp_output_3474_0_g61524 + appendResult2043_g61524 ) + appendResult2047_g61524 + Motion_20_Vertical4280_g61524 + Motion_20_Squash4418_g61524 ) + Motion_30_Details263_g61524 );
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch3312_g61524 = Vertex_Motion_World1118_g61524;
				#else
				float3 staticSwitch3312_g61524 = ( Vertex_Motion_Object833_g61524 + ( 0.0 * _VertexDataMode ) );
				#endif
				float4 temp_output_94_19_g62115 = TVE_VertexCoords;
				float3 Position83_g62115 = ObjectPosition4223_g61524;
				float temp_output_84_0_g62115 = _LayerVertexValue;
				float4 lerpResult87_g62115 = lerp( TVE_VertexParams , SAMPLE_TEXTURE2D_ARRAY_LOD( TVE_VertexTex, samplerTVE_VertexTex, ( (temp_output_94_19_g62115).zw + ( (temp_output_94_19_g62115).xy * (Position83_g62115).xz ) ),temp_output_84_0_g62115, 0.0 ) , TVE_VertexUsage[(int)temp_output_84_0_g62115]);
				half4 Global_Object_Params4173_g61524 = lerpResult87_g62115;
				half Global_VertexSize174_g61524 = Global_Object_Params4173_g61524.w;
				float lerpResult16_g62149 = lerp( 0.0 , _GlobalSize , TVE_Enabled);
				float lerpResult346_g61524 = lerp( 1.0 , Global_VertexSize174_g61524 , lerpResult16_g62149);
				float3 appendResult3480_g61524 = (float3(lerpResult346_g61524 , lerpResult346_g61524 , lerpResult346_g61524));
				half3 ObjectData20_g62147 = appendResult3480_g61524;
				half3 _Vector11 = half3(1,1,1);
				half3 WorldData19_g62147 = _Vector11;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g62147 = WorldData19_g62147;
				#else
				float3 staticSwitch14_g62147 = ObjectData20_g62147;
				#endif
				half3 Vertex_Size1741_g61524 = staticSwitch14_g62147;
				half3 _Vector5 = half3(1,1,1);
				float temp_output_7_0_g62100 = _SizeFadeEndValue;
				float temp_output_335_0_g61524 = saturate( ( ( ( distance( _WorldSpaceCameraPos , ObjectPosition4223_g61524 ) * ( 1.0 / TVE_DistanceFadeBias ) ) - temp_output_7_0_g62100 ) / ( _SizeFadeStartValue - temp_output_7_0_g62100 ) ) );
				float3 appendResult3482_g61524 = (float3(temp_output_335_0_g61524 , temp_output_335_0_g61524 , temp_output_335_0_g61524));
				float3 lerpResult3556_g61524 = lerp( _Vector5 , appendResult3482_g61524 , _SizeFadeMode);
				half3 ObjectData20_g62097 = lerpResult3556_g61524;
				half3 WorldData19_g62097 = _Vector5;
				#ifdef TVE_VERTEX_DATA_BATCHED
				float3 staticSwitch14_g62097 = WorldData19_g62097;
				#else
				float3 staticSwitch14_g62097 = ObjectData20_g62097;
				#endif
				float3 Vertex_SizeFade1740_g61524 = staticSwitch14_g62097;
				half3 Grass_Perspective2661_g61524 = half3(0,0,0);
				float3 Final_VertexPosition890_g61524 = ( ( ( staticSwitch3312_g61524 * Vertex_Size1741_g61524 * Vertex_SizeFade1740_g61524 ) + Mesh_PivotsOS2291_g61524 + Grass_Perspective2661_g61524 ) + _DisableSRPBatcher );
				
				half Mesh_Height1524_g61524 = v.ase_color.a;
				float temp_output_7_0_g62140 = _GradientMinValue;
				half Gradient_Tint2784_g61524 = saturate( ( ( Mesh_Height1524_g61524 - temp_output_7_0_g62140 ) / ( _GradientMaxValue - temp_output_7_0_g62140 ) ) );
				float vertexToFrag11_g62137 = Gradient_Tint2784_g61524;
				o.ase_texcoord2.x = vertexToFrag11_g62137;
				float3 temp_cast_8 = (_NoiseScaleValue).xxx;
				float3 WorldPosition3905_g61524 = vertexToFrag3890_g61524;
				float temp_output_7_0_g62120 = _NoiseMinValue;
				half Noise_Tint2802_g61524 = saturate( ( ( tex3Dlod( TVE_WorldTex3D, float4( ( temp_cast_8 * WorldPosition3905_g61524 * 0.1 ), 0.0) ).r - temp_output_7_0_g62120 ) / ( _NoiseMaxValue - temp_output_7_0_g62120 ) ) );
				float vertexToFrag11_g62143 = Noise_Tint2802_g61524;
				o.ase_texcoord2.y = vertexToFrag11_g62143;
				float2 vertexToFrag11_g62084 = ( ( v.ase_texcoord.xy * (_MainUVs).xy ) + (_MainUVs).zw );
				o.ase_texcoord2.zw = vertexToFrag11_g62084;
				o.ase_texcoord3.xyz = vertexToFrag3890_g61524;
				float3 ase_worldTangent = TransformObjectToWorldDir(v.ase_tangent.xyz);
				o.ase_texcoord4.xyz = ase_worldTangent;
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord5.xyz = ase_worldNormal;
				float ase_vertexTangentSign = v.ase_tangent.w * unity_WorldTransformParams.w;
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				o.ase_texcoord6.xyz = ase_worldBitangent;
				float Mesh_Occlusion318_g61524 = v.ase_color.g;
				float temp_output_7_0_g62136 = _VertexOcclusionMinValue;
				float temp_output_3377_0_g61524 = saturate( ( ( Mesh_Occlusion318_g61524 - temp_output_7_0_g62136 ) / ( _VertexOcclusionMaxValue - temp_output_7_0_g62136 ) ) );
				float vertexToFrag11_g62138 = temp_output_3377_0_g61524;
				o.ase_texcoord3.w = vertexToFrag11_g62138;
				
				float temp_output_7_0_g62146 = TVE_CameraFadeStart;
				float lerpResult4755_g61524 = lerp( 1.0 , saturate( ( ( distance( ase_worldPos , _WorldSpaceCameraPos ) - temp_output_7_0_g62146 ) / ( TVE_CameraFadeEnd - temp_output_7_0_g62146 ) ) ) , _FadeCameraValue);
				float vertexToFrag11_g62121 = lerpResult4755_g61524;
				o.ase_texcoord4.w = vertexToFrag11_g62121;
				
				o.ase_color = v.ase_color;
				o.ase_normal = v.ase_normal;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord5.w = 0;
				o.ase_texcoord6.w = 0;
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = Final_VertexPosition890_g61524;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_tangent : TANGENT;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord3 = v.ase_texcoord3;
				o.ase_color = v.ase_color;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_tangent = v.ase_tangent;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord3 = patch[0].ase_texcoord3 * bary.x + patch[1].ase_texcoord3 * bary.y + patch[2].ase_texcoord3 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float vertexToFrag11_g62137 = IN.ase_texcoord2.x;
				float3 lerpResult2779_g61524 = lerp( (_GradientColorTwo).rgb , (_GradientColorOne).rgb , vertexToFrag11_g62137);
				float vertexToFrag11_g62143 = IN.ase_texcoord2.y;
				float3 lerpResult2800_g61524 = lerp( (_NoiseColorTwo).rgb , (_NoiseColorOne).rgb , vertexToFrag11_g62143);
				half Leaves_Mask4511_g61524 = 1.0;
				float3 lerpResult4521_g61524 = lerp( float3( 1,1,1 ) , ( lerpResult2779_g61524 * lerpResult2800_g61524 * float3(1,1,1) ) , Leaves_Mask4511_g61524);
				float3 lerpResult4519_g61524 = lerp( float3( 1,1,1 ) , (_MainColor).rgb , Leaves_Mask4511_g61524);
				float2 vertexToFrag11_g62084 = IN.ase_texcoord2.zw;
				half2 Main_UVs15_g61524 = vertexToFrag11_g62084;
				float4 tex2DNode29_g61524 = tex2D( _MainAlbedoTex, Main_UVs15_g61524 );
				half3 Main_Albedo99_g61524 = ( lerpResult4519_g61524 * (tex2DNode29_g61524).rgb );
				half3 Blend_Albedo265_g61524 = Main_Albedo99_g61524;
				half3 Blend_AlbedoTinted2808_g61524 = ( lerpResult4521_g61524 * Blend_Albedo265_g61524 );
				float dotResult3616_g61524 = dot( Blend_AlbedoTinted2808_g61524 , float3(0.2126,0.7152,0.0722) );
				float3 temp_cast_0 = (dotResult3616_g61524).xxx;
				float4 temp_output_91_19_g62122 = TVE_ColorsCoords;
				float3 vertexToFrag3890_g61524 = IN.ase_texcoord3.xyz;
				float3 WorldPosition3905_g61524 = vertexToFrag3890_g61524;
				float3 Position58_g62122 = WorldPosition3905_g61524;
				float temp_output_82_0_g62122 = _LayerColorsValue;
				float4 lerpResult88_g62122 = lerp( TVE_ColorsParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ColorsTex, samplerTVE_ColorsTex, ( (temp_output_91_19_g62122).zw + ( (temp_output_91_19_g62122).xy * (Position58_g62122).xz ) ),temp_output_82_0_g62122 ) , TVE_ColorsUsage[(int)temp_output_82_0_g62122]);
				half Global_ColorsTex_A1701_g61524 = (lerpResult88_g62122).a;
				half Global_Colors_Influence3668_g61524 = saturate( Global_ColorsTex_A1701_g61524 );
				float3 lerpResult3618_g61524 = lerp( Blend_AlbedoTinted2808_g61524 , temp_cast_0 , Global_Colors_Influence3668_g61524);
				half3 Global_ColorsTex_RGB1700_g61524 = (lerpResult88_g62122).rgb;
				#ifdef UNITY_COLORSPACE_GAMMA
				float staticSwitch1_g62151 = 2.0;
				#else
				float staticSwitch1_g62151 = 4.594794;
				#endif
				half3 Global_Colors1954_g61524 = ( Global_ColorsTex_RGB1700_g61524 * staticSwitch1_g62151 );
				float lerpResult3870_g61524 = lerp( 1.0 , IN.ase_color.r , _ColorsVariationValue);
				half Global_Colors_Value3650_g61524 = ( _GlobalColors * lerpResult3870_g61524 );
				float4 tex2DNode35_g61524 = tex2D( _MainMaskTex, Main_UVs15_g61524 );
				half Main_Mask57_g61524 = tex2DNode35_g61524.b;
				float temp_output_7_0_g62131 = _ColorsMaskMinValue;
				half Global_Colors_Mask3692_g61524 = saturate( ( ( Main_Mask57_g61524 - temp_output_7_0_g62131 ) / ( _ColorsMaskMaxValue - temp_output_7_0_g62131 ) ) );
				float lerpResult16_g62101 = lerp( 0.0 , ( Global_Colors_Value3650_g61524 * Global_Colors_Mask3692_g61524 ) , TVE_Enabled);
				float3 lerpResult3628_g61524 = lerp( Blend_AlbedoTinted2808_g61524 , ( lerpResult3618_g61524 * Global_Colors1954_g61524 ) , lerpResult16_g62101);
				half3 Blend_AlbedoColored863_g61524 = lerpResult3628_g61524;
				float3 temp_output_799_0_g61524 = (_SubsurfaceColor).rgb;
				float dotResult3930_g61524 = dot( temp_output_799_0_g61524 , float3(0.2126,0.7152,0.0722) );
				float3 temp_cast_3 = (dotResult3930_g61524).xxx;
				float3 lerpResult3932_g61524 = lerp( temp_output_799_0_g61524 , temp_cast_3 , Global_Colors_Influence3668_g61524);
				float3 lerpResult3942_g61524 = lerp( temp_output_799_0_g61524 , ( lerpResult3932_g61524 * Global_Colors1954_g61524 ) , ( Global_Colors_Value3650_g61524 * Global_Colors_Mask3692_g61524 ));
				half3 Subsurface_Color1722_g61524 = lerpResult3942_g61524;
				half MainLight_Subsurface4041_g61524 = TVE_MainLightParams.a;
				half Subsurface_Intensity1752_g61524 = ( _SubsurfaceValue * MainLight_Subsurface4041_g61524 );
				float temp_output_7_0_g62148 = _SubsurfaceMaskMinValue;
				half Subsurface_Mask1557_g61524 = saturate( ( ( Main_Mask57_g61524 - temp_output_7_0_g62148 ) / ( _SubsurfaceMaskMaxValue - temp_output_7_0_g62148 ) ) );
				half3 Subsurface_Transmission884_g61524 = ( Subsurface_Color1722_g61524 * Subsurface_Intensity1752_g61524 * Subsurface_Mask1557_g61524 );
				half3 MainLight_Direction3926_g61524 = TVE_MainLightDirection;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 normalizeResult2169_g61524 = normalize( ase_worldViewDir );
				float3 ViewDir_Normalized3963_g61524 = normalizeResult2169_g61524;
				float dotResult785_g61524 = dot( -MainLight_Direction3926_g61524 , ViewDir_Normalized3963_g61524 );
				float saferPower1624_g61524 = abs( (dotResult785_g61524*0.5 + 0.5) );
				#ifdef UNITY_PASS_FORWARDADD
				float staticSwitch1602_g61524 = 0.0;
				#else
				float staticSwitch1602_g61524 = ( pow( saferPower1624_g61524 , _MainLightAngleValue ) * _MainLightScatteringValue );
				#endif
				half Mask_Subsurface_View782_g61524 = staticSwitch1602_g61524;
				float3 unpack4112_g61524 = UnpackNormalScale( tex2D( _MainNormalTex, Main_UVs15_g61524 ), _MainNormalValue );
				unpack4112_g61524.z = lerp( 1, unpack4112_g61524.z, saturate(_MainNormalValue) );
				half3 Main_Normal137_g61524 = unpack4112_g61524;
				float3 ase_worldTangent = IN.ase_texcoord4.xyz;
				float3 ase_worldNormal = IN.ase_texcoord5.xyz;
				float3 ase_worldBitangent = IN.ase_texcoord6.xyz;
				float3 tanToWorld0 = float3( ase_worldTangent.x, ase_worldBitangent.x, ase_worldNormal.x );
				float3 tanToWorld1 = float3( ase_worldTangent.y, ase_worldBitangent.y, ase_worldNormal.y );
				float3 tanToWorld2 = float3( ase_worldTangent.z, ase_worldBitangent.z, ase_worldNormal.z );
				float3 tanNormal4099_g61524 = Main_Normal137_g61524;
				float3 worldNormal4099_g61524 = float3(dot(tanToWorld0,tanNormal4099_g61524), dot(tanToWorld1,tanNormal4099_g61524), dot(tanToWorld2,tanNormal4099_g61524));
				float3 Main_Normal_WS4101_g61524 = worldNormal4099_g61524;
				float dotResult777_g61524 = dot( MainLight_Direction3926_g61524 , Main_Normal_WS4101_g61524 );
				float lerpResult4198_g61524 = lerp( 1.0 , saturate( dotResult777_g61524 ) , _MainLightNormalValue);
				#ifdef UNITY_PASS_FORWARDADD
				float staticSwitch1604_g61524 = 0.0;
				#else
				float staticSwitch1604_g61524 = lerpResult4198_g61524;
				#endif
				half Mask_Subsurface_Normal870_g61524 = staticSwitch1604_g61524;
				half3 Subsurface_Scattering1693_g61524 = ( Subsurface_Transmission884_g61524 * Blend_AlbedoColored863_g61524 * ( Mask_Subsurface_View782_g61524 * Mask_Subsurface_Normal870_g61524 ) );
				half3 Blend_AlbedoAndSubsurface149_g61524 = ( Blend_AlbedoColored863_g61524 + Subsurface_Scattering1693_g61524 );
				half3 Global_OverlayColor1758_g61524 = (TVE_OverlayColor).rgb;
				half VertexDynamicMode4798_g61524 = _VertexDynamicMode;
				float lerpResult4801_g61524 = lerp( Main_Normal_WS4101_g61524.y , IN.ase_normal.y , VertexDynamicMode4798_g61524);
				float lerpResult3567_g61524 = lerp( 0.5 , 1.0 , lerpResult4801_g61524);
				half Main_AlbedoTex_G3526_g61524 = tex2DNode29_g61524.g;
				float4 temp_output_93_19_g62109 = TVE_ExtrasCoords;
				float3 Position82_g62109 = WorldPosition3905_g61524;
				float temp_output_84_0_g62109 = _LayerExtrasValue;
				float4 lerpResult88_g62109 = lerp( TVE_ExtrasParams , SAMPLE_TEXTURE2D_ARRAY( TVE_ExtrasTex, samplerTVE_ExtrasTex, ( (temp_output_93_19_g62109).zw + ( (temp_output_93_19_g62109).xy * (Position82_g62109).xz ) ),temp_output_84_0_g62109 ) , TVE_ExtrasUsage[(int)temp_output_84_0_g62109]);
				float4 break89_g62109 = lerpResult88_g62109;
				half Global_Extras_Overlay156_g61524 = break89_g62109.b;
				float lerpResult1065_g61524 = lerp( 1.0 , IN.ase_color.r , _OverlayVariationValue);
				half Overlay_Variation4560_g61524 = lerpResult1065_g61524;
				half Overlay_Commons1365_g61524 = ( _GlobalOverlay * Global_Extras_Overlay156_g61524 * Overlay_Variation4560_g61524 );
				float temp_output_7_0_g62145 = _OverlayMaskMinValue;
				half Overlay_Mask269_g61524 = saturate( ( ( ( ( ( lerpResult3567_g61524 * 0.5 ) + Main_AlbedoTex_G3526_g61524 ) * Overlay_Commons1365_g61524 ) - temp_output_7_0_g62145 ) / ( _OverlayMaskMaxValue - temp_output_7_0_g62145 ) ) );
				float3 lerpResult336_g61524 = lerp( Blend_AlbedoAndSubsurface149_g61524 , Global_OverlayColor1758_g61524 , Overlay_Mask269_g61524);
				half3 Final_Albedo359_g61524 = lerpResult336_g61524;
				float3 temp_cast_7 = (1.0).xxx;
				float vertexToFrag11_g62138 = IN.ase_texcoord3.w;
				float3 lerpResult2945_g61524 = lerp( (_VertexOcclusionColor).rgb , temp_cast_7 , vertexToFrag11_g62138);
				float3 Vertex_Occlusion648_g61524 = lerpResult2945_g61524;
				
				float localCustomAlphaClip3735_g61524 = ( 0.0 );
				float3 normalizeResult3971_g61524 = normalize( cross( ddy( WorldPosition ) , ddx( WorldPosition ) ) );
				float3 NormalsWS_Derivates3972_g61524 = normalizeResult3971_g61524;
				float dotResult3851_g61524 = dot( ViewDir_Normalized3963_g61524 , NormalsWS_Derivates3972_g61524 );
				float lerpResult3993_g61524 = lerp( 1.0 , saturate( abs( dotResult3851_g61524 ) ) , _FadeGlancingValue);
				half Fade_Glancing3853_g61524 = lerpResult3993_g61524;
				float vertexToFrag11_g62121 = IN.ase_texcoord4.w;
				half Fade_Camera3743_g61524 = vertexToFrag11_g62121;
				float temp_output_41_0_g62144 = ( Fade_Glancing3853_g61524 * Fade_Camera3743_g61524 );
				half Final_AlphaFade3727_g61524 = saturate( ( temp_output_41_0_g62144 + ( temp_output_41_0_g62144 * tex3D( TVE_ScreenTex3D, ( TVE_ScreenTexCoord * WorldPosition3905_g61524 ) ).r ) ) );
				float Main_Alpha316_g61524 = ( _MainColor.a * tex2DNode29_g61524.a );
				float Mesh_Variation16_g61524 = IN.ase_color.r;
				float temp_output_4023_0_g61524 = (Mesh_Variation16_g61524*0.5 + 0.5);
				half Global_Extras_Alpha1033_g61524 = break89_g62109.a;
				float temp_output_4022_0_g61524 = ( temp_output_4023_0_g61524 - ( 1.0 - Global_Extras_Alpha1033_g61524 ) );
				half AlphaTreshold2132_g61524 = _Cutoff;
				#ifdef TVE_ALPHA_CLIP
				float staticSwitch4017_g61524 = ( temp_output_4022_0_g61524 + AlphaTreshold2132_g61524 );
				#else
				float staticSwitch4017_g61524 = temp_output_4022_0_g61524;
				#endif
				float lerpResult16_g62113 = lerp( 0.0 , _GlobalAlpha , TVE_Enabled);
				float lerpResult4011_g61524 = lerp( 1.0 , staticSwitch4017_g61524 , lerpResult16_g62113);
				half Global_Alpha315_g61524 = saturate( lerpResult4011_g61524 );
				#ifdef TVE_ALPHA_CLIP
				float staticSwitch3792_g61524 = ( ( Main_Alpha316_g61524 * Global_Alpha315_g61524 ) - ( AlphaTreshold2132_g61524 - 0.5 ) );
				#else
				float staticSwitch3792_g61524 = ( Main_Alpha316_g61524 * Global_Alpha315_g61524 );
				#endif
				half Final_Alpha3754_g61524 = staticSwitch3792_g61524;
				float temp_output_661_0_g61524 = ( Final_AlphaFade3727_g61524 * Final_Alpha3754_g61524 );
				float Alpha3735_g61524 = temp_output_661_0_g61524;
				float Treshold3735_g61524 = 0.5;
				{
				#if TVE_ALPHA_CLIP
				clip(Alpha3735_g61524 - Treshold3735_g61524);
				#endif
				}
				half Final_Clip914_g61524 = saturate( Alpha3735_g61524 );
				
				
				float3 Albedo = ( Final_Albedo359_g61524 * Vertex_Occlusion648_g61524 );
				float Alpha = Final_Clip914_g61524;
				float AlphaClipThreshold = 0.5;

				half4 color = half4( Albedo, Alpha );

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				return color;
			}
			ENDHLSL
		}
		
	}
	
	CustomEditor "TVEShaderCoreGUI"
	Fallback "Hidden/BOXOPHOBIC/The Vegetation Engine/Fallback"
	
}
/*ASEBEGIN
Version=18934
1920;0;1920;1029;2817.212;264.7897;1;True;False
Node;AmplifyShaderEditor.FunctionNode;1023;-1856,512;Inherit;False;Define Pipeline Universal;-1;;56547;71dc7add32e5f6247b1fb74ecceddd3e;0;0;1;FLOAT;529
Node;AmplifyShaderEditor.RangedFloatNode;822;-1984,-768;Half;False;Property;_IsSubsurfaceShader;_IsSubsurfaceShader;205;1;[HideInInspector];Create;True;0;0;0;True;0;False;1;1;1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1808,-640;Half;False;Property;_render_dst;_render_dst;208;1;[HideInInspector];Create;True;0;2;Opaque;0;Transparent;1;0;True;0;False;10;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;1024;-1344,-768;Inherit;False;Compile All Shaders;-1;;62152;e67c8238031dbf04ab79a5d4d63d1b4f;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1632,-640;Half;False;Property;_render_zw;_render_zw;209;1;[HideInInspector];Create;True;0;2;Opaque;0;Transparent;1;0;True;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;1008;-2177,-257;Inherit;False;Base Shader;0;;61524;856f7164d1c579d43a5cf4968a75ca43;82,3880,1,4028,1,4029,1,3900,1,3903,1,4204,1,3904,1,3908,1,4172,1,1300,1,1298,1,4179,1,3586,0,4499,1,3658,1,1708,1,3509,1,1712,2,3873,1,893,1,4544,1,1717,1,1714,1,1715,1,1718,1,916,1,1763,0,1762,0,3568,1,1949,1,1776,1,3475,1,4210,1,1745,1,3479,0,4510,0,3501,1,3221,2,1646,1,1757,1,1271,1,3889,0,2807,1,3886,0,2953,1,3887,0,3243,0,3888,0,3957,1,2172,1,3883,0,3728,1,3949,0,4781,0,2658,0,1742,1,3484,0,3575,0,1737,0,1733,0,1735,0,1734,0,1736,0,878,0,1550,0,4069,0,4070,0,4067,0,4072,0,4068,0,860,1,2260,1,2261,1,2032,1,2054,1,2062,1,2039,1,4177,1,4217,1,3592,1,2750,0,4242,0;0;19;FLOAT3;0;FLOAT3;528;FLOAT3;2489;FLOAT;531;FLOAT;4135;FLOAT;529;FLOAT;3678;FLOAT;530;FLOAT;4127;FLOAT;4122;FLOAT;4134;FLOAT;1235;FLOAT3;1230;FLOAT;1461;FLOAT;1290;FLOAT;721;FLOAT;532;FLOAT;629;FLOAT3;534
Node;AmplifyShaderEditor.RangedFloatNode;168;-2176,-768;Half;False;Property;_IsLeafShader;_IsLeafShader;204;1;[HideInInspector];Create;True;0;0;0;True;0;False;1;1;1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;907;-1456,-640;Half;False;Property;_subsurface_shadow;_subsurface_shadow;203;1;[HideInInspector];Create;True;0;0;0;True;0;False;1;0;1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-2000,-640;Half;False;Property;_render_src;_render_src;207;1;[HideInInspector];Create;True;0;0;0;True;0;False;5;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;1025;-1536,-768;Inherit;False;Compile Core;-1;;61522;634b02fd1f32e6a4c875d8fc2c450956;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-2176,-640;Half;False;Property;_render_cull;_render_cull;206;1;[HideInInspector];Create;True;0;3;Both;0;Back;1;Front;2;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;471;-2176,512;Inherit;False;Define Shader Vegetation;-1;;61523;b458122dd75182d488380bd0f592b9e6;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1016;-1376,-256;Float;False;True;-1;2;TVEShaderCoreGUI;0;14;BOXOPHOBIC/The Vegetation Engine/Vegetation/Leaf Subsurface Lit;28cd5599e02859647ae1798e4fcaef6c;True;Forward;0;1;Forward;18;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;True;True;2;True;10;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;4;False;0;True;True;1;1;True;20;0;True;7;1;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;True;1;True;17;True;0;False;-1;True;False;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;Hidden/BOXOPHOBIC/The Vegetation Engine/Fallback;0;0;Standard;38;Workflow;0;0;Surface;0;0;  Refraction Model;0;0;  Blend;0;0;Two Sided;0;0;Fragment Normal Space,InvertActionOnDeselection;0;0;Transmission;1;0;  Transmission Shadow;0.5,True,907;0;Translucency;0;0;  Translucency Strength;1,False,-1;0;  Normal Distortion;0.5,False,-1;0;  Scattering;2,False,-1;0;  Direct;0.9,False,-1;0;  Ambient;0.1,False,-1;0;  Shadow;0.5,False,-1;0;Cast Shadows;1;0;  Use Shadow Threshold;0;0;Receive Shadows;1;0;GPU Instancing;1;0;LOD CrossFade;1;0;Built-in Fog;1;0;_FinalColorxAlpha;0;0;Meta Pass;1;0;Override Baked GI;0;0;Extra Pre Pass;0;0;DOTS Instancing;1;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,-1;0;  Type;0;0;  Tess;16,False,-1;0;  Min;10,False,-1;0;  Max;25,False,-1;0;  Edge Length;16,False,-1;0;  Max Displacement;25,False,-1;0;Write Depth;0;0;  Early Z;0;0;Vertex Position,InvertActionOnDeselection;0;0;0;6;False;True;True;True;True;True;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1015;-1376,-256;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;28cd5599e02859647ae1798e4fcaef6c;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;False;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1020;-1376,-256;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;28cd5599e02859647ae1798e4fcaef6c;True;Universal2D;0;5;Universal2D;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;False;0;False;True;1;1;True;20;0;True;7;1;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;True;17;True;0;False;-1;True;False;0;False;-1;0;False;-1;True;1;LightMode=Universal2D;False;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1017;-1376,-256;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;28cd5599e02859647ae1798e4fcaef6c;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;False;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1019;-1376,-256;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;28cd5599e02859647ae1798e4fcaef6c;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1018;-1376,-256;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;28cd5599e02859647ae1798e4fcaef6c;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;False;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.CommentaryNode;33;-2176,-384;Inherit;False;1024.392;100;Final;0;;0,1,0.5,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;449;-2176,384;Inherit;False;1026.438;100;Features;0;;0,1,0.5,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;37;-2176,-896;Inherit;False;1024.392;100;Internal;0;;1,0.252,0,1;0;0
WireConnection;1016;0;1008;0
WireConnection;1016;1;1008;528
WireConnection;1016;2;1008;2489
WireConnection;1016;9;1008;3678
WireConnection;1016;4;1008;530
WireConnection;1016;5;1008;531
WireConnection;1016;6;1008;532
WireConnection;1016;14;1008;1230
WireConnection;1016;8;1008;534
ASEEND*/
//CHKSM=5EC817508E6227C7B6D457D5165EC9D905732AE6
