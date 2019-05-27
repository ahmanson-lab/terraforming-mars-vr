// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.04 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.04;sub:START;pass:START;ps:flbk:Mobile/VertexLit,lico:0,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:False,hqlp:False,tesm:0,blpr:2,bsrc:0,bdst:0,culm:2,dpts:6,wrdp:False,dith:6,ufog:False,aust:False,igpj:True,qofs:0,qpre:3,rntp:2,fgom:True,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:True;n:type:ShaderForge.SFN_Final,id:740,x:33475,y:32174,varname:node_740,prsc:2|custl-861-OUT;n:type:ShaderForge.SFN_Tex2d,id:4840,x:32082,y:32415,varname:node_4840,prsc:2,ntxv:0,isnm:False|UVIN-5684-OUT,TEX-7418-TEX;n:type:ShaderForge.SFN_Tex2d,id:9889,x:32726,y:32359,varname:_node_4840_copy,prsc:2,ntxv:0,isnm:False|UVIN-6274-OUT,TEX-7418-TEX;n:type:ShaderForge.SFN_ScreenPos,id:1043,x:31672,y:32282,varname:node_1043,prsc:2,sctp:1;n:type:ShaderForge.SFN_Add,id:6274,x:32515,y:32307,varname:node_6274,prsc:2|A-3990-OUT,B-3945-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:3945,x:32306,y:32421,varname:node_3945,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:3990,x:32306,y:32263,varname:node_3990,prsc:2|A-4301-OUT,B-4840-R;n:type:ShaderForge.SFN_Slider,id:4301,x:31884,y:32265,ptovrint:False,ptlb:Noise Factor,ptin:_NoiseFactor,varname:node_4301,prsc:2,min:0,cur:0,max:5;n:type:ShaderForge.SFN_Tex2d,id:7712,x:32726,y:32644,ptovrint:False,ptlb:Mask,ptin:_Mask,varname:_node_4840_copy_copy,prsc:2,ntxv:0,isnm:False|UVIN-1340-OUT;n:type:ShaderForge.SFN_Add,id:1340,x:32515,y:32507,varname:node_1340,prsc:2|A-3945-UVOUT,B-6956-OUT;n:type:ShaderForge.SFN_Slider,id:6801,x:31884,y:32726,ptovrint:False,ptlb:Mask Noise Factor,ptin:_MaskNoiseFactor,varname:node_6801,prsc:2,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:6956,x:32306,y:32583,varname:node_6956,prsc:2|A-4840-R,B-6801-OUT;n:type:ShaderForge.SFN_VertexColor,id:53,x:32726,y:32494,varname:node_53,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1014,x:32969,y:32557,varname:node_1014,prsc:2|A-53-A,B-7712-RGB,C-9641-OUT;n:type:ShaderForge.SFN_Multiply,id:7068,x:32969,y:32409,varname:node_7068,prsc:2|A-9889-R,B-53-RGB;n:type:ShaderForge.SFN_Tex2dAsset,id:7418,x:31884,y:32529,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:node_7418,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:1249,x:31293,y:32351,ptovrint:False,ptlb:U Pan,ptin:_UPan,varname:node_1249,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:8313,x:31293,y:32434,ptovrint:False,ptlb:V Pan,ptin:_VPan,varname:node_8313,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_Append,id:2893,x:31478,y:32368,varname:node_2893,prsc:2|A-1249-OUT,B-8313-OUT;n:type:ShaderForge.SFN_Time,id:9411,x:31478,y:32523,varname:node_9411,prsc:2;n:type:ShaderForge.SFN_Multiply,id:5730,x:31672,y:32434,varname:node_5730,prsc:2|A-2893-OUT,B-9411-T;n:type:ShaderForge.SFN_Add,id:5684,x:31884,y:32353,varname:node_5684,prsc:2|A-1043-UVOUT,B-5730-OUT;n:type:ShaderForge.SFN_DepthBlend,id:9641,x:32726,y:32810,varname:node_9641,prsc:2|DIST-7585-OUT;n:type:ShaderForge.SFN_Slider,id:7585,x:32376,y:32810,ptovrint:False,ptlb:Depth Blend,ptin:_DepthBlend,varname:node_7585,prsc:2,min:0,cur:0,max:2;n:type:ShaderForge.SFN_Color,id:6180,x:32969,y:32264,ptovrint:False,ptlb:Tint,ptin:_Tint,varname:node_6180,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:861,x:33223,y:32331,varname:node_861,prsc:2|A-6180-RGB,B-7068-OUT,C-1014-OUT,D-3355-OUT,E-53-A;n:type:ShaderForge.SFN_Slider,id:3355,x:32989,y:32772,ptovrint:False,ptlb:Multiply,ptin:_Multiply,varname:node_3355,prsc:2,min:0,cur:5,max:1500;proporder:6180-7712-7418-4301-6801-1249-8313-7585-3355;pass:END;sub:END;*/

Shader "FORGE3D/Exhaust Flame" {
    Properties {
        _Tint ("Tint", Color) = (0.5,0.5,0.5,1)
        _Mask ("Mask", 2D) = "white" {}
        _Noise ("Noise", 2D) = "white" {}
        _NoiseFactor ("Noise Factor", Range(0, 5)) = 0
        _MaskNoiseFactor ("Mask Noise Factor", Range(0, 1)) = 0
        _UPan ("U Pan", Float ) = 0
        _VPan ("V Pan", Float ) = 0
        _DepthBlend ("Depth Blend", Range(0, 2)) = 0
        _Multiply ("Multiply", Range(0, 1500)) = 5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            Cull Off
            ZTest Always
            ZWrite Off
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 2.0
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _TimeEditor;
            uniform float _NoiseFactor;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float _MaskNoiseFactor;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _UPan;
            uniform float _VPan;
            uniform float _DepthBlend;
            uniform float4 _Tint;
            uniform float _Multiply;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                o.screenPos = float4( o.pos.xy / o.pos.w, 0, 0 );
                o.screenPos.y *= _ProjectionParams.x;
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
/////// Vectors:
////// Lighting:
                float4 node_9411 = _Time + _TimeEditor;
                float2 node_5684 = (float2(i.screenPos.x*(_ScreenParams.r/_ScreenParams.g), i.screenPos.y).rg+(float2(_UPan,_VPan)*node_9411.g));
                float4 node_4840 = tex2D(_Noise,TRANSFORM_TEX(node_5684, _Noise));
                float2 node_6274 = ((_NoiseFactor*node_4840.r)+i.uv0);
                float4 _node_4840_copy = tex2D(_Noise,TRANSFORM_TEX(node_6274, _Noise));
                float2 node_1340 = (i.uv0+(node_4840.r*_MaskNoiseFactor));
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(node_1340, _Mask));
                float3 finalColor = (_Tint.rgb*(_node_4840_copy.r*i.vertexColor.rgb)*(i.vertexColor.a*_Mask_var.rgb*saturate((sceneZ-partZ)/_DepthBlend))*_Multiply*i.vertexColor.a);
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Mobile/VertexLit"
    CustomEditor "ShaderForgeMaterialInspector"
}
