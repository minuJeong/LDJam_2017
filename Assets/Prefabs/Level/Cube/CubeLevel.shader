// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2920,x:32719,y:32712,varname:node_2920,prsc:2|emission-8380-RGB;n:type:ShaderForge.SFN_Tex2d,id:8380,x:32415,y:32670,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:60be4cf9ff2ab184493984c39fc303d0,ntxv:0,isnm:False|UVIN-7623-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:8886,x:31451,y:32551,varname:node_8886,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:23,x:31809,y:32868,ptovrint:False,ptlb:TileScale,ptin:_TileScale,varname:_TileScale,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.125;n:type:ShaderForge.SFN_Multiply,id:5893,x:32023,y:32754,varname:node_5893,prsc:2|A-1576-OUT,B-23-OUT;n:type:ShaderForge.SFN_Divide,id:7028,x:32023,y:32624,varname:node_7028,prsc:2|A-1576-OUT,B-23-OUT;n:type:ShaderForge.SFN_Subtract,id:7623,x:32224,y:32624,varname:node_7623,prsc:2|A-5893-OUT,B-7028-OUT;n:type:ShaderForge.SFN_ObjectPosition,id:5936,x:31451,y:32680,varname:node_5936,prsc:2;n:type:ShaderForge.SFN_Subtract,id:2025,x:31641,y:32624,varname:node_2025,prsc:2|A-8886-XYZ,B-5936-XYZ;n:type:ShaderForge.SFN_ComponentMask,id:1576,x:31809,y:32624,varname:node_1576,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-2025-OUT;proporder:8380-23;pass:END;sub:END;*/

Shader "LDJam/CubeLevel" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _TileScale ("TileScale", Float ) = 0.125
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _TileScale;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
////// Lighting:
////// Emissive:
                float2 node_1576 = (i.posWorld.rgb-objPos.rgb).rg;
                float2 node_7623 = ((node_1576*_TileScale)-(node_1576/_TileScale));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_7623, _MainTex));
                float3 emissive = _MainTex_var.rgb;
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
