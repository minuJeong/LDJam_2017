// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:1,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:6,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:False,qofs:100,qpre:4,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:8427,x:32782,y:32428,varname:node_8427,prsc:2|emission-1189-RGB;n:type:ShaderForge.SFN_FragmentPosition,id:1245,x:31256,y:32538,varname:node_1245,prsc:2;n:type:ShaderForge.SFN_ObjectPosition,id:7789,x:31256,y:32387,varname:node_7789,prsc:2;n:type:ShaderForge.SFN_Divide,id:1628,x:31780,y:32456,varname:node_1628,prsc:2|A-6747-OUT,B-681-OUT;n:type:ShaderForge.SFN_ValueProperty,id:681,x:31603,y:32604,ptovrint:False,ptlb:Radius,ptin:_Radius,varname:node_681,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Power,id:7131,x:31955,y:32456,varname:node_7131,prsc:2|VAL-1628-OUT,EXP-7523-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7523,x:31780,y:32604,ptovrint:False,ptlb:OutlineSharp,ptin:_OutlineSharp,varname:node_7523,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:4;n:type:ShaderForge.SFN_Subtract,id:4455,x:31431,y:32456,varname:node_4455,prsc:2|A-7789-XYZ,B-1245-XYZ;n:type:ShaderForge.SFN_Length,id:6747,x:31603,y:32456,varname:node_6747,prsc:2|IN-4455-OUT;n:type:ShaderForge.SFN_Clamp01,id:5294,x:32146,y:32456,varname:node_5294,prsc:2|IN-7131-OUT;n:type:ShaderForge.SFN_Tex2d,id:1189,x:32502,y:32509,ptovrint:False,ptlb:ColorDRF,ptin:_ColorDRF,varname:node_1189,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:4cd8ce02fcdd29d4c81fee29b089f9de,ntxv:0,isnm:False|UVIN-4667-OUT;n:type:ShaderForge.SFN_Append,id:4667,x:32326,y:32477,varname:node_4667,prsc:2|A-5294-OUT,B-6886-OUT;n:type:ShaderForge.SFN_Vector1,id:6886,x:32146,y:32577,varname:node_6886,prsc:2,v1:0.5;proporder:681-7523-1189;pass:END;sub:END;*/

Shader "LDJam/eye" {
    Properties {
        _Radius ("Radius", Float ) = 1
        _OutlineSharp ("OutlineSharp", Float ) = 4
        _ColorDRF ("ColorDRF", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "Queue"="Overlay+100"
            "RenderType"="Opaque"
            "DisableBatching"="True"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            ZTest Always
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _Radius;
            uniform float _OutlineSharp;
            uniform sampler2D _ColorDRF; uniform float4 _ColorDRF_ST;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
////// Lighting:
////// Emissive:
                float2 node_4667 = float2(saturate(pow((length((objPos.rgb-i.posWorld.rgb))/_Radius),_OutlineSharp)),0.5);
                float4 _ColorDRF_var = tex2D(_ColorDRF,TRANSFORM_TEX(node_4667, _ColorDRF));
                float3 emissive = _ColorDRF_var.rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
