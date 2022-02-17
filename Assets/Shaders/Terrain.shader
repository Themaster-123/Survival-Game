// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Surviva Gamel/Terrain"
{
	Properties
	{
		_SlopeCutoff("Slope Cutoff", Range( -1 , 1)) = 0
		_GrassColor("Grass Color", Color) = (0,1,0.0666666,0)
		_DirtColor("Dirt Color", Color) = (1,0.5861825,0,0)
		_CellShading("Cell Shading", Int) = 0
		_NoiseMin("Noise Min", Float) = 0
		_NoiseMax("Noise Max", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 5.0
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
		};

		uniform int _CellShading;
		uniform float _NoiseMin;
		uniform float _NoiseMax;
		uniform float _SlopeCutoff;
		uniform float4 _GrassColor;
		uniform float4 _DirtColor;


		float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }

		float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }

		float snoise( float3 v )
		{
			const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
			float3 i = floor( v + dot( v, C.yyy ) );
			float3 x0 = v - i + dot( i, C.xxx );
			float3 g = step( x0.yzx, x0.xyz );
			float3 l = 1.0 - g;
			float3 i1 = min( g.xyz, l.zxy );
			float3 i2 = max( g.xyz, l.zxy );
			float3 x1 = x0 - i1 + C.xxx;
			float3 x2 = x0 - i2 + C.yyy;
			float3 x3 = x0 - 0.5;
			i = mod3D289( i);
			float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
			float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
			float4 x_ = floor( j / 7.0 );
			float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
			float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 h = 1.0 - abs( x ) - abs( y );
			float4 b0 = float4( x.xy, y.xy );
			float4 b1 = float4( x.zw, y.zw );
			float4 s0 = floor( b0 ) * 2.0 + 1.0;
			float4 s1 = floor( b1 ) * 2.0 + 1.0;
			float4 sh = -step( h, 0.0 );
			float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
			float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
			float3 g0 = float3( a0.xy, h.x );
			float3 g1 = float3( a0.zw, h.y );
			float3 g2 = float3( a1.xy, h.z );
			float3 g3 = float3( a1.zw, h.w );
			float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
			g0 *= norm.x;
			g1 *= norm.y;
			g2 *= norm.z;
			g3 *= norm.w;
			float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
			m = m* m;
			m = m* m;
			float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
			return 42.0 * dot( m, px);
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			int temp_output_75_0 = ( _CellShading + 1 );
			float4 ase_vertex4Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 transform39 = mul(unity_ObjectToWorld,ase_vertex4Pos);
			float simplePerlin3D34 = snoise( transform39.xyz*0.14 );
			simplePerlin3D34 = simplePerlin3D34*0.5 + 0.5;
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			ase_vertexNormal = normalize( ase_vertexNormal );
			float4 transform17 = mul(unity_ObjectToWorld,float4( ase_vertexNormal , 0.0 ));
			float dotResult11 = dot( transform17 , float4( float3(0,1,0) , 0.0 ) );
			float4 ifLocalVar13 = 0;
			if( dotResult11 >= _SlopeCutoff )
				ifLocalVar13 = _GrassColor;
			else
				ifLocalVar13 = _DirtColor;
			o.Albedo = ( (_NoiseMin + (( floor( ( temp_output_75_0 * (0.0 + (simplePerlin3D34 - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) ) ) / temp_output_75_0 ) - 0.0) * (_NoiseMax - _NoiseMin) / (1.0 - 0.0)) * ifLocalVar13 ).rgb;
			o.Alpha = 1;
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
			#pragma target 5.0
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
				float3 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
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
Version=18935
2140;275;1289;542;1120.717;-22.40934;2.009662;True;True
Node;AmplifyShaderEditor.PosVertexDataNode;36;-1349.404,-358.0804;Inherit;False;1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;39;-1031.366,-320.6332;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;34;-787.6771,-355.4313;Inherit;True;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.14;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;72;-595.2065,-489.3008;Inherit;False;Property;_CellShading;Cell Shading;3;0;Create;True;0;0;0;False;0;False;0;6;False;0;1;INT;0
Node;AmplifyShaderEditor.TFHCRemapNode;47;-522.5123,-332.9587;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;75;-389.4411,-474.4838;Inherit;False;2;2;0;INT;0;False;1;INT;1;False;1;INT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;2;-917.3151,317.1555;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-276.5244,-320.3439;Inherit;False;2;2;0;INT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;12;-678.7953,520.7734;Inherit;False;Constant;_Vector0;Vector 0;1;0;Create;True;0;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;17;-581.2491,289.8957;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FloorOpNode;73;-95.93748,-322.2762;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;74;97.37611,-393.0139;Inherit;False;2;0;FLOAT;0;False;1;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-186.2167,845.1429;Inherit;False;Property;_SlopeCutoff;Slope Cutoff;0;0;Create;True;0;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-17.8486,-126.8113;Inherit;False;Property;_NoiseMin;Noise Min;4;0;Create;True;0;0;0;False;0;False;0;0.62;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;33;-359.7702,91.59255;Inherit;False;Property;_GrassColor;Grass Color;1;0;Create;True;0;0;0;False;0;False;0,1,0.0666666,0;0,0.8301887,0.05209027,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;11;-438.1299,337.0422;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-35.19961,-44.27271;Inherit;False;Property;_NoiseMax;Noise Max;5;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;29;-450.751,602.4473;Inherit;False;Property;_DirtColor;Dirt Color;2;0;Create;True;0;0;0;False;0;False;1,0.5861825,0,0;1,0.5861825,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ConditionalIfNode;13;284.061,116.2231;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;78;245.6207,-177.4648;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;493.0478,-16.88739;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-90.11346,1138.155;Inherit;False;Property;_FadeThreshold;Fade Threshold;6;0;Create;True;0;0;0;False;0;False;0;1.68;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;98;196.0284,1126.337;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;-17.69538,610.1431;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;100;167.8914,712.1684;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;95;216.2218,899.3024;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;96;256.428,669.2809;Inherit;False;COLOR;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;93;496.2603,747.6156;Inherit;True;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;3;COLOR;0,0,0,0;False;4;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;659.8582,19.61906;Float;False;True;-1;7;ASEMaterialInspector;0;0;Standard;Surviva Gamel/Terrain;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;39;0;36;0
WireConnection;34;0;39;0
WireConnection;47;0;34;0
WireConnection;75;0;72;0
WireConnection;71;0;75;0
WireConnection;71;1;47;0
WireConnection;17;0;2;0
WireConnection;73;0;71;0
WireConnection;74;0;73;0
WireConnection;74;1;75;0
WireConnection;11;0;17;0
WireConnection;11;1;12;0
WireConnection;13;0;11;0
WireConnection;13;1;28;0
WireConnection;13;2;33;0
WireConnection;13;3;33;0
WireConnection;13;4;29;0
WireConnection;78;0;74;0
WireConnection;78;3;77;0
WireConnection;78;4;76;0
WireConnection;81;0;78;0
WireConnection;81;1;13;0
WireConnection;102;0;11;0
WireConnection;102;1;82;0
WireConnection;100;0;102;0
WireConnection;100;2;28;0
WireConnection;95;0;100;0
WireConnection;95;2;28;0
WireConnection;96;0;95;0
WireConnection;96;1;95;0
WireConnection;96;2;95;0
WireConnection;93;3;29;0
WireConnection;93;4;33;0
WireConnection;0;0;81;0
ASEEND*/
//CHKSM=46DC0144868E861C71DB16A02DC122815D323742