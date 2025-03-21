// Toony Colors Pro+Mobile 2
// (c) 2014-2023 Jean Moreno

Shader "Water"
{
	Properties
	{
		[TCP2HeaderHelp(Base)]
		_MainColor ("Main Color Color", Color) = (1,1,1,1)
		[TCP2ColorNoAlpha] _HColor ("Highlight Color", Color) = (0.75,0.75,0.75,1)
		[TCP2ColorNoAlpha] _SColor ("Shadow Color", Color) = (0.2,0.2,0.2,1)
		[TCP2Separator]

		[TCP2Header(Ramp Shading)]
		_RampThreshold ("Threshold", Range(0.01,1)) = 0.5
		_RampSmoothing ("Smoothing", Range(0.001,1)) = 0.5
		[TCP2Separator]
		
		[TCP2HeaderHelp(Rim Lighting)]
		[Toggle(TCP2_RIM_LIGHTING)] _UseRim ("Enable Rim Lighting", Float) = 0
		[TCP2ColorNoAlpha] _RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.5)
		_RimMin ("Rim Min", Range(0,2)) = 0.5
		_RimMax ("Rim Max", Range(0,2)) = 1
		[TCP2Separator]
		
		[TCP2HeaderHelp(Triplanar Mapping)]
		_TriGround ("Ground", 2D) = "white" {}
		[TCP2UVScrolling] _TriGround_SC ("Ground UV Scrolling", Vector) = (1,1,0,0)
		[NoScaleOffset] _TriSide ("Walls", 2D) = "white" {}
		[TCP2Vector4Floats(Contrast X,Contrast Y,Contrast Z,Smoothing,1,16,1,16,1,16,0.01,1)] _TriplanarBlendStrength ("Triplanar Parameters", Vector) = (2,8,2,0.5)
		[TCP2Separator]
		
		[TCP2HeaderHelp(Vertex Waves Animation)]
		_WavesSpeed ("Speed", Float) = 2
		_WavesHeight ("Height", Float) = 0.1
		_WavesFrequency ("Frequency", Range(0,10)) = 1
		
		[TCP2HeaderHelp(Depth Based Effects)]
		_FoamSpread ("Foam Spread", Range(0,5)) = 2
		_FoamStrength ("Foam Strength", Range(0,1)) = 0.8
		_FoamColor ("Foam Color (RGB) Opacity (A)", Color) = (0.9,0.9,0.9,1)
		_FoamTex ("Foam Texture Custom", 2D) = "black" {}
		_FoamSmoothness ("Foam Smoothness", Range(0,0.5)) = 0.02
		
		// Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"RenderType"="Opaque"
		}

		CGINCLUDE

		#include "UnityCG.cginc"
		#include "UnityLightingCommon.cginc"	// needed for LightColor

		// Texture/Sampler abstraction
		#define TCP2_TEX2D_WITH_SAMPLER(tex)						UNITY_DECLARE_TEX2D(tex)
		#define TCP2_TEX2D_NO_SAMPLER(tex)							UNITY_DECLARE_TEX2D_NOSAMPLER(tex)
		#define TCP2_TEX2D_SAMPLE(tex, samplertex, coord)			UNITY_SAMPLE_TEX2D_SAMPLER(tex, samplertex, coord)
		#define TCP2_TEX2D_SAMPLE_LOD(tex, samplertex, coord, lod)	UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex, samplertex, coord, lod)

		// Shader Properties
		TCP2_TEX2D_WITH_SAMPLER(_TriGround);
		TCP2_TEX2D_WITH_SAMPLER(_TriSide);
		TCP2_TEX2D_WITH_SAMPLER(_FoamTex);
		UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
		
		// Shader Properties
		float _WavesFrequency;
		float _WavesHeight;
		float _WavesSpeed;
		float4 _TriGround_ST;
		half4 _TriGround_SC;
		float4 _TriplanarBlendStrength;
		fixed4 _MainColor;
		float _FoamSpread;
		float _FoamStrength;
		fixed4 _FoamColor;
		float4 _FoamTex_ST;
		float _FoamSmoothness;
		float _RampThreshold;
		float _RampSmoothing;
		fixed4 _HColor;
		fixed4 _SColor;
		float _RimMin;
		float _RimMax;
		fixed4 _RimColor;

		ENDCG

		// Main Surface Shader

		CGPROGRAM

		#pragma surface surf ToonyColorsCustom vertex:vertex_surface exclude_path:deferred exclude_path:prepass keepalpha nolightmap nofog nolppv
		#pragma target 2.5

		//================================================================
		// SHADER KEYWORDS

		#pragma shader_feature_local_fragment TCP2_RIM_LIGHTING

		//================================================================
		// STRUCTS

		// Vertex input
		struct appdata_tcp2
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 texcoord0 : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
		#if defined(LIGHTMAP_ON) && defined(DIRLIGHTMAP_COMBINED)
			half4 tangent : TANGENT;
		#endif
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct Input
		{
			half3 viewDir;
			float3 worldPos;
			half3 worldNormal; INTERNAL_DATA
			half3 worldNormalVertex;
			float4 screenPosition;
			float2 texcoord0;
		};

		//================================================================

		// Custom SurfaceOutput
		struct SurfaceOutputCustom
		{
			half atten;
			half3 Albedo;
			half3 Normal;
			half3 worldNormal;
			half3 Emission;
			half Specular;
			half Gloss;
			half Alpha;
			half ndv;
			half ndvRaw;

			Input input;

			// Shader Properties
			float __rampThreshold;
			float __rampSmoothing;
			float3 __highlightColor;
			float3 __shadowColor;
			float __ambientIntensity;
			float __rimMin;
			float __rimMax;
			float3 __rimColor;
			float __rimStrength;
		};

		//================================================================
		// VERTEX FUNCTION

		void vertex_surface(inout appdata_tcp2 v, out Input output)
		{
			UNITY_INITIALIZE_OUTPUT(Input, output);

			// Texture Coordinates
			output.texcoord0 = v.texcoord0.xy;
			// Shader Properties Sampling
			float __wavesFrequency = ( _WavesFrequency );
			float __wavesHeight = ( _WavesHeight );
			float __wavesSpeed = ( _WavesSpeed );

			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			
			// Vertex water waves
			float _waveFrequency = __wavesFrequency;
			float _waveHeight = __wavesHeight;
			float3 _vertexWavePos = worldPos.xyz * _waveFrequency;
			float _phase = _Time.y * __wavesSpeed;
			float waveFactorX = sin(_vertexWavePos.x + _phase) * _waveHeight;
			float waveFactorZ = sin(_vertexWavePos.z + _phase) * _waveHeight;
			v.vertex.z += (waveFactorX + waveFactorZ);
			float4 clipPos = UnityObjectToClipPos(v.vertex);

			// Screen Position
			float4 screenPos = ComputeScreenPos(clipPos);
			output.screenPosition = screenPos;
			COMPUTE_EYEDEPTH(output.screenPosition.z);
			half3 worldNormal = UnityObjectToWorldNormal(v.normal);

			output.worldNormalVertex = worldNormal;

		}

		//================================================================
		// SURFACE FUNCTION

		void surf(Input input, inout SurfaceOutputCustom output)
		{
			// Shader Properties Sampling
			float4 __triplanarParameters = ( _TriplanarBlendStrength.xyzw );
			float4 __mainColor = ( _MainColor.rgba );
			float __foamSpread = ( _FoamSpread );
			float __foamStrength = ( _FoamStrength );
			float4 __foamColor = ( _FoamColor.rgba );
			float3 __foamTextureCustom = ( TCP2_TEX2D_SAMPLE(_FoamTex, _FoamTex, input.texcoord0.xy * _FoamTex_ST.xy + _FoamTex_ST.zw).rgb );
			float __foamMask = ( .0 );
			float __foamSmoothness = ( _FoamSmoothness );
			output.__rampThreshold = ( _RampThreshold );
			output.__rampSmoothing = ( _RampSmoothing );
			output.__highlightColor = ( _HColor.rgb );
			output.__shadowColor = ( _SColor.rgb );
			output.__ambientIntensity = ( 1.0 );
			output.__rimMin = ( _RimMin );
			output.__rimMax = ( _RimMax );
			output.__rimColor = ( _RimColor.rgb );
			output.__rimStrength = ( 1.0 );

			output.input = input;

			half3 worldNormal = WorldNormalVector(input, output.Normal);
			output.worldNormal = worldNormal;

			half ndv = abs(dot(input.viewDir, normalize(output.Normal.xyz)));
			half ndvRaw = ndv;
			output.ndv = ndv;
			output.ndvRaw = ndvRaw;

			// Sample depth texture and calculate difference with local depth
			float sceneDepth = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(input.screenPosition));
			if (unity_OrthoParams.w > 0.0)
			{
				// Orthographic camera
				#if defined(UNITY_REVERSED_Z)
					sceneDepth = 1.0 - sceneDepth;
				#endif
				sceneDepth = (sceneDepth * _ProjectionParams.z) + _ProjectionParams.y;
			}
			else
			{
				// Perspective camera
				sceneDepth = LinearEyeDepth(sceneDepth);
			}
			
			float localDepth = input.screenPosition.z;
			float depthDiff = abs(sceneDepth - localDepth);

			output.Albedo = half3(1,1,1);
			output.Alpha = 1;

			half4 albedoAlpha = half4(output.Albedo, output.Alpha);
			
			// Triplanar Texture Blending
			half2 uv_ground = input.worldPos.xz;
			half2 uv_sideX = input.worldPos.zy;
			half2 uv_sideZ = input.worldPos.xy;
			float3 triplanarNormal = input.worldNormalVertex;
			
			//ground
			half4 triplanar = ( TCP2_TEX2D_SAMPLE(_TriGround, _TriGround, uv_ground * _TriGround_ST.xy + frac(_Time.yy * _TriGround_SC.xy) + _TriGround_ST.zw).rgba );
			
			//walls
			fixed4 tex_sideX = ( TCP2_TEX2D_SAMPLE(_TriSide, _TriSide, uv_sideX).rgba );
			fixed4 tex_sideZ = ( TCP2_TEX2D_SAMPLE(_TriSide, _TriSide, uv_sideZ).rgba );
			
			//blending
			half3 blendWeights = pow(abs(triplanarNormal), __triplanarParameters.xyz / __triplanarParameters.w);
			blendWeights = blendWeights / (blendWeights.x + abs(blendWeights.y) + blendWeights.z);
			
			triplanar = tex_sideX * blendWeights.x + triplanar * blendWeights.y + tex_sideZ * blendWeights.z;
			albedoAlpha *= triplanar;
			output.Albedo = albedoAlpha.rgb;
			
			output.Albedo *= __mainColor.rgb;
			
			// Depth-based water foam
			half foamSpread = __foamSpread;
			half foamStrength = __foamStrength;
			half4 foamColor = __foamColor;
			
			half3 foam = __foamTextureCustom;
			float foamDepth = saturate(foamSpread * depthDiff) * (1.0 - __foamMask);
			half foamSmooth = __foamSmoothness;
			half foamTerm = (smoothstep(foam.r - foamSmooth, foam.r + foamSmooth, saturate(foamStrength - foamDepth)) * saturate(1 - foamDepth)) * foamColor.a;
			output.Albedo.rgb = lerp(output.Albedo.rgb, foamColor.rgb, foamTerm);
			output.Alpha = lerp(output.Alpha, foamColor.a, foamTerm);

		}

		//================================================================
		// LIGHTING FUNCTION

		inline half4 LightingToonyColorsCustom(inout SurfaceOutputCustom surface, half3 viewDir, UnityGI gi)
		{

			half ndv = surface.ndv;
			half3 lightDir = gi.light.dir;
			#if defined(UNITY_PASS_FORWARDBASE)
				half3 lightColor = _LightColor0.rgb;
				half atten = surface.atten;
			#else
				// extract attenuation from point/spot lights
				half3 lightColor = _LightColor0.rgb;
				half atten = max(gi.light.color.r, max(gi.light.color.g, gi.light.color.b)) / max(_LightColor0.r, max(_LightColor0.g, _LightColor0.b));
			#endif

			half3 normal = normalize(surface.Normal);
			half ndl = dot(normal, lightDir);
			half3 ramp;
			
			#define		RAMP_THRESHOLD	surface.__rampThreshold
			#define		RAMP_SMOOTH		surface.__rampSmoothing
			ndl = saturate(ndl);
			ramp = smoothstep(RAMP_THRESHOLD - RAMP_SMOOTH*0.5, RAMP_THRESHOLD + RAMP_SMOOTH*0.5, ndl);

			// Apply attenuation (shadowmaps & point/spot lights attenuation)
			ramp *= atten;

			// Highlight/Shadow Colors
			#if !defined(UNITY_PASS_FORWARDBASE)
				ramp = lerp(half3(0,0,0), surface.__highlightColor, ramp);
			#else
				ramp = lerp(surface.__shadowColor, surface.__highlightColor, ramp);
			#endif

			// Output color
			half4 color;
			color.rgb = surface.Albedo * lightColor.rgb * ramp;
			color.a = surface.Alpha;

			// Apply indirect lighting (ambient)
			half occlusion = 1;
			#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
				half3 ambient = gi.indirect.diffuse;
				ambient *= surface.Albedo * occlusion * surface.__ambientIntensity;

				color.rgb += ambient;
			#endif

			// Rim Lighting
			#if defined(TCP2_RIM_LIGHTING)
			#if !defined(UNITY_PASS_FORWARDADD)
			half rim = 1 - surface.ndvRaw;
			rim = ( rim );
			half rimMin = surface.__rimMin;
			half rimMax = surface.__rimMax;
			rim = smoothstep(rimMin, rimMax, rim);
			half3 rimColor = surface.__rimColor;
			half rimStrength = surface.__rimStrength;
			color.rgb += rim * rimColor * rimStrength;
			#endif
			#endif

			return color;
		}

		void LightingToonyColorsCustom_GI(inout SurfaceOutputCustom surface, UnityGIInput data, inout UnityGI gi)
		{
			half3 normal = surface.Normal;

			// GI without reflection probes
			gi = UnityGlobalIllumination(data, 1.0, normal); // occlusion is applied in the lighting function, if necessary

			surface.atten = data.atten; // transfer attenuation to lighting function
			gi.light.color = _LightColor0.rgb; // remove attenuation

		}

		ENDCG

	}

	CustomEditor "ToonyColorsPro.ShaderGenerator.MaterialInspector_SG2"
}

