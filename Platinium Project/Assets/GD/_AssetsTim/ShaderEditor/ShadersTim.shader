// Upgrade NOTE: upgraded instancing buffer 'ShadersTimWall_Glowing' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ShadersTim/Wall_Glowing"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 5
		_TestVar02("Test Var 02", Range( -1 , 10)) = 1.379347
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_OpacityMask("Opacity Mask", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _OpacityMask;
		uniform float _Cutoff = 5;

		UNITY_INSTANCING_BUFFER_START(ShadersTimWall_Glowing)
			UNITY_DEFINE_INSTANCED_PROP(float, _TestVar02)
#define _TestVar02_arr ShadersTimWall_Glowing
		UNITY_INSTANCING_BUFFER_END(ShadersTimWall_Glowing)

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float _TestVar02_Instance = UNITY_ACCESS_INSTANCED_PROP(_TestVar02_arr, _TestVar02);
			float fresnelNdotV28 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode28 = ( 0.01176473 + 1.0 * pow( 1.0 - fresnelNdotV28, _TestVar02_Instance ) );
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 Fresnel77 = ( fresnelNode28 * tex2D( _TextureSample0, uv_TextureSample0 ) );
			float4 temp_output_79_0 = Fresnel77;
			o.Albedo = temp_output_79_0.rgb;
			o.Emission = temp_output_79_0.rgb;
			o.Alpha = 1;
			clip( _OpacityMask - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16100
242;73;1297;651;953.177;296.7525;1.691969;True;False
Node;AmplifyShaderEditor.CommentaryNode;76;-1417.239,315.0397;Float;False;1285.189;528.9811;Fresnel effect w/ Texture;7;77;29;30;31;28;27;35;Fresnel effect w/ Texture;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-1365.239,448.0396;Float;False;Constant;_Float2;Float 2;1;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-1365.14,530.5861;Float;False;InstancedProperty;_TestVar02;Test Var 02;1;0;Create;True;0;0;False;0;1.379347;1.379347;-1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1367.239,365.0396;Float;False;Constant;_Float3;Float 3;1;0;Create;True;0;0;False;0;0.01176473;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;35;-1044.636,603.0736;Float;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;False;0;607b1a7c1171e8642a455f4d0a40e7f4;607b1a7c1171e8642a455f4d0a40e7f4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;28;-1049.092,385.0915;Float;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-644.3792,490.5648;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;77;-377.0869,496.8151;Float;True;Fresnel;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;74;-2182.365,216.1227;Float;False;716.2424;382.7383;Vertex Position;4;20;17;18;19;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;75;-2182.05,629.1362;Float;False;512.0006;501.3133;ColorFull Noise;3;23;24;25;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;16;-2184.93,-833.248;Float;False;1761.221;1031.067;Noise;9;14;8;7;6;9;11;5;72;73;Noise;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;81;-2206.589,1244.276;Float;False;1731.945;740.6361;Comment;10;55;54;52;51;66;53;65;56;57;78;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;80;586.2166,431.4174;Float;False;Property;_OpacityMask;Opacity Mask;5;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;24;-2132.05,900.4495;Float;True;20;Y_Gradient;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-1440.255,-545.9051;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;72;-1257.387,-154.4115;Float;False;Property;_Multiplicateur_Noise;Multiplicateur_Noise;2;0;Create;True;0;0;False;0;1;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;11;-1692.59,-783.248;Float;False;Constant;_Vector1;Vector 1;1;0;Create;True;0;0;False;0;30,30;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;6;-2134.931,-566.595;Float;False;Constant;_Speed;Speed;1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;7;-1970.714,-566.2588;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;5;-1138.594,-552.1998;Float;True;Simplex2D;1;0;FLOAT2;0.1,0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-942.0275,-303.0864;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;17;-2088.53,266.1227;Float;True;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;8;-1748.121,-583.5673;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;70;393.4299,-112.1435;Float;True;Property;_TextureSample1;Texture Sample 1;4;0;Create;True;0;0;False;0;607b1a7c1171e8642a455f4d0a40e7f4;607b1a7c1171e8642a455f4d0a40e7f4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-1854.977,342.8185;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-2156.59,1534.372;Float;False;Constant;_Float4;Float 4;1;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;54;-2005.397,1538.615;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;52;-1785.881,1294.276;Float;False;Constant;_Tailledubruit;Taille du bruit;1;0;Create;True;0;0;False;0;3,3;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;51;-1841.412,1493.956;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;53;-1556.989,1446.963;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinTimeNode;66;-1580.055,1759.091;Float;True;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;23;-2120.085,679.1362;Float;True;14;Noise;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;65;-1318.172,1731.912;Float;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;57;-943.5307,1650.797;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;78;-717.6456,1643.428;Float;True;Dissolve;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-1905.05,798.8815;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;14;-706.4879,-305.8531;Float;True;Noise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;20;-1709.122,340.861;Float;True;Y_Gradient;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-2132.365,453.7983;Float;False;Constant;_Teleport;Teleport;1;0;Create;True;0;0;False;0;0;0;-1;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;56;-1307.296,1444.544;Float;True;Simplex2D;1;0;FLOAT2;0.1,0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;79;467.8049,190.4493;Float;True;77;Fresnel;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;824.8675,-44.99602;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;ShadersTim/Wall_Glowing;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;5;True;True;0;True;Transparent;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;1;1,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;28;1;31;0
WireConnection;28;2;30;0
WireConnection;28;3;29;0
WireConnection;27;0;28;0
WireConnection;27;1;35;0
WireConnection;77;0;27;0
WireConnection;9;0;11;0
WireConnection;9;1;8;0
WireConnection;7;0;6;0
WireConnection;5;0;9;0
WireConnection;73;0;5;0
WireConnection;73;1;72;0
WireConnection;8;1;7;0
WireConnection;18;0;17;0
WireConnection;18;1;19;0
WireConnection;54;0;55;0
WireConnection;51;1;54;0
WireConnection;53;0;52;0
WireConnection;53;1;51;0
WireConnection;65;0;66;4
WireConnection;57;0;56;0
WireConnection;57;1;65;0
WireConnection;78;0;57;0
WireConnection;25;0;23;0
WireConnection;25;1;24;0
WireConnection;14;0;73;0
WireConnection;20;0;18;0
WireConnection;56;0;53;0
WireConnection;0;0;79;0
WireConnection;0;2;79;0
WireConnection;0;10;80;0
ASEEND*/
//CHKSM=242E2386AD69DDCE5DD8F12BDA6860046C4BF5C9