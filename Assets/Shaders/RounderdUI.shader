Shader "UI/RoundedUI"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}

        _Color ("Background Color", Color) = (1,1,1,1)

        // 今回からピクセル単位
        _Radius ("Corner Radius (px)", Range(0, 100)) = 12

        // 今回からピクセル単位
        _BorderWidth ("Border Width (px)", Range(0, 20)) = 1

        _BorderColor ("Border Color", Color) = (0.8,0.8,0.8,1)

        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255

        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]

        Blend SrcAlpha OneMinusSrcAlpha

        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
            };

            sampler2D _MainTex;

            fixed4 _Color;
            fixed4 _BorderColor;

            float _Radius;
            float _BorderWidth;

            float4 _ClipRect;


            v2f vert(appdata_t v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPosition = v.vertex;
                o.uv = v.texcoord;

                o.color = v.color * _Color;

                return o;
            }


            // 角丸長方形のSigned Distance
            float RoundedBoxSDF(
                float2 p,
                float2 halfSize,
                float radius
            )
            {
                float2 q =
                    abs(p)
                    - halfSize
                    + radius;

                return
                    length(max(q, 0.0))
                    + min(max(q.x, q.y), 0.0)
                    - radius;
            }


            fixed4 frag(v2f i) : SV_Target
            {
                // =========================================
                // UIの実際の画面上サイズを推定
                // =========================================

                float uvPerPixelX =
                    length(
                        float2(
                            ddx(i.uv.x),
                            ddy(i.uv.x)
                        )
                    );

                float uvPerPixelY =
                    length(
                        float2(
                            ddx(i.uv.y),
                            ddy(i.uv.y)
                        )
                    );

                float widthPx =
                    1.0 / max(uvPerPixelX, 0.000001);

                float heightPx =
                    1.0 / max(uvPerPixelY, 0.000001);


                // =========================================
                // UVをピクセル空間へ変換
                // =========================================

                float2 sizePx =
                    float2(
                        widthPx,
                        heightPx
                    );

                float2 p =
                    (i.uv - 0.5)
                    * sizePx;

                float2 halfSize =
                    sizePx * 0.5;


                // =========================================
                // Radiusを安全な範囲へ制限
                // =========================================

                float maxRadius =
                    min(
                        halfSize.x,
                        halfSize.y
                    );

                float radius =
                    min(
                        _Radius,
                        maxRadius
                    );


                // =========================================
                // 外側の角丸形状
                // =========================================

                float outerDistance =
                    RoundedBoxSDF(
                        p,
                        halfSize,
                        radius
                    );

                float aa =
                    max(
                        fwidth(outerDistance),
                        0.5
                    );

                float outerAlpha =
                    1.0
                    - smoothstep(
                        -aa,
                        aa,
                        outerDistance
                    );


                // =========================================
                // 内側
                // =========================================

                float border =
                    max(
                        _BorderWidth,
                        0.0
                    );

                float2 innerHalfSize =
                    max(
                        halfSize - border,
                        float2(0.0, 0.0)
                    );

                float innerRadius =
                    max(
                        radius - border,
                        0.0
                    );

                float innerDistance =
                    RoundedBoxSDF(
                        p,
                        innerHalfSize,
                        innerRadius
                    );

                float innerAlpha =
                    1.0
                    - smoothstep(
                        -aa,
                        aa,
                        innerDistance
                    );


                // =========================================
                // 背景と枠線
                // =========================================

                float borderMask =
                    saturate(
                        outerAlpha
                        - innerAlpha
                    );

                fixed4 result =
                    i.color;

                result.rgb =
                    lerp(
                        result.rgb,
                        _BorderColor.rgb,
                        borderMask
                    );

                result.a =
                    outerAlpha
                    * i.color.a;


                // =========================================
                // Unity UIのMask / ScrollView対応
                // =========================================

                result.a *=
                    UnityGet2DClipping(
                        i.worldPosition.xy,
                        _ClipRect
                    );

                return result;
            }

            ENDCG
        }
    }
}