/* TCP_DATA u config(ver:"2.9.8";unity:"2021.3.24f1";tmplt:"SG2_Template_Default";features:list["UNITY_5_4","UNITY_5_5","UNITY_5_6","UNITY_2017_1","UNITY_2018_1","UNITY_2018_2","UNITY_2018_3","UNITY_2019_1","UNITY_2019_2","UNITY_2019_3","UNITY_2019_4","UNITY_2020_1","UNITY_2021_1","UNITY_2021_2","TRIPLANAR","SPECULAR_SHADER_FEATURE","RIM_DIR_PERSP_CORRECTION","RIM","RIM_SHADER_FEATURE","MATCAP_SHADER_FEATURE","VERTEX_SIN_WAVES","SMOOTH_FOAM","DEPTH_BUFFER_FOAM","VSW_WORLDPOS","VSW_AXIS_Z"];flags:list[];flags_extra:dict[];keywords:dict[RENDER_TYPE="Opaque",RampTextureDrawer="[TCP2Gradient]",RampTextureLabel="Ramp Texture",SHADER_TARGET="2.5",RIM_LABEL="Rim Lighting",BASEGEN_ALBEDO_DOWNSCALE="1",BASEGEN_MASKTEX_DOWNSCALE="1/2",BASEGEN_METALLIC_DOWNSCALE="1/4",BASEGEN_SPECULAR_DOWNSCALE="1/4",BASEGEN_DIFFUSEREMAPMIN_DOWNSCALE="1/4",BASEGEN_MASKMAPREMAPMIN_DOWNSCALE="1/4"];shaderProperties:list[sp(name:"Main Color";imps:list[imp_mp_color(def:RGBA(1, 1, 1, 1);hdr:False;cc:4;chan:"RGBA";prop:"_MainColor";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"33a7a5aa-6523-49a7-b965-13ec45cf56c6";op:Multiply;lbl:"Main Color Color";gpu_inst:False;locked:False;impl_index:-1)];layers:list[];unlocked:list[];layer_blend:dict[];custom_blend:dict[];clones:dict[];isClone:False),,,,,,,,,,,sp(name:"Ground Texture";imps:list[imp_mp_texture(uto:True;tov:"";tov_lbl:"";gto:False;sbt:False;scr:True;scv:"";scv_lbl:"";gsc:False;roff:False;goff:False;sin_anm:False;sin_anmv:"";sin_anmv_lbl:"";gsin:False;notile:False;triplanar_local:False;def:"white";locked_uv:True;uv:0;cc:4;chan:"RGBA";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:Texcoord;uv_chan:"XZ";tpln_scale:1;uv_shaderproperty:__NULL__;uv_cmp:__NULL__;sep_sampler:__NULL__;prop:"_TriGround";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"9ffbfd1b-bf24-4fad-9cc1-9fdc18e4f029";op:Multiply;lbl:"Ground";gpu_inst:False;locked:True;impl_index:0)];layers:list[];unlocked:list[];layer_blend:dict[];custom_blend:dict[];clones:dict[];isClone:False),,,,,,,,,,,,,,,,,,,,,,,,,sp(name:"Sketch Texture";imps:list[imp_mp_texture(uto:True;tov:"";tov_lbl:"";gto:False;sbt:False;scr:False;scv:"";scv_lbl:"";gsc:False;roff:True;goff:False;sin_anm:False;sin_anmv:"";sin_anmv_lbl:"";gsin:False;notile:False;triplanar_local:False;def:"black";locked_uv:False;uv:6;cc:3;chan:"AAA";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:Triplanar;uv_chan:"XZ";tpln_scale:1;uv_shaderproperty:__NULL__;uv_cmp:__NULL__;sep_sampler:__NULL__;prop:"_SketchTexture";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"d929d449-d0fc-4400-9c90-ab4b329e7932";op:Multiply;lbl:"Sketch Texture";gpu_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];layer_blend:dict[];custom_blend:dict[];clones:dict[];isClone:False),sp(name:"Ground Texture Normal Map";imps:list[imp_mp_texture(uto:True;tov:"_TriGround_ST";tov_lbl:"_TriGround_ST";gto:False;sbt:False;scr:False;scv:"";scv_lbl:"";gsc:False;roff:False;goff:False;sin_anm:False;sin_anmv:"";sin_anmv_lbl:"";gsin:False;notile:False;triplanar_local:False;def:"bump";locked_uv:True;uv:0;cc:4;chan:"RGBA";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:Texcoord;uv_chan:"XZ";tpln_scale:1;uv_shaderproperty:__NULL__;uv_cmp:__NULL__;sep_sampler:__NULL__;prop:"_TriGroundBump";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"9cd98234-9e1b-4b09-bb3f-4bf3b96a863d";op:Multiply;lbl:"Ground Normal Map";gpu_inst:False;locked:True;impl_index:0)];layers:list[];unlocked:list[];layer_blend:dict[];custom_blend:dict[];clones:dict[];isClone:False),sp(name:"Specular Color";imps:list[imp_mp_color(def:RGBA(0.5, 0.5, 0.5, 1);hdr:False;cc:3;chan:"RGB";prop:"_SpecularColor";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"bbc4727d-581d-433e-bd76-52878643dfd8";op:Multiply;lbl:"Specular Color";gpu_inst:False;locked:False;impl_index:0),imp_mp_texture(uto:True;tov:"";tov_lbl:"";gto:False;sbt:False;scr:False;scv:"";scv_lbl:"";gsc:False;roff:False;goff:False;sin_anm:False;sin_anmv:"";sin_anmv_lbl:"";gsin:False;notile:False;triplanar_local:False;def:"white";locked_uv:False;uv:6;cc:3;chan:"RGB";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:Triplanar;uv_chan:"XZ";tpln_scale:1;uv_shaderproperty:__NULL__;uv_cmp:__NULL__;sep_sampler:__NULL__;prop:"_SpecularMask";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"ee5aace2-5e5d-4968-9578-54bfebe0b9f5";op:Multiply;lbl:"Specular Mask";gpu_inst:False;locked:False;impl_index:-1)];layers:list[];unlocked:list[];layer_blend:dict[];custom_blend:dict[];clones:dict[];isClone:False)];customTextures:list[ct(cimp:imp_mp_texture(uto:True;tov:"";tov_lbl:"";gto:False;sbt:False;scr:False;scv:"";scv_lbl:"";gsc:False;roff:False;goff:False;sin_anm:False;sin_anmv:"";sin_anmv_lbl:"";gsin:False;notile:False;triplanar_local:False;def:"white";locked_uv:False;uv:6;cc:4;chan:"RGBA";mip:0;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:Triplanar;uv_chan:"XZ";tpln_scale:1;uv_shaderproperty:__NULL__;uv_cmp:__NULL__;sep_sampler:__NULL__;prop:"_MyTexture";md:"";gbv:False;custom:True;refs:"";pnlock:False;guid:"a814225c-a08e-4403-9c05-748971fdd673";op:Multiply;lbl:"Speclar_mask";gpu_inst:False;locked:False;impl_index:-1);exp:True;uv_exp:False;imp_lbl:"Texture")];codeInjection:codeInjection(injectedFiles:list[];mark:False);matLayers:list[]) */
/* TCP_HASH 8ed827406b7ae472c1eb97601b00ca0a */
