
Shader "PxShader"
{
	Properties
	{
		//Base Props
		[PerRendererData] _MainTex("Texture", 2D) = "white" {}
		[PerRendererData] _Opacity("Opacity", Range(0,1)) = 1
		[PerRendererData] _UvPositions("UvPositions", Vector) = (0,1,0,1)
		//Frag Props
		[PerRendererData] _AddBlending("AdditiveBlending", float) = 0
		[PerRendererData] _AddIntensity("AddIntensity", Range(0, 1)) = 0
		[PerRendererData] _AddColor("AddColor", Color) = (1, 1, 1, 1)
		[PerRendererData] _MultiplyBlending("MultiplyBlending", float) = 0
		[PerRendererData] _MultiplyIntensity("MultiplyIntensity", Range(0, 1)) = 0
		[PerRendererData] _MultiplyColor("MultiplyColor", Color) = (1, 1, 1, 1)
		[PerRendererData] _ColorBlending("ColorBlending", float) = 0
		[PerRendererData] _ColorIntensity("ColorIntensity", Range(0, 1)) = 0
		[PerRendererData] _ColorColor("ColorColor", Color) = (1, 1, 1, 1)
		[PerRendererData] _GrayscaleBlending("GrayscaleBlending", float) = 0
		[PerRendererData] _GrayscaleIntensity("GrayscaleIntensity", Range(0, 1)) = 0
		[PerRendererData] _Flash("Flash", float) = 0
		[PerRendererData] _FlashIntensity("FlashIntensity", Range(0, 1)) = 0
		[PerRendererData] _FlashColor("FlashColor", Color) = (1, 1, 1, 1)
		//Vertex Props
		[PerRendererData] _TransparencyCutoff("TransparencyCutoff", float) = 0
		[PerRendererData] _TransCutoffPos("TransCutoffPos", Range(0, 1)) = 0
		[PerRendererData] _TransCutoffGradient("TransCutoffGradient", Range(0, 1)) = 0
		//Stencil Props
		_StencilComp ("Stencil Comparison", Float) = 8.000000
		_Stencil ("Stencil ID", Float) = 0.000000
		_StencilOp ("Stencil Operation", Float) = 0.000000
		_StencilWriteMask ("Stencil Write Mask", Float) = 255.000000
		_StencilReadMask ("Stencil Read Mask", Float) = 255.000000
		_ColorMask ("Color Mask", Float) = 15.000000
			//Lerp Tween
			//_SecondTex("Second Texture", 2D) = "white" {}
			//_Tween("Tween", Range(0, 1)) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"PreviewType" = "Plane"
				"RenderType" = "Transparent"
				"DisableBatching" = "True"
				//Reference for Render Order
				//Background (1000), Geometry(2000), Transparent (3000), Overlay (4000+1)
			}

			Pass
			{
				ZWrite Off
				Cull Off
				//ZTest On
				
	            Stencil
	            {
					Ref [_Stencil]
					ReadMask [_StencilReadMask]
					WriteMask [_StencilWriteMask]
					Comp [_StencilComp]
					Pass [_StencilOp]
	            }
				Blend SrcAlpha OneMinusSrcAlpha
				ColorMask [_ColorMask]
				
				
				CGPROGRAM
				#include "UnityCG.cginc"
				#pragma multi_compile_instancing
				#pragma vertex vert
				#pragma fragment frag

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float2 uv : TEXCOORD0;
				};

				//Vert Shader
				v2f vert(appdata v)
				{
					v2f o;

					//v.vertex.x += 20 * sin(_Time  * 200);
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}
				//Pass to Frag
				
				UNITY_INSTANCING_BUFFER_START(Props)
					//UNITY_DEFINE_INSTANCED_PROP(float2, _UvXpos)
					//UNITY_DEFINE_INSTANCED_PROP(float2, _UvYpos)
					UNITY_DEFINE_INSTANCED_PROP(float4, _UvPositions)
					UNITY_DEFINE_INSTANCED_PROP(float, _Opacity)
					UNITY_DEFINE_INSTANCED_PROP(float, _AddBlending)
					UNITY_DEFINE_INSTANCED_PROP(float, _AddIntensity)
					UNITY_DEFINE_INSTANCED_PROP(float4, _AddColor)
					UNITY_DEFINE_INSTANCED_PROP(float,  _MultiplyBlending)
					UNITY_DEFINE_INSTANCED_PROP(float,  _MultiplyIntensity)
					UNITY_DEFINE_INSTANCED_PROP(float4, _MultiplyColor)
					UNITY_DEFINE_INSTANCED_PROP(float,  _ColorBlending)
					UNITY_DEFINE_INSTANCED_PROP(float,  _ColorIntensity)
					UNITY_DEFINE_INSTANCED_PROP(float4, _ColorColor)
					UNITY_DEFINE_INSTANCED_PROP(float,  _GrayscaleBlending)
					UNITY_DEFINE_INSTANCED_PROP(float,  _GrayscaleIntensity)
					UNITY_DEFINE_INSTANCED_PROP(float, _Flash)
					UNITY_DEFINE_INSTANCED_PROP(float, _FlashIntensity)
					UNITY_DEFINE_INSTANCED_PROP(float4, _FlashColor)
					UNITY_DEFINE_INSTANCED_PROP(float, _TransparencyCutoff)
					UNITY_DEFINE_INSTANCED_PROP(float, _TransCutoffPos)
					UNITY_DEFINE_INSTANCED_PROP(float, _TransCutoffGradient)
				UNITY_INSTANCING_BUFFER_END(Props)

				sampler2D _MainTex;

				//Frag Color Methods
				float4 grayscale(float4 color) 
				{
					float4 c = color;
					float lum = color.r * 0.3 + color.g * 0.59 + color.b * 0.11;
					float3 lumRgb = float3(lum,lum,lum);
					c.rgb = lerp(color.rgb, lumRgb, UNITY_ACCESS_INSTANCED_PROP(Props, _GrayscaleIntensity));
					return c;
				}

				float4 colorOverlay(float4 color) 
				{
					float4 c = color;
					float4 tint = UNITY_ACCESS_INSTANCED_PROP(Props, _ColorColor);
					c.rgb = lerp(color.rgb, tint.rgb, UNITY_ACCESS_INSTANCED_PROP(Props, _ColorIntensity));
					return c;
				}
				float4 add(float4 color)
				{
					float4 c = color;
					float4 tint = UNITY_ACCESS_INSTANCED_PROP(Props, _AddColor);
					c.rgb = lerp(color.rgb, tint.rgb += color.rgb, UNITY_ACCESS_INSTANCED_PROP(Props, _AddIntensity));
					return c;
				}
				float4 flash(float4 color)
				{
					float4 c = color;
					float4 tint = UNITY_ACCESS_INSTANCED_PROP(Props, _FlashColor);
					c.rgb = lerp(color.rgb, tint.rgb += color.rgb, UNITY_ACCESS_INSTANCED_PROP(Props, _FlashIntensity));
					return c;
				}
				float4 multiply(float4 color)
				{
					float4 c = color;
					float4 tint = UNITY_ACCESS_INSTANCED_PROP(Props, _MultiplyColor);
					c.rgb = lerp(color.rgb, tint.rgb *= color.rgb, UNITY_ACCESS_INSTANCED_PROP(Props, _MultiplyIntensity));
					return c;
				}
				//Frag UV Methods
				//float transCutoff(float2 uv, float alpha)
				//{
				//	if(uv.y < UNITY_ACCESS_INSTANCED_PROP(Props, _TransCutoffPos))
				//	{
				//		float a = alpha;
				//		a *= UNITY_ACCESS_INSTANCED_PROP(Props, _TransCutoffGradient);
				//		float threshold = .9;
				//		a *= (uv.y - (UNITY_ACCESS_INSTANCED_PROP(Props, _TransCutoffPos) * threshold)) / ((UNITY_ACCESS_INSTANCED_PROP(Props, _TransCutoffPos) - (UNITY_ACCESS_INSTANCED_PROP(Props, _TransCutoffPos) * threshold)));
				//		a = a < 0 ? 0 : a;
				//		return a;
				//		//return UNITY_ACCESS_INSTANCED_PROP(Props, _TransCutoffGradient) * alpha;
				//	}
				//	return alpha;
				//}
				float transCutoff(float2 uv, float4 uvPositions, float alpha)
				{
					float a = alpha;
					if(uv.y > uvPositions.z && uv.y < uvPositions.w)
					{
						//return 0;
						float textureRange = uvPositions.w - uvPositions.z; 			

						float startRange = uvPositions.z + textureRange * UNITY_ACCESS_INSTANCED_PROP(Props, _TransCutoffPos); 
						float endRange = startRange - textureRange * .05; 
						if(uv.y < startRange)
						{
							a *= (uv.y - endRange) / (startRange - endRange);
							//a * = (uv.y - endRange) * (1 / (startRange - endRange));
							a = a < 0 ? 0 : a; //clamp to 0 if negative
							return a;
						}									
					}
					return alpha;
				}

				//Frag Shader
				float4 frag(v2f i) : SV_Target
				{
					float4 color = tex2D(_MainTex, i.uv);
					//Color pixels w/ full opacity
					if (color.a == 1) 
					{
						if (UNITY_ACCESS_INSTANCED_PROP(Props, _GrayscaleBlending)) 
						{
							color = grayscale(color);
						}
						if (UNITY_ACCESS_INSTANCED_PROP(Props, _AddBlending))
						{
							color = add(color);
						}
						if (UNITY_ACCESS_INSTANCED_PROP(Props, _MultiplyBlending))
						{
							color = multiply(color);
						}
						if (UNITY_ACCESS_INSTANCED_PROP(Props, _ColorBlending))
						{
							color = colorOverlay(color);
						}
						if (UNITY_ACCESS_INSTANCED_PROP(Props, _Flash))
						{
							color = flash(color);
						}
					}		
					if(UNITY_ACCESS_INSTANCED_PROP(Props, _TransparencyCutoff))
					{
						//color.a = transCutoff(i.uv, color.a);
						color.a = transCutoff(i.uv, UNITY_ACCESS_INSTANCED_PROP(Props, _UvPositions), color.a);
					}
					//fixed4 waveyDispl = lerp(fixed4(1,0,0,1), fixed4(0,1,0,1), (sin(i.uv.x * 100) + 1) / 2);
					//float2 displUV = float2(waveyDispl.y * 1 - waveyDispl.x * 100, 0);

					//if(var.y > 0.425){
					//	color.a *= .5;
					//}
					//color.a *= i.uv.y; 

					
					//Transparency Shader Thing
					//if(i.uv.y < 0.58)
					//{
					//	color.a *= (i.uv.y - 0.47 ) / (0.58 - 0.47);
					//	//var.y = 0;
					//}

					color.a *= UNITY_ACCESS_INSTANCED_PROP(Props, _Opacity);
					return color;


					//Lerp two sprite images
					//float4 lerpColor1 = tex2D(_MainTex, i.uv);
					//float4 lerpColor2 = tex2D(_SecondTex, i.uv);
					//float4 lerpResult = lerp(lerpColor1, lerpColor2, _Tween);
				}
				ENDCG
			}
		}
}