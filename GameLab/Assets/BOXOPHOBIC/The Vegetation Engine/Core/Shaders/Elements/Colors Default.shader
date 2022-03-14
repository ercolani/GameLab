// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BOXOPHOBIC/The Vegetation Engine/Elements/Default/Colors Default"
{
	Properties
	{
		[StyledBanner(Color Element)]_Banner("Banner", Float) = 0
		[StyledMessage(Info, Use the Colors elements to add color tinting to the vegetation assets. Element Texture A is used as alpha mask. Particle Color is used as color multiplier and Alpha as Element Intensity multiplier., 0,0)]_Message("Message", Float) = 0
		[StyledCategory(Render Settings)]_RenderCat("[ Render Cat ]", Float) = 0
		_ElementIntensity("Render Intensity", Range( 0 , 1)) = 1
		[StyledMessage(Warning, When using all layers the Global Volume will create one render texture for each layer to render the elements. Try using fewer layers when possible., _ElementLayerWarning, 1, 10, 10)]_ElementLayerWarning("Render Layer Warning", Float) = 0
		[StyledMessage(Info, When using a higher Layer number the Global Volume will create more render textures to render the elements. Try using fewer layers when possible., _ElementLayerMessage, 1, 10, 10)]_ElementLayerMessage("Render Layer Message", Float) = 0
		[StyledMask(TVELayers, Default 0 Layer_1 1 Layer_2 2 Layer_3 3 Layer_4 4 Layer_5 5 Layer_6 6 Layer_7 7 Layer_8 8, 0, 0)]_ElementLayerMask("Render Layer", Float) = 1
		[Enum(Constant,0,Seasons,1)]_ElementMode("Render Mode", Float) = 0
		[Enum(Multiply Material Colors,14,Replace Material Colors,15)]_ElementColorsMode("Render Effect", Float) = 15
		[StyledCategory(Element Settings)]_ElementCat("[ Element Cat ]", Float) = 0
		[NoScaleOffset][StyledTextureSingleLine]_MainTex("Element Texture", 2D) = "white" {}
		[Space(10)][StyledRemapSlider(_MainTexMinValue, _MainTexMaxValue, 0, 1)]_MainTexRemap("Element Remap", Vector) = (0,0,0,0)
		[HideInInspector]_MainTexMinValue("Element Min", Range( 0 , 1)) = 0
		[HideInInspector]_MainTexMaxValue("Element Max", Range( 0 , 1)) = 1
		[StyledVector(9)]_MainUVs("Element UVs", Vector) = (1,1,0,0)
		[HDR][Gamma]_MainColor("Element Color", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR][Gamma]_AdditionalColor1("Winter Color", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR][Gamma]_AdditionalColor2("Spring Color", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR][Gamma]_AdditionalColor3("Summer Color", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR][Gamma]_AdditionalColor4("Autumn Color", Color) = (0.5019608,0.5019608,0.5019608,1)
		[StyledRemapSlider(_NoiseMinValue, _NoiseMaxValue, 0, 1, 10, 0)]_NoiseRemap("Noise Remap", Vector) = (0,0,0,0)
		[StyledCategory(Fading Settings)]_FadeCat("[ Fade Cat ]", Float) = 0
		[HDR][StyledToggle]_ElementRaycastMode("Enable Raycast Fading", Float) = 0
		[StyledToggle]_ElementVolumeFadeMode("Enable Volume Edge Fading", Float) = 0
		[StyledMessage(Info, The Raycast feature currently only works with particle systems and non instanced materials. GPU Instancing will be disabled if the Raycast features is enabled., 10, 0)]_RaycastMessage("Raycast Message", Float) = 0
		[HideInInspector]_RaycastFadeValue("Raycast Fade Mask", Float) = 1
		[Space(10)][StyledLayers()]_RaycastLayerMask("Raycast Layer", Float) = 1
		_RaycastDistanceEndValue("Raycast Distance", Float) = 2
		[ASEEnd][StyledCategory(Advanced Settings)]_AdvancedCat("[ Advanced Cat ]", Float) = 0
		[HideInInspector]_ElementLayerValue("Legacy Layer Value", Float) = -1
		[HideInInspector]_InvertX("Legacy Invert Mode", Float) = 0
		[HideInInspector]_ElementFadeSupport("Legacy Edge Fading", Float) = 0
		[HideInInspector]_IsVersion("_IsVersion", Float) = 0
		[HideInInspector]_IsElementShader("_IsElementShader", Float) = 1
		[HideInInspector]_IsColorsElement("_IsColorsElement", Float) = 1
		[HideInInspector]_render_colormask("_render_colormask", Float) = 15

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "PreviewType"="Plane" "DisableBatching"="True" }
	LOD 0

		CGINCLUDE
		#pragma target 2.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaToMask Off
		Cull Off
		ColorMask [_render_colormask]
		ZWrite Off
		ZTest LEqual
		
		
		
		Pass
		{
			Name "Unlit"

			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#define ASE_NEEDS_FRAG_COLOR
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			// Element Type Define
			#define TVE_IS_COLORS_ELEMENT


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform half _IsColorsElement;
			uniform half _Banner;
			uniform half _Message;
			uniform half _render_colormask;
			uniform half _FadeCat;
			uniform half _AdvancedCat;
			uniform float _ElementFadeSupport;
			uniform half _ElementLayerValue;
			uniform half _IsElementShader;
			uniform half _RaycastDistanceEndValue;
			uniform half _ElementCat;
			uniform half _RaycastLayerMask;
			uniform half _ElementRaycastMode;
			uniform half _RaycastMessage;
			uniform half _RenderCat;
			uniform half _ElementLayerMessage;
			uniform half4 _MainTexRemap;
			uniform float _IsVersion;
			uniform half4 _NoiseRemap;
			uniform float _InvertX;
			uniform half _ElementLayerWarning;
			uniform half _ElementLayerMask;
			uniform half4 _MainColor;
			uniform half4 TVE_SeasonOptions;
			uniform half4 _AdditionalColor1;
			uniform half4 _AdditionalColor2;
			uniform half TVE_SeasonLerp;
			uniform half4 _AdditionalColor3;
			uniform half4 _AdditionalColor4;
			uniform half _ElementMode;
			uniform sampler2D _MainTex;
			uniform half4 _MainUVs;
			uniform half _MainTexMinValue;
			uniform half _MainTexMaxValue;
			uniform half _ElementIntensity;
			uniform half4 TVE_ColorsCoords;
			uniform half4 TVE_ExtrasCoords;
			uniform half4 TVE_MotionCoords;
			uniform half4 TVE_VertexCoords;
			uniform half TVE_ElementsFadeValue;
			uniform half _ElementVolumeFadeMode;
			uniform half _RaycastFadeValue;
			uniform half _ElementColorsMode;
			half4 IS_ELEMENT( half4 Colors, half4 Extras, half4 Motion, half4 Vertex )
			{
				#if defined (TVE_IS_COLORS_ELEMENT)
				return Colors;
				#elif defined (TVE_IS_EXTRAS_ELEMENT)
				return Extras;
				#elif defined (TVE_IS_MOTION_ELEMENT)
				return Motion;
				#elif defined (TVE_IS_VERTEX_ELEMENT)
				return Vertex;
				#else
				return Colors;
				#endif
			}
			

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_color = v.color;
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				half4 Color_Main_RGBA49_g20382 = _MainColor;
				half TVE_SeasonOptions_X50_g20382 = TVE_SeasonOptions.x;
				half4 Color_Winter_RGBA58_g20382 = _AdditionalColor1;
				half4 Color_Spring_RGBA59_g20382 = _AdditionalColor2;
				half TVE_SeasonLerp54_g20382 = TVE_SeasonLerp;
				half4 lerpResult13_g20382 = lerp( Color_Winter_RGBA58_g20382 , Color_Spring_RGBA59_g20382 , TVE_SeasonLerp54_g20382);
				half TVE_SeasonOptions_Y51_g20382 = TVE_SeasonOptions.y;
				half4 Color_Summer_RGBA60_g20382 = _AdditionalColor3;
				half4 lerpResult14_g20382 = lerp( Color_Spring_RGBA59_g20382 , Color_Summer_RGBA60_g20382 , TVE_SeasonLerp54_g20382);
				half TVE_SeasonOptions_Z52_g20382 = TVE_SeasonOptions.z;
				half4 Color_Autumn_RGBA61_g20382 = _AdditionalColor4;
				half4 lerpResult15_g20382 = lerp( Color_Summer_RGBA60_g20382 , Color_Autumn_RGBA61_g20382 , TVE_SeasonLerp54_g20382);
				half TVE_SeasonOptions_W53_g20382 = TVE_SeasonOptions.w;
				half4 lerpResult12_g20382 = lerp( Color_Autumn_RGBA61_g20382 , Color_Winter_RGBA58_g20382 , TVE_SeasonLerp54_g20382);
				half Element_Mode55_g20382 = _ElementMode;
				half4 lerpResult30_g20382 = lerp( Color_Main_RGBA49_g20382 , ( ( TVE_SeasonOptions_X50_g20382 * lerpResult13_g20382 ) + ( TVE_SeasonOptions_Y51_g20382 * lerpResult14_g20382 ) + ( TVE_SeasonOptions_Z52_g20382 * lerpResult15_g20382 ) + ( TVE_SeasonOptions_W53_g20382 * lerpResult12_g20382 ) ) , Element_Mode55_g20382);
				half4 temp_output_487_0_g20382 = ( lerpResult30_g20382 * i.ase_color );
				half3 temp_output_486_0_g20382 = (temp_output_487_0_g20382).rgb;
				half3 Final_Colors_RGB142_g20382 = temp_output_486_0_g20382;
				half4 tex2DNode17_g20382 = tex2D( _MainTex, ( ( ( 1.0 - i.ase_texcoord1.xy ) * (_MainUVs).xy ) + (_MainUVs).zw ) );
				half temp_output_7_0_g20725 = _MainTexMinValue;
				half4 temp_cast_0 = (temp_output_7_0_g20725).xxxx;
				half4 break469_g20382 = saturate( ( ( tex2DNode17_g20382 - temp_cast_0 ) / ( _MainTexMaxValue - temp_output_7_0_g20725 ) ) );
				half MainTex_A74_g20382 = break469_g20382.a;
				half4 Colors37_g20727 = TVE_ColorsCoords;
				half4 Extras37_g20727 = TVE_ExtrasCoords;
				half4 Motion37_g20727 = TVE_MotionCoords;
				half4 Vertex37_g20727 = TVE_VertexCoords;
				half4 localIS_ELEMENT37_g20727 = IS_ELEMENT( Colors37_g20727 , Extras37_g20727 , Motion37_g20727 , Vertex37_g20727 );
				half4 temp_output_35_0_g20714 = localIS_ELEMENT37_g20727;
				half temp_output_7_0_g20730 = TVE_ElementsFadeValue;
				half2 temp_cast_1 = (temp_output_7_0_g20730).xx;
				half2 temp_output_851_0_g20382 = saturate( ( ( abs( (( (temp_output_35_0_g20714).zw + ( (temp_output_35_0_g20714).xy * (WorldPosition).xz ) )*2.002 + -1.001) ) - temp_cast_1 ) / ( 1.0 - temp_output_7_0_g20730 ) ) );
				half2 break852_g20382 = ( temp_output_851_0_g20382 * temp_output_851_0_g20382 );
				half lerpResult842_g20382 = lerp( 1.0 , ( 1.0 - saturate( ( break852_g20382.x + break852_g20382.y ) ) ) , _ElementVolumeFadeMode);
				half Fade_EdgeMask656_g20382 = lerpResult842_g20382;
				half Element_Intensity56_g20382 = ( _ElementIntensity * i.ase_color.a * Fade_EdgeMask656_g20382 * _RaycastFadeValue );
				half Final_Colors_A144_g20382 = ( temp_output_487_0_g20382.a * MainTex_A74_g20382 * Element_Intensity56_g20382 );
				half4 appendResult470_g20382 = (half4(Final_Colors_RGB142_g20382 , Final_Colors_A144_g20382));
				half Element_EffectColors914_g20382 = _ElementColorsMode;
				
				
				finalColor = ( appendResult470_g20382 + ( Element_EffectColors914_g20382 * 0.0 ) );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "TVEShaderElementGUI"
	
	
}
/*ASEBEGIN
Version=18934
1920;0;1920;1029;1707.644;1049.456;1;True;False
Node;AmplifyShaderEditor.FunctionNode;186;-640,-768;Inherit;False;Define Element Colors;61;;19978;378049ebac362e14aae08c2daa8ed737;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-384,-768;Half;False;Property;_Banner;Banner;0;0;Create;True;0;0;0;True;1;StyledBanner(Color Element);False;0;0;1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;100;-256,-768;Half;False;Property;_Message;Message;1;0;Create;True;0;0;0;True;1;StyledMessage(Info, Use the Colors elements to add color tinting to the vegetation assets. Element Texture A is used as alpha mask. Particle Color is used as color multiplier and Alpha as Element Intensity multiplier., 0,0);False;0;0;1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;178;-640,-640;Half;False;Property;_render_colormask;_render_colormask;63;1;[HideInInspector];Create;True;0;0;0;True;0;False;15;14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;204;-640,-512;Inherit;False;Base Element;2;;20382;0e972c73cae2ee54ea51acc9738801d0;6,477,0,478,0,145,0,481,0,576,1,491,1;0;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;-304,-512;Half;False;True;-1;2;TVEShaderElementGUI;0;1;BOXOPHOBIC/The Vegetation Engine/Elements/Default/Colors Default;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;True;2;5;False;-1;10;False;-1;0;1;False;-1;1;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;2;False;-1;True;True;True;True;True;False;0;True;178;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;0;False;-1;True;False;0;False;-1;0;False;-1;True;4;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;PreviewType=Plane;DisableBatching=True=DisableBatching;True;0;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;1;True;False;;False;0
WireConnection;0;0;204;0
ASEEND*/
//CHKSM=B3F10492BDAA028A7461BCD9DE643945E7BB30E3