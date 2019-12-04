// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WallShieldEffect"
{
	Properties
	{
		_Tailledubruit("Taille du bruit", Vector) = (3,3,0,0)
		_Vitessedetilingdudissolve("Vitesse de tiling du dissolve", Range( 0 , 1)) = 0.2912621
		_VitesseduDissolve("Vitesse du Dissolve", Range( 0 , 10)) = 0.2912621
		_RatioAffichageTexture("Ratio Affichage Texture", Vector) = (2,2,0,0)
		_SpeedTexture("Speed - Texture", Range( 0 , 3)) = 0.1907865
		_ShieldRimPower("Shield Rim Power", Range( 0 , 10)) = 0
		_Color2("Color 2", Color) = (0.514151,1,0.6202108,0)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
		};

		uniform float4 _Color2;
		uniform float _ShieldRimPower;
		uniform sampler2D _TextureSample0;
		uniform float2 _RatioAffichageTexture;
		uniform float _SpeedTexture;
		uniform float2 _Tailledubruit;
		uniform float _Vitessedetilingdudissolve;
		uniform float _VitesseduDissolve;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV9 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode9 = ( 1.0 + 1.0 * pow( 1.0 - fresnelNdotV9, (10.0 + (_ShieldRimPower - 0.0) * (0.0 - 10.0) / (10.0 - 0.0)) ) );
			float4 Fresnel_Albedo_Emission19 = ( _Color2 * fresnelNode9 );
			float mulTime47 = _Time.y * _SpeedTexture;
			float2 panner51 = ( mulTime47 * float2( 0.5,0.5 ) + float2( 0,0 ));
			float2 uv_TexCoord45 = i.uv_texcoord * _RatioAffichageTexture + panner51;
			float4 HexaTexture_Mooving110 = ( ( Fresnel_Albedo_Emission19 * tex2D( _TextureSample0, uv_TexCoord45 ) ) * 1.0 );
			float4 temp_output_111_0 = HexaTexture_Mooving110;
			o.Albedo = temp_output_111_0.rgb;
			o.Emission = temp_output_111_0.rgb;
			float2 temp_cast_2 = (0.0).xx;
			float2 uv_TexCoord84 = i.uv_texcoord * temp_cast_2 + HexaTexture_Mooving110.rg;
			float mulTime56 = _Time.y * _Vitessedetilingdudissolve;
			float2 panner57 = ( mulTime56 * float2( 0.5,0.5 ) + float2( 0,0 ));
			float2 uv_TexCoord59 = i.uv_texcoord * _Tailledubruit + panner57;
			float simplePerlin2D61 = snoise( uv_TexCoord59 );
			float mulTime106 = _Time.y * _VitesseduDissolve;
			float Dissolve64 = step( simplePerlin2D61 , (0.0 + (sin( mulTime106 ) - 0.0) * (1.0 - 0.0) / (1.0 - 0.0)) );
			o.Alpha = ( uv_TexCoord84 * Dissolve64 ).x;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows exclude_path:deferred 

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
			sampler3D _DitherMaskLOD;
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
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
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
-1920;1;1920;1018;3783.66;929.2565;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;21;-2354.235,1169.956;Float;False;1298.572;709.1167;Comment;9;10;11;12;9;13;19;15;14;18;FresnelEffect;1,0.514151,0.514151,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;113;-4808.036,397.1695;Float;False;2333.759;607.5359;Comment;11;78;42;38;43;53;47;51;52;45;79;110;Texture Hexagon;0.5762695,1,0.514151,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-2304.234,1503.252;Float;False;Property;_ShieldRimPower;Shield Rim Power;8;0;Create;True;0;0;False;0;0;6;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;11;-2016.76,1506.604;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;10;False;3;FLOAT;10;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-4758.036,788.6779;Float;False;Property;_SpeedTexture;Speed - Texture;5;0;Create;True;0;0;False;0;0.1907865;0.1;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;47;-4470.817,796.0143;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;109;-2340.736,317.4126;Float;False;2088.462;749.7;Comment;12;55;56;57;58;59;107;62;61;63;106;108;64;Dissolve;1,1,0.6084906,1;0;0
Node;AmplifyShaderEditor.ColorNode;12;-1849.674,1219.956;Float;False;Property;_Color2;Color 2;10;0;Create;True;0;0;False;0;0.514151,1,0.6202108,0;0.3254717,0.9795578,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;9;-1817.525,1411.785;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-1546.875,1334.904;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-2290.736,537.626;Float;False;Property;_Vitessedetilingdudissolve;Vitesse de tiling du dissolve;3;0;Create;True;0;0;False;0;0.2912621;0.17;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;51;-4248.226,751.7057;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;52;-4254.364,622.889;Float;False;Property;_RatioAffichageTexture;Ratio Affichage Texture;4;0;Create;True;0;0;False;0;2,2;4.6,0.98;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;45;-3943.359,691.3679;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;19;-1374.661,1398.683;Float;True;Fresnel_Albedo_Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;56;-1979.367,611.751;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;108;-2088.436,859.9796;Float;False;Property;_VitesseduDissolve;Vitesse du Dissolve;3;0;Create;True;0;0;False;0;0.2912621;2;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;58;-1759.851,367.4126;Float;False;Property;_Tailledubruit;Taille du bruit;2;0;Create;True;0;0;False;0;3,3;3,3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;57;-1815.383,567.0918;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;106;-1803.836,864.7797;Float;False;1;0;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;38;-3666.854,447.1694;Float;True;19;Fresnel_Albedo_Emission;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;43;-3664.95,658.7491;Float;True;Property;_TextureSample0;Texture Sample 0;13;0;Create;True;0;0;False;0;5123286d0d21aae4a9cd0d860908c6f2;ff9193904a908544e99c273139cbe678;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-3268.524,562.0842;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-3278.598,773.5446;Float;False;Constant;_Transparance;Transparance;15;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;107;-1601.836,801.7798;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;59;-1530.96,520.0988;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;62;-1357.555,814.1125;Float;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-2950.113,607.4234;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;61;-1281.266,516.3798;Float;True;Simplex2D;1;0;FLOAT2;0.1,0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;63;-777.1598,687.9333;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;110;-2739.484,609.2516;Float;True;HexaTexture_Mooving;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;112;-3366.252,-490.1802;Float;True;110;HexaTexture_Mooving;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;64;-495.2751,691.5645;Float;True;Dissolve;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-3232.681,-589.5759;Float;False;Constant;_Float1;Float 1;15;0;Create;True;0;0;False;0;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;84;-2886.957,-519.3068;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;65;-2871.488,-242.324;Float;True;64;Dissolve;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;81;-3739.758,1185.411;Float;False;1319.927;659.6378;Comment;10;24;27;25;26;28;29;33;34;32;40;Deformation des vertexs;0.7216981,0.890788,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;23;-4859.352,1203.138;Float;False;1080.219;862.8781;Color a zone - Vertexs;8;7;6;4;5;2;3;1;22;Partie 1 - Vertex - Hitboxes;0.1132075,0,0,1;0;0
Node;AmplifyShaderEditor.ColorNode;7;-4806.859,1859.016;Float;False;Property;_Color1;Color 1;7;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-3259.23,1388.171;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-1855.532,1660;Float;False;Property;_OpacityFresnelMultiplicator;Opacity Fresnel Multiplicator;9;0;Create;True;0;0;False;0;6.116504;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-3186.495,1597.549;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;29;-3120.791,1346.623;Float;True;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;18;-1335.865,1631.252;Float;True;Fresnel_Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-1541.532,1635;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;24;-3619.735,1236.711;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;90;-2626.301,-348.8527;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DistanceOpNode;2;-4485.306,1343.785;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareLower;4;-4271.906,1406.685;Float;False;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;6;-4809.352,1690.631;Float;False;Property;_Color0;Color 0;6;0;Create;True;0;0;False;0;1,0,0.09962893,0;1,0,0.09962893,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-3421.14,1463.439;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-4774.006,1586.185;Float;False;Property;_HitSize;HitSize;1;0;Create;True;0;0;False;0;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-3689.757,1559.826;Float;False;Property;_AnimationSpeed;Animation Speed;11;0;Create;True;0;0;False;0;14.52895;0;0;25;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;22;-4024.135,1408.832;Float;True;AlbedoPartie1;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;33;-2910.665,1534.399;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;40;-2693.6,1524.984;Float;True;LocalVertexOffset;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-3485.169,1730.049;Float;False;Property;_AnimationScale;AnimationScale;12;0;Create;True;0;0;False;0;0.05361792;0;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;25;-3632.538,1405.139;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;3;-4785.306,1429.785;Float;False;Property;_HitPosition;HitPosition;0;0;Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;111;-2738.146,-647.4332;Float;False;110;HexaTexture_Mooving;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;1;-4807.253,1253.138;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-2356.894,-661.8983;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;WallShieldEffect;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;11;0;10;0
WireConnection;47;0;53;0
WireConnection;9;3;11;0
WireConnection;13;0;12;0
WireConnection;13;1;9;0
WireConnection;51;1;47;0
WireConnection;45;0;52;0
WireConnection;45;1;51;0
WireConnection;19;0;13;0
WireConnection;56;0;55;0
WireConnection;57;1;56;0
WireConnection;106;0;108;0
WireConnection;43;1;45;0
WireConnection;42;0;38;0
WireConnection;42;1;43;0
WireConnection;107;0;106;0
WireConnection;59;0;58;0
WireConnection;59;1;57;0
WireConnection;62;0;107;0
WireConnection;79;0;42;0
WireConnection;79;1;78;0
WireConnection;61;0;59;0
WireConnection;63;0;61;0
WireConnection;63;1;62;0
WireConnection;110;0;79;0
WireConnection;64;0;63;0
WireConnection;84;0;89;0
WireConnection;84;1;112;0
WireConnection;28;0;24;0
WireConnection;28;1;26;0
WireConnection;34;0;32;0
WireConnection;29;0;28;0
WireConnection;18;0;14;0
WireConnection;14;0;9;0
WireConnection;14;1;15;0
WireConnection;90;0;84;0
WireConnection;90;1;65;0
WireConnection;2;0;1;0
WireConnection;2;1;3;0
WireConnection;4;0;2;0
WireConnection;4;1;5;0
WireConnection;4;2;6;0
WireConnection;4;3;7;0
WireConnection;26;0;25;1
WireConnection;26;1;27;0
WireConnection;22;0;4;0
WireConnection;33;0;29;0
WireConnection;33;3;34;0
WireConnection;33;4;32;0
WireConnection;40;0;33;0
WireConnection;0;0;111;0
WireConnection;0;2;111;0
WireConnection;0;9;90;0
ASEEND*/
//CHKSM=5039F73BC51F6AD9BD87A32264F5BE2BC822F4B